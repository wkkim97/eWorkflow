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

public partial class Approval_Document_BvsSeedImport : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_KEY_LIST = "VIEWSTATE_KEY_BVS_SEED_IMPORT_LIST";
    string Radio;

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
                ViewState[VIEWSTATE_KEY_LIST] = new List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT>();
                this.radGrdProduct.DataSource = (List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT>)ViewState[VIEWSTATE_KEY_LIST];
                this.radGrdProduct.DataBind();

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
        hddDocumentID.Value = "D0045";
        //hddProcessID.Value = "";

        InitControls();
    }
    #endregion

    private string GetSelectedType()
    {
        string rtnValue = string.Empty;

        if (this.RadrdoCommercial.Checked) rtnValue = this.RadrdoCommercial.Value;
        else if (this.RadrdoSample.Checked) rtnValue = this.RadrdoSample.Value;

        return rtnValue;
    }


    #region InitControl
    private void InitControls()
    {
        DTO_DOC_BVS_SEED_IMPORT doc;

        if (!hddProcessID.Value.Equals(string.Empty))
        {
            using(BvsSeedImportMgr mgr = new BvsSeedImportMgr())
            {
                doc = mgr.SelectBvsSeedImport(hddProcessID.Value);
                if (doc != null)
                {
                    this.hddProcessStatus.Value = doc.PROCESS_STATUS;

                    if (doc.TYPE.Equals(this.RadrdoCommercial.Value)) this.RadrdoCommercial.Checked = true;
                    else if (doc.TYPE.Equals(this.RadrdoSample.Value)) this.RadrdoSample.Checked = true;

                    if (this.RadrdoCommercial.Value == doc.TYPE)
                    {
                        RadrdoCommercial.Checked = true;
                    }
                    if (this.RadrdoSample.Value == doc.TYPE) RadrdoSample.Checked = true;
                    this.RadExpectedDate.SelectedDate = doc.EXPECTED_DATE;
                    this.radDropVendor.SelectedValue = doc.VENDOR;

                    this.RadtxtRemark.Text = doc.REMARK;

                    //Product
                    List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT> products = mgr.SelectBvsSeedImportProduct(this.hddProcessID.Value);
                    ViewState[VIEWSTATE_KEY_LIST] = products;

                    this.radGrdProduct.DataSource = products;
                    this.radGrdProduct.DataBind();

                    webMaster.DocumentNo = doc.DOC_NUM;
                }
            }
        }
        else
        {
            DateTime oneTwentyAgo = DateTime.Today.AddDays(60);

            this.RadExpectedDate.SelectedDate = oneTwentyAgo;
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
            List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT> items = (List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT>)serializer.Deserialize<List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT>>(this.hddGridItems.Value);

            ViewState[VIEWSTATE_KEY_LIST] = items;
            this.radGrdProduct.DataSource = items;
            this.radGrdProduct.DataBind();
        }

        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadrdoCreate, radGrdProduct, null);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadrdoChange, radGrdProduct, null);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoFM, radGrdProduct, null);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoNH, radGrdProduct, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_BvsSeedImport_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";

        string strType = this.RadrdoCommercial.Checked ? this.RadrdoCommercial.Value : "0000";
        if (!ClientScript.IsStartupScriptRegistered("setVisibleControl"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleControl", "setVisibleControl('" + strType + "');", true);

    }

    private void Approval_Document_BvsSeedImport_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("Rebind"))
        {
            UpdateGridData(999, string.Empty, string.Empty);
        }
    }
    #endregion

    /// <summary>
    /// 그리드 이벤트
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    #region [ Add Row ]
    private void UpdateGridData(int idx, string code, string name)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT> items = (List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT>)serializer.Deserialize<List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT>>(this.hddGridItems.Value);

        ViewState[VIEWSTATE_KEY_LIST] = items;
        this.radGrdProduct.DataSource = items;
        this.radGrdProduct.DataBind();
    }
    #endregion

    #region DoRequest
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

    #region SaveDocument
    private string SaveDocument(ApprovalUtil.ApprovalStatus approvalStatus)
    {
        DTO_DOC_BVS_SEED_IMPORT doc = new DTO_DOC_BVS_SEED_IMPORT();

        doc.PROCESS_ID = this.hddProcessID.Value;
        doc.SUBJECT = GetSelectedType() + " / " + this.RadExpectedDate.SelectedDate.Value.ToString("yyyy-MM-dd") + " / " + radDropVendor.SelectedText;   // subject

        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = approvalStatus.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;

        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;

        doc.TYPE = GetSelectedType();
        doc.EXPECTED_DATE = this.RadExpectedDate.SelectedDate;
        doc.VENDOR = this.radDropVendor.SelectedValue;
        doc.REMARK = this.RadtxtRemark.Text;

        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;
        doc.CREATE_DATE = DateTime.Now;

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT> products = (List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT>)serializer.Deserialize<List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT>>(this.hddGridItems.Value);   //그리드의 DTO

        foreach (DTO_DOC_BVS_SEED_IMPORT_PRODUCT product in products)
        {
            if ( product.IDX == 1)
            {
                doc.SUBJECT = doc.SUBJECT + " / " + product.CROP + " / " + product.VARIETY + " / " + product.QTY_EA;
            }
            product.PROCESS_ID = this.hddProcessID.Value;
            product.CREATOR_ID = Sessions.UserID;
        }
        
        webMaster.Subject = doc.SUBJECT;

        using (BvsSeedImportMgr mgr = new BvsSeedImportMgr())
        {
            return mgr.MergeBvsSeedImport(doc, products);
        }
    }

    #endregion

    #region ValidationCheck
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus approvalStatus)
    {
        string message = string.Empty;

        if (approvalStatus == ApprovalUtil.ApprovalStatus.Request)
        {

            //if (radRdoCP.Checked == true && radRdoFM.Checked == false && radRdoNH.Checked == false)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Distribution channel";

            if (GetSelectedType() == "")
                message += "Type";

            if (!RadExpectedDate.SelectedDate.HasValue)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Expected Date";

            if (radDropVendor.SelectedIndex == -1)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Vendor";

            if (this.RadtxtRemark.Text.IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Remark";

            //Prodcut
            if (this.radGrdProduct.MasterTableView.Items.Count < 1)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Product";

        }

        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;
    }
    #endregion


    #region ItemDataBound
    protected void radGrdProduct_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;

        }
    } 
    #endregion

    // Grid RESET
    protected void RadGrd_Reset(object sender, EventArgs e)
    {
        ViewState[VIEWSTATE_KEY_LIST] = new List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT>();
        this.radGrdProduct.DataSource = (List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT>)ViewState[VIEWSTATE_KEY_LIST];
        this.radGrdProduct.DataBind();
    }


    #region Delete Event
    protected void radGrdProduct_ItemCommand(object sender, GridCommandEventArgs e)
    {
         try
        {
            if (e.CommandName.Equals("Remove"))
            {
                int index = Convert.ToInt32(e.CommandArgument);

                using (BvsSeedImportMgr Mgr = new BvsSeedImportMgr())
                {
                    Mgr.DeleteBvsSeedImportItemsByIndex(this.hddProcessID.Value, index);

                }

                List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT> list = (List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT>)ViewState[VIEWSTATE_KEY_LIST];
                list.RemoveAll(p => p.IDX == index);

                this.radGrdProduct.DataSource = list;
                this.radGrdProduct.DataBind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    

    #endregion


    protected void RadrdoNew_Click(object sender, EventArgs e)
    {
        this.informationMessage = "";
        RadioCheck();
        ViewState[VIEWSTATE_KEY_LIST] = new List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT>();
        this.radGrdProduct.DataSource = (List<DTO_DOC_BVS_SEED_IMPORT_PRODUCT>)ViewState[VIEWSTATE_KEY_LIST];
        this.radGrdProduct.DataBind();
    }

    private void RadioCheck()
    {
        if (RadrdoCommercial.Checked == true)
        {
            foreach (GridColumn column in radGrdProduct.Columns)
            {
                if (column.UniqueName == "TP_PRICE" || column.UniqueName == "AMOUNT")
                    column.Display = true;
                        //(column as GridTemplateColumn).Display = true;

            }
        }
        else if (RadrdoSample.Checked == true)
        {
            foreach (GridColumn column in radGrdProduct.Columns)
            {
                if (column.UniqueName == "TP_PRICE" || column.UniqueName == "AMOUNT")
                    column.Display = false;
                    //(column as GridTemplateColumn).Display = false;

            }
        }
    }
    
}