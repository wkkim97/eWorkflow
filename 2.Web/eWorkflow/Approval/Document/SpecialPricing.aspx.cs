using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Approval.Dto;
using Telerik.Web.UI;
using System.Web.Script.Serialization;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Approval.Mgr;

public partial class Approval_Document_SpecialPricing : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_KEY = "VIEWSTATE_KEY_INTERACTION_HEALTHCARE_PRO";

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
                ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_SPECIAL_PRICING_PRODUCT>();
                this.radGrdProduct.DataSource = (List<DTO_DOC_SPECIAL_PRICING_PRODUCT>)ViewState[VIEWSTATE_KEY];
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

    private void InitPageInfo()
    {
        this.hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        this.hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        this.hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        this.hddDocumentID.Value = "D0009";
        //this.HddProcessID.Value = "P000000492";

        InitControls();
    }

    private void InitControls()
    {
        if (!hddProcessID.Value.IsNullOrEmptyEx())
        {
            string processId = this.hddProcessID.Value;

            using (SpecialPricingMgr mgr = new SpecialPricingMgr())
            {
                DTO_DOC_SPECIAL_PRICING doc = mgr.SelectSpecialPricing(processId);

                foreach (Control control in divType.Controls)
                {
                    if (control is RadButton)
                    {
                        if ((control as RadButton).Value == doc.TYPE)
                        {
                            (control as RadButton).Checked = true; break;

                        }
                    }
                }

                if (doc.IS_ALL_CUSTOMER.Equals("Y")) this.radBtnAll.Checked = true;
                else this.radBtnSelected.Checked = true;

                this.radTxtCustomer.Text = doc.CUSTOMER_NAME;
                this.hddCustomerId.Value = doc.CUSTOMER_CODE;
                this.radTxtPeriod.Text = doc.PERIOD;
                this.radDatEnforcement.SelectedDate = doc.ENFORCEMENT_DATE;
                this.radTxtReason.Text = doc.REASON;
                webMaster.DocumentNo = doc.DOC_NUM;

                List<DTO_DOC_SPECIAL_PRICING_PRODUCT> products = mgr.SelectSpecialPricingProduct(processId);
                ViewState[VIEWSTATE_KEY] = products;

                this.radGrdProduct.DataSource = products;
                this.radGrdProduct.DataBind();
            }
        }
        else
        {
            ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_SPECIAL_PRICING_PRODUCT>();
            this.radGrdProduct.DataSource = (List<DTO_DOC_SPECIAL_PRICING_PRODUCT>)ViewState[VIEWSTATE_KEY];
            this.radGrdProduct.DataBind();

        }
    }

    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 

        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radGrdProduct, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_SpecialPricing_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";

        string isAll = "None";
        if (this.radBtnAll.Checked || this.radBtnSelected.Checked) isAll = (this.radBtnAll.Checked ? "Y" : "N");
        if (!ClientScript.IsStartupScriptRegistered("setVisibleControl"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleControl", "setVisibleControl('" + GetTypeValue() + "','" + isAll + "');", true);

    }

    void Approval_Document_SpecialPricing_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("Rebind"))
        {
            UpdateGridData();
        }
    }

    #region [ Add Button Event ]

    private void UpdateGridData()
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_SPECIAL_PRICING_PRODUCT> items = (List<DTO_DOC_SPECIAL_PRICING_PRODUCT>)serializer.Deserialize<List<DTO_DOC_SPECIAL_PRICING_PRODUCT>>(this.hddGridItems.Value);

        foreach (DTO_DOC_SPECIAL_PRICING_PRODUCT product in items)
        {
            product.DC = product.PRICE_BEFORE - product.PRICE_AFTER;
            product.TOTAL_SALES = product.ORDER_QTY * product.PRICE_AFTER;
            product.TOTAL_DC = product.ORDER_QTY * (product.PRICE_BEFORE - product.PRICE_AFTER);
            product.REQUEST_QTY = product.ORDER_QTY;
            product.REQUEST_PRICE = product.PRICE_AFTER;
        }

        ViewState[VIEWSTATE_KEY] = items;
        this.radGrdProduct.DataSource = items;
        this.radGrdProduct.DataBind();

    }

    #endregion

    #region [ 문서상단 버튼 ]

    private string GetTypeText()
    {
        string type = string.Empty;
        if (this.radBtnVolumn.Checked) type = this.radBtnVolumn.Text;
        else if (this.radBtnQuaterly.Checked) type = this.radBtnQuaterly.Text;
        else if (this.radBtnSpecial.Checked) type = this.radBtnSpecial.Text;
        else if (this.radBtnMaximum.Checked) type = this.radBtnMaximum.Text;
        return type;
    }

    private string GetTypeValue()
    {
        string type = string.Empty;
        if (this.radBtnVolumn.Checked) type = this.radBtnVolumn.Value;
        else if (this.radBtnQuaterly.Checked) type = this.radBtnQuaterly.Value;
        else if (this.radBtnSpecial.Checked) type = this.radBtnSpecial.Value;
        else if (this.radBtnMaximum.Checked) type = this.radBtnMaximum.Value;
        return type;
    }

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        UpdateGridData();

        string message = string.Empty;
        if (GetTypeValue().IsNullOrEmptyEx())
            message += "Type";

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            if (GetTypeValue() != "0004")
            {
                List<DTO_DOC_SPECIAL_PRICING_PRODUCT> products = (List<DTO_DOC_SPECIAL_PRICING_PRODUCT>)ViewState[VIEWSTATE_KEY];
                if (products.Count < 1)
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Price Information";
            }

            if (!(this.radBtnAll.Checked || this.radBtnSelected.Checked))
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Customer Information";
            }
            else
            {
                if (this.radBtnSelected.Checked && this.radTxtCustomer.Text.Trim().IsNullOrEmptyEx())
                {
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Customer Information";
                }
            }

        }

        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + message;
            return false;
        }
        else return true;
    }

    private string SaveDocument(ApprovalUtil.ApprovalStatus status)
    {
        DTO_DOC_SPECIAL_PRICING doc = new DTO_DOC_SPECIAL_PRICING();
        doc.PROCESS_ID = this.hddProcessID.Value;
        doc.SUBJECT = GetTypeText() + "/" + this.radTxtCustomer.Text;
        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.TYPE = GetTypeValue();
        doc.IS_ALL_CUSTOMER = this.radBtnAll.Checked ? "Y" : "N";
        doc.CUSTOMER_CODE = this.hddCustomerId.Value;
        doc.CUSTOMER_NAME = this.radTxtCustomer.Text;
        doc.PERIOD = this.radTxtPeriod.Text;
        doc.ENFORCEMENT_DATE = this.radDatEnforcement.SelectedDate;
        doc.REASON = this.radTxtReason.Text;
        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;

        List<DTO_DOC_SPECIAL_PRICING_PRODUCT> products = (List<DTO_DOC_SPECIAL_PRICING_PRODUCT>)ViewState[VIEWSTATE_KEY];

        foreach (DTO_DOC_SPECIAL_PRICING_PRODUCT product in products)
        {
            if (product.REQUEST_DATE.HasValue && product.REQUEST_DATE.Value.Year == 0001)
                product.REQUEST_DATE = null;
            product.CREATOR_ID = Sessions.UserID;
        }

        using (SpecialPricingMgr mgr = new SpecialPricingMgr())
        {
            return mgr.MergeSpecialPricing(doc, products);
        }
    }

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
    protected void radGrdProduct_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            string productCode = e.CommandArgument.ToString();

            using (SpecialPricingMgr mgr = new SpecialPricingMgr())
            {
                mgr.DeleteSpecialPricingProduct(this.hddProcessID.Value, productCode);
            }

            List<DTO_DOC_SPECIAL_PRICING_PRODUCT> list = (List<DTO_DOC_SPECIAL_PRICING_PRODUCT>)ViewState[VIEWSTATE_KEY];

            list.RemoveAll(p => p.PRODUCT_CODE == productCode);

            this.radGrdProduct.DataSource = list;
            this.radGrdProduct.DataBind();
        }
    }
 
}