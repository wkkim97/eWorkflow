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


public partial class Approval_Document_SampleRequest : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_SAMPLE_REQUEST_ITEMS_KEY = "VIEWSTATE_SAMPLE_REQUEST_ITEMS_KEY";
    private static List<DTO_CODE_SUB> location = null;

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
                if (Sessions.OrgName.ToString() == "BCSKR-INDS")
                {
                    this.radBtnIS.Enabled = true;
                    this.radBtnCP.Enabled = false;
                    this.radBtnBVS.Enabled = false;
                    this.radBtnES.Enabled = false;

                    //this.radGrdSampleItemList.MasterTableView.GetColumn("PRICE").Display = false;
                }
                else
                {
                    this.radBtnIS.Enabled = false;
                    this.radBtnCP.Enabled = true;
                    this.radBtnBVS.Enabled = true;
                    this.radBtnES.Enabled = true;
                }


                ViewState[VIEWSTATE_SAMPLE_REQUEST_ITEMS_KEY] = new List<DTO_DOC_SAMPLE_REQUEST_ITEMS>();
                this.radGrdSampleItemList.DataSource = (List<DTO_DOC_SAMPLE_REQUEST_ITEMS>)ViewState[VIEWSTATE_SAMPLE_REQUEST_ITEMS_KEY];
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
    #endregion

    #region InitPageInfo
    private void InitPageInfo()
    {
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0011";
        //hddProcessID.Value = "P000000676";

        //hddProcessStatus.Value = ApprovalUtil.ApprovalStatus.Temp.ToString();

        InitControls();
    }
    #endregion

    private void SetDropdownControl()
    {
        using (Bayer.eWF.BSL.Common.Mgr.CodeMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CodeMgr())
        {
            location = mgr.SelectCodeSubList("S018");
        }
    }

    #region InitControls
    private void InitControls()
    {
        DTO_DOC_SAMPLE_REQUEST doc;
        if (!hddProcessID.Value.Equals(String.Empty))
        {
            string processId = this.hddProcessID.Value;
            using (Bayer.eWF.BSL.Approval.Mgr.SampleRequestMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.SampleRequestMgr())
            {
                doc = mgr.SelectSampleRequet(hddProcessID.Value);
                this.hddProcessStatus.Value = doc.PROCESS_STATUS;

                if (doc != null)
                {
                    if (!doc.BU.IsNullOrEmptyEx())
                    {
                        if (doc.BU.Equals(this.radBtnCP.Value)) this.radBtnCP.Checked = true;
                        else if (doc.BU.Equals(this.radBtnIS.Value)) this.radBtnIS.Checked = true;
                        else if (doc.BU.Equals(this.radBtnBVS.Value)) { 
                            this.radBtnBVS.Checked = true;
                            //--eWorkflow Optimization 2020 
                            this.radBtnBVS.Visible = true;
                            //--eWorkflow Optimization 2020 
                        }
                        else if (doc.BU.Equals(this.radBtnES.Value)) this.radBtnES.Checked = true;
                    }

                    if (doc.CATEGORY_CODE == "Claim")  // Claim compensation
                    {
                        radRdoCategory4.Visible = true;
                    }
                    else
                    {
                        radRdoCategory4.Visible = false;
                    }



                    if (doc.CATEGORY_CODE == radRdoCategory1.Value)
                        this.radRdoCategory1.Checked = true;
                    else if (doc.CATEGORY_CODE == radRdoCategory2.Value)
                        this.radRdoCategory2.Checked = true;
                    else if (doc.CATEGORY_CODE == radRdoCategory3.Value)
                        this.radRdoCategory3.Checked = true;
                    else if (doc.CATEGORY_CODE == radRdoCategory4.Value)  // Claim compensation
                        this.radRdoCategory4.Checked = true;
                    else if (doc.CATEGORY_CODE == radRdoCategory5.Value)  // CMKT Campain
                        this.radRdoCategory5.Checked = true;
                    else if (doc.CATEGORY_CODE == radRdoCategory6.Value)  // Return Sample
                        this.radRdoCategory6.Checked = true;


                    this.radTxtPurpose.Text = doc.PURPOSE;
                    this.radTxtAddress.Text = doc.TO_ADDRESS;
                    this.radTxtDirectAddress.Text = doc.DIRECT_TO_ADDRESS;
                    this.radTxtDirectPhone.Text = doc.DIRECT_TO_PHONE;
                    this.radTxtRelevant_e_WorkflowNo.Text = doc.RELEVANT_E_WORKFLOW_NO;

                    List<DTO_DOC_SAMPLE_REQUEST_ITEMS> items = mgr.SelectSampleRequestItems(hddProcessID.Value);
                    ViewState[VIEWSTATE_SAMPLE_REQUEST_ITEMS_KEY] = items;

                    this.radGrdSampleItemList.DataSource = items;
                    this.radGrdSampleItemList.DataBind();
                    
                    webMaster.DocumentNo = doc.DOC_NUM;

                }
            }
        }
        SetDropdownControl();
        //HidenPrice();

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
            List<DTO_DOC_SAMPLE_REQUEST_ITEMS> items = (List<DTO_DOC_SAMPLE_REQUEST_ITEMS>)serializer.Deserialize<List<DTO_DOC_SAMPLE_REQUEST_ITEMS>>(this.hddGridItems.Value);

            ViewState[VIEWSTATE_SAMPLE_REQUEST_ITEMS_KEY] = items;
            this.radGrdSampleItemList.DataSource = items;
            this.radGrdSampleItemList.DataBind();
        }

        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radBtnAdd, radGrdSampleItemList, null);
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

    #endregion

    #region [ Add Row ]

    private void UpdateGridData()
    {
        UpdateGridData(999, string.Empty, string.Empty);
    }

    private void UpdateGridData(int idx, string code, string name)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_SAMPLE_REQUEST_ITEMS> items = (List<DTO_DOC_SAMPLE_REQUEST_ITEMS>)serializer.Deserialize<List<DTO_DOC_SAMPLE_REQUEST_ITEMS>>(this.hddGridItems.Value);

        if (idx < 999)
        {
            var costcenter = (from item in items
                              where item.IDX == idx
                              select item).FirstOrDefault();
            if (costcenter != null)
            {
                costcenter.COST_CODE = code;
                costcenter.COST_NAME = name;
            }
        }

        ViewState[VIEWSTATE_SAMPLE_REQUEST_ITEMS_KEY] = items;
        this.radGrdSampleItemList.DataSource = items;
        this.radGrdSampleItemList.DataBind();

    }
    #endregion


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
                List<DTO_DOC_SAMPLE_REQUEST_ITEMS> list = (List<DTO_DOC_SAMPLE_REQUEST_ITEMS>)ViewState[VIEWSTATE_SAMPLE_REQUEST_ITEMS_KEY];
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


    #region [ 문서상단 버튼 ]

    private string GetBU()
    {
        string rtnValue = string.Empty;

        if (this.radBtnCP.Checked) rtnValue = this.radBtnCP.Value;
        else if (this.radBtnIS.Checked) rtnValue = this.radBtnIS.Value;
        else if (this.radBtnBVS.Checked) rtnValue = this.radBtnBVS.Value;
        else if (this.radBtnES.Checked) rtnValue = this.radBtnES.Value;

        return rtnValue;
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

            if (GetBU().IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "BG";
            if (!(radRdoCategory1.Checked || radRdoCategory2.Checked || radRdoCategory3.Checked || radRdoCategory5.Checked || radRdoCategory6.Checked))
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Category";
            if (radTxtPurpose.Text.Length <= 0)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose";
            //Prodcut
            if (this.radGrdSampleItemList.MasterTableView.Items.Count < 1)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Product";

        }
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;

    }

    private string GetCategory()
    {
        string category = string.Empty;
        if (this.radRdoCategory1.Checked) category = this.radRdoCategory1.Text;
        else if (this.radRdoCategory2.Checked) category = this.radRdoCategory2.Text;
        else if (this.radRdoCategory3.Checked) category = this.radRdoCategory3.Text;
        else if (this.radRdoCategory4.Checked) category = this.radRdoCategory4.Text;
        else if (this.radRdoCategory5.Checked) category = this.radRdoCategory5.Text;
        else if (this.radRdoCategory6.Checked) category = this.radRdoCategory6.Text;
        return category;
    }

    private string DocumentSave(string processStatus)
    {
        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_SAMPLE_REQUEST doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_SAMPLE_REQUEST();
        doc.PROCESS_STATUS = processStatus;
        doc.PROCESS_ID = hddProcessID.Value;
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = Sessions.UserID;
        doc.SUBJECT = GetBU() + "/" + GetCategory() + "/" + this.radTxtPurpose.Text;
        webMaster.Subject = doc.SUBJECT;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.CREATE_DATE = DateTime.Now;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.CREATOR_ID = Sessions.UserID;
        doc.IS_DISUSED = "N";

        doc.BU = GetBU();

        if (radRdoCategory1.Checked)
            doc.CATEGORY_CODE = radRdoCategory1.Value;
        else if (radRdoCategory2.Checked)
            doc.CATEGORY_CODE = radRdoCategory2.Value;
        else if (radRdoCategory3.Checked)
            doc.CATEGORY_CODE = radRdoCategory3.Value;
        else if (radRdoCategory4.Checked)
            doc.CATEGORY_CODE = radRdoCategory4.Value;
        else if (radRdoCategory5.Checked)
            doc.CATEGORY_CODE = radRdoCategory5.Value;
        else if (radRdoCategory6.Checked)
            doc.CATEGORY_CODE = radRdoCategory6.Value;


        GridFooterItem itemTotal = (GridFooterItem)this.radGrdSampleItemList.MasterTableView.GetItems(GridItemType.Footer)[0];

        if (this.radGrdSampleItemList.MasterTableView.Items.Count >= 1)
            { doc.TOTAL_AMOUNT = Convert.ToDecimal((itemTotal["AMOUNT"].Text.Replace(",", ""))); }
        
        doc.PURPOSE = this.radTxtPurpose.Text;
        doc.TO_ADDRESS = this.radTxtAddress.Text;
        doc.DIRECT_TO_ADDRESS = this.radTxtDirectAddress.Text;
        doc.DIRECT_TO_PHONE = this.radTxtDirectPhone.Text;
        doc.RELEVANT_E_WORKFLOW_NO = this.radTxtRelevant_e_WorkflowNo.Text;


        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_SAMPLE_REQUEST_ITEMS> items = (List<DTO_DOC_SAMPLE_REQUEST_ITEMS>)serializer.Deserialize<List<DTO_DOC_SAMPLE_REQUEST_ITEMS>>(this.hddGridItems.Value);
        foreach (DTO_DOC_SAMPLE_REQUEST_ITEMS item in items)
        {
            DTO_CODE_SUB code = location.Find(p => p.CODE_NAME == item.LOCATION_NAME.Trim());
            if (code != null)
            {
                item.LOCATION_CODE = code.SUB_CODE;
            }
            item.PROCESS_ID = this.hddProcessID.Value;
            item.CREATOR_ID = Sessions.UserID;
        }

        using (Bayer.eWF.BSL.Approval.Mgr.SampleRequestMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.SampleRequestMgr())
        {
            processID = mgr.MergeSampleRequest(doc, items);
        }
        return processID;
    }


    protected void radGrdSampleItemList_PreRender(object sender, EventArgs e)
    {
        if (radGrdSampleItemList.FindControl(radGrdSampleItemList.MasterTableView.ClientID + "_LOCATION_NAME") != null)
        {
            RadDropDownList DropLocation = radGrdSampleItemList.FindControl(radGrdSampleItemList.MasterTableView.ClientID + "_LOCATION_NAME").FindControl("radDropLocation") as RadDropDownList;
            DropLocation.DataTextField = "CODE_NAME";
            DropLocation.DataValueField = "SUB_CODE";
            DropLocation.DataSource = location;
            DropLocation.DataBind();
        }

    }
    private void HidenPrice()
    {
        if (Sessions.OrgName.ToString() == "BKL-BGP-GCF-IT-SFS1")
        {
            this.radGrdSampleItemList.MasterTableView.GetColumn("PRICE").Display = false;
        }
        else
        {
            this.radGrdSampleItemList.MasterTableView.GetColumn("PRICE").Display = true;


        }
        radGrdSampleItemList.Rebind();
    }
}
    #endregion
