using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Approval.Dao;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using Bayer.eWF.BSL.Common.Dto;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;

public partial class Approval_Document_FreeGoods : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_KEY = "VIEWSTATE_KEY_FREE_GOODS";
    private const string VIEWSTATE_KEY_Customer = "VIEWSTATE_KEY_FREE_GOODS_Customer";
    
    string Sample;
    int Dropindex;
    string RadioCheck;
    string Location;

    [Serializable]
    public class Receipient 
    {
        public string APPROVER_ID { get; set; }
    }

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

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {                

                ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_FREE_GOODS_INFO>();
                this.RadGrdSampleInfo.DataSource = (List<DTO_DOC_FREE_GOODS_INFO>)ViewState[VIEWSTATE_KEY];
                this.RadGrdSampleInfo.DataBind();

                ViewState[VIEWSTATE_KEY_Customer] = new List<DTO_DOC_FREE_GOODS_CUSTOMER>();
                this.RadGrdCustomer.DataSource = (List<DTO_DOC_FREE_GOODS_CUSTOMER>)ViewState[VIEWSTATE_KEY_Customer];
                this.RadGrdCustomer.DataBind();

                InitPageInfo();
            }
            AfterProcess();            
            PageLoadInfo();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    private void AfterProcess()
    {        
        using (FreeGoodsMgr mgr = new FreeGoodsMgr())
        {
            List<DTO_DOC_FREE_GOODS_INFO> infos = mgr.SelectFreeGoodsInfo(this.hddProcessID.Value);

            this.RadGrdSampleInfo.DataSource = infos;
            this.RadGrdSampleInfo.DataBind();

            List<DTO_DOC_FREE_GOODS_CUSTOMER> Customers = mgr.SelectFreeGoodsCustomer(this.hddProcessID.Value);            

            this.RadGrdCustomer.DataSource = Customers;
            this.RadGrdCustomer.DataBind();   
        }
    }   
    #endregion

    #region PreRender
    protected override void OnPreRender(EventArgs e)
    {
        if (!webMaster.DocumentNo.Equals(string.Empty))
        {
            //webMaster.SetEnableControls(Recipt, true);

            HtmlInputHidden requestid = this.webMaster.FindControl("hddRequester") as HtmlInputHidden;
            HtmlInputHidden recipients = this.webMaster.FindControl("hddRecipients") as HtmlInputHidden;
            this.hddRequestId.Value = requestid.Value;
            this.hddReceipientId.Value = recipients.Value;

            using (FreeGoodsMgr mgr = new FreeGoodsMgr())
            {
                List<DTO_DOC_FREE_GOODS_INFO> infos = mgr.SelectFreeGoodsInfo(this.hddProcessID.Value);

                this.RadGrdSampleInfo.DataSource = infos;
                this.RadGrdSampleInfo.DataBind();

                List<DTO_DOC_FREE_GOODS_CUSTOMER> Customers = mgr.SelectFreeGoodsCustomer(this.hddProcessID.Value);

                this.RadGrdCustomer.DataSource = Customers;
                this.RadGrdCustomer.DataBind();
            }
        }
        base.OnPreRender(e);
    } 
    #endregion

    #region InitPageInfo
    private void InitPageInfo()
    {
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0030";
        //hddProcessID.Value = "";

        InitControls();
    }

    // Select 
    private void InitControls()
    {
        DTO_DOC_FREE_GOODS doc;        
        if (!hddProcessID.Value.Equals(string.Empty))
        {
            using (FreeGoodsMgr mgr = new FreeGoodsMgr())
            {
                doc = mgr.SelectFreeGoods(hddProcessID.Value);
                if (doc != null)
                {
                    this.hddProcessStatus.Value = doc.PROCESS_STATUS;

                    //BU
                    if (doc.BU == "CH")
                    {
                        //radRdoBuCH.Checked = true;
                        radRdoBuCH.Visible = true;

                        //radBtnCC.Checked = false;
                        radRdoBuCC.Visible = false;
                    }
                    else if (doc.BU == "CC")
                    {
                        //radRdoBuCH.Checked = false;
                        radRdoBuCH.Visible = false;

                        //radBtnCC.Checked = true;
                        radRdoBuCC.Visible = true;
                    }
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
                    //Location
                    foreach (Control control in this.divLocation.Controls)
                    {
                        if (control is RadButton)
                        {
                            if ((control as RadButton).Value.Equals(doc.LOCATION))
                            {
                                (control as RadButton).Checked = true; break;
                            }
                        }
                    }
                    // title , purpose
                    if (this.radRdoSample.Value == doc.TITLE_CODE)
                    {
                        this.radRdoSample.Checked = true;
                        this.RadDropSample.SelectedValue = doc.PURPOSE_CODE;
                    }
                    else if (this.radRdoOther.Value == doc.TITLE_CODE)
                    {
                        this.radRdoOther.Checked = true;
                        this.RadDropOther.SelectedValue = doc.PURPOSE_CODE;
                    }
                    this.rxtTxtDBoxCode.Text = doc.D_BOX;
                    this.rxtTxtPO_NO.Text = doc.PO_NO;
                    this.radTextComment.Text = doc.COMMENT;
                   
                    // 컨트롤 비활성화 
                    ApprovalUtil.ApprovalStatus status = (ApprovalUtil.ApprovalStatus)Enum.Parse(typeof(ApprovalUtil.ApprovalStatus), hddProcessStatus.Value);


                    if (doc.TITLE_CODE == "Sample" && doc.PURPOSE_CODE != "Sample03")
                    {
                        List<DTO_DOC_FREE_GOODS_INFO> infos = mgr.SelectFreeGoodsInfo(this.hddProcessID.Value);
                        ViewState[VIEWSTATE_KEY] = infos;

                        this.RadGrdSampleInfo.DataSource = infos;
                        this.RadGrdSampleInfo.DataBind();
                        // Delete 버튼 삭제                       
                    }
                    else if (doc.TITLE_CODE == "Other" || doc.PURPOSE_CODE == "Sample03")
                    {
                        List<DTO_DOC_FREE_GOODS_CUSTOMER> Customers = mgr.SelectFreeGoodsCustomer(this.hddProcessID.Value);
                        ViewState[VIEWSTATE_KEY_Customer] = Customers;

                        this.RadGrdCustomer.DataSource = Customers;
                        this.RadGrdCustomer.DataBind();                        
                    }
                    // Control 상태 유지
                    if (doc.BU == this.radRdoBuAH.Value) Location = "AH"; else Location = "else";
                    if (doc.TITLE_CODE == this.radRdoSample.Value)
                    {
                        Sample = "Sample";
                        RadioCheck = "info";
                        Dropindex = RadDropSample.SelectedIndex;
                    }
                    else if (doc.TITLE_CODE == this.radRdoOther.Value)
                    {
                        Sample = "Other";
                        RadioCheck = "customer";
                        Dropindex = RadDropOther.SelectedIndex;
                    }                   

                    webMaster.DocumentNo = doc.DOC_NUM;   // doc Number                    
                    // 후처리 CANCEL
                    if (doc.DOC_NUM != null)
                    {                        
                        foreach (GridColumn column in this.RadGrdSampleInfo.Columns)
                        {
                            if (column.UniqueName == "CANCEL01")
                                (column as GridTemplateColumn).Display = true;
                        }
                        foreach (GridColumn column in this.RadGrdCustomer.Columns)
                        {
                            if (column.UniqueName == "CANCEL02")
                                (column as GridTemplateColumn).Display = true;
                        }
                    }
                    if (doc.PROCESS_STATUS.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || doc.PROCESS_STATUS.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString()) || doc.PROCESS_STATUS.Equals(ApprovalUtil.ApprovalStatus.Request.ToString())) 
                    {
                        foreach (GridColumn column in this.RadGrdSampleInfo.Columns)
                        {
                            if (column.UniqueName == "CANCEL01")
                                (column as GridTemplateColumn).Display = false;
                        }
                        foreach (GridColumn column in this.RadGrdCustomer.Columns)
                        {
                            if (column.UniqueName == "CANCEL02")
                                (column as GridTemplateColumn).Display = false;
                        }
                    }

                    if (!ClientScript.IsStartupScriptRegistered("SetVisible"))
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "SetVisible", "SetVisible('" + Location + "','" + Sample + "','" + Dropindex + "','" + RadioCheck + "');", true);

                                      
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
            List<DTO_DOC_FREE_GOODS_INFO> items = (List<DTO_DOC_FREE_GOODS_INFO>)serializer.Deserialize<List<DTO_DOC_FREE_GOODS_INFO>>(this.hddGridItems.Value);

            ViewState[VIEWSTATE_KEY] = items;
            this.RadGrdSampleInfo.DataSource = items;
            this.RadGrdSampleInfo.DataBind();
        }
        if (this.hddGridItemsCustomer.Value != "")
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<DTO_DOC_FREE_GOODS_CUSTOMER> items = (List<DTO_DOC_FREE_GOODS_CUSTOMER>)serializer.Deserialize<List<DTO_DOC_FREE_GOODS_CUSTOMER>>(this.hddGridItemsCustomer.Value);

            ViewState[VIEWSTATE_KEY_Customer] = items;
            this.RadGrdCustomer.DataSource = items;
            this.RadGrdCustomer.DataBind();
        }


        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadbtnAdd, RadGrdCustomer, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadDropSample, RadGrdSampleInfo, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadDropOther, RadGrdCustomer, null);        
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.RadGrdSampleInfo);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.RadGrdCustomer);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_FreeGoods_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";

        // Control 상태 유지
        if (this.radRdoBuAH.Checked == true) Location = "AH"; else Location = "else";
        if (this.radRdoSample.Checked == true)
        {
            Sample = "Sample";
            RadioCheck = "info";
            Dropindex = RadDropSample.SelectedIndex;
        }
        else if (this.radRdoOther.Checked == true)
        {
            Sample = "Other";
            RadioCheck = "customer";
            Dropindex = RadDropOther.SelectedIndex;
        }

        //Sample, Dropindex, RadioCheck  
        if (!ClientScript.IsStartupScriptRegistered("SetVisible"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "SetVisible", "SetVisible('" + Location + "','" + Sample + "','" + Dropindex + "','" + RadioCheck + "');", true);
    }

    // Ajax
    private void Approval_Document_FreeGoods_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("Rebind"))
        {
            UpdateGridData(false,0,null,null,null);
        }
        if (e.Argument.StartsWith("ApplySample"))
        {
            string[] values = e.Argument.Split(new char[] { ':' });
            if (values.Length > 4)
            {
                string idx = values[1];
                string code = values[2];
                string name = values[3];
                string samplecode = values[4];

                UpdateGridData(false, Convert.ToInt32(idx), code, name, samplecode);
            }
        }
        if (e.Argument.StartsWith("ApplyCustomer"))
        {
            string[] values = e.Argument.Split(new char[] { ':' });
            if (values.Length > 4)
            {
                string idx = values[1];
                string code = values[2];
                string name = values[3];
                string samplecode = values[4];

                UpdateCustomer(false, Convert.ToInt32(idx), code, name, samplecode);
            }
        }
    }
    #endregion

    #region DoSave
    protected override void DoSave()
    {
        try
        {
            if (ValidationCheck(ApprovalUtil.ApprovalStatus.Saved))
            {
                hddProcessID.Value = SaveDocument(ApprovalUtil.ApprovalStatus.Saved);
                webMaster.ProcessID = hddProcessID.Value;
                base.DoSave();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    #endregion

    #region DoRequest
    protected override void DoRequest()
    {
        try
        {
            if (ValidationCheck(ApprovalUtil.ApprovalStatus.Request))
            {
                hddProcessID.Value = SaveDocument(ApprovalUtil.ApprovalStatus.Request);
                webMaster.ProcessID = hddProcessID.Value;
                base.DoRequest();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    #endregion

    #region Radio Select
    private string GetSelectedBu()
    {
        string Bu = string.Empty;
        foreach (Control control in divBU.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    Bu = (control as RadButton).Value;
                    break;
                }
            }
        }
        return Bu;
    }

    private string GetSelectLocation()
    {
        string Location = string.Empty;
        foreach (Control control in divLocation.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    Location = (control as RadButton).Value;
                    break;
                }
            }
        }
        return Location;
    } 
    #endregion

    private string SaveDocument(ApprovalUtil.ApprovalStatus approvalStatus)
    {
        DTO_DOC_FREE_GOODS doc = new DTO_DOC_FREE_GOODS();

        string processID = string.Empty;

        string SelectedBu = GetSelectedBu();
        string SelectedLocation = GetSelectLocation();

        doc.PROCESS_ID = this.hddProcessID.Value;
        
        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = approvalStatus.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;

        doc.BU = SelectedBu;
        doc.LOCATION = SelectedLocation;

        if (this.radRdoSample.Checked == true)
        {
            doc.TITLE_CODE = radRdoSample.Value;
            doc.PURPOSE_CODE = this.RadDropSample.SelectedValue;

            doc.SUBJECT = SelectedBu + "/" + radRdoSample.Text + "/" + this.RadDropSample.SelectedText;       // BU + Title + Purpose
            webMaster.Subject = doc.SUBJECT;
        }
        if (this.radRdoOther.Checked == true)
        {
            doc.TITLE_CODE = radRdoOther.Value;
            doc.PURPOSE_CODE = this.RadDropOther.SelectedValue;

            doc.SUBJECT = SelectedBu + "/" + radRdoOther.Text + "/" + this.RadDropOther.SelectedText;       // BU  + Purpose
            webMaster.Subject = doc.SUBJECT;
        }
        doc.D_BOX = this.rxtTxtDBoxCode.Text;
        doc.COMMENT = this.radTextComment.Text;
        doc.PO_NO = this.rxtTxtPO_NO.Text;

        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;
        // info Grid
        if (this.hddGridSelect.Value == "Gridinfo")
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<DTO_DOC_FREE_GOODS_INFO> infos = (List<DTO_DOC_FREE_GOODS_INFO>)serializer.Deserialize<List<DTO_DOC_FREE_GOODS_INFO>>(this.hddGridItems.Value);
            foreach (DTO_DOC_FREE_GOODS_INFO item in infos)
            {
                item.PROCESS_ID = this.hddProcessID.Value;
                item.CREATOR_ID = Sessions.UserID;
            }

            using (FreeGoodsMgr mgr = new FreeGoodsMgr())
            {
                processID =  mgr.MergeFreeGoodsInfo(doc, infos);
            }
        }

        // Customer Grid
        if (this.hddGridSelect.Value == "GridCustomer")
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<DTO_DOC_FREE_GOODS_CUSTOMER> customeritems = (List<DTO_DOC_FREE_GOODS_CUSTOMER>)serializer.Deserialize<List<DTO_DOC_FREE_GOODS_CUSTOMER>>(this.hddGridItemsCustomer.Value);

            foreach (DTO_DOC_FREE_GOODS_CUSTOMER item in customeritems)
            {
                item.PROCESS_ID = this.hddProcessID.Value;
                item.CREATOR_ID = Sessions.UserID;
            }

            using (FreeGoodsMgr mgr = new FreeGoodsMgr())
            {
                processID = mgr.MergeFreeGoodsCustomer(doc, customeritems);
            }
        }

        return processID;
    }

   

    #region ValidationCheck
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus approvalStatus)
    {
        string message = string.Empty;        

        if (approvalStatus == ApprovalUtil.ApprovalStatus.Request)
        {
            if (GetSelectedBu().IsNullOrEmptyEx())
                message = "BU";
            if (GetSelectedBu() != "AH" && GetSelectLocation().IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Location";

            if (radRdoSample.Checked == false && radRdoOther.Checked == false)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Title";

            if (radRdoSample.Checked == true && this.RadDropSample.SelectedIndex == -1)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose";

            if (radRdoOther.Checked == true && this.RadDropOther.SelectedIndex == -1)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose";

            if (radRdoSample.Checked == true && this.RadDropSample.SelectedIndex == 2 && this.rxtTxtDBoxCode.Text.Length <= 0)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "IMPACT no.";       
        }

        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        //----------------------------------
        // 중복 체크 
        // Duplicate Table에 UserID로 데이터 삭제 후 
        // info 그리드의 data를 임시 Duplicate table에 insert 해준다.
        //----------------------------------
        if (this.hddGridSelect.Value == "Gridinfo")
        {
            string SelectedBu = GetSelectedBu();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<DTO_DOC_FREE_GOODS_INFO> infos = (List<DTO_DOC_FREE_GOODS_INFO>)serializer.Deserialize<List<DTO_DOC_FREE_GOODS_INFO>>(this.hddGridItems.Value);

            List<DTO_DOC_FREE_GOODS_CHECK_DUPLICATE> duplicates = new List<DTO_DOC_FREE_GOODS_CHECK_DUPLICATE>();

            using (FreeGoodsDao dao = new FreeGoodsDao())
            {
                dao.DeleteFreeGoodsCheckDuplicate(this.Sessions.UserID);
            }
            foreach (DTO_DOC_FREE_GOODS_INFO item in infos)
            {
                DTO_DOC_FREE_GOODS_CHECK_DUPLICATE duplicate = new DTO_DOC_FREE_GOODS_CHECK_DUPLICATE();
                duplicate.USER_ID = this.Sessions.UserID;
                duplicate.PURPOSE_CODE = this.RadDropSample.SelectedValue;
                duplicate.BU = SelectedBu;
                duplicate.INSTITUE_CODE = item.INSTITUE_CODE;
                duplicate.INSTITUE_NAME = item.INSTITUE_NAME;
                duplicate.HCP_CODE = item.HCP_CODE;
                duplicate.HCP_NAME = item.HCP_NAME;

                if (this.RadDropSample.SelectedValue == "DC_U")
                {
                    duplicate.SAP_PRODUCT_CODE = item.SAMPLE_CODE;             // 2015-06-05(Bayer : Youngwoo Lee)
                }
                else
                {
                    duplicate.SAP_PRODUCT_CODE = item.SAP_PRODUCT_CODE;   //
                }

                duplicates.Add(duplicate);
            }
            duplicates.ToList();

            using (FreeGoodsMgr mgr = new FreeGoodsMgr())
            {
                mgr.InsertFreeGoodsCheckDuplicate(duplicates);
                // 중복 조회
                List<DTO_DOC_FREE_GOODS_CHECK_DUPLICATE> check = mgr.SelectFreeGoodsCheckDuplicate(this.Sessions.UserID, RadDropSample.SelectedValue, SelectedBu);

                if (check.Count > 0)
                {
                    string checkMessage = string.Empty;

                    if (RadDropSample.SelectedValue == "DC_U")
                    {
                        for (int i = 0; i < check.Count; i++)
                        {
                            checkMessage += "Institue : " + check[i].INSTITUE_NAME;
                        }
                    }
                    else if (RadDropSample.SelectedValue == "DOSAGE" && SelectedBu != "CC")
                    {
                        for (int i = 0; i < check.Count; i++)
                        {
                            checkMessage += (checkMessage.IsNullOrEmptyEx() ? "" : " ,") + check[i].HCP_NAME;
                        }
                    }
                    else if (RadDropSample.SelectedValue == "DOSAGE" && SelectedBu == "CC")
                    {
                        for (int i = 0; i < check.Count; i++)
                        {
                            checkMessage += (checkMessage.IsNullOrEmptyEx() ? "" : " ,") + check[i].INSTITUE_NAME;
                            checkMessage += (checkMessage.IsNullOrEmptyEx() ? "" : " ,") + check[i].HCP_NAME;
                        }
                    }                   
                    this.informationMessage = "중복된 데이터 : " + checkMessage;
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        else
            return true;
        
    }
    #endregion

    #region ItemDataBound

    protected void RadGrdSampleInfo_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {            
            GridDataItem item = (e.Item as GridDataItem);
            RadButton btnTxt = item["CANCEL01"].FindControl("RadbtnCancel01") as RadButton;

            if (item["STATES"].Text.Trim().Replace("&nbsp;", "") == string.Empty) 
            {
                if (!hddReceipientId.Value.Equals(""))
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    List<Receipient> Lists = serializer.Deserialize<List<Receipient>>(hddReceipientId.Value);
                    for (int i = 0; i < Lists.Count; i++)
                    {
                        if (this.Sessions.UserID == Lists[i].APPROVER_ID)
                        {
                            btnTxt.Enabled = false;
                        }
                    }
                }
            }

            if (item["STATES"].Text.Trim() == "C")
            {              
                btnTxt.Text = "CI";
                if (this.Sessions.UserID == this.hddRequestId.Value)
                {
                    btnTxt.Enabled = false;
                }
            }

            if (item["STATES"].Text.Trim() == "CD")
            {
                if (this.Sessions.UserID == this.hddRequestId.Value)
                {
                    btnTxt.Enabled = false;
                }
                btnTxt.Text = "CD";
                if (!hddReceipientId.Value.Equals(""))
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    List<Receipient> Lists = serializer.Deserialize<List<Receipient>>(hddReceipientId.Value);
                    for (int i = 0; i < Lists.Count; i++)
                    {
                        if (this.Sessions.UserID == Lists[i].APPROVER_ID)
                        {
                            btnTxt.Enabled = false;
                        }
                    }
                }
            }
        }
    }
    protected void RadGrdCustomer_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (e.Item as GridDataItem);
            RadButton btnTxt = item["CANCEL02"].FindControl("RadbtnCancel02") as RadButton;

            if (item["STATES"].Text.Trim().Replace("&nbsp;", "") == string.Empty)
            {
                if (!hddReceipientId.Value.Equals(""))
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    List<Receipient> Lists = serializer.Deserialize<List<Receipient>>(hddReceipientId.Value);
                    for (int i = 0; i < Lists.Count; i++)
                    {
                        if (this.Sessions.UserID == Lists[i].APPROVER_ID)
                        {
                            btnTxt.Enabled = false;
                        }
                    }
                }
            }

            if (item["STATES"].Text.Trim() == "C")
            {
                btnTxt.Text = "CI";
                if (this.Sessions.UserID == this.hddRequestId.Value)
                {
                    btnTxt.Enabled = false;
                }
            }

            if (item["STATES"].Text.Trim() == "CD")
            {
                if (this.Sessions.UserID == this.hddRequestId.Value)
                {
                    btnTxt.Enabled = false;
                }
                btnTxt.Text = "CD";
                if (!hddReceipientId.Value.Equals(""))
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    List<Receipient> Lists = serializer.Deserialize<List<Receipient>>(hddReceipientId.Value);
                    for (int i = 0; i < Lists.Count; i++)
                    {
                        if (this.Sessions.UserID == Lists[i].APPROVER_ID)
                        {
                            btnTxt.Enabled = false;
                        }
                    }
                }
            }
        }
    }
    
    #endregion

    #region Grid Event
    private void UpdateGridData(bool addRow, int idx, string code, string name, string samplecode)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_FREE_GOODS_INFO> infos = (List<DTO_DOC_FREE_GOODS_INFO>)serializer.Deserialize<List<DTO_DOC_FREE_GOODS_INFO>>(this.hddGridItems.Value);

        if (addRow)
        {
            var lastData = infos.OrderByDescending(d => d.IDX).FirstOrDefault();
            int newIdx = 1;
            if (lastData != null)
            {
                newIdx = Convert.ToInt32(lastData.IDX);
                newIdx++;
            }

            if (infos == null) infos = new List<DTO_DOC_FREE_GOODS_INFO>();
            DTO_DOC_FREE_GOODS_INFO item = new DTO_DOC_FREE_GOODS_INFO();
            item.IDX = newIdx;
            item.QTY = 1;                       // Quantity : 1            
            infos.Add(item);
        }

        if (idx < 999)
        {
            var itemSample = (from item in infos
                               where item.IDX == idx
                               select item).FirstOrDefault();

            if (itemSample != null)
            {
                itemSample.SAMPLE_CODE = code;
                itemSample.SAMPLE_NAME = name;
                itemSample.SAP_PRODUCT_CODE = samplecode;  //마스터 테이블의 SAMPLE_CODE
            }
        }



        ViewState[VIEWSTATE_KEY] = infos;
        this.RadGrdSampleInfo.DataSource = infos;
        this.RadGrdSampleInfo.DataBind();
    }
    #endregion

    #region Grid Delete (한줄 지우기)
    //info
    protected void RadGrdSampleInfo_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Remove"))
            {
                int index = Convert.ToInt32(e.CommandArgument);

                using (FreeGoodsDao Dao = new FreeGoodsDao())
                {
                    Dao.DeleteFreeGoodsInfo(this.hddProcessID.Value, index);
                }
                List<DTO_DOC_FREE_GOODS_INFO> list = (List<DTO_DOC_FREE_GOODS_INFO>)ViewState[VIEWSTATE_KEY];
                list.RemoveAll(p => p.IDX == index);

                this.RadGrdSampleInfo.DataSource = list;
                this.RadGrdSampleInfo.DataBind();
            }
            
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    //customer
    protected void RadGrdCustomer_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Remove"))
            {
                int index = Convert.ToInt32(e.CommandArgument);

                using (FreeGoodsDao Dao = new FreeGoodsDao())
                {
                    Dao.DeleteFreeGoodsCustomer(this.hddProcessID.Value, index);
                }
                List<DTO_DOC_FREE_GOODS_CUSTOMER> list = (List<DTO_DOC_FREE_GOODS_CUSTOMER>)ViewState[VIEWSTATE_KEY_Customer];
                list.RemoveAll(p => p.IDX == index);

                this.RadGrdCustomer.DataSource = list;
                this.RadGrdCustomer.DataBind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    #endregion

    protected void RadButton2_Click(object sender, EventArgs e)
    {
        UpdateCustomer(true);
    }

    private void UpdateCustomer(bool addRow)
    {
        UpdateCustomer(addRow, 999, string.Empty, string.Empty,string.Empty);
    }

    private void UpdateCustomer(bool addRow, int idx, string code, string name,string samplecode )
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_FREE_GOODS_CUSTOMER> customer = (List<DTO_DOC_FREE_GOODS_CUSTOMER>)serializer.Deserialize<List<DTO_DOC_FREE_GOODS_CUSTOMER>>(this.hddGridItemsCustomer.Value);

        if (addRow)
        {
            var lastData = customer.OrderByDescending(d => d.IDX).FirstOrDefault();
            int newIdx = 1;
            if (lastData != null)
            {
                newIdx = Convert.ToInt32(lastData.IDX);
                newIdx++;
            }

            if (customer == null) customer = new List<DTO_DOC_FREE_GOODS_CUSTOMER>();
            DTO_DOC_FREE_GOODS_CUSTOMER item = new DTO_DOC_FREE_GOODS_CUSTOMER();
            item.IDX = newIdx;
            customer.Add(item);
        }
        
        if (idx < 999)
        {
            var itemSample = (from item in customer
                              where item.IDX == idx
                              select item).FirstOrDefault();

            if (itemSample != null)
            {
                itemSample.SAMPLE_CODE = code;
                itemSample.SAMPLE_NAME = name;
                itemSample.SAP_PRODUCT_CODE = samplecode;
            }
        }

        ViewState[VIEWSTATE_KEY_Customer] = customer;
        this.RadGrdCustomer.DataSource = customer;
        this.RadGrdCustomer.DataBind();
    }       
    
  
    protected void RadDropOther_ItemSelected(object sender, DropDownListEventArgs e)
    {        
        GridReset();
    }

    private void GridReset()
    {
        ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_FREE_GOODS_INFO>();
        this.RadGrdSampleInfo.DataSource = (List<DTO_DOC_FREE_GOODS_INFO>)ViewState[VIEWSTATE_KEY];
        this.RadGrdSampleInfo.DataBind();
        

        ViewState[VIEWSTATE_KEY_Customer] = new List<DTO_DOC_FREE_GOODS_CUSTOMER>();
        this.RadGrdCustomer.DataSource = (List<DTO_DOC_FREE_GOODS_CUSTOMER>)ViewState[VIEWSTATE_KEY_Customer];
        this.RadGrdCustomer.DataBind();
        
    }


    
}