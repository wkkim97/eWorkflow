using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Approval.Dao;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using Bayer.eWF.BSL.Common.Dto;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Telerik.Web.UI;
using System.Web.Script.Serialization;

public partial class Approval_Document_ListPrice : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_KEY = "VIEWSTATE_KEY_LIST_PRICE";
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
                ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_LIST_PRICE_PRODUCT>();
                this.RadGrdProduct.DataSource = (List<DTO_DOC_LIST_PRICE_PRODUCT>)ViewState[VIEWSTATE_KEY];
                this.RadGrdProduct.DataBind();
                
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
        hddDocumentID.Value = "D0044";
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        //hddProcessID.Value = "P000000864";


        InitControls();
    }
    #endregion

    #region InitControl (select)
    private void InitControls()
    {
        DTO_DOC_LIST_PRICE doc;

        if (!hddProcessID.Value.Equals(string.Empty))
        {
            using (ListPriceMgr mgr = new ListPriceMgr())
            {
                doc = mgr.SelectListPrice(hddProcessID.Value);
                if (doc != null)
                {
                    this.hddProcessStatus.Value = doc.PROCESS_STATUS;
                    if (this.RadrdoNew.Value == doc.CATEGORY) RadrdoNew.Checked = true;
                    if (this.RadrdoChange.Value == doc.CATEGORY) RadrdoChange.Checked = true;
                    if (this.RadrdoCodeChange.Value == doc.CATEGORY) RadrdoCodeChange.Checked = true;
                    if (this.RadrdoMRP.Value == doc.TYPE) RadrdoMRP.Checked = true;
                    if (this.RadrdoNonMRP.Value == doc.TYPE) RadrdoNonMRP.Checked = true;

                    //BU
                    if (doc.BU == "CH")
                    {
                        //radBtnCH.Checked = true;
                        radRdoBuCH.Visible = true;

                        //radBtnCC.Checked = false;
                        radRdoBuCC.Visible = false;
                    }
                    else if (doc.BU == "CC")
                    {
                        //radBtnCH.Checked = false;
                        radRdoBuCH.Visible = false;

                        //radBtnCC.Checked = true;
                        radRdoBuCC.Visible = true;
                    }
                    else if (doc.BU == "DC")
                    {
                        //radRdoBuDC.Checked = true;
                        radRdoBuDC.Visible = true;
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
                    this.RadtxtDescription.Text = doc.DESCRIPTION;
                    this.RadDatePeriodFrom.SelectedDate = doc.CONTRACT_PERIOD_FROM;
                    this.RadDatePeriodTo.SelectedDate = doc.CONTRACT_PERIOD_TO;
                    // PRODUCT LIST
                    List<DTO_DOC_LIST_PRICE_PRODUCT> products = mgr.SelectListPriceProduct(hddProcessID.Value);
                    ViewState[VIEWSTATE_KEY] = products;
                    RadioCheck();

                    this.RadGrdProduct.DataSource = products;
                    this.RadGrdProduct.DataBind();
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
            UpdateGridData();
        }

        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadrdoNew, RadGrdProduct, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadrdoChange, RadGrdProduct, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadrdoCodeChange, RadGrdProduct, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadGrdProduct, RadGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.RadGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_BasePrice_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";
      
       

    }

    void Approval_Document_BasePrice_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("Rebind"))
        {
            this.informationMessage = "";
            UpdateGridData();
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

    #region ValidationCheck
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus approvalStatus)
    {
        UpdateGridData();
        string message = string.Empty;

        if (approvalStatus == ApprovalUtil.ApprovalStatus.Request)
        {
            if (RadrdoNew.Checked == false && RadrdoChange.Checked == false && RadrdoCodeChange.Checked == false)
                message = "Category";
            if (RadrdoMRP.Checked == false && RadrdoNonMRP.Checked == false)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Type";
            if (GetSelectedBU().IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "BU";
            if (RadtxtDescription.Text.Length <= 0)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Description";
            if (!RadDatePeriodFrom.SelectedDate.HasValue)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Contract period From";
            if (!RadDatePeriodTo.SelectedDate.HasValue)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Contract period To";

            //if (RadGrdProduct.Items.Count < 1)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Product Information";
        }

        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;
    }
    #endregion

    #region SaveDocument
    private string SaveDocument(ApprovalUtil.ApprovalStatus approvalStatus)
    {
        DTO_DOC_LIST_PRICE price = new DTO_DOC_LIST_PRICE();
        string Bu = GetSelectedBU();
        price.PROCESS_ID = this.hddProcessID.Value;
        price.SUBJECT = GetSelectedCatetory() + "/" + GetSelectedType() + "/" + GetSelectedBU();
        webMaster.Subject = price.SUBJECT;
        price.DOC_NUM = string.Empty;
        price.PROCESS_STATUS = approvalStatus.ToString();
        price.REQUESTER_ID = Sessions.UserID;
        price.REQUEST_DATE = DateTime.Now;
        price.COMPANY_CODE = Sessions.CompanyCode;
        price.ORGANIZATION_NAME = Sessions.OrgName;
        price.LIFE_CYCLE = webMaster.LifeCycle;
        if (RadrdoNew.Checked == true) price.CATEGORY = RadrdoNew.Value;
        if (RadrdoChange.Checked == true) price.CATEGORY = RadrdoChange.Value;
        if (RadrdoCodeChange.Checked == true) price.CATEGORY = RadrdoCodeChange.Value;
        if (RadrdoMRP.Checked == true) price.TYPE = RadrdoMRP.Value;
        if (RadrdoNonMRP.Checked == true) price.TYPE = RadrdoNonMRP.Value;
        price.BU = Bu;
        price.DESCRIPTION = RadtxtDescription.Text;
        price.CONTRACT_PERIOD_FROM = RadDatePeriodFrom.SelectedDate;
        price.CONTRACT_PERIOD_TO = RadDatePeriodTo.SelectedDate;

        price.IS_DISUSED = "N";
        price.CREATOR_ID = Sessions.UserID;
        price.CREATE_DATE = DateTime.Now;

        // List<DTO_DOC_LIST_PRICE_PRODUCT> products = (List<DTO_DOC_LIST_PRICE_PRODUCT>)ViewState[VIEWSTATE_KEY];
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_LIST_PRICE_PRODUCT> products = (List<DTO_DOC_LIST_PRICE_PRODUCT>)serializer.Deserialize<List<DTO_DOC_LIST_PRICE_PRODUCT>>(this.hddGridItems.Value);
        foreach (DTO_DOC_LIST_PRICE_PRODUCT item in products)
        {
            item.PROCESS_ID = this.hddProcessID.Value;
            item.CREATOR_ID = Sessions.UserID;
            item.CREATE_DATE = DateTime.Now;
        }
        using (ListPriceMgr mgr = new ListPriceMgr())
        {
            return mgr.MergeListPrice(price, products);
        }
    }

    #endregion

    /// <summary>
    /// BU 반환
    /// </summary>
    /// <returns></returns>
    private string GetSelectedBU()
    {
        string bu = string.Empty;
        foreach (Control control in divBU.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    bu = (control as RadButton).Value;
                    break;
                }
            }
        }
        return bu;
    }

    /// <summary>
    /// TYPE 반환
    /// </summary>
    /// <returns></returns>
    private string GetSelectedType()
    {
        string type = string.Empty;
        foreach (Control control in divType.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    type = (control as RadButton).Value;
                    break;
                }
            }
        }
        return type;
    }

    /// <summary>
    /// Category 반환
    /// </summary>
    /// <returns></returns>
    private string GetSelectedCatetory()
    {
        string category = string.Empty;
        foreach (Control control in divCategory.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    category = (control as RadButton).Value;
                    break;
                }
            }
        }
        return category;
    }

    /// <summary>
    /// 그리드 이벤트
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    #region [ Add Row ]

    private void UpdateGridData()
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_LIST_PRICE_PRODUCT> items = (List<DTO_DOC_LIST_PRICE_PRODUCT>)serializer.Deserialize<List<DTO_DOC_LIST_PRICE_PRODUCT>>(this.hddGridItems.Value);

        ViewState[VIEWSTATE_KEY] = items;
        this.RadGrdProduct.DataSource = items;
        this.RadGrdProduct.DataBind();

    }

    #endregion

    #region Grdi ItemDataBound
    protected void RadGrdProduct_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem form = (GridDataItem)e.Item;

            RadNumericTextBox dataField1 = (RadNumericTextBox)form["CURRENT_PRICE"].FindControl("RadtxtCurrent");
            if (dataField1 != null)
                dataField1.Focus();

            RadNumericTextBox dataField2 = (RadNumericTextBox)form["REVISED_PRICE_INCLUDE_VAT"].FindControl("RadtxtRevised1");
            if (dataField2 != null)
                dataField2.Focus();

            RadNumericTextBox dataField3 = (RadNumericTextBox)form["REVISED_PRICE_EXCLUDE_VAT"].FindControl("RadtxtRevised2");
            if (dataField3 != null)
                dataField3.Focus();
        }
    }
    #endregion

    #region Category : Radio Event
    protected void RadrdoNew_Click(object sender, EventArgs e)
    {
        this.informationMessage = "";
        RadioCheck();
        ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_LIST_PRICE_PRODUCT>();
        this.RadGrdProduct.DataSource = (List<DTO_DOC_LIST_PRICE_PRODUCT>)ViewState[VIEWSTATE_KEY];
        this.RadGrdProduct.DataBind();
    }

    private void RadioCheck()
    {
        if (RadrdoNew.Checked == true)
        {
            foreach (GridColumn column in RadGrdProduct.Columns)
            {
                if (column.UniqueName == "BASE_PRICE")
                    (column as GridTemplateColumn).Display = false;                
                if (column.UniqueName == "CURRENT_PRICE" || column.UniqueName == "CURRENT_PRICE_INCLUDE_VAT")
                    (column as GridTemplateColumn).Display = true;
                if (column.UniqueName == "REVISED_PRICE_INCLUDE_VAT" || column.UniqueName == "REVISED_PRICE_EXCLUDE_VAT")
                    (column as GridTemplateColumn).Display = false;
                
            }
        }
        else if (RadrdoChange.Checked == true)
        {
            foreach (GridColumn column in RadGrdProduct.Columns)
            {
                if (column.UniqueName == "BASE_PRICE")
                    (column as GridTemplateColumn).Display = true;
                if (column.UniqueName == "CURRENT_PRICE" || column.UniqueName == "CURRENT_PRICE_INCLUDE_VAT")
                    (column as GridTemplateColumn).Display = false;
                if (column.UniqueName == "REVISED_PRICE_INCLUDE_VAT" || column.UniqueName == "REVISED_PRICE_EXCLUDE_VAT")
                    (column as GridTemplateColumn).Display = true;
                
            }
        }
        else if (RadrdoCodeChange.Checked == true)
        {
            foreach (GridColumn column in RadGrdProduct.Columns)
            {
                if (column.UniqueName == "BASE_PRICE")
                    (column as GridTemplateColumn).Display = true;
                if (column.UniqueName == "CURRENT_PRICE" || column.UniqueName == "CURRENT_PRICE_INCLUDE_VAT")
                    (column as GridTemplateColumn).Display = false;
                if (column.UniqueName == "REVISED_PRICE_INCLUDE_VAT" || column.UniqueName == "REVISED_PRICE_EXCLUDE_VAT")
                    (column as GridTemplateColumn).Display = true;

            }
        }
    }
    #endregion

    #region Grid Row Delete Event
    protected void RadGrdProduct_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Remove"))
            {
                string productCode = Convert.ToString(e.CommandArgument);

                using (ListPriceDao Dao = new ListPriceDao())
                {
                    Dao.DeleteListPriceProduct(this.hddProcessID.Value, productCode);
                }
                List<DTO_DOC_LIST_PRICE_PRODUCT> list = (List<DTO_DOC_LIST_PRICE_PRODUCT>)ViewState[VIEWSTATE_KEY];
                list.RemoveAll(p => p.PRODUCT_CODE == productCode);

                this.RadGrdProduct.DataSource = list;
                this.RadGrdProduct.DataBind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }

    }
    #endregion
}