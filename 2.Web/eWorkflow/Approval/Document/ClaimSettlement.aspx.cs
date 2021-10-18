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

public partial class Approval_Document_ClaimSettlement : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_CLAIM_SETTLEMENT_LIST_KEY = "VIEWSTATE_KEY_CLAIM_SETTLEMENT_LIST";

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
                ViewState[VIEWSTATE_CLAIM_SETTLEMENT_LIST_KEY] = new List<DTO_DOC_CLAIM_SETTLEMENT_LIST>();
                this.radGrdClaimSettlementList.DataSource = (List<DTO_DOC_CLAIM_SETTLEMENT_LIST>)ViewState[VIEWSTATE_CLAIM_SETTLEMENT_LIST_KEY];
                this.radGrdClaimSettlementList.DataBind();
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
        hddDocumentID.Value = "D0026";
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        //hddProcessID.Value = "P000000570";

        //hddProcessStatus.Value = ApprovalUtil.ApprovalStatus.Temp.ToString();

        InitControls();
    }
    #endregion

    #region InitControls
    private void InitControls()
    {

        DTO_DOC_CLAIM_SETTLEMENT doc;
        if (!hddProcessID.Value.Equals(String.Empty))
        {
            string processId = this.hddProcessID.Value;
            using (Bayer.eWF.BSL.Approval.Mgr.ClaimSettlementMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.ClaimSettlementMgr())
            {
                doc = mgr.SelectClaimSettlement(hddProcessID.Value);
                this.hddProcessStatus.Value = doc.PROCESS_STATUS;

                if (doc != null)
                {
                    if (doc.TYPE == "Cash")
                        this.radRdoType1.Checked = true;
                    else if (doc.TYPE == "Commodity")
                        this.radRdoType2.Checked = true;

                    // CLAIM SETTLEMENT LIST
                    List<DTO_DOC_CLAIM_SETTLEMENT_LIST> lists = mgr.SelectClaimSettlementList(hddProcessID.Value);
                    ViewState[VIEWSTATE_CLAIM_SETTLEMENT_LIST_KEY] = lists;

                    this.radGrdClaimSettlementList.DataSource = lists;
                    this.radGrdClaimSettlementList.DataBind();
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

        if (this.hddGridItems.Value != "")
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<DTO_DOC_CLAIM_SETTLEMENT_LIST> items = (List<DTO_DOC_CLAIM_SETTLEMENT_LIST>)serializer.Deserialize<List<DTO_DOC_CLAIM_SETTLEMENT_LIST>>(this.hddGridItems.Value);

            ViewState[VIEWSTATE_CLAIM_SETTLEMENT_LIST_KEY] = items;
            this.radGrdClaimSettlementList.DataSource = items;
            this.radGrdClaimSettlementList.DataBind();
        }

        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radBtnAdd, radGrdClaimSettlementList, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdClaimSettlementList);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_Donation_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";
    }

    void Approval_Document_Donation_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("ApplyProduct"))
        {
            string[] values = e.Argument.Split(new char[] { ':' });
            if (values.Length > 3)
            {
                string idx = values[1];
                string code = values[2];
                string name = values[3];

                UpdateGridData(false, Convert.ToInt32(idx), code, name);

            }
        }
    }
    #endregion

    #region RadioButtonEvent

    protected void RadrdoBtn_Click(object sender, EventArgs e)
    {
        if (this.hddGridItems.Value == "")
        {
            RadrdoBtn_Click();
            ViewState[VIEWSTATE_CLAIM_SETTLEMENT_LIST_KEY] = new List<DTO_DOC_CLAIM_SETTLEMENT_LIST>();
            this.radGrdClaimSettlementList.DataSource = (List<DTO_DOC_CLAIM_SETTLEMENT_LIST>)ViewState[VIEWSTATE_CLAIM_SETTLEMENT_LIST_KEY];
            this.radGrdClaimSettlementList.DataBind();
        }
        else
        {
            RadrdoBtn_Click();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<DTO_DOC_CLAIM_SETTLEMENT_LIST> items = (List<DTO_DOC_CLAIM_SETTLEMENT_LIST>)serializer.Deserialize<List<DTO_DOC_CLAIM_SETTLEMENT_LIST>>(this.hddGridItems.Value);
            ViewState[VIEWSTATE_CLAIM_SETTLEMENT_LIST_KEY] = items;
            this.radGrdClaimSettlementList.DataSource = items;
            this.radGrdClaimSettlementList.DataBind();
        }
    }

    private void RadrdoBtn_Click()
    {
        if (radRdoType1.Checked == true)
        {
            foreach (GridColumn column in radGrdClaimSettlementList.Columns)
            {
                if (column.UniqueName == "BANK_ACCOUNT")
                    (column as GridTemplateColumn).Display = true;
                if (column.UniqueName == "PRODUCT_CODE")
                    (column as GridTemplateColumn).Display = false;
                if (column.UniqueName == "PRODUCT_NAME")
                    (column as GridTemplateColumn).Display = false;
                if (column.UniqueName == "QTY")
                    (column as GridTemplateColumn).Display = false;
            }
        }
        else if (radRdoType2.Checked == true)
        {
            foreach (GridColumn column in radGrdClaimSettlementList.Columns)
            {
                if (column.UniqueName == "BANK_ACCOUNT")
                    (column as GridTemplateColumn).Display = false;
                if (column.UniqueName == "PRODUCT_CODE")
                    (column as GridTemplateColumn).Display = false;
                if (column.UniqueName == "PRODUCT_NAME")
                    (column as GridTemplateColumn).Display = true;
                if (column.UniqueName == "QTY")
                    (column as GridTemplateColumn).Display = true;
            }
        }
    }
    #endregion

    #region Add Button Event

    protected void radBtnAdd_Click(object sender, EventArgs e)
    {
        UpdateGridData(true);
    }

    private void UpdateGridData(bool addRow)
    {
        UpdateGridData(addRow, 999, string.Empty, string.Empty);
    }


    private void UpdateGridData(bool addRow, int idx, string code, string name)
    {
        RadrdoBtn_Click();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_CLAIM_SETTLEMENT_LIST> items = (List<DTO_DOC_CLAIM_SETTLEMENT_LIST>)serializer.Deserialize<List<DTO_DOC_CLAIM_SETTLEMENT_LIST>>(this.hddGridItems.Value);

        if (addRow)
        {
            var lastData = items.OrderByDescending(d => d.IDX).FirstOrDefault();
            int newIdx = 1;
            if (lastData != null)
            {
                newIdx = Convert.ToInt32(lastData.IDX);
                newIdx++;
            }

            if (items == null) items = new List<DTO_DOC_CLAIM_SETTLEMENT_LIST>();

            DTO_DOC_CLAIM_SETTLEMENT_LIST item = new DTO_DOC_CLAIM_SETTLEMENT_LIST();
            item.IDX = newIdx;
            items.Add(item);
        }

        if (idx < 999)
        {
            var itemProduct = (from item in items
                               where item.IDX == idx
                               select item).FirstOrDefault();

            if (itemProduct != null)
            {
                itemProduct.PRODUCT_CODE = code;
                itemProduct.PRODUCT_NAME = name + "(" + code + ")";
            }
        }

        ViewState[VIEWSTATE_CLAIM_SETTLEMENT_LIST_KEY] = items;
        this.radGrdClaimSettlementList.DataSource = items;
        this.radGrdClaimSettlementList.DataBind();

    }


    #endregion

    protected void radGrdClaimSettlementList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem form = (GridDataItem)e.Item;

            RadTextBox data1 = (RadTextBox)form["BANK_ACCOUNT"].FindControl("radGrdTxtBankAccount");
            if (data1 != null)
                data1.Focus();

            RadTextBox data2 = (RadTextBox)form["PRODUCT_CODE"].FindControl("radGrdTxtProductCode");
            if (data2 != null)
                data2.Focus();

            RadTextBox data3 = (RadTextBox)form["PRODUCT_NAME"].FindControl("radGrdTxtProductName");
            if (data3 != null)
                data3.Focus();

            RadNumericTextBox data4 = (RadNumericTextBox)form["QTY"].FindControl("RadGrdtxtQty");
            if (data4 != null)
                data4.Focus();
        }
    }

    protected void radGrdClaimSettlementList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            int index = Convert.ToInt32(e.CommandArgument);

            using (ClaimSettlementMgr mgr = new ClaimSettlementMgr())
            {
                mgr.DeleteClaimSettlementListByIndex(this.hddProcessID.Value, index);
            }
            List<DTO_DOC_CLAIM_SETTLEMENT_LIST> list = (List<DTO_DOC_CLAIM_SETTLEMENT_LIST>)ViewState[VIEWSTATE_CLAIM_SETTLEMENT_LIST_KEY];

            list.RemoveAll(p => p.IDX == index);

            this.radGrdClaimSettlementList.DataSource = list;
            this.radGrdClaimSettlementList.DataBind();
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
            if (!(radRdoType1.Checked || radRdoType2.Checked))
                message += "Type";
            if (this.radGrdClaimSettlementList.Items.Count < 1)
                message += message.IsNullOrEmptyEx() ? "Grid Item" : ",Grid Item";
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
        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_CLAIM_SETTLEMENT doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_CLAIM_SETTLEMENT();
        doc.PROCESS_STATUS = processStatus;
        doc.PROCESS_ID = hddProcessID.Value;
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = Sessions.UserID;
        doc.SUBJECT = "CLAIM SETTLEMENT";
        webMaster.Subject = doc.SUBJECT;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.CREATE_DATE = DateTime.Now;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.CREATOR_ID = Sessions.UserID;
        doc.IS_DISUSED = "N";

        if (radRdoType1.Checked)
            doc.TYPE = radRdoType1.Value;
        if (radRdoType2.Checked)
            doc.TYPE = radRdoType2.Value;

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_CLAIM_SETTLEMENT_LIST> lists = (List<DTO_DOC_CLAIM_SETTLEMENT_LIST>)serializer.Deserialize<List<DTO_DOC_CLAIM_SETTLEMENT_LIST>>(this.hddGridItems.Value);
        foreach (DTO_DOC_CLAIM_SETTLEMENT_LIST list in lists)
        {
            list.PROCESS_ID = this.hddProcessID.Value;
            list.CREATOR_ID = Sessions.UserID;
        }

        using (Bayer.eWF.BSL.Approval.Mgr.ClaimSettlementMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.ClaimSettlementMgr())
        {
            processID = mgr.MergeClaimSettlement(doc, lists);
        }
        return processID;
    }


}