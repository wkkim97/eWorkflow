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
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Mgr;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Telerik.Web.UI;
using System.Web.Script.Serialization;


public partial class Approval_Document_MaterialPrice : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_KEY = "VIEWSTATE_KEY_MATERIAL_PRICE";

    string category;
    string materialtype;
    string counter;

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
                this.hddCompanyCode.Value = Sessions.CompanyCode; //회사코드 설정

                ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_MATERIAL_PRICE_LIST>();
                this.RadGrdMaterial.DataSource = (List<DTO_DOC_MATERIAL_PRICE_LIST>)ViewState[VIEWSTATE_KEY];
                this.RadGrdMaterial.DataBind();

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
        hddDocumentID.Value = "D0027";
        //hddProcessID.Value = "";

        InitControls();
    }
    #endregion

    #region InitControl (select)
    private void InitControls()
    {
        //DropDownList Incoterm setting
        using (CodeMgr mgr = new CodeMgr())
        {
            List<DTO_CODE_SUB> titles = mgr.SelectCodeSubList("S020");
            this.RadDropIncoterm.DataSource = titles;
            this.RadDropIncoterm.DataBind();
            if (this.RadDropIncoterm.Items.Count > 0) this.RadDropIncoterm.SelectedIndex = -1;
        }
        //DropDownList Payment term setting
        using (CodeMgr mgr = new CodeMgr())
        {
            List<DTO_CODE_SUB> titles = mgr.SelectCodeSubList("S021");
            this.RadDropPayment.DataSource = titles;
            this.RadDropPayment.DataBind();
            if (this.RadDropPayment.Items.Count > 0) this.RadDropPayment.SelectedIndex = -1;
        }

        DTO_DOC_MATERIAL_PRICE doc;

        if (!hddProcessID.Value.Equals(string.Empty))
        {
            using (MaterialPriceMgr mgr = new MaterialPriceMgr())
            {
                doc = mgr.SelectMaterialPrice(hddProcessID.Value);
                if (doc != null)
                {
                    this.hddProcessStatus.Value = doc.PROCESS_STATUS;
                    if (radRdoCreate.Value == doc.CATEGORY_CODE)
                    {
                        radRdoCreate.Checked = true;
                        //Create Purpose
                        foreach (Control control in this.divCreate.Controls)
                        {
                            if (control is RadButton)
                            {
                                if ((control as RadButton).Value.Equals(doc.PURPOSE_CODE))
                                {
                                    (control as RadButton).Checked = true; break;
                                }
                            }
                        }
                    }
                    if (RadrdoChange.Value == doc.CATEGORY_CODE) 
                    {
                        RadrdoChange.Checked = true;
                        //Change Purpose
                        foreach (Control control in this.divChange.Controls)
                        {
                            if (control is RadButton)
                            {
                                if ((control as RadButton).Value.Equals(doc.PURPOSE_CODE))
                                {
                                    (control as RadButton).Checked = true; break;
                                }
                            }
                        }
                    }
                    //Material Type
                    foreach (Control control in this.divType.Controls)
                    {
                        if (control is RadButton)
                        {
                            if ((control as RadButton).Value.Equals(doc.MATERIAL_TYPE))
                            {
                                (control as RadButton).Checked = true; break;
                            }
                        }
                    }
                    if (this.RadRdoExist.Value == doc.K_REACH_VERIFICATION) this.RadRdoExist.Checked = true;
                    if (this.RadRdoNew.Value == doc.K_REACH_VERIFICATION) this.RadRdoNew.Checked = true;
                    this.RadTextCounter.Text = doc.COUNTER_MESSAGE;
                    this.RadTextProduct.Text = doc.PRODUCT;
                    
                    // 오토 Completed box - textbox로 변경
                    //Product
                    //List<DTO_DOC_MATERIAL_PRICE_PRODUCT> products = mgr.SelectMaterialPriceProduct(this.hddProcessID.Value);
                    //foreach (DTO_DOC_MATERIAL_PRICE_PRODUCT product in products)
                    //{
                    //    AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                    //    entry.Value = product.PRODUCT_CODE;
                    //    entry.Text = product.PRODUCT_NAME;
                    //    this.radAcomProduct.Entries.Add(entry);
                    //}

                    this.RadTextSnumber.Text = doc.SUPPLIER_NUMBER;
                    this.RadTextSname.Text = doc.SUPPLIER_NAME;
                    this.RadDropIncoterm.SelectedValue = doc.INCOTERMS;
                    this.RadDropPayment.SelectedValue = doc.PAYMENT_TERMS;
                    this.RadTextRemark.Text = doc.REMARK;

                    if (this.radRdoYes.Value == doc.IS_SECONDARY_SEAL)
                        this.radRdoYes.Checked = true;
                    else
                        this.radRdoNo.Checked = true;

                    List<DTO_DOC_MATERIAL_PRICE_LIST> materials = mgr.SelectMaterialPriceList(this.hddProcessID.Value);
                    ViewState[VIEWSTATE_KEY] = materials;

                    this.RadGrdMaterial.DataSource = materials;
                    this.RadGrdMaterial.DataBind();

                    webMaster.DocumentNo = doc.DOC_NUM;

                    if (RadRdoFormulation.Checked == true) materialtype = this.RadRdoFormulation.Value;
                    if (RadRdoFormulation.Checked != true) materialtype = "else";

                    if (!ClientScript.IsStartupScriptRegistered("fn_setVisible"))
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "fn_setVisible", "fn_setVisible('" + doc.CATEGORY_CODE + "','" + materialtype + "','" + doc.K_REACH_VERIFICATION + "');", true);
                    RadioCheck();
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
            List<DTO_DOC_MATERIAL_PRICE_LIST> items = (List<DTO_DOC_MATERIAL_PRICE_LIST>)serializer.Deserialize<List<DTO_DOC_MATERIAL_PRICE_LIST>>(this.hddGridItems.Value);

            ViewState[VIEWSTATE_KEY] = items;
            this.RadGrdMaterial.DataSource = items;
            this.RadGrdMaterial.DataBind();
        }
        

        if (radRdoCreate.Checked == true) category = this.radRdoCreate.Value;
        if (RadrdoChange.Checked == true) category = this.RadrdoChange.Value;
        if (RadRdoFormulation.Checked == true) materialtype = this.RadRdoFormulation.Value;
        if (RadRdoFormulation.Checked != true) materialtype = "else";
        if (RadRdoExist.Checked == true) counter = this.RadRdoExist.Value;
        if (RadRdoNew.Checked == true) counter = this.RadRdoNew.Value;

        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radBtnAdd, RadGrdMaterial, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoCreate, RadGrdMaterial, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadrdoChange, RadGrdMaterial, null);
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";

        if (!ClientScript.IsStartupScriptRegistered("fn_setVisible"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "fn_setVisible", "fn_setVisible('" + category + "','" + materialtype + "','" + counter + "');", true);

        
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

    private string GetCategory()
    {
        string category = string.Empty;
        if (this.radRdoCreate.Checked) category = this.radRdoCreate.Value;
        else if (this.RadrdoChange.Checked) category = this.RadrdoChange.Value;
        return category;
    }

    #region SaveDocument
    private string SaveDocument(ApprovalUtil.ApprovalStatus approvalStatus)
    {
        DTO_DOC_MATERIAL_PRICE doc = new DTO_DOC_MATERIAL_PRICE();

        string CreatePurpose = GetCreatePurpose();
        string ChangePurpose = GetChangePurpose();
        string MaterialType = GetMaterialType();

        doc.PROCESS_ID = this.hddProcessID.Value;
        //Subject
        doc.SUBJECT = GetMaterialTypeText() + "/" + this.RadTextSname.Text + "/" + GetCategory();                                 
        webMaster.Subject = doc.SUBJECT;
        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = approvalStatus.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;

        if (this.radRdoCreate.Checked)
        {
            doc.CATEGORY_CODE = radRdoCreate.Value;
            doc.PURPOSE_CODE = CreatePurpose;
        }
        if (this.RadrdoChange.Checked)
        {
            doc.CATEGORY_CODE = RadrdoChange.Value;
            doc.PURPOSE_CODE = ChangePurpose;
        }
        doc.MATERIAL_TYPE = MaterialType;
        doc.PRODUCT = this.RadTextProduct.Text;

        if (this.RadRdoExist.Checked) doc.K_REACH_VERIFICATION = RadRdoExist.Value;
        if (this.RadRdoNew.Checked) doc.K_REACH_VERIFICATION = RadRdoNew.Value;
        doc.COUNTER_MESSAGE = this.RadTextCounter.Text;
        doc.SUPPLIER_NUMBER = this.RadTextSnumber.Text;
        doc.SUPPLIER_NAME = this.RadTextSname.Text;
        doc.INCOTERMS = this.RadDropIncoterm.SelectedValue;
        doc.PAYMENT_TERMS = this.RadDropPayment.SelectedValue;
        doc.REMARK = this.RadTextRemark.Text;

        //2014.12.11 컬럼 추가 : IS_SECONDARY_SEAL
        if (this.radRdoYes.Checked == true) doc.IS_SECONDARY_SEAL = this.radRdoYes.Value; else doc.IS_SECONDARY_SEAL = this.radRdoNo.Value;

        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;

        //product - Use for (Product) - 
        //List<DTO_DOC_MATERIAL_PRICE_PRODUCT> products = new List<DTO_DOC_MATERIAL_PRICE_PRODUCT>();
        //foreach (AutoCompleteBoxEntry entry in this.radAcomProduct.Entries)
        //{
        //    DTO_DOC_MATERIAL_PRICE_PRODUCT product = new DTO_DOC_MATERIAL_PRICE_PRODUCT();
        //    product.PROCESS_ID = this.hddProcessID.Value;
        //    product.PRODUCT_CODE = entry.Value;
        //    product.PRODUCT_NAME = entry.Text;
        //    product.CREATOR_ID = Sessions.UserID;
        //    products.Add(product);
        //}

        //Grid
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_MATERIAL_PRICE_LIST> material = (List<DTO_DOC_MATERIAL_PRICE_LIST>)serializer.Deserialize<List<DTO_DOC_MATERIAL_PRICE_LIST>>(this.hddGridItems.Value);
        foreach (DTO_DOC_MATERIAL_PRICE_LIST item in material)
        {
            item.PROCESS_ID = this.hddProcessID.Value;
            item.CREATOR_ID = Sessions.UserID;
        }

        using (MaterialPriceMgr mgr = new MaterialPriceMgr())
        {
            return mgr.MergeMaterialPrice(doc, material);
        }
    }

    #region GET Radio Value
    private string GetChangePurpose()
    {
        string ChangePurpose = string.Empty;
        foreach (Control control in divChange.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    ChangePurpose = (control as RadButton).Value;
                    break;
                }
            }
        }
        return ChangePurpose;
    }

    private string GetCreatePurpose()
    {
        string createPurpose = string.Empty;
        foreach (Control control in divCreate.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    createPurpose = (control as RadButton).Value;
                    break;
                }
            }
        }
        return createPurpose;
    }

    private string GetMaterialType()
    {
        string MaterialType = string.Empty;
        foreach (Control control in divType.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    MaterialType = (control as RadButton).Value;
                    break;
                }
            }
        }
        return MaterialType;
    }

    private string GetMaterialTypeText()
    {
        string material = string.Empty;
        foreach (Control control in divType.Controls)
        {
            if(control is RadButton)
            {
                if ((control as RadButton).Checked) { 
                    material = (control as RadButton).Text; 
                    break;
                }
            }
        }
        return material;
    }
    #endregion

    #endregion

    #region ValidationCheck
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus approvalStatus)
    {
        string message = string.Empty;

        if (approvalStatus == ApprovalUtil.ApprovalStatus.Request)
        {
            //if (radRdoCreate.Checked == false && RadrdoChange.Checked == false)
            //    message = "Category";
            //if (radRdoCreate.Checked == true && GetCreatePurpose().IsNullOrEmptyEx())
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose";
            //if (RadrdoChange.Checked == true && GetChangePurpose().IsNullOrEmptyEx())
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose";
            //if (GetMaterialType().IsNullOrEmptyEx())
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Material Type";
            //if (RadTextProduct.Text.Length <= 0)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Use for (product)";
            //if (RadTextSnumber.Text.Length <= 0)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Supplier Number";
            //if (RadTextSname.Text.Length <= 0)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Supplier Name";
            //if (RadDropIncoterm.SelectedIndex == -1)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Incoterms";
            //if (RadDropPayment.SelectedIndex == -1)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Payment Terms";
            //if (RadTextRemark.Text.Length <= 0 )
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Remark";
            //if (radRdoYes.Checked == false && radRdoNo.Checked == false)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Secondary seal";
            //if (RadGrdMaterial.Items.Count < 1)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Material Information";
        }

        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;
    }
    #endregion

    #region GRID EVENT
    protected void radBtnAdd_Click(object sender, EventArgs e)
    {
        UpdateGridData(true);

    }

    private void UpdateGridData(bool addRow)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_MATERIAL_PRICE_LIST> items = (List<DTO_DOC_MATERIAL_PRICE_LIST>)serializer.Deserialize<List<DTO_DOC_MATERIAL_PRICE_LIST>>(this.hddGridItems.Value);

        if (addRow)
        {
            var lastData = items.OrderByDescending(d => d.IDX).FirstOrDefault();
            int newIdx = 1;
            if (lastData != null)
            {
                newIdx = Convert.ToInt32(lastData.IDX);
                newIdx++;
            }

            if (items == null) items = new List<DTO_DOC_MATERIAL_PRICE_LIST>();
            DTO_DOC_MATERIAL_PRICE_LIST item = new DTO_DOC_MATERIAL_PRICE_LIST();
            item.IDX = newIdx;
            item.UNIT_PRICE = 0;
            item.TO_BE_UNIT_PRICE = 0;
            items.Add(item);
        }
        
        ViewState[VIEWSTATE_KEY] = items;
        this.RadGrdMaterial.DataSource = items;
        this.RadGrdMaterial.DataBind();
    }

    protected void RadGrdMaterial_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
    }
    #endregion

    #region Grid Row Delete Event
    protected void RadGrdMaterial_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Remove"))
            {
                int index = Convert.ToInt32(e.CommandArgument);

                using (MaterialPriceDao Dao = new MaterialPriceDao())
                {
                    Dao.DeleteMaterialPriceList(this.hddProcessID.Value, index);
                }
                List<DTO_DOC_MATERIAL_PRICE_LIST> list = (List<DTO_DOC_MATERIAL_PRICE_LIST>)ViewState[VIEWSTATE_KEY];
                list.RemoveAll(p => p.IDX == index);               

                this.RadGrdMaterial.DataSource = list;
                this.RadGrdMaterial.DataBind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    #endregion    
    protected void RadrdoChange_Click(object sender, EventArgs e)
    {
        UpdateGridData(false);
        RadioCheck();
        if (this.hddGridItems.Value == "")
        {         
            ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_MATERIAL_PRICE_LIST>();
            this.RadGrdMaterial.DataSource = (List<DTO_DOC_MATERIAL_PRICE_LIST>)ViewState[VIEWSTATE_KEY];
            this.RadGrdMaterial.DataBind();
        }
        else
        {            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<DTO_DOC_MATERIAL_PRICE_LIST> items = (List<DTO_DOC_MATERIAL_PRICE_LIST>)serializer.Deserialize<List<DTO_DOC_MATERIAL_PRICE_LIST>>(this.hddGridItems.Value);
            ViewState[VIEWSTATE_KEY] = items;
            this.RadGrdMaterial.DataSource = items;
            this.RadGrdMaterial.DataBind();
        }
    }

    private void RadioCheck()
    {
        if (this.RadrdoChange.Checked == true)
        {
            this.RadGrdMaterial.MasterTableView.GetColumn("TO_BE_UNIT_PRICE").Display = true;
            this.RadGrdMaterial.MasterTableView.GetColumn("TO_BE_UNIT_PRICE").HeaderStyle.ForeColor = System.Drawing.Color.Red;
            this.RadGrdMaterial.MasterTableView.GetColumn("UNIT_PRICE").HeaderText = "Unit Price<br/>(As-Is)";

            hddGridItems.Value = null;
        }
        else if (this.radRdoCreate.Checked == true)
        {
            this.RadGrdMaterial.MasterTableView.GetColumn("TO_BE_UNIT_PRICE").Display = false;
            this.RadGrdMaterial.MasterTableView.GetColumn("UNIT_PRICE").HeaderText = "Unit Price<br/>(To-Is)";
        }
        RadGrdMaterial.Rebind();
    }    
}