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

public partial class Approval_Document_BaseDiscountPrice : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_BASE_DISCOUNT_PRODUCT_KEY = "VIEWSTATE_BASE_DISCOUNT_PRODUCT_KEY";

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
                ViewState[VIEWSTATE_BASE_DISCOUNT_PRODUCT_KEY] = new List<DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT>();
                this.radGrdBaseDiscountPriceProduct.DataSource = (List<DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT>)ViewState[VIEWSTATE_BASE_DISCOUNT_PRODUCT_KEY];
                this.radGrdBaseDiscountPriceProduct.DataBind();
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
        hddDocumentID.Value = "D0028";
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        

        //hddProcessStatus.Value = ApprovalUtil.ApprovalStatus.Temp.ToString();

        InitControls();
    }
    #endregion

    #region InitControls
    private void InitControls()
    {

        DTO_DOC_BASE_DISCOUNT_PRICE doc;
        if (!hddProcessID.Value.Equals(String.Empty))
        {
            string processId = this.hddProcessID.Value;
            using (Bayer.eWF.BSL.Approval.Mgr.BaseDiscountPriceMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.BaseDiscountPriceMgr())
            {
                doc = mgr.SelectBaseDiscountPrice(hddProcessID.Value);
                this.hddProcessStatus.Value = doc.PROCESS_STATUS;

                if (doc != null)
                {
                    if (doc.TYPE == radRdoType1.Value){
                        this.radRdoType1.Checked = true;
                        this.radToDate.Visible = false;
                    }
                    else if (doc.TYPE == radRdoType2.Value){
                        this.radRdoType2.Checked = true;
                        this.radToDate.Visible = false;
                    }
                    else if (doc.TYPE == radRdoType3.Value)
                    {
                        this.radRdoType3.Checked = true;
                        this.radToDate.Visible = true;
                    }
                      

                    //this.radtxtCustomer.Text = doc.CUSTOMER_NAME;
                    if (doc.PRODUCT_TYPE == radRdoProduct1.Value)
                        this.radRdoProduct1.Checked = true;
                    else if (doc.PRODUCT_TYPE == radRdoProduct2.Value)
                        this.radRdoProduct2.Checked = true;
                    else if (doc.PRODUCT_TYPE == radRdoProduct3.Value)
                        this.radRdoProduct3.Checked = true;
                    else if (doc.PRODUCT_TYPE == radRdoProduct4.Value)
                        this.radRdoProduct4.Checked = true;
                    this.radTxtDescription.Text = doc.DESCRIPTION;

                    this.radFromDate.SelectedDate = doc.DATE_FROM;
                    this.radToDate.SelectedDate = doc.DATE_TO;

                    List<DTO_DOC_BASE_DISCOUNT_PRICE_CUSTOMER> customers = mgr.SelectBaseDiscountPriceCustomer(this.hddProcessID.Value);
                    foreach (DTO_DOC_BASE_DISCOUNT_PRICE_CUSTOMER customer in customers)
                    {
                        AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                        entry.Value = customer.CUSTOMER_CODE;
                        entry.Text = customer.CUSTOMER_NAME;
                        this.radAcomCustomer.Entries.Add(entry);
                    }

                    List<DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT> products = mgr.SelectBaseDiscountPriceProduct(hddProcessID.Value);
                    ViewState[VIEWSTATE_BASE_DISCOUNT_PRODUCT_KEY] = products;

                    this.radGrdBaseDiscountPriceProduct.DataSource = products;
                    this.radGrdBaseDiscountPriceProduct.DataBind();

                    if (!(doc.PROCESS_STATUS.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || doc.PROCESS_STATUS.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString())))
                    {
                        if (customers.Count < 1)
                        {
                            this.radAcomCustomer.Visible = false;
                            this.lblNotCustomer.Visible = true;
                        }

                    }
                    radRdoType1_Click();
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
        
        
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoType1, radGrdBaseDiscountPriceProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoType2, radGrdBaseDiscountPriceProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoType3, radGrdBaseDiscountPriceProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), radGrdBaseDiscountPriceProduct);
        
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.hddAddRow);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_BaseDiscountPrice_AjaxRequest;

        UpdateGridData();
    }

    void Approval_Document_BaseDiscountPrice_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.Equals("Rebind")) this.hddAddRow.Value = "Y";
        else this.hddAddRow.Value = "N";
        UpdateGridData();
    }

    #endregion

    #region Radio Button Event
    protected void radRdoType1_Click(object sender, EventArgs e)
    {
        ViewState[VIEWSTATE_BASE_DISCOUNT_PRODUCT_KEY] = new List<DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT>();
        this.radGrdBaseDiscountPriceProduct.DataSource = string.Empty;
        this.radGrdBaseDiscountPriceProduct.DataBind();
        radRdoType1_Click();
    }

    private void radRdoType1_Click()
    {
        if(radRdoType1.Checked == true)
        {
            this.radToDate.Visible = false;
            foreach (GridColumn column in radGrdBaseDiscountPriceProduct.Columns)
            {
                if (column.UniqueName == "CHANGE_PRICE")
                    (column as GridTemplateColumn).Display = false;
                if (column.UniqueName == "DISCOUNT_PRICE")
                    (column as GridTemplateColumn).Display = false;
                if (column.UniqueName == "BASE_PRICE_TO_BE")
                    (column as GridTemplateColumn).Display = false;
            }
        }
        else if (radRdoType2.Checked == true)
        {
            this.radToDate.Visible = false;
            foreach (GridColumn column in radGrdBaseDiscountPriceProduct.Columns)
            {
                if (column.UniqueName == "CHANGE_PRICE")
                    (column as GridTemplateColumn).Display = true;
                if (column.UniqueName == "DISCOUNT_PRICE")
                    (column as GridTemplateColumn).Display = false;
                if (column.UniqueName == "BASE_PRICE_TO_BE")
                    (column as GridTemplateColumn).Display = true;
            }
        }
        else if (radRdoType3.Checked == true)
        {
            this.radToDate.Visible = true;
            foreach (GridColumn column in radGrdBaseDiscountPriceProduct.Columns)
            {
                if (column.UniqueName == "CHANGE_PRICE")
                    (column as GridTemplateColumn).Display = false;
                if (column.UniqueName == "DISCOUNT_PRICE")
                    (column as GridTemplateColumn).Display = true;
                if (column.UniqueName == "BASE_PRICE_TO_BE")
                    (column as GridTemplateColumn).Display = false;
            }
        }
    }
    #endregion

    #region Add Button Click


    private void UpdateGridData()
    {
        //radRdoType1_Click();
        if (this.hddGridItems.Value.IsNullOrEmptyEx()) return;
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT> items = (List<DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT>)serializer.Deserialize<List<DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT>>(this.hddGridItems.Value);

        ViewState[VIEWSTATE_BASE_DISCOUNT_PRODUCT_KEY] = items;
        this.radGrdBaseDiscountPriceProduct.DataSource = items;
        this.radGrdBaseDiscountPriceProduct.DataBind();
    }
    #endregion

    protected void radGrdBaseDiscountPriceProduct_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            int index = Convert.ToInt32(e.CommandArgument);

            using (BaseDiscountPriceMgr mgr = new BaseDiscountPriceMgr())
            {
                mgr.DeleteBaseDiscountPriceProductByIndex(this.hddProcessID.Value, index);
            }
            List<DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT> product = (List<DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT>)ViewState[VIEWSTATE_BASE_DISCOUNT_PRODUCT_KEY];

            product.RemoveAll(p => p.IDX == index);

            this.radGrdBaseDiscountPriceProduct.DataSource = product;
            this.radGrdBaseDiscountPriceProduct.DataBind();
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
            if (!(radRdoType1.Checked || radRdoType2.Checked || radRdoType3.Checked))
                message += "Type";
            //if(this.radtxtCustomer.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "Customer" : ",Customer";
            if (!(radRdoProduct1.Checked || radRdoProduct2.Checked || radRdoProduct3.Checked || radRdoProduct4.Checked))
                message += message.IsNullOrEmptyEx() ? "Product" : ",Product";
            if (radTxtDescription.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Description" : ",Description";
            if (radRdoType1.Checked || radRdoType2.Checked)
                if (!radFromDate.SelectedDate.HasValue)
                {
                    message += message.IsNullOrEmptyEx() ? "Effective Date" : ",Effective date";
                }
            if (radRdoType3.Checked)
                if (!(radFromDate.SelectedDate.HasValue || radToDate.SelectedDate.HasValue))
                {
                    message += message.IsNullOrEmptyEx() ? "Effective Date" : ",Effective date";
                }
            if (this.radGrdBaseDiscountPriceProduct.Items.Count < 1)
                message += message.IsNullOrEmptyEx() ? "Grid Item" : ",Grid Item";
        }
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;

    }

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

    private string GetSelectedProduct()
    {
        string product = string.Empty;
        foreach (Control control in divProduct.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {              
                    product = (control as RadButton).Value;
                    break;
                }
            }
        }
        return product;
    }


    private string DocumentSave(string processStatus)
    {
        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_BASE_DISCOUNT_PRICE doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_BASE_DISCOUNT_PRICE();
        doc.PROCESS_STATUS = processStatus;
        doc.PROCESS_ID = hddProcessID.Value;
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = Sessions.UserID;
//        if (this.radFromDate.SelectedDate.HasValue)
//            doc.SUBJECT = GetSelectedType() + "/" + GetSelectedProduct() + "/" + this.radFromDate.SelectedDate.Value.ToString("yyyy-MM-dd") 
//        else doc.SUBJECT = GetSelectedType() + "/" + GetSelectedProduct();

        if (this.radFromDate.SelectedDate.HasValue)
            doc.SUBJECT = GetSelectedType() + "/" + GetSelectedProduct() + "/" + this.radFromDate.SelectedDate.Value.ToString("yyyy-MM-dd") + "/" + radAcomCustomer.Text;
        else doc.SUBJECT = GetSelectedType() + "/" + GetSelectedProduct() + "/" + radAcomCustomer.Text;

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
        if (radRdoType3.Checked)
            doc.TYPE = radRdoType3.Value;

        doc.CUSTOMER_CODE = hddCustomerCode.Value;
        //doc.CUSTOMER_NAME = radtxtCustomer.Text;

        if (radRdoProduct1.Checked)
            doc.PRODUCT_TYPE = radRdoProduct1.Value;
        if (radRdoProduct2.Checked)
            doc.PRODUCT_TYPE = radRdoProduct2.Value;
        if (radRdoProduct3.Checked)
            doc.PRODUCT_TYPE = radRdoProduct3.Value;
        if (radRdoProduct4.Checked)
            doc.PRODUCT_TYPE = radRdoProduct4.Value;

        doc.DESCRIPTION = radTxtDescription.Text;
        doc.DATE_FROM = radFromDate.SelectedDate;
        doc.DATE_TO = radToDate.SelectedDate;

        List<DTO_DOC_BASE_DISCOUNT_PRICE_CUSTOMER> customers = new List<DTO_DOC_BASE_DISCOUNT_PRICE_CUSTOMER>();
        foreach (AutoCompleteBoxEntry entry in this.radAcomCustomer.Entries)
        {
            DTO_DOC_BASE_DISCOUNT_PRICE_CUSTOMER customer = new DTO_DOC_BASE_DISCOUNT_PRICE_CUSTOMER();
            customer.PROCESS_ID = this.hddProcessID.Value;
            customer.CUSTOMER_CODE = entry.Value;
            customer.CUSTOMER_NAME = entry.Text;
            customer.CREATOR_ID = Sessions.UserID;
            customers.Add(customer);
        }


        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT> products = (List<DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT>)serializer.Deserialize<List<DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT>>(this.hddGridItems.Value);
        foreach (DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT product in products)
        {
            product.PROCESS_ID = this.hddProcessID.Value;
            product.CREATOR_ID = Sessions.UserID;
        }

        using (Bayer.eWF.BSL.Approval.Mgr.BaseDiscountPriceMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.BaseDiscountPriceMgr())
        {
            processID = mgr.MergeBaseDiscountPrice(doc, products, customers);
        }
        return processID;
    }
}