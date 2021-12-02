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

public partial class Approval_Document_RebatePolicy : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_KEY_LIST = "VIEWSTATE_KEY_REBATE_POLICY_LIST";
    private const string VIEWSTATE_KEY_LIST_NEW = "VIEWSTATE_KEY_REBATE_POLICY_PRODUCE_NEW";


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

    protected override void OnPreRender(EventArgs e)
    {
        if (webMaster.ProcessStatus.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || webMaster.ProcessStatus.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString()))
        {
            Grid_reset();
        }
       
        base.OnPreRender(e);

    }
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //ViewState[VIEWSTATE_KEY_LIST] = new List<DTO_DOC_REBATE_POLICY_PRODUCT_LIST>();
               //this.radGrdProduct.DataSource = (List<DTO_DOC_REBATE_POLICY_PRODUCT_LIST>)ViewState[VIEWSTATE_KEY_LIST];
                //this.radGrdProduct.DataBind();

                



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
        hddDocumentID.Value = "D0035";
        //hddProcessID.Value = "";

        InitControls();
    }
    #endregion

    #region InitControl
    private void InitControls()
    {
        DTO_DOC_REBATE_POLICY doc;

        if (!hddProcessID.Value.Equals(string.Empty))
        {
            using(RebatePolicyMgr mgr = new RebatePolicyMgr())
            {
                doc = mgr.SelectRebatePolicy(hddProcessID.Value);
                if (doc != null)
                {
                    this.hddProcessStatus.Value = doc.PROCESS_STATUS;
                    // BG
                    foreach (Control control in this.divBG.Controls)
                    {
                        if (control is RadButton)
                        {
                            if ((control as RadButton).Value.Equals(doc.BG))
                            {
                                (control as RadButton).Checked = true; break;
                            }
                        }
                    }

                    //2014.12.18 Distribution channel 필드추가
                    if (doc.DISTRIBUTION == radRdoFM.Value) this.radRdoFM.Checked = true;
                    else if (doc.DISTRIBUTION == radRdoNH.Value) this.radRdoNH.Checked = true;

                    this.RadDateStart.SelectedDate = doc.START_DATE;
                    // 12월12일 remark field 추가
                    this.RadtxtRemark.Text = doc.REMARK;

                    //eWorkflow Optimization 2020 
                    this.radTxtCustomer.Text = doc.CUSTOMER;
                    this.radNumTotalAmount.Value = (double?)doc.TOTAL_AMOUNT;

                    if (doc.TYPE== RadrdoSellingCreate.Value) this.RadrdoSellingCreate.Checked = true;
                    else if(doc.TYPE == RadrdoSellingChange.Value) this.RadrdoSellingChange.Checked = true;
                    else if (doc.TYPE == RadrdoReturnCreate.Value) this.RadrdoReturnCreate.Checked = true;
                    else if (doc.TYPE == RadrdoReturnChange.Value) this.RadrdoReturnChange.Checked = true;



                    //Type
                    List<DTO_DOC_REBATE_POLICY_PRODUCT> products = mgr.SelectRebatePolicyProduct(hddProcessID.Value);       // 디비에서 가져온 LIST
                    List<DTO_DOC_REBATE_POLICY_PRODUCT_LIST> gridList = new List<DTO_DOC_REBATE_POLICY_PRODUCT_LIST>();     // 그리드에 뿌릴 LIST                   
                    if (doc.TYPE == RadrdoCreate.Value)
                    {
                        this.RadrdoCreate.Checked = true;
                        this.RadrdoCreate.Visible = true;
                        this.radGrdProduct.Visible = true;
                        this.radGrdProduct_NEW.Visible = false;
                        foreach (DTO_DOC_REBATE_POLICY_PRODUCT product in products)
                        {
                            DTO_DOC_REBATE_POLICY_PRODUCT_LIST griditem = new DTO_DOC_REBATE_POLICY_PRODUCT_LIST();
                            
                            griditem.PROCESS_ID = product.PROCESS_ID;
                            griditem.CREATOR_ID = product.CREATOR_ID;
                            griditem.AS_TO = "TO-BE";
                            griditem.PRODUCT_CODE = product.TO_BE_PRODUCT_CODE;
                            griditem.PRODUCT_NAME = product.TO_BE_PRODUCT_NAME;
                            //griditem.CHANNEL_CODE = product.TO_BE_CHANNEL_CODE;
                            //griditem.CHANNEL_NAME = product.TO_BE_CHANNEL_NAME;
                            //griditem.DISTRIBUTION = product.TO_BE_DISTRIBUTION;
                            griditem.LIST = product.TO_BE_LIST;
                            griditem.INVOICE = product.TO_BE_INVOICE;
                            griditem.NET1 = product.TO_BE_NET1;
                            griditem.NET2 = product.TO_BE_NET2;

                            gridList.Add(griditem);
                        }
                        gridList.ToList();
                        ViewState[VIEWSTATE_KEY_LIST] = gridList;
                        this.radGrdProduct.DataSource = gridList;
                        this.radGrdProduct.DataBind();

                    }

                    if (doc.TYPE == RadrdoChange.Value)
                    {
                        this.RadrdoChange.Checked = true;
                        this.RadrdoChange.Visible = true;
                        this.radGrdProduct.Visible = true;
                        this.radGrdProduct_NEW.Visible = false;
                        foreach (GridColumn column in radGrdProduct.Columns)
                        {
                            if (column.UniqueName == "AS_TO")
                                (column as GridBoundColumn).Display = true;
                        }
                        bool multi = true;              // 그리드에  2줄로 넣기 위한 구분자
                        for (int i = 0; i < products.Count; i++)
                        {
                            if (multi)
                            {
                                DTO_DOC_REBATE_POLICY_PRODUCT_LIST griditem = new DTO_DOC_REBATE_POLICY_PRODUCT_LIST();
                                griditem.AS_TO = "AS-IS";
                                griditem.PROCESS_ID = products[i].PROCESS_ID;
                                griditem.CREATOR_ID = products[i].CREATOR_ID;
                                griditem.PRODUCT_CODE = products[i].AS_IS_PRODUCT_CODE;
                                griditem.PRODUCT_NAME = products[i].AS_IS_PRODUCT_NAME;
                                //griditem.CHANNEL_CODE = products[i].AS_IS_CHANNEL_CODE;
                                //griditem.CHANNEL_NAME = products[i].AS_IS_CHANNEL_NAME;
                                //griditem.DISTRIBUTION = products[i].AS_IS_DISTRIBUTION;
                                griditem.LIST = products[i].AS_IS_LIST;
                                griditem.INVOICE = products[i].AS_IS_INVOICE;
                                griditem.NET1 = products[i].AS_IS_NET1;
                                griditem.NET2 = products[i].AS_IS_NET2;

                                gridList.Add(griditem);   // AS-IS Row
                            }
                            if (multi)
                            {
                                DTO_DOC_REBATE_POLICY_PRODUCT_LIST griditem = new DTO_DOC_REBATE_POLICY_PRODUCT_LIST();
                                griditem.AS_TO = "TO-BE";
                                griditem.PRODUCT_CODE = products[i].TO_BE_PRODUCT_CODE;
                                griditem.PRODUCT_NAME = products[i].TO_BE_PRODUCT_NAME;
                                //griditem.CHANNEL_CODE = products[i].TO_BE_CHANNEL_CODE;
                                //griditem.CHANNEL_NAME = products[i].TO_BE_CHANNEL_NAME;
                                //griditem.DISTRIBUTION = products[i].TO_BE_DISTRIBUTION;
                                griditem.LIST = products[i].TO_BE_LIST;
                                griditem.INVOICE = products[i].TO_BE_INVOICE;
                                griditem.NET1 = products[i].TO_BE_NET1;
                                griditem.NET2 = products[i].TO_BE_NET2;

                                gridList.Add(griditem);   // TO-BE Row
                            }
                        }
                        gridList.ToList();
                        ViewState[VIEWSTATE_KEY_LIST] = gridList;
                        this.radGrdProduct.DataSource = gridList;
                        this.radGrdProduct.DataBind();
                    }

                    if (this.radGrdProduct_NEW.Visible) { 
                        List<DTO_DOC_REBATE_POLICY_PRODUCT_NEW> products_NEW = mgr.SelectRebatePolicyProduct_NEW(hddProcessID.Value);
                        ViewState[VIEWSTATE_KEY_LIST_NEW] = products_NEW;
                        this.radGrdProduct_NEW.DataSource = products_NEW;
                        this.radGrdProduct_NEW.DataBind();
                    }
                    
                    webMaster.DocumentNo = doc.DOC_NUM;

                    if (!ClientScript.IsStartupScriptRegistered("setVisible"))
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisible", "setVisible('" + doc.BG + "');", true);
                }
            }

        }
        else
        {
            ViewState[VIEWSTATE_KEY_LIST_NEW] = new List<DTO_DOC_REBATE_POLICY_PRODUCT_NEW>();
            this.radGrdProduct_NEW.DataSource = (List<DTO_DOC_REBATE_POLICY_PRODUCT_NEW>)ViewState[VIEWSTATE_KEY_LIST_NEW];
            this.radGrdProduct_NEW.DataBind();
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
            List<DTO_DOC_REBATE_POLICY_PRODUCT_LIST> items = (List<DTO_DOC_REBATE_POLICY_PRODUCT_LIST>)serializer.Deserialize<List<DTO_DOC_REBATE_POLICY_PRODUCT_LIST>>(this.hddGridItems.Value);

            ViewState[VIEWSTATE_KEY_LIST] = items;
            this.radGrdProduct.DataSource = items;
            this.radGrdProduct.DataBind();
        }

        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadrdoCreate, radGrdProduct, null);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadrdoChange, radGrdProduct, null);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoFM, radGrdProduct, null);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoNH, radGrdProduct, null);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdProduct);


        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadrdoSellingCreate, radGrdProduct_NEW, null);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadrdoSellingChange, radGrdProduct_NEW, null);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadrdoReturnCreate, radGrdProduct_NEW, null);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadrdoReturnChange, radGrdProduct_NEW, null);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoFM, radGrdProduct_NEW, null);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoNH, radGrdProduct_NEW, null);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.RadrdoSellingCreate, this.radGrdProduct_NEW);

        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.RadrdoSellingCreate, this.radGrdProduct_NEW);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.RadrdoSellingChange, this.radGrdProduct_NEW);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.RadrdoReturnCreate, this.radGrdProduct_NEW);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.RadrdoReturnChange, this.radGrdProduct_NEW);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radRdoFM, this.radGrdProduct_NEW);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radRdoNH, this.radGrdProduct_NEW);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radRdoCP, this.radGrdProduct_NEW);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radRdoIS, this.radGrdProduct_NEW);        
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radRdoES, this.radGrdProduct_NEW);
       



        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdProduct_NEW);

        

        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_RebatePolicy_AjaxRequest;
        //RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit_NEW";
        if (this.hddGridItems_NEW.Value.IsNotNullOrEmptyEx())
        {
            UpdateGridData_NEW();
        }

        Radio = GetSelectedBG();

        if (!ClientScript.IsStartupScriptRegistered("setVisible"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisible", "setVisible('" + Radio + "');", true);
        Grid_reset();
    }

    private void Approval_Document_RebatePolicy_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("Rebind"))
        {
            var RebateType = GetSelectedRebateType();
            if (RebateType == "Change" || RebateType == "Create")
            UpdateGridData();
            else
            UpdateGridData_NEW();
            this.hddAddRow.Value = "Y";
        }
        else
        {
            this.hddAddRow.Value = "N";
        }

    }
    #endregion

    /// <summary>
    /// 그리드 이벤트
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    #region [ Add Row ]
    private void UpdateGridData()
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_REBATE_POLICY_PRODUCT_LIST> items = (List<DTO_DOC_REBATE_POLICY_PRODUCT_LIST>)serializer.Deserialize<List<DTO_DOC_REBATE_POLICY_PRODUCT_LIST>>(this.hddGridItems.Value);

        ViewState[VIEWSTATE_KEY_LIST] = items;
        this.radGrdProduct.DataSource = items;
        this.radGrdProduct.DataBind();
    }

    private void UpdateGridData_NEW()
    {

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_REBATE_POLICY_PRODUCT_NEW> items
            = (List<DTO_DOC_REBATE_POLICY_PRODUCT_NEW>)serializer.Deserialize<List<DTO_DOC_REBATE_POLICY_PRODUCT_NEW>>(this.hddGridItems_NEW.Value);

        foreach (DTO_DOC_REBATE_POLICY_PRODUCT_NEW product in items)
        {
            //product.AMOUNT = product.DISCOUNT_AMOUNT * product.QTY;
        }
        ViewState[VIEWSTATE_KEY_LIST_NEW] = items;
        this.radGrdProduct_NEW.DataSource = items;
        this.radGrdProduct_NEW.DataBind();

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
        DTO_DOC_REBATE_POLICY doc = new DTO_DOC_REBATE_POLICY();

        doc.PROCESS_ID = this.hddProcessID.Value;
        if (this.RadDateStart.SelectedDate.HasValue)
            doc.SUBJECT = GetSelectedBG() + "/" + GetSelectedRebateType() + "/" + this.RadDateStart.SelectedDate.Value.ToString("yyyy-MM-dd");   // subject
        else doc.SUBJECT = GetSelectedBG() + "/" + GetSelectedRebateType();
        webMaster.Subject = doc.SUBJECT;

        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = approvalStatus.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;

        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;

        string SelectedBG = GetSelectedBG();
        doc.BG = SelectedBG;

        //2014.12.12 컬럼 추가 : Distribuution channel 필드추가 
        if (SelectedBG == "CP")
        {
            if (radRdoFM.Checked) doc.DISTRIBUTION = radRdoFM.Value;
            else if (radRdoNH.Checked) doc.DISTRIBUTION = radRdoNH.Value;
        }

        if (this.RadrdoCreate.Checked == true) doc.TYPE = this.RadrdoCreate.Value;
        else if (this.RadrdoChange.Checked == true) doc.TYPE = this.RadrdoChange.Value;
        else if (this.RadrdoReturnChange.Checked == true) doc.TYPE = this.RadrdoReturnChange.Value;
        else if (this.RadrdoSellingCreate.Checked == true) doc.TYPE = this.RadrdoSellingCreate.Value;
        else if (this.RadrdoSellingChange.Checked == true) doc.TYPE = this.RadrdoSellingChange.Value;
        else if (this.RadrdoReturnCreate.Checked == true) doc.TYPE = this.RadrdoReturnCreate.Value;

        doc.START_DATE = this.RadDateStart.SelectedDate;
        //eWorkflow Optimization 2020
        doc.CUSTOMER = this.radTxtCustomer.Text;
        if (radNumTotalAmount.Text.Length > 0)
            doc.TOTAL_AMOUNT = Convert.ToDecimal(radNumTotalAmount.Text);
        
        //eWorkflow Optimization 2020

        doc.REMARK = this.RadtxtRemark.Text;
        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;
        doc.CREATE_DATE = DateTime.Now;
        List<DTO_DOC_REBATE_POLICY_PRODUCT> products = new List<DTO_DOC_REBATE_POLICY_PRODUCT>();
       ;
        var RebateType = GetSelectedRebateType();
        if (RebateType == "Change" || RebateType == "Create")
        {
           

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<DTO_DOC_REBATE_POLICY_PRODUCT_LIST> GridItem = (List<DTO_DOC_REBATE_POLICY_PRODUCT_LIST>)serializer.Deserialize<List<DTO_DOC_REBATE_POLICY_PRODUCT_LIST>>(this.hddGridItems.Value);   //그리드의 DTO

            if (this.RadrdoCreate.Checked == true)      //Create
            {
                foreach (DTO_DOC_REBATE_POLICY_PRODUCT_LIST item in GridItem)
                {
                    DTO_DOC_REBATE_POLICY_PRODUCT product = new DTO_DOC_REBATE_POLICY_PRODUCT();
                    product.PROCESS_ID = this.hddProcessID.Value;
                    product.CREATOR_ID = Sessions.UserID;
                    product.TO_BE_PRODUCT_CODE = item.PRODUCT_CODE;
                    product.TO_BE_PRODUCT_NAME = item.PRODUCT_NAME;
                    //product.TO_BE_CHANNEL_CODE = item.CHANNEL_CODE;
                    //product.TO_BE_CHANNEL_NAME = item.CHANNEL_NAME;
                    //product.TO_BE_DISTRIBUTION = item.DISTRIBUTION;
                    product.TO_BE_LIST = item.LIST;
                    product.TO_BE_INVOICE = item.INVOICE;
                    product.TO_BE_NET1 = item.NET1;
                    product.TO_BE_NET2 = item.NET2;

                    products.Add(product);
                }
                products.ToList();
            }
            else if (this.RadrdoChange.Checked == true)    //Change
            {
                for (int i = 0; i < GridItem.Count; i = i + 2)
                {
                    DTO_DOC_REBATE_POLICY_PRODUCT product = new DTO_DOC_REBATE_POLICY_PRODUCT();
                    //AS-IS
                    product.PROCESS_ID = this.hddProcessID.Value;
                    product.CREATOR_ID = Sessions.UserID;
                    product.AS_IS_PRODUCT_CODE = GridItem[i].PRODUCT_CODE;
                    product.AS_IS_PRODUCT_NAME = GridItem[i].PRODUCT_NAME;
                    //product.AS_IS_CHANNEL_CODE = GridItem[i].CHANNEL_CODE;
                    //product.AS_IS_CHANNEL_NAME = GridItem[i].CHANNEL_NAME;
                    //product.AS_IS_DISTRIBUTION = GridItem[i].DISTRIBUTION;
                    product.AS_IS_LIST = GridItem[i].LIST;
                    product.AS_IS_INVOICE = GridItem[i].INVOICE;
                    product.AS_IS_NET1 = GridItem[i].NET1;
                    product.AS_IS_NET2 = GridItem[i].NET2;
                    //TO-BE
                    product.TO_BE_PRODUCT_CODE = GridItem[i + 1].PRODUCT_CODE;
                    product.TO_BE_PRODUCT_NAME = GridItem[i + 1].PRODUCT_NAME;
                    //product.TO_BE_CHANNEL_CODE = GridItem[i + 1].CHANNEL_CODE;
                    //product.TO_BE_CHANNEL_NAME = GridItem[i + 1].CHANNEL_NAME;
                    //product.TO_BE_DISTRIBUTION = GridItem[i + 1].DISTRIBUTION;
                    product.TO_BE_LIST = GridItem[i + 1].LIST;
                    product.TO_BE_INVOICE = GridItem[i + 1].INVOICE;
                    product.TO_BE_NET1 = GridItem[i + 1].NET1;
                    product.TO_BE_NET2 = GridItem[i + 1].NET2;
                    products.Add(product);

                }
                products.ToList();
            }
            
        }
        else
        {
           

        }
        List<DTO_DOC_REBATE_POLICY_PRODUCT_NEW> products_NEW = (List<DTO_DOC_REBATE_POLICY_PRODUCT_NEW>)ViewState[VIEWSTATE_KEY_LIST_NEW];
        foreach (DTO_DOC_REBATE_POLICY_PRODUCT_NEW product in products_NEW)
        {
            product.PROCESS_ID = this.hddProcessID.Value;
            product.CREATOR_ID = Sessions.UserID;
        }
        
        using (RebatePolicyMgr mgr = new RebatePolicyMgr())
        {
            //var RebateType = GetSelectedRebateType();
            if (RebateType == "Change" || RebateType == "Create")
            {
                return mgr.MergeRebatePolicy(doc, products);
            }
            else
            {
                return mgr.MergeRebatePolicy_NEW(doc, products_NEW);
            }
                //return mgr.MergeRebatePolicy(doc, products);
                
        }


    }

    #endregion

    #region ValidationCheck
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus approvalStatus)
    {
        UpdateGridData_NEW();
        string message = string.Empty;
        
        if (approvalStatus == ApprovalUtil.ApprovalStatus.Request)
        {
            if (GetSelectedBG().IsNullOrEmptyEx())
                message = "BG";

            //if (radRdoCP.Checked == true && radRdoFM.Checked == false && radRdoNH.Checked == false)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Distribution channel";

            if (RadrdoCreate.Checked == false && RadrdoChange.Checked == false && RadrdoSellingCreate.Checked == false && RadrdoSellingChange.Checked == false && RadrdoReturnCreate.Checked == false && RadrdoReturnChange.Checked == false)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Type";

            if (!RadDateStart.SelectedDate.HasValue)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Start Date";
        }

        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;
    }
    #endregion

    /// <summary>
    /// BG 반환
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// TYPE 반환
    /// </summary>
    /// <returns></returns>
    private string GetSelectedRebateType()
    {
        string type = string.Empty;
        if (this.RadrdoCreate.Checked) type = this.RadrdoCreate.Value;
        else if (this.RadrdoChange.Checked) type = this.RadrdoChange.Value;
        else if (this.RadrdoSellingCreate.Checked) type = this.RadrdoSellingCreate.Value;
        else if (this.RadrdoSellingChange.Checked) type = this.RadrdoSellingChange.Value;
        else if (this.RadrdoReturnCreate.Checked) type = this.RadrdoReturnCreate.Value;
        else if (this.RadrdoReturnChange.Checked) type = this.RadrdoReturnChange.Value;

        return type;
    }

    #region ItemDataBound
    protected void radGrdProduct_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {            

            GridDataItem item = (e.Item as GridDataItem);
            if (item["AS_TO"].Text == "TO-BE")
            {
                item["AS_TO"].ForeColor = System.Drawing.Color.Red;
                item["LIST"].ForeColor = System.Drawing.Color.Red;
                item["INVOICE"].ForeColor = System.Drawing.Color.Red;
                item["NET1"].ForeColor = System.Drawing.Color.Red;
                item["NET2"].ForeColor = System.Drawing.Color.Red;                
            }
        }
    }

    protected void radGrdProduct_ItemDataBound_NEW(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;

        }
    }
    #endregion


    protected void RadRebateType_Click(object sender,EventArgs e)
    {
        Grid_reset();
        List<DTO_DOC_REBATE_POLICY_PRODUCT_NEW> list = (List<DTO_DOC_REBATE_POLICY_PRODUCT_NEW>)ViewState[VIEWSTATE_KEY_LIST_NEW];
        list.Clear();
        this.radGrdProduct_NEW.DataSource = list;
        this.radGrdProduct_NEW.DataBind();
        

    }

    private void Grid_reset()
    {
        var RebateType = GetSelectedRebateType();
        this.radGrdProduct_NEW.MasterTableView.GetColumn("LIST_PRICE").Display = false;
        this.radGrdProduct_NEW.MasterTableView.GetColumn("LIST_PRICE_NEW").Display = false;
        this.radGrdProduct_NEW.MasterTableView.GetColumn("INVOICE_PRICE").Display = false;
        this.radGrdProduct_NEW.MasterTableView.GetColumn("INVOICE_PRICE_NEW").Display = false;
        this.radGrdProduct_NEW.MasterTableView.GetColumn("NET_PRICE_NEW").Display = false;
        this.radGrdProduct_NEW.MasterTableView.GetColumn("SELLING_DIFF").Display = false;
        this.radGrdProduct_NEW.MasterTableView.GetColumn("SELLING_EXPECTED").Display = false;
        this.radGrdProduct_NEW.MasterTableView.GetColumn("SELLING_TOTAL").Display = false;
        this.radGrdProduct_NEW.MasterTableView.GetColumn("RETURN_PRICE").Display = false;
        this.radGrdProduct_NEW.MasterTableView.GetColumn("RETURN_PRICE_NEW").Display = false;
        this.radGrdProduct_NEW.MasterTableView.GetColumn("RETURN_DIFF").Display = false;
        this.radGrdProduct_NEW.MasterTableView.GetColumn("RETURN_EXPECTED").Display = false;
        this.radGrdProduct_NEW.MasterTableView.GetColumn("RETURN_TOTAL").Display = false;

        if (RebateType.Equals("SellingCreate"))
        {
            this.radGrdProduct_NEW.MasterTableView.GetColumn("LIST_PRICE_NEW").Display = true;
            this.radGrdProduct_NEW.MasterTableView.GetColumn("INVOICE_PRICE_NEW").Display = true;
            this.radGrdProduct_NEW.MasterTableView.GetColumn("NET_PRICE_NEW").Display = true;
            this.radGrdProduct_NEW.MasterTableView.GetColumn("SELLING_EXPECTED").Display = true;
            this.radGrdProduct_NEW.MasterTableView.GetColumn("SELLING_TOTAL").Display = true;
        }
        else if (RebateType.Equals("SellingChange"))
        {
            this.radGrdProduct_NEW.MasterTableView.GetColumn("LIST_PRICE").Display = true;
            this.radGrdProduct_NEW.MasterTableView.GetColumn("INVOICE_PRICE").Display = true;
            this.radGrdProduct_NEW.MasterTableView.GetColumn("INVOICE_PRICE_NEW").Display = true;
            this.radGrdProduct_NEW.MasterTableView.GetColumn("SELLING_DIFF").Display = true;
            this.radGrdProduct_NEW.MasterTableView.GetColumn("SELLING_EXPECTED").Display = true;
            this.radGrdProduct_NEW.MasterTableView.GetColumn("SELLING_TOTAL").Display = true;
        }
        else if (RebateType.Equals("ReturnCreate"))
        {
            this.radGrdProduct_NEW.MasterTableView.GetColumn("RETURN_PRICE").Display = true;
            this.radGrdProduct_NEW.MasterTableView.GetColumn("RETURN_PRICE_NEW").Display = true;
            this.radGrdProduct_NEW.MasterTableView.GetColumn("RETURN_DIFF").Display = true;
            this.radGrdProduct_NEW.MasterTableView.GetColumn("RETURN_EXPECTED").Display = true;
            this.radGrdProduct_NEW.MasterTableView.GetColumn("RETURN_TOTAL").Display = true;
        }
        else if (RebateType.Equals("ReturnChange"))
        {
            this.radGrdProduct_NEW.MasterTableView.GetColumn("RETURN_PRICE").Display = true;
            this.radGrdProduct_NEW.MasterTableView.GetColumn("RETURN_PRICE_NEW").Display = true;
            this.radGrdProduct_NEW.MasterTableView.GetColumn("RETURN_DIFF").Display = true;
            this.radGrdProduct_NEW.MasterTableView.GetColumn("RETURN_EXPECTED").Display = true;
            this.radGrdProduct_NEW.MasterTableView.GetColumn("RETURN_TOTAL").Display = true;
        }
        this.radGrdProduct_NEW.Rebind();
    }



    // Grid RESET
    protected void RadGrd_Reset(object sender, EventArgs e)
    {
        var RebateType = GetSelectedRebateType();
        if (RebateType == "Change" || RebateType == "Create") { 
            if (this.RadrdoCreate.Checked == true)
            {
                //this.hddType.Value = RadrdoCreate.Value;
                foreach (GridColumn column in radGrdProduct.Columns)
                {
                    if (column.UniqueName == "AS_TO")
                        (column as GridBoundColumn).Display = false;
                }
            }
        
            if (this.RadrdoChange.Checked == true)
            {
                //this.hddType.Value = RadrdoChange.Value;
                foreach (GridColumn column in radGrdProduct.Columns)
                {
                    if (column.UniqueName == "AS_TO")
                        (column as GridBoundColumn).Display = true;
                }
            }
            ViewState[VIEWSTATE_KEY_LIST] = new List<DTO_DOC_REBATE_POLICY_PRODUCT_LIST>();
            this.radGrdProduct.DataSource = (List<DTO_DOC_REBATE_POLICY_PRODUCT_LIST>)ViewState[VIEWSTATE_KEY_LIST];
            this.radGrdProduct.DataBind();
            
        }
        else
        {
            List<DTO_DOC_REBATE_POLICY_PRODUCT_NEW> list = (List<DTO_DOC_REBATE_POLICY_PRODUCT_NEW>)ViewState[VIEWSTATE_KEY_LIST_NEW];
            list.Clear();
            this.radGrdProduct_NEW.DataSource = list;
            this.radGrdProduct_NEW.DataBind();
        }
        Grid_reset();


    }


    #region Grid PreRender Event
    protected void radGrdProduct_PreRender(object sender, EventArgs e)
    {
        var RebateType = GetSelectedRebateType();
        if (RebateType == "Change" || this.RadrdoChange.Checked == true)
        {
            for (int rowIndex = radGrdProduct.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridDataItem row = radGrdProduct.Items[rowIndex];
                GridDataItem previousRow = radGrdProduct.Items[rowIndex + 1];

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (row["PRODUCT_NAME"].Text == previousRow["PRODUCT_NAME"].Text)
                    {
                        row["PRODUCT_NAME"].RowSpan = previousRow["PRODUCT_NAME"].RowSpan < 2 ? 2 : previousRow["PRODUCT_NAME"].RowSpan + 1;
                        previousRow["PRODUCT_NAME"].Attributes.CssStyle.Add("Display", "none");                        
                    }

                    if (row["PRODUCT_NAME"].Text == previousRow["PRODUCT_NAME"].Text)
                    {
                        row["REMOVE_BUTTON"].RowSpan = previousRow["REMOVE_BUTTON"].RowSpan < 2 ? 2 : previousRow["REMOVE_BUTTON"].RowSpan + 1;
                        previousRow["REMOVE_BUTTON"].Attributes.CssStyle.Add("Display", "none");
                    }                   
                }
            }
        }
    } 
    #endregion
   

    #region Delete Event
    protected void radGrdProduct_ItemCommand(object sender, GridCommandEventArgs e)
    {
         try
        {
            if (e.CommandName.Equals("Remove"))
            {
                string productCode = Convert.ToString(e.CommandArgument);

                using (RebatePolicyDao Dao = new RebatePolicyDao())
                {
                    Dao.DeleteRebatePolicyProduct(this.hddProcessID.Value, productCode);
                }

                List<DTO_DOC_REBATE_POLICY_PRODUCT_LIST> list = (List<DTO_DOC_REBATE_POLICY_PRODUCT_LIST>)ViewState[VIEWSTATE_KEY_LIST];
                list.RemoveAll(p => p.PRODUCT_CODE == productCode);

                this.radGrdProduct.DataSource = list;
                this.radGrdProduct.DataBind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    protected void radGrdProduct_ItemCommand_NEW(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Remove"))
            {
                UpdateGridData_NEW();
                string productCode = Convert.ToString(e.CommandArgument);

                using (RebatePolicyDao Dao = new RebatePolicyDao())
                {
                    Dao.DeleteRebatePolicyProduct_NEW(this.hddProcessID.Value, productCode);
                }

                List<DTO_DOC_REBATE_POLICY_PRODUCT_NEW> list = (List<DTO_DOC_REBATE_POLICY_PRODUCT_NEW>)ViewState[VIEWSTATE_KEY_LIST_NEW];
                list.RemoveAll(p => p.PRODUCT_CODE == productCode);

                this.radGrdProduct_NEW.DataSource = list;
                this.radGrdProduct_NEW.DataBind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    #endregion

}