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

public partial class Approval_Document_ARAdjustment : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY = "VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY";

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


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                this.hddCompanyCode.Value = Sessions.CompanyCode; //회사코드 설정
                ViewState[VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY] = new List<DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT>();
                this.radGrdSampleItemList.DataSource = (List<DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT>)ViewState[VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY];
                this.radGrdSampleItemList.DataBind();
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
        hddDocumentID.Value = "D0038";
        //hddProcessID.Value = "P000000910";

        //hddProcessStatus.Value = ApprovalUtil.ApprovalStatus.Temp.ToString();

        InitControls();
    }

    private void InitControls()
    {
        if (this.hddProcessID.Value.IsNotNullOrEmptyEx())
        {
            using (BCSARAdjustmentMgr mgr = new BCSARAdjustmentMgr())
            {
                DTO_DOC_BCS_AR_ADJUSTMENT doc = mgr.SelectBCSARAdjustment(this.hddProcessID.Value);
                string over = string.Empty;


                if (doc.TITLE_CODE == "FRS")
                {
                    this.radRdoTitle1.Checked = true;
                    this.radRdoTitle1.Visible = true;
                }
                else if (doc.TITLE_CODE == "PAR")
                {
                    this.radRdoTitle2.Checked = true;
                    this.radRdoTitle2.Visible = true;
                    divOverLimit.Attributes.Add("style", "display: inline; visibility: visible");
                    if (doc.OVER_LIMIT == this.radChkOverLimit.Value)
                    {
                        this.radChkOverLimit.Checked = true;
                    }
                    else
                    {
                        this.radChkOverLimit.Checked = false;
                    }
                }
                else if (doc.TITLE_CODE == this.radRdoTitle3.Value)
                    this.radRdoTitle3.Checked = true;
                else if (doc.TITLE_CODE == this.radRdoTitle4.Value)
                    this.radRdoTitle4.Checked = true;


                //if (doc.TITLE_CODE == this.radRdoTitle1.Value)
                //    this.radRdoTitle1.Checked = true;
                //else if (doc.TITLE_CODE == this.radRdoTitle2.Value)
                //{
                //    this.radRdoTitle2.Checked = true;
                //    divOverLimit.Attributes.Add("style", "display: inline; visibility: visible");
                //    if (doc.OVER_LIMIT == this.radChkOverLimit.Value)
                //    {
                //        this.radChkOverLimit.Checked = true;
                //    }
                //    else
                //    {
                //        this.radChkOverLimit.Checked = false;
                //    }
                //}



                if (doc.BG == this.radRdoBgCP.Value)
                {
                    this.radRdoBgCP.Checked = true;
                    if (doc.DISTRIBUTION_CHANNEL == this.radRdoDisCha1.Value)
                        this.radRdoDisCha1.Checked = true;
                    else if (doc.DISTRIBUTION_CHANNEL == this.radRdoDisCha2.Value)
                    {
                        this.radRdoDisCha2.Checked = true;
                        foreach (Control control in this.divNH.Controls)
                        {
                            if (control is RadButton)
                            {
                                if ((control as RadButton).Value.Equals(doc.NH_CHANNEL))
                                {
                                    (control as RadButton).Checked = true; break;
                                }
                            }
                        }
                    }
                    
                }
                else if (doc.BG == this.radRdoBgIS.Value)
                    this.radRdoBgIS.Checked = true;
                else if (doc.BG == this.radRdoBgBVS.Value)
                    this.radRdoBgBVS.Checked = true;
                else if (doc.BG == this.radRdoBgES.Value)
                    this.radRdoBgES.Checked = true;

                this.radDdlReason.SelectedValue = doc.REASON;

                foreach (string ReasonCheck in doc.REASON_CHECK.Split(new char[] { ';' }))
                {
                    foreach (Control control in this.divCheck.Controls)
                    {
                        if (control is RadButton)
                        {
                            if ((control as RadButton).Value.Equals(ReasonCheck))
                            {
                                (control as RadButton).Checked = true; break;
                            }
                        }
                    }
                }

                //필드명은 그대로 Purpose로 사용하고 화면상에는 Comment 로 변경한다. 2017-01-04 Youngwoo Lee
                this.radTxtPurpose.Text = doc.PURPOSE_DESC;
                this.radNumAmount.Value = (double?)doc.ADJUST_AMOUNT;
                webMaster.DocumentNo = doc.DOC_NUM;

                //Product
                List<DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT> products = mgr.SelectBCSARAdjustmentProduct(this.hddProcessID.Value);
                ViewState[VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY] = products;

                this.radGrdSampleItemList.DataSource = products;
                this.radGrdSampleItemList.DataBind();



                List<DTO_DOC_BCS_AR_ADJUSTMENT_CUSTOMER> customers = mgr.SelectBCSARAdjustmentCustomer(this.hddProcessID.Value);
                foreach (DTO_DOC_BCS_AR_ADJUSTMENT_CUSTOMER customer in customers)
                {
                    AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                    entry.Value = customer.CUSTOMER_CODE;
                    entry.Text = customer.CUSTOMER_NAME;
                    this.radAcomWholesaler.Entries.Add(entry);
                }

                if (!(doc.PROCESS_STATUS.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || doc.PROCESS_STATUS.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString())))
                {
                    //if (products.Count < 1)
                    //{
                    //    this.radAcomProduct.Visible = false;
                    //    this.lblNotProduct.Visible = true;
                    //}

                    if (customers.Count < 1)
                    {
                        this.radAcomWholesaler.Visible = false;
                        this.lblNotCustomer.Visible = true;
                    }
                }
                if (!ClientScript.IsStartupScriptRegistered("setVisibleControl"))
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleControl", "setVisibleControl('" + GetSelectedBG() + "','" + GetTitleCode() + "','" + doc.DISTRIBUTION_CHANNEL +"');", true);
            }
            
        }
        
    }

    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 
        
        if (!ClientScript.IsStartupScriptRegistered("setVisibleControl"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleControl", "setVisibleControl('" + GetSelectedBG() + "','" + GetTitleCode() + "','" + hddNH.Value + "');", true);

        if (this.hddGridItems.Value != "")
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT> products = (List<DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT>)serializer.Deserialize<List<DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT>>(this.hddGridItems.Value);

            ViewState[VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY] = products;
            this.radGrdSampleItemList.DataSource = products;
            this.radGrdSampleItemList.DataBind();

        }


        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdSampleItemList);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_SampleRequest_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";
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

                UpdateGridData(Convert.ToInt32(idx), code, name);
            }
        }
    }


    private void UpdateGridData()
    {
        UpdateGridData(999, string.Empty, string.Empty);
    }

    private void UpdateGridData(int idx, string code, string name)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT> products = (List<DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT>)serializer.Deserialize<List<DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT>>(this.hddGridItems.Value);

        ViewState[VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY] = products;
        this.radGrdSampleItemList.DataSource = products;
        this.radGrdSampleItemList.DataBind();

    }
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            if (radRdoTitle1.Checked == false && radRdoTitle2.Checked == false && radRdoTitle3.Checked == false && radRdoTitle4.Checked == false)
                message += "Title"; 

            if (this.GetSelectedBG().IsNullOrEmptyEx())
                message += message.IsNullOrEmptyEx() ? "BG" : ", BG"; 

            if (this.radNumAmount.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Adjustment Amount" : ", Adjustment Amount";

            if (this.radRdoTitle2.Checked)
            {
                if (this.radDdlReason.SelectedValue.IsNullOrEmptyEx())
                    message += message.IsNullOrEmptyEx() ? "Reason" : ", Reason";
            }

            //Prodcut Check part remove :이정호 님의 요청으로 
            //if (this.radGrdSampleItemList.MasterTableView.Items.Count < 1)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Product";

            if (this.radTxtPurpose.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Comment" : ", Comment";
        }


        if (message.Length > 0)
        {
            if (this.radRdoTitle2.Checked)
            {
                this.divOverLimit.Attributes.Add("style", "display: inline; visibility: visible");
                this.divLink.Attributes.Add("style", "display: block; visibility: visible");
            }
            else
            {
                this.divOverLimit.Attributes.Add("style", "display: inline; visibility: none");
                this.divLink.Attributes.Add("style", "display: block; visibility: none");
            }

            this.informationMessage = this.Msgs("0002")[3].ToString() + message;
            return false;
        }
        else return true;
    }

    private string GetSelectedBG()
    {
        string bg = string.Empty;
        foreach (Control control in divBG.Controls)
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

    private string GetSelectedNH()
    {
        string NH = string.Empty;
        foreach (Control control in divNH.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    NH = (control as RadButton).Value;
                    break;
                }
            }
        }
        return NH;
    }

    private string GetTitleCode()
    {
        string titleCode = string.Empty;        
        if (this.radRdoTitle1.Checked) titleCode = this.radRdoTitle1.Value;
        else if (this.radRdoTitle2.Checked) titleCode = this.radRdoTitle2.Value;
        else if (this.radRdoTitle3.Checked) titleCode = this.radRdoTitle3.Value;
        else if (this.radRdoTitle4.Checked) titleCode = this.radRdoTitle4.Value;
        return titleCode;
    }

    private string GetTitleText()
    {
        string titleText = string.Empty;
        if (this.radRdoTitle1.Checked) titleText = this.radRdoTitle1.Text;
        else if (this.radRdoTitle2.Checked) titleText = this.radRdoTitle2.Text;
        else if (this.radRdoTitle3.Checked) titleText = this.radRdoTitle3.Text;
        else if (this.radRdoTitle4.Checked) titleText = this.radRdoTitle4.Text;
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
                List<DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT> list = (List<DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT>)ViewState[VIEWSTATE_BCS_AR_ADJUSTMENT_PRODUCT_KEY];
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

    private string SaveDocument(ApprovalUtil.ApprovalStatus status)
    {
        DTO_DOC_BCS_AR_ADJUSTMENT doc = new DTO_DOC_BCS_AR_ADJUSTMENT();
        doc.PROCESS_ID = this.hddProcessID.Value;
        doc.SUBJECT = GetTitleText() + " / " + GetSelectedBG();
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

        string channel = string.Empty;
        string NHchannel = string.Empty;
        doc.TITLE_CODE = GetTitleCode();
        doc.BG = GetSelectedBG();
        if (this.radRdoBgCP.Checked)
        {
            if (this.radRdoDisCha1.Checked) 
            {
                channel = radRdoDisCha1.Value;
                NHchannel = null;
            }
            else if (this.radRdoDisCha2.Checked)
            {
                channel = radRdoDisCha2.Value;
                NHchannel = GetSelectedNH();
            }

            webMaster.Subject = doc.SUBJECT + " / " + channel;
        }
        doc.DISTRIBUTION_CHANNEL = channel;
        doc.NH_CHANNEL = NHchannel;
        doc.ADJUST_AMOUNT = (decimal?)this.radNumAmount.Value;
        if (this.radChkOverLimit.Checked) doc.OVER_LIMIT = this.radChkOverLimit.Value;
        else doc.OVER_LIMIT = "N";
        
        doc.REASON = this.radDdlReason.SelectedValue;

        string ReasonCheck = string.Empty;
        foreach (Control control in this.divCheck.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                    ReasonCheck += (control as RadButton).Value + ";";
            }
        }
        doc.REASON_CHECK = ReasonCheck;

        doc.PURPOSE_DESC = this.radTxtPurpose.Text;


        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT> products = (List<DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT>)serializer.Deserialize<List<DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT>>(this.hddGridItems.Value);
        foreach (DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT product in products)
        {
            product.PROCESS_ID = this.hddProcessID.Value;
            product.CREATOR_ID = Sessions.UserID;
        }

        //List<DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT> products = new List<DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT>();
        //foreach (AutoCompleteBoxEntry entry in this.radAcomProduct.Entries)
        //{
        //    DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT product = new DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT();
        //    product.PROCESS_ID = this.hddProcessID.Value;
        //    product.PRODUCT_CODE = entry.Value;
        //    product.PRODUCT_NAME = entry.Text;
        //    product.CREATOR_ID = Sessions.UserID;
        //    products.Add(product);
        //}

        List<DTO_DOC_BCS_AR_ADJUSTMENT_CUSTOMER> customers = new List<DTO_DOC_BCS_AR_ADJUSTMENT_CUSTOMER>();
        foreach (AutoCompleteBoxEntry entry in this.radAcomWholesaler.Entries)
        {
            DTO_DOC_BCS_AR_ADJUSTMENT_CUSTOMER customer = new DTO_DOC_BCS_AR_ADJUSTMENT_CUSTOMER();
            customer.PROCESS_ID = this.hddProcessID.Value;
            customer.CUSTOMER_CODE = entry.Value;
            customer.CUSTOMER_NAME = entry.Text;
            customer.CREATOR_ID = Sessions.UserID;
            customers.Add(customer);
        }

        using (BCSARAdjustmentMgr mgr = new BCSARAdjustmentMgr())
        {
            return mgr.MergeBCSARAdjustment(doc, products, customers);
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

