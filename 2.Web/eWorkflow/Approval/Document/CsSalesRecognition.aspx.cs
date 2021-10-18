using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Bayer.eWF.BSL.Approval.Dao;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Telerik.Web.UI;
using System.Web.Script.Serialization;

public partial class Approval_Document_CsSalesRecognition : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_CS_SALES_RECOGNITION_PRODUCT_KEY = "VIEWSTATE_CS_SALES_RECOGNITION_PRODUCT_KEY";

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
    protected override void OnPreRender(EventArgs e)
    {
        if (webMaster.ProcessStatus.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || webMaster.ProcessStatus.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString()))
        {
            //radRadioCheck();
        }
        if (webMaster.DocumentNo != "")
        {
            //webMaster.SetEnableControls(Dealno, true);

        }
        base.OnPreRender(e);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                this.hddCompanyCode.Value = Sessions.CompanyCode; //회사코드 설정
                //ViewState[VIEWSTATE_CS_SALES_RECOGNITION_PRODUCT_KEY] = new List<DTO_DOC_CS_SALES_RECOGNITION_PRODUCT>();
                //this.radGrdSampleItemList.DataSource = (List<DTO_DOC_CS_SALES_RECOGNITION_PRODUCT>)ViewState[VIEWSTATE_CS_SALES_RECOGNITION_PRODUCT_KEY];
                //this.radGrdSampleItemList.DataBind();
                InitPageInfo();
            }
            PageLoadInfo();


        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    private void InitPageInfo()
    {
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0053";
        //hddProcessID.Value = "P000000910";

        //hddProcessStatus.Value = ApprovalUtil.ApprovalStatus.Temp.ToString();

        InitControls();
    }

    private void InitControls()
    {
        if (this.hddProcessID.Value.IsNotNullOrEmptyEx())
        {
            using (CSSalesRecognitionMgr mgr = new CSSalesRecognitionMgr())
            {
                DTO_DOC_CS_SALES_RECOGNITION doc = mgr.SelectCSSalesRecognition(this.hddProcessID.Value);

                foreach (Control control in this.divBU.Controls)
                {
                    if (control is RadButton)
                    {
                        if ((control as RadButton).Value.Equals(doc.BU))
                        {
                            (control as RadButton).Checked = true; break;
                        }
                    }
                }
                this.hddCustomerCode.Value = doc.CUSTOMER_CODE;
                this.radGrdTxtCustomer.Text = doc.CUSTOMER_NAME;
                this.radDdlReason.SelectedValue = doc.REASON;
                this.RadTextComment.Text = doc.COMMENT;
                webMaster.DocumentNo = doc.DOC_NUM;

                List<DTO_DOC_CS_SALES_RECOGNITION_PRODUCT> products = mgr.SelectCSSalesRecognitionProduct(this.hddProcessID.Value);
                ViewState[VIEWSTATE_CS_SALES_RECOGNITION_PRODUCT_KEY] = products;


 
                foreach (DTO_DOC_CS_SALES_RECOGNITION_PRODUCT product in products)
                {
                    AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                    entry.Value = product.PRODUCT_CODE;
                    entry.Text = product.PRODUCT_NAME;
                    //this.radAutoProduct.Entries.Add(entry);
                }

                this.radGrdSampleItemList.DataSource = products;
                this.radGrdSampleItemList.DataBind();


            }

        }
        
    }

    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 
        
        //if (!ClientScript.IsStartupScriptRegistered("setVisibleControl"))
        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleControl", "setVisibleControl('','" + GetTitleCode() + "','');", true);
        
        if (this.hddGridItems.Value != "")
        {
            UpdateGridData();
        }
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdSampleItemList);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_CSException_AjaxRequest;
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_SampleRequest_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";
    }
    void Approval_Document_CSException_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("removegrid"))
        {
            ViewState[VIEWSTATE_CS_SALES_RECOGNITION_PRODUCT_KEY] = new List<DTO_DOC_CS_SALES_RECOGNITION_PRODUCT>();
            this.radGrdSampleItemList.DataSource = (List<DTO_DOC_CS_SALES_RECOGNITION_PRODUCT>)ViewState[VIEWSTATE_CS_SALES_RECOGNITION_PRODUCT_KEY];
            this.radGrdSampleItemList.DataBind();
        }
    }
        void Approval_Document_SampleRequest_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("Rebind"))
        {
            UpdateGridData();
        }
        else if (e.Argument.StartsWith("Costcenter"))
        {
            string[] values = e.Argument.Split(new char[] { ':' });
            if (values.Length > 3)
            {
                string idx = values[1];
                string code = values[2];
                string name = values[3];

              //UpdateGridData(Convert.ToInt32(idx), code, name);
            }
        }
    }
    #region Category : Radio Event
    protected void RadrdoNew_Click(object sender, EventArgs e)
    {
        this.informationMessage = "";
        //RadioCheck();
        this.hddCustomerCode.Value = "";
        this.hddCustomerType.Value = "";
        this.radGrdTxtCustomer.Text = "";
        ViewState[VIEWSTATE_CS_SALES_RECOGNITION_PRODUCT_KEY] = new List<DTO_DOC_CS_SALES_RECOGNITION_PRODUCT>();
        this.radGrdSampleItemList.DataSource = (List<DTO_DOC_CS_SALES_RECOGNITION_PRODUCT>)ViewState[VIEWSTATE_CS_SALES_RECOGNITION_PRODUCT_KEY];
        this.radGrdSampleItemList.DataBind();
    }
  
    #endregion
    // private void UpdateGridData()
    //{
    //    UpdateGridData(999, string.Empty, string.Empty);
    //}

    private void UpdateGridData()
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_CS_SALES_RECOGNITION_PRODUCT> products = (List<DTO_DOC_CS_SALES_RECOGNITION_PRODUCT>)serializer.Deserialize<List<DTO_DOC_CS_SALES_RECOGNITION_PRODUCT>>(this.hddGridItems.Value);
        foreach (DTO_DOC_CS_SALES_RECOGNITION_PRODUCT product in products)
        {
            //product.AMOUNT = product.DISCOUNT_AMOUNT * product.QTY;
        }
        ViewState[VIEWSTATE_CS_SALES_RECOGNITION_PRODUCT_KEY] = products;
        this.radGrdSampleItemList.DataSource = products;
        this.radGrdSampleItemList.DataBind();

    }
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {

            if (GetSelectedBU().IsNullOrEmptyEx())
                message += message.IsNullOrEmptyEx() ? "Business Unit" : ", Business Unit";

            if (this.radDdlReason.SelectedValue.IsNullOrEmptyEx())
                message += message.IsNullOrEmptyEx() ? "Reason" : ", Reason";
            
            if (this.radGrdTxtCustomer.Text.IsNullOrEmptyEx())
                message += message.IsNullOrEmptyEx() ? "Customer" : ", Customer";

        }


        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + message;
            return false;
        }
        else return true;
    }

    private string GetSelectedBU()
    {
        string bg = string.Empty;
        foreach (Control control in divBU.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    bg = (control as RadButton).Value;
                    break;
                }
            }
        }
        return bg;
    }


    protected void radGrdSampleItemList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Remove"))
            {
                int index = Convert.ToInt32(e.CommandArgument);

                using (SampleRequestMgr Mgr = new SampleRequestMgr())
                {
                    Mgr.DeleteSampleRequestItemsByIndex(this.hddProcessID.Value, index);
                }
                List<DTO_DOC_CS_SALES_RECOGNITION_PRODUCT> list = (List<DTO_DOC_CS_SALES_RECOGNITION_PRODUCT>)ViewState[VIEWSTATE_CS_SALES_RECOGNITION_PRODUCT_KEY];
                list.RemoveAll(p => p.IDX == index);

                this.radGrdSampleItemList.DataSource = list;
                this.radGrdSampleItemList.DataBind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    protected void radGrdProduct_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;

        }
    }

    private string SaveDocument(ApprovalUtil.ApprovalStatus status)
    {
        DTO_DOC_CS_SALES_RECOGNITION doc = new DTO_DOC_CS_SALES_RECOGNITION();
        doc.PROCESS_ID = this.hddProcessID.Value;

        var strSelectedBU = GetSelectedBU();
        doc.REASON = this.radDdlReason.SelectedValue.Trim();

        doc.SUBJECT = strSelectedBU + " / " + this.radDdlReason.SelectedValue.Trim() + " / " + this.radGrdTxtCustomer.Text;
        webMaster.Subject = doc.SUBJECT;

        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;

        doc.BU = strSelectedBU;
        doc.CUSTOMER_CODE = this.hddCustomerCode.Value;
        doc.CUSTOMER_NAME = this.radGrdTxtCustomer.Text;
        doc.COMMENT = this.RadTextComment.Text;

        List<DTO_DOC_CS_SALES_RECOGNITION_PRODUCT> products ;

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        products = (List<DTO_DOC_CS_SALES_RECOGNITION_PRODUCT>)serializer.Deserialize<List<DTO_DOC_CS_SALES_RECOGNITION_PRODUCT>>(this.hddGridItems.Value);
        foreach (DTO_DOC_CS_SALES_RECOGNITION_PRODUCT product in products)
        {
            product.PROCESS_ID = this.hddProcessID.Value;
            product.CREATOR_ID = Sessions.UserID;
        }

        using (CSSalesRecognitionMgr mgr = new CSSalesRecognitionMgr())
        {
            return mgr.MergeCSSalesRecognition(doc, products);
            
        }

    }

    protected override void DoSave()
    {
        try
        {
            if (ValidationCheck(ApprovalUtil.ApprovalStatus.Saved))
            {
                hddProcessID.Value = SaveDocument(ApprovalUtil.ApprovalStatus.Saved);
                webMaster.ProcessID = this.hddProcessID.Value;
                base.DoSave();
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
                hddProcessID.Value = SaveDocument(hddProcessID.Value.Length > 0 ? ApprovalUtil.ApprovalStatus.Saved : ApprovalUtil.ApprovalStatus.Temp);
                webMaster.ProcessID = hddProcessID.Value;
                base.DoRequest();
            }

        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }

    }
}


