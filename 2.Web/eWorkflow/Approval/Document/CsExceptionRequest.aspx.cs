using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Common.Dto;
using Telerik.Web.UI;
using Bayer.eWF.BSL.Approval.Mgr;
using System.Web.Script.Serialization;

public partial class Approval_Document_CsExceptionRequest : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY = "VIEWSTATE_CS_EXCEPTION_REQUEST_PRODUCT_KEY";

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
                //ViewState[VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY] = new List<DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT>();
                //this.radGrdSampleItemList.DataSource = (List<DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT>)ViewState[VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY];
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
        hddDocumentID.Value = "D0049";
        //hddProcessID.Value = "P000000910";

        //hddProcessStatus.Value = ApprovalUtil.ApprovalStatus.Temp.ToString();

        InitControls();
    }

    private void InitControls()
    {
        if (this.hddProcessID.Value.IsNotNullOrEmptyEx())
        {
            using (CSExceptionrequestMgr mgr = new CSExceptionrequestMgr())
            {
                DTO_DOC_CS_EXCEPTION_REQUEST doc = mgr.SelectCSExceptionrequest(this.hddProcessID.Value);
                string over = string.Empty;
                if (doc.TITLE_CODE == this.radRdoTitle1.Value)
                    this.radRdoTitle1.Checked = true;
                else if (doc.TITLE_CODE == this.radRdoTitle2.Value)
                {
                    this.radRdoTitle2.Checked = true;
                    
                }
                if (doc.BG == this.radRdoBgCP.Value)
                    this.radRdoBgCP.Checked = true;
                else if (doc.BG == this.radRdoBgES.Value)
                {
                    this.radRdoBgES.Checked = true;
                }
                else if (doc.BG == this.radRdoBgIS.Value)
                {
                    this.radRdoBgIS.Checked = true;
                }

                if (doc.DISTRIBUTION_CHANNEL == this.radRdoFM.Value)
                    this.radRdoFM.Checked = true;
                else if (doc.DISTRIBUTION_CHANNEL == this.radRdoNH.Value)
                {
                    this.radRdoNH.Checked = true;

                }
                this.hddCustomerCode.Value = doc.CUSTOMER_CODE;
                this.radGrdTxtCustomer.Text = doc.CUSTOMER_NAME;
                //필드명은 그대로 Purpose로 사용하고 화면상에는 Comment 로 변경한다. 2017-01-04 Youngwoo Lee
                //this.radTxtPurpose.Text = doc.PURPOSE_DESC;
                this.radNumAmount.Value = (double?)doc.ADJUST_AMOUNT;
                webMaster.DocumentNo = doc.DOC_NUM;
                this.radTextSubsidyCommnet.Text = doc.COMMENT;
                this.radDropReason.SelectedValue = doc.EXCEPTION_REASON;
                this.RadTxtTitle.Text = doc.EXCEPTION_TITLE;
                this.RadTextBackground.Text = doc.EXCEPTION_BACKGROUND;
                this.RadTextProposal.Text = doc.EXCEPTION_PROPOSAL;
                this.RadTextProcess.Text = doc.EXCEPTION_PROCESS;
                this.RadTextFinancial.Text = doc.EXCEPTION_FINANCIAL_IMPACK;
                this.RadTextExceptionComment.Text = doc.EXCEPTION_COMMENT;
                List<DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT> products = mgr.SelectCSExceptionrequestProduct(this.hddProcessID.Value);
                ViewState[VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY] = products;

                if (doc.TITLE_CODE == "Exception")
                {
                    foreach (DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT product in products)
                    {
                        AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                        entry.Value = product.PRODUCT_CODE;
                        entry.Text = product.PRODUCT_NAME;
                        this.radAutoProduct.Entries.Add(entry);
                    }
                }
                else
                {
                   
                   
                    this.radGrdSampleItemList.DataSource = products;
                    this.radGrdSampleItemList.DataBind();
                }

                
               
            }
            
        }
        
    }

    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 
        
        if (!ClientScript.IsStartupScriptRegistered("setVisibleControl"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleControl", "setVisibleControl('','" + GetTitleCode() + "','');", true);
        
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
            ViewState[VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY] = new List<DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT>();
            this.radGrdSampleItemList.DataSource = (List<DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT>)ViewState[VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY];
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
        RadioCheck();
        this.hddCustomerCode.Value = "";
        this.hddCustomerType.Value = "";
        this.radGrdTxtCustomer.Text = "";
        ViewState[VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY] = new List<DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT>();
        this.radGrdSampleItemList.DataSource = (List<DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT>)ViewState[VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY];
        this.radGrdSampleItemList.DataBind();
    }
    private void RadioCheck()
    {
        if (radRdoFM.Checked == true)
        {
            foreach (GridColumn column in radGrdSampleItemList.Columns)
            {
                if (column.UniqueName == "VOLUME_RB")
                    (column as GridTemplateColumn).Display = false;
                if (column.UniqueName == "VS_INV")
                    (column as GridTemplateColumn).Display = true;
            }
        }
        else if (radRdoNH.Checked == true)
        {
            foreach (GridColumn column in radGrdSampleItemList.Columns)
            {
                if (column.UniqueName == "VOLUME_RB")
                    (column as GridTemplateColumn).Display = true;
                if (column.UniqueName == "VS_INV")
                    (column as GridTemplateColumn).Display = false;
            }
        }
    }
    #endregion
    // private void UpdateGridData()
    //{
    //    UpdateGridData(999, string.Empty, string.Empty);
    //}

    private void UpdateGridData()
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT> products = (List<DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT>)serializer.Deserialize<List<DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT>>(this.hddGridItems.Value);
        foreach (DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT product in products)
        {
            //product.AMOUNT = product.DISCOUNT_AMOUNT * product.QTY;
        }
        ViewState[VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY] = products;
        this.radGrdSampleItemList.DataSource = products;
        this.radGrdSampleItemList.DataBind();

    }
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            if (radRdoFM.Checked == false && radRdoNH.Checked == false)
                message += "Distribution Channel";
            if(this.RadTxtTitle.Text.Length<=0)
                message += ", Title";
            //if (radRdoTitle2.Checked) { 
            //    if (this.radNumAmount.Text.Length <= 0)
            //        message += message.IsNullOrEmptyEx() ? "Difference Amount" : ", Difference Amount";
            //}


            //Prodcut Check part remove :이정호 님의 요청으로 
            //if (this.radGrdSampleItemList.MasterTableView.Items.Count < 1)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Product";

            // if (this.radTxtPurpose.Text.Length <= 0)
            //     message += message.IsNullOrEmptyEx() ? "Comment" : ", Comment";
        }


        if (message.Length > 0)
        {
            

            this.informationMessage = this.Msgs("0002")[3].ToString() + message;
            return false;
        }
        else return true;
    }

   

    private string GetTitleCode()
    {
        string titleCode = string.Empty;        
        if (this.radRdoTitle1.Checked) titleCode = this.radRdoTitle1.Value;
        else if (this.radRdoTitle2.Checked) titleCode = this.radRdoTitle2.Value;
       
        return titleCode;
    }

    private string GetTitleText()
    {
        string titleText = string.Empty;
        if (this.radRdoTitle1.Checked) titleText = this.radRdoTitle1.Text;
        else if (this.radRdoTitle2.Checked) titleText = this.radRdoTitle2.Text;
        return titleText;
    }
    private string GetChannel()
    {
        string titleText = string.Empty;
        if (this.radRdoFM.Checked) titleText = this.radRdoFM.Text;
        else if (this.radRdoNH.Checked) titleText = this.radRdoNH.Text;
        return titleText;
    }
    private string GetBS()
    {
        string titleText = string.Empty;
        if (this.radRdoBgCP.Checked) titleText = this.radRdoBgCP.Text;
        else if (this.radRdoBgIS.Checked) titleText = this.radRdoBgIS.Value;
        else if (this.radRdoBgES.Checked) titleText = this.radRdoBgES.Text;
        return titleText;
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
                List<DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT> list = (List<DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT>)ViewState[VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY];
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
        DTO_DOC_CS_EXCEPTION_REQUEST doc = new DTO_DOC_CS_EXCEPTION_REQUEST();
        doc.PROCESS_ID = this.hddProcessID.Value;
        //doc.SUBJECT = GetTitleText() + " / CP";
        doc.SUBJECT = this.RadTxtTitle.Text;
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

        doc.TITLE_CODE = GetTitleCode();
        doc.BG = GetBS();
        //webMaster.Subject = doc.SUBJECT + " / " + doc.TITLE_CODE;        
        doc.DISTRIBUTION_CHANNEL = GetChannel();
        doc.NH_CHANNEL = "";

        doc.CUSTOMER_CODE = this.hddCustomerCode.Value;
        doc.CUSTOMER_NAME = this.radGrdTxtCustomer.Text;
        doc.COMMENT = this.radTextSubsidyCommnet.Text;
        doc.EXCEPTION_REASON = this.radDropReason.SelectedText;
        doc.EXCEPTION_TITLE = this.RadTxtTitle.Text;
        
        doc.EXCEPTION_BACKGROUND= this.RadTextBackground.Text;
        doc.EXCEPTION_PROPOSAL = this.RadTextProposal.Text;
        doc.EXCEPTION_PROCESS = this.RadTextProcess.Text;
        doc.EXCEPTION_FINANCIAL_IMPACK = this.RadTextFinancial.Text;
        doc.EXCEPTION_COMMENT = this.RadTextExceptionComment.Text;

        doc.ADJUST_AMOUNT = (decimal?)this.radNumAmount.Value;
        List<DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT> products ;
        if (GetTitleCode() == "Exception")
        {
            products = new List<DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT>();
            int idx_num = 1;
            foreach (AutoCompleteBoxEntry entry in this.radAutoProduct.Entries)
            {
                DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT product = new DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT();

                product.PROCESS_ID = this.hddProcessID.Value;
                product.IDX = idx_num;
                product.PRODUCT_CODE = entry.Value;
                product.PRODUCT_NAME = entry.Text;
                product.CREATOR_ID = Sessions.UserID;
                products.Add(product);
                idx_num++;
            }
        }
        else
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            products = (List<DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT>)serializer.Deserialize<List<DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT>>(this.hddGridItems.Value);
            foreach (DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT product in products)
            {
                product.PROCESS_ID = this.hddProcessID.Value;
                product.CREATOR_ID = Sessions.UserID;
            }
        }
        

        

       
       
        using (CSExceptionrequestMgr mgr = new CSExceptionrequestMgr())
        {
            return mgr.MergeCSExceptionrequest(doc, products);
            
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

