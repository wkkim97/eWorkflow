using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNSoft.eWF.FrameWork.Web;
using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Dao;
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Mgr;
using Bayer.eWF.BSL.Common.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using System.Web.Script.Serialization;
using Telerik.Web.UI;

public partial class Approval_Document_BmskRebate : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_BMSK_REBATE_PRODUCT_KEY = "VIEWSTATE_BMSK_REBATE_PRODUCT_KEY";
    #region OnPreInit Event
    /// <summary>
    /// 최초 페이지 로드시 호출
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        var masterPage = Master;
        this.webMaster = (masterPage as Master_eWorks_Document);
        base.OnPreInit(e);
    }
    #endregion

    #region Page Load Event
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                this.hddCompanyCode.Value = Sessions.CompanyCode;

                ViewState[VIEWSTATE_BMSK_REBATE_PRODUCT_KEY] = new List<DTO_DOC_BMSK_REBATE_PRODUCT>();
                this.radGrdRebateProduct.DataSource = (List<DTO_DOC_BMSK_REBATE_PRODUCT>)ViewState[VIEWSTATE_BMSK_REBATE_PRODUCT_KEY];
                this.radGrdRebateProduct.DataBind();
                InitPageInfo();
            }
            PageLoadInfo();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    #endregion

    #region InitPageInfo
    private void InitPageInfo()
    {
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0033";
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        //hddProcessID.Value = "P000000725";

        //hddProcessStatus.Value = ApprovalUtil.ApprovalStatus.Temp.ToString();

        InitControls();
    }
    #endregion

    #region InitControls
    private void InitControls()
    {
        DTO_DOC_BMSK_REBATE doc;
        if (!hddProcessID.Value.Equals(String.Empty))
        {
            string processId = this.hddProcessID.Value;
            using (Bayer.eWF.BSL.Approval.Mgr.BmskRebateMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.BmskRebateMgr())
            {
                doc = mgr.SelectBmskRebate(hddProcessID.Value);
                this.hddProcessStatus.Value = doc.PROCESS_STATUS;

                if (doc != null)
                {
                    this.radTxtSubject.Text = doc.TITLE;
                    if (doc.BU == radRdoBu1.Value)
                        this.radRdoBu1.Checked = true;
                    else if (doc.BU == radRdoBu2.Value)
                        this.radRdoBu2.Checked = true;

                    this.radFromDate.SelectedDate = doc.VALIDITY_FROM;
                    this.radToDate.SelectedDate = doc.VALIDITY_TO;
                    this.radTxtTarget.Text = doc.TARGET_PERFORM;
                    this.radTxtEstimated.Text = doc.ESTIMATED_PERFORM;

                    List<DTO_DOC_BMSK_REBATE_PRODUCT> products = mgr.SelectBmskRebateProduct(hddProcessID.Value);
                    ViewState[VIEWSTATE_BMSK_REBATE_PRODUCT_KEY] = products;

                    this.radGrdRebateProduct.DataSource = products;
                    this.radGrdRebateProduct.DataBind();

                    List<DTO_DOC_BMSK_REBATE_CUSTOMER> customers = mgr.SelectBmskRebateCustomer(this.hddProcessID.Value);
                    foreach (DTO_DOC_BMSK_REBATE_CUSTOMER customer in customers)
                    {
                        AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                        entry.Value = customer.CUSTOMER_CODE;
                        entry.Text = customer.CUSTOMER_NAME;
                        this.radAcomCustomer.Entries.Add(entry);
                    }

                    webMaster.DocumentNo = doc.DOC_NUM;
                }
            }
        }

    }
    #endregion

    #region PageLoadInfo
    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 
        if (!this.hddGridItems.Value.IsNullOrEmptyEx()) UpdateGridData();
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdRebateProduct);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_BmskRebate_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";
    }


    #endregion

    void Approval_Document_BmskRebate_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("Rebind"))
        {
            UpdateGridData();
        }
    }

    #region [ Add Row ]

    private void UpdateGridData()
    {
        UpdateGridData(999, string.Empty, string.Empty);
    }

    private void UpdateGridData(int idx, string code, string name)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_BMSK_REBATE_PRODUCT> items = (List<DTO_DOC_BMSK_REBATE_PRODUCT>)serializer.Deserialize<List<DTO_DOC_BMSK_REBATE_PRODUCT>>(this.hddGridItems.Value);

        if (idx < 999)
        {
            var product = (from item in items
                              where item.IDX == idx
                              select item).FirstOrDefault();
            if (product != null)
            {
                product.PRODUCT_CODE = code;
                product.PRODUCT_NAME = name;
            }
        }

        ViewState[VIEWSTATE_BMSK_REBATE_PRODUCT_KEY] = items;
        this.radGrdRebateProduct.DataSource = items;
        this.radGrdRebateProduct.DataBind();

    }
    #endregion


    protected void radGrdRebateProduct_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Remove"))
            {
                int index = Convert.ToInt32(e.CommandArgument);

                using (BmskRebateMgr Mgr = new BmskRebateMgr())
                {
                    Mgr.DeleteBmskRebateProductByIndex(this.hddProcessID.Value, index);
                }
                List<DTO_DOC_BMSK_REBATE_PRODUCT> product = (List<DTO_DOC_BMSK_REBATE_PRODUCT>)ViewState[VIEWSTATE_BMSK_REBATE_PRODUCT_KEY];
                product.RemoveAll(p => p.IDX == index);

                this.radGrdRebateProduct.DataSource = product;
                this.radGrdRebateProduct.DataBind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    protected override void DoRequest()
    {
        try
        {
            if (ValidationCheck(ApprovalUtil.ApprovalStatus.Request))
            {
                string ApprovalStatus = hddProcessID.Value.Length > 0 ? ApprovalUtil.ApprovalStatus.Saved.ToString() : ApprovalUtil.ApprovalStatus.Temp.ToString();
                hddProcessID.Value = DocumentSave(ApprovalStatus);
                webMaster.ProcessID = hddProcessID.Value;
                base.DoRequest();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    protected override void DoApproval()
    {
        // TODO :

        base.DoApproval();
    }

    protected override void DoSave()
    {
        try
        {
            if (ValidationCheck(ApprovalUtil.ApprovalStatus.Saved))
            {
                hddProcessID.Value = DocumentSave(ApprovalUtil.ApprovalStatus.Saved.ToString());
                webMaster.ProcessID = hddProcessID.Value;
                base.DoSave();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            if(this.radTxtSubject.Text.Length <= 0)
                message = "Subject";
            //if(!(this.radRdoBu1.Checked || this.radRdoBu2.Checked))
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "BU";
            //if (radAcomCustomer.Entries.Count < 1)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Customer";
            //if (!radFromDate.SelectedDate.HasValue)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Validity(From)";
            //if (!radToDate.SelectedDate.HasValue)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Validity(To)";
            //if(this.radTxtTarget.Text.Length <= 0)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Target Perfomance";
            //if (this.radTxtEstimated.Text.Length <= 0)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Estimated Perfomance";
            //if (this.radGrdRebateProduct.Items.Count < 1)
            //    message += message.IsNullOrEmptyEx() ? "Grid Item" : ",Grid Item";
        }
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;

    }

    private string DocumentSave(string processStatus)
    {
        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_BMSK_REBATE doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_BMSK_REBATE();
        doc.PROCESS_STATUS = processStatus;
        doc.PROCESS_ID = hddProcessID.Value;
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = Sessions.UserID;
        doc.SUBJECT = this.radTxtSubject.Text;
        webMaster.Subject = doc.SUBJECT;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.CREATE_DATE = DateTime.Now;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.CREATOR_ID = Sessions.UserID;
        doc.IS_DISUSED = "N";


        doc.TITLE = radTxtSubject.Text;
        if (radRdoBu1.Checked)
            doc.BU = radRdoBu1.Value;
        else if (radRdoBu2.Checked)
            doc.BU = radRdoBu2.Value;
        doc.VALIDITY_FROM = radFromDate.SelectedDate;
        doc.VALIDITY_TO = radToDate.SelectedDate;
        doc.TARGET_PERFORM = radTxtTarget.Text;
        doc.ESTIMATED_PERFORM = radTxtEstimated.Text;


        List<DTO_DOC_BMSK_REBATE_CUSTOMER> customers = new List<DTO_DOC_BMSK_REBATE_CUSTOMER>();
        foreach (AutoCompleteBoxEntry entry in this.radAcomCustomer.Entries)
        {
            DTO_DOC_BMSK_REBATE_CUSTOMER customer = new DTO_DOC_BMSK_REBATE_CUSTOMER();
            customer.PROCESS_ID = this.hddProcessID.Value;
            customer.CUSTOMER_CODE = entry.Value;
            customer.CUSTOMER_NAME = entry.Text;
            customer.CREATOR_ID = Sessions.UserID;
            customers.Add(customer);
        }

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_BMSK_REBATE_PRODUCT> products = (List<DTO_DOC_BMSK_REBATE_PRODUCT>)serializer.Deserialize<List<DTO_DOC_BMSK_REBATE_PRODUCT>>(this.hddGridItems.Value);
        foreach (DTO_DOC_BMSK_REBATE_PRODUCT product in products)
        {
            product.PROCESS_ID = this.hddProcessID.Value;
            product.CREATOR_ID = Sessions.UserID;
        }

        using (Bayer.eWF.BSL.Approval.Mgr.BmskRebateMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.BmskRebateMgr())
        {
            processID = mgr.MergeBmskRebate(doc, products, customers);
        }
        return processID;
    }
}