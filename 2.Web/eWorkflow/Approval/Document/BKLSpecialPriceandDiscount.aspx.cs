using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DNSoft.eW.FrameWork;
using Telerik.Web.UI;
using System.Web.Script.Serialization;
using Bayer.eWF.BSL.Approval.Dto;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Approval.Mgr;

public partial class Approval_Document_BKLSpecialPriceandDiscount : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_KEY = "VIEWSTATE_KEY_PRODUCT";
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

    protected override void OnPreRender(EventArgs e)
    {
        if (webMaster.ProcessStatus.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || webMaster.ProcessStatus.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString()))
        {
            radRadioCheck();
        }
        if (webMaster.DocumentNo != "")
        {
            webMaster.SetEnableControls(Dealno, true);

        }
        base.OnPreRender(e);
        
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                this.hddCompanyCode.Value = Sessions.CompanyCode; //회사코드 설정
             //   ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT>();
             //   this.radGrdProduct.DataSource = (List<DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT>)ViewState[VIEWSTATE_KEY];
             //   this.radGrdProduct.DataBind();
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
        hddDocumentID.Value = "D0058";
        //hddProcessID.Value = "";


        InitControls();
    }

    private void InitControls()
    {
        if (!this.hddProcessID.Value.IsNullOrEmptyEx())
        {
            using (BKLSpecialPriceandDiscountMgr mgr = new BKLSpecialPriceandDiscountMgr())
            {

                Tuple<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT
                , List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_CUSTOMER>
                , List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_HOSPITAL>
                , List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT>> doc = mgr.SelectBKLPriceAndMargin(this.hddProcessID.Value);

                this.HddProcessStatus.Value = doc.Item1.PROCESS_STATUS;
               
                foreach (Control control in divSalesGroup.Controls)
                {
                    if (control is RadButton)
                    {
                        if ((control as RadButton).Value == doc.Item1.BU)
                        {
                            (control as RadButton).Checked = true;
                            break;
                        }
                    }
                }

                //EXCEPTION
                foreach (Control control in this.divExceptionGroup.Controls)
                {
                    if (control is RadButton)
                    {
                        if ((control as RadButton).Value == doc.Item1.EXCEPTION)
                        {
                            (control as RadButton).Checked = true;
                            break;
                        }
                    }
                }

                //Saem Discount
                foreach (Control control in this.divSameDiscountGroup.Controls)
                {
                    if (control is RadButton)
                    {
                        if ((control as RadButton).Value == doc.Item1.SAMEDISCOUNT)
                        {
                            (control as RadButton).Checked = true;
                            break;
                        }
                    }
                }

                ////Product Family

                //foreach (Control control in divProductFamily.Controls)
                //{
                //    if (control is RadButton)
                //    {
                //        if ((control as RadButton).Value == doc.Item1.PRODUCT_FAMILY)
                //        {
                //            (control as RadButton).Checked = true;
                //            break;
                //        }
                //    }
                //}

                //Attachment Check
                foreach (string AttachCheck in doc.Item1.ATTACH_CHECK.Split(new char[] { ';' }))
                {
                    foreach (Control control in this.divCheck.Controls)
                    {
                        if (control is RadButton)
                        {
                            if ((control as RadButton).Value.Equals(AttachCheck))
                            {
                                (control as RadButton).Checked = true; break;
                            }
                        }
                    }
                }

                //TYPE
                if (doc.Item1.BU != "AH")
                {
                    
                    if (doc.Item1.TYPE.Equals(this.radBtnBasic.Value)) this.radBtnBasic.Checked = true;
                    else if (doc.Item1.TYPE.Equals(this.radBtnWRP.Value)) this.radBtnWRP.Checked = true;                    
                    else if (doc.Item1.TYPE.Equals(this.radBtnWORP.Value)) this.radBtnWORP.Checked = true;
                    else if (doc.Item1.TYPE.Equals(this.radBtnCustomer.Value)) this.radBtnCustomer.Checked = true;
                }


                //<% --2022.05.27 add for change INC14831972 start-- %>
                //ApprovalContext.USP_SELECT_BKL_SPECIAL_PRICE_AND_DISCOUNT, 页面load从SP取数据
                if (this.radBtnExceptionYes.Value == "YES") 
                {
                    //if  (this.radBtnDiscountNo.Value == "YES")
                    //{
                        this.RadTextBackground.Text = doc.Item1.BACKGROUND;
                        this.RadTextProposal.Text = doc.Item1.PROPOSAL;
                        this.RadTextProcess.Text = doc.Item1.PROCESS;
                        this.RadTextFinancial.Text = doc.Item1.FINANCIAL_IMPACT;
                        this.RadTextExceptionComment.Text = doc.Item1.COMMENT;
                    //}

                    //if (this.radBtnDiscountYes.Value == "YES") 
                    //{
                        this.RadTextSPPriceExcp.Text = doc.Item1.SPPriceExcp;
                    //}
                }

                //<%--                2022.05.27 add for change INC14831972 start， issue code，no need to recover--%>
                ////this.RadTextBackground.Text = doc.Item1.BACKGROUND;
                ////this.RadTextProposal.Text = doc.Item1.PROPOSAL;
                ////this.RadTextProcess.Text = doc.Item1.PROCESS;
                ////this.RadTextFinancial.Text = doc.Item1.FINANCIAL_IMPACT;
                ////this.RadTextExceptionComment.Text = doc.Item1.COMMENT;
                //<%--                2022.05.27 add for change INC14831972 end--%>


                // 2nd Wholesaler
                this.radTxtSecondWholesaler.Text = doc.Item1.SECOND_WHOLESALER;

                this.radDatPeriodFrom.SelectedDate = doc.Item1.CONTRACT_FROM;
                this.radDatPeriodTo.SelectedDate = doc.Item1.CONTRACT_TO;
                this.radDatPeriodFrom_W_BKL.SelectedDate = doc.Item1.CONTRACT_FROM_W_BKL;
                this.radDatPeriodTo_W_BKL.SelectedDate = doc.Item1.CONTRACT_TO_W_BKL;

                // Extension Agreement Check (2018-04-06:Youngwoo Lee)
                if (doc.Item1.EXTENTION_AGREEMENT_CHECK.Equals(this.radBtnExtension.Value))
                {
                    this.radBtnExtension.Checked = true;
                    this.radTxtRemark.Text = doc.Item1.REMARK;
                    this.radTxtReDocNo.Text = doc.Item1.RE_EWORKFLOW_DOC_NO;


                    this.radDatPeriodFromExtention.SelectedDate = doc.Item1.EXTENSION_FROM_W_BKL;

                    this.radDatPeriodToExtention.SelectedDate = doc.Item1.EXTENSION_TO_W_BKL;
                }

                this.radNumTotalAmount.Value = (double?)doc.Item1.TOTAL_AMOUNT;
                if (doc.Item1.DOC_NUM != "")
                {
                    Dealno.Attributes.CssStyle.Add("display", "block");
                    txtDealno.Attributes.CssStyle.Add("border-bottom", "1px solid red");
                    txtDealno.Attributes.CssStyle.Add("padding-bottom", "3px");
                    txtDealno.Text = doc.Item1.DEAL_NO;

                    if (doc.Item1.RE_EWORKFLOW_DOC_NO.IsNullOrEmptyEx())
                        radTxtReDocNo.Text = doc.Item1.DOC_NUM;
                    else
                        radTxtReDocNo.Text = doc.Item1.RE_EWORKFLOW_DOC_NO;

                }
                webMaster.DocumentNo = doc.Item1.DOC_NUM;

                List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_CUSTOMER> customers = doc.Item2;
                foreach (DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_CUSTOMER customer in customers)
                {
                    AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                    entry.Value = customer.CUSTOMER_CODE;
                    entry.Text = customer.CUSTOMER_NAME;
                    this.radAcomWholesaler.Entries.Add(entry);
                }

                List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_HOSPITAL> hospitals = doc.Item3;
                foreach (DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_HOSPITAL hospital in hospitals)
                {
                    AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                    entry.Value = hospital.HOSPITAL_CODE;
                    entry.Text = hospital.HOSPITAL_NAME;
                    this.radAcomHospital.Entries.Add(entry);
                }

                List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT> products = doc.Item4;
                ViewState[VIEWSTATE_KEY] = products;


                this.radGrdProduct.DataSource = products;
                this.radGrdProduct.DataBind();

                if (!(doc.Item1.PROCESS_STATUS.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || doc.Item1.PROCESS_STATUS.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString())))
                {
                    if (hospitals.Count < 1)
                    {
                        this.radAcomHospital.Visible = false;
                        this.lblNotHospital.Visible = true;
                    }

                    if (customers.Count < 1)
                    {
                        this.radAcomWholesaler.Visible = false;
                        this.lblNotWholesaler.Visible = true;
                    }
                }

            }
        }
        else
        {
            
            ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT>();
            this.radGrdProduct.DataSource = (List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT>)ViewState[VIEWSTATE_KEY];
            this.radGrdProduct.DataBind();

        }
        if (!ClientScript.IsStartupScriptRegistered("setVisible"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisible", "setVisible('" + GetSalesGroup() + "');", true);
        radRadioCheck();
        
    }

    #region PageLoadInfo
    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 

        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnBasic, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnWRP, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnWORP, this.radGrdProduct);        
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnCustomer, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnSM, this.radGrdProduct);
        
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnHH, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnRI, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnSM, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnWH, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnCH, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnRMD, this.radGrdProduct);

        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnExceptionYes, this.radGrdProduct);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnExceptionNo, this.radGrdProduct);

        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnDiscountYes, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnDiscountNo, this.radGrdProduct);


        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnCustomer, this.radGrdProduct);
       
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdProduct);
        
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_BKLSpecialPriceandDiscount_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";
        
        if (this.hddGridItems.Value.IsNotNullOrEmptyEx())
        {
            UpdateGridData();
        }
        
        if (!ClientScript.IsStartupScriptRegistered("setVisible"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisible", "setVisible('" + GetSalesGroup() + "');", true);

        // Extension Agreement Check
        string ExtensionAgreementCheck = this.radBtnExtension.Checked ? this.radBtnExtension.Value : "N";
        if (!ClientScript.IsStartupScriptRegistered("setVisibleControl"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleControl", "setVisibleControl('" + ExtensionAgreementCheck + "');", true);



    }

    void Approval_Document_BKLSpecialPriceandDiscount_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.Equals("Rebind"))
        {
            UpdateGridData();
            this.hddAddRow.Value = "Y";
        }
        else
        {
            this.hddAddRow.Value = "N";
        }
    }
    #endregion

    #region [ Product Grid ]

    private void UpdateGridData()
    {
        
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT> items
            = (List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT>)serializer.Deserialize<List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT>>(this.hddGridItems.Value);

        foreach (DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT product in items)
        {
            //product.AMOUNT = product.DISCOUNT_AMOUNT * product.QTY;
        }
        ViewState[VIEWSTATE_KEY] = items;
        this.radGrdProduct.DataSource = items;
        this.radGrdProduct.DataBind();

    }

    protected void radGrdProduct_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            UpdateGridData();
            int idx = Convert.ToInt32(e.CommandArgument.ToString().Trim());

            using (BKLSpecialPriceandDiscountMgr mgr = new BKLSpecialPriceandDiscountMgr())
            {
                mgr.DeleteBKLPriceAndMarginProduct(this.hddProcessID.Value, idx);
            }

            List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT> list = (List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT>)ViewState[VIEWSTATE_KEY];

            list.RemoveAll(p => p.IDX == idx);


            this.radGrdProduct.DataSource = list;
            this.radGrdProduct.DataBind();
        }

    }
    protected void radGrdProduct_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;

        }
    }






    #endregion

    #region [ Sales Group Event ]
    protected void radBtnDealNoSave_Click(object sender, EventArgs e)
    {
        
        using (Bayer.eWF.BSL.Approval.Mgr.BKLSpecialPriceandDiscountMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.BKLSpecialPriceandDiscountMgr())
        {
            mgr.Update_Dealno(this.hddProcessID.Value, this.txtDealno.Text, Sessions.UserID);
           
            
          
            //excel.DataSource = mgr.Download_Detail_Report(DOCUMENT_ID_SE, FROM_DATE, TO_DATE);
            //excel.DataSource = mgr.SelectAdminReportingDocumentList("D0036");
        }

    }
    protected void radBtnSalesGroup_Click(object sender, EventArgs e)
    {
        radRadioCheck();
        List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT> list = (List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT>)ViewState[VIEWSTATE_KEY];
        list.Clear();
        this.radGrdProduct.DataSource = list;
        this.radGrdProduct.DataBind();
        
    }

    protected void radBtnSalesGroup_Click_1(object sender, EventArgs e)
    {
        //radRadioCheck();
       
    }
    //radBtnExceptionGroup_Click
    protected void radBtnExceptionGroup_Click(object sender, EventArgs e)
    {
        string exceptionValue=GetExceptionValue();
       
        this.radBtnDiscountYes.Checked=false;
        this.radBtnDiscountNo.Checked = false;
        if (exceptionValue == "NO") { 
            this.radBtnDiscountYes.ReadOnly=true;
            this.radBtnDiscountNo.ReadOnly = true;
        }
        else
        {
            this.radBtnDiscountYes.ReadOnly = false;
            this.radBtnDiscountNo.ReadOnly = false;
        }
        radRadioCheck();
        List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT> list = (List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT>)ViewState[VIEWSTATE_KEY];
        list.Clear();
        this.radGrdProduct.DataSource = list;
        this.radGrdProduct.DataBind();

        //radRadioCheck();
        //List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT> list = (List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT>)ViewState[VIEWSTATE_KEY];
        //list.Clear();
        //this.radGrdProduct.DataSource = list;
        //this.radGrdProduct.DataBind();

    }


    //2022.03.22 add comment only for radBtnSameDiscountGroup_Click
    //radBtnSameDiscountGroup_Click
    protected void radBtnSameDiscountGroup_Click(object sender, EventArgs e)
    {
        radRadioCheck();
        List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT> list = (List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT>)ViewState[VIEWSTATE_KEY];
        list.Clear();
        this.radGrdProduct.DataSource = list;
        this.radGrdProduct.DataBind();

    }

    private void radRadioCheck()
    {
        string typeValue = GetTypeValue();
        string salesGroup = GetSalesGroup();

        this.radGrdProduct.MasterTableView.GetColumn("LIST_PRICE").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("BASIC_DISCOUNT").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("AS_IS_DISCOUNT").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("W_H_DISCOUNT").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("TO_BE_DISCOUNT").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("VOLUME").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("NET_AMOUNT").Display = false; 
        this.radGrdProduct.MasterTableView.GetColumn("NET_SELLING_PRICE").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("DISCOUNT_RATE").Display = false;
        

        //(this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS") as GridTemplateColumn).ReadOnly = true;


        if (salesGroup.Equals("CH"))
        {
            this.radGrdProduct.MasterTableView.GetColumn("LIST_PRICE").Display = true;
            this.radGrdProduct.MasterTableView.GetColumn("NET_SELLING_PRICE").Display = true;
            this.radGrdProduct.MasterTableView.GetColumn("DISCOUNT_RATE").Display = true;
            //this.radBtnWORP.Visible = false;
            //this.radBtnWRP.Visible = false;
            
        }
        //else if(salesGroup.Equals("AH"))
        //{
        //    this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS").Display = true;
        //    this.radGrdProduct.MasterTableView.GetColumn("MARGIN_TO_BE").Display = true;
        //    (this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS") as GridTemplateColumn).ReadOnly = false;
        //    (this.radGrdProduct.MasterTableView.GetColumn("MARGIN_TO_BE") as GridTemplateColumn).ReadOnly = false;
        //
        //    typeValue = GetTypeValueAH();
        //    if (typeValue.Equals("CustomerA"))
        //    {
        //        this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS").Display = false;
        //        this.radGrdProduct.MasterTableView.GetColumn("MARGIN_TO_BE").Display = false;
        //        this.radGrdProduct.MasterTableView.GetColumn("DISCOUNT_AMOUNT").Display = false;
        //        this.radGrdProduct.MasterTableView.GetColumn("QTY").Display = false;
        //        this.radGrdProduct.MasterTableView.GetColumn("SUPPLY_PRICE").Display = false;
        //        this.radGrdProduct.MasterTableView.GetColumn("PASS_THROUGH_MARGIN").Display = false;
        //        this.radGrdProduct.MasterTableView.GetColumn("WHOLESALER_MARGIN").Display = false;
        //        this.radGrdProduct.MasterTableView.GetColumn("TOTAL_MARGIN").Display = false;
        //        this.radGrdProduct.MasterTableView.GetColumn("VOLUME").Display = false;
        //    }
        //}
        else
        {
            //this.radBtnWORP.Visible = true;
            //this.radBtnWRP.Visible = true;
            //this.radBtnWORP.ReadOnly = false;
            //this.radBtnWRP.ReadOnly = false;

            this.radGrdProduct.MasterTableView.GetColumn("LIST_PRICE").Display = true;
            if (typeValue.Equals("Special Supply w R.P.") || typeValue.Equals("Special Supply w.o. R.P."))
            {
                this.radGrdProduct.MasterTableView.GetColumn("BASIC_DISCOUNT").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("AS_IS_DISCOUNT").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("W_H_DISCOUNT").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("TO_BE_DISCOUNT").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("VOLUME").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("NET_AMOUNT").Display = true;
                if(typeValue.Equals("Special Supply w.o. R.P.")) 
                    (this.radGrdProduct.MasterTableView.GetColumn("W_H_DISCOUNT") as GridTemplateColumn).ReadOnly = true;
                if (typeValue.Equals("Special Supply w R.P."))
                    (this.radGrdProduct.MasterTableView.GetColumn("W_H_DISCOUNT") as GridTemplateColumn).ReadOnly = false;
            }
            else if (typeValue.Equals("Basic"))
            {                
                this.radGrdProduct.MasterTableView.GetColumn("AS_IS_DISCOUNT").Display = true;                
                this.radGrdProduct.MasterTableView.GetColumn("TO_BE_DISCOUNT").Display = true;
            }
            else if (typeValue.Equals("Customer Contract Price"))
            {
                this.radGrdProduct.MasterTableView.GetColumn("NET_SELLING_PRICE").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("DISCOUNT_RATE").Display = true;
            }
            
            

        }
        this.radGrdProduct.Rebind();
    }

    #endregion

    #region [ 문서 상단 버튼 ]

    private string GetTypeText()
    {
        string type = string.Empty;
        if (this.radBtnBasic.Checked) type = this.radBtnBasic.Text;
        else if (this.radBtnWRP.Checked) type = this.radBtnWRP.Text;
        else if (this.radBtnWORP.Checked) type = this.radBtnWORP.Text;
        else if (this.radBtnCustomer.Checked) type = this.radBtnCustomer.Text;
        return type;
    }

    private string GetTypeValue()
    {
        string type = string.Empty;
        if (this.radBtnBasic.Checked) type = this.radBtnBasic.Value;
        else if (this.radBtnWRP.Checked) type = this.radBtnWRP.Value;
        else if (this.radBtnWORP.Checked) type = this.radBtnWORP.Value;
        else if (this.radBtnCustomer.Checked) type = this.radBtnCustomer.Value;        
        return type;
    }

    private string GetExceptionValue()
    {
        string type = string.Empty;
        if (this.radBtnExceptionYes.Checked) type = this.radBtnExceptionYes.Value;
        else if (this.radBtnExceptionNo.Checked) type = this.radBtnExceptionNo.Value;
        return type;
    }

    private string GetSameDiscountValue()
    {
        string type = string.Empty;
        if (this.radBtnDiscountYes.Checked) type = this.radBtnDiscountYes.Value;
        else if (this.radBtnDiscountNo.Checked) type = this.radBtnDiscountNo.Value;
        return type;
    }

    //private string GetTypeValueAH()
    //{
    //    string selectedtype = string.Empty;
    //    foreach (Control control in this.divAH.Controls)
    //    {
    //        if (control is RadButton)
    //        {
    //            if ((control as RadButton).Checked)
    //            {
    //                selectedtype = (control as RadButton).Value;
    //                break;
    //            }
    //        }
    //    }
    //    return selectedtype;
    //}

    //private string GetTypeTextAH()
    //{
    //    string selectedtype = string.Empty;
    //    foreach (Control control in this.divAH.Controls)
    //    {
    //        if (control is RadButton)
    //        {
    //            if ((control as RadButton).Checked)
    //            {
    //                selectedtype = (control as RadButton).Text;
    //                break;
    //            }
    //        }
    //    }
    //    return selectedtype;
    //}

    private string GetSalesGroup()
    {
        string selectedSalesGroup = string.Empty;
        foreach (Control control in this.divSalesGroup.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    selectedSalesGroup = (control as RadButton).Value;
                    break;
                }
            }
        }

        return selectedSalesGroup;
    }

    //private string GetProductFamily()
    //{
    //    string selectedProductFamily = string.Empty;
    //    foreach (Control control in this.divProductFamily.Controls)
    //    {
    //        if (control is RadButton)
    //        {
    //            if ((control as RadButton).Checked)
    //            {
    //                selectedProductFamily = (control as RadButton).Value;
    //                break;
    //            }
    //        }
    //    }

    //    return selectedProductFamily;
    //}


    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        UpdateGridData();
        string salesGroup = GetSalesGroup();
        string message = string.Empty;

        if (salesGroup.IsNullOrEmptyEx())
            message += "BU";

        //if (salesGroup == "HH")
        //{
        //    string ProductFamily = GetProductFamily();
        //    if (ProductFamily.IsNullOrEmptyEx())
        //        message += "Product Family";
        //}


        //<%--                2022.05.27 add for validation, test OK, change INC14831972, add for Validation-- start%>

        //业务场景问题：
        //1.'Same Discount' 的yes，no 是or的关系？ Yes
        //2.'Same Discount' 的yes，no的时候，追加的部分都是必填项目，用户多次选择修改之后，再提交，是否是二择一？提交之后验证。
        //3.'Same Discount' 的yes，no在提交之后，是否可以在页面修改这个提交的内容，如果可以i修改，是否是覆盖上次的选择和填的内容？只保留最新提交的记录
        //4. 用户界面是否有可以删除这个文档的地方，此文档页面或者其他的页面，如有，需要调查相关的代码存储过程，和测试相关的场景？草稿是否可以删除，
        //提交, draft, edit, pending approval

        //'Same Discount' = YES
        if (radBtnDiscountYes.Checked) {
            if (RadTextSPPriceExcp.Text.Length <= 0)
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Speical Price or Exception Doc No.";
            }
        }

        //'Same Discount' = No
        if (radBtnDiscountNo.Checked)
        {
            if (RadTextBackground.Text.Length <= 0)
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Background";
            }
            if (RadTextProposal.Text.Length <= 0)
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Proposal";
            }
            if (RadTextProcess.Text.Length <= 0)
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Process";
            }
            if (RadTextFinancial.Text.Length <= 0)
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Financial impact";
            }
            if (RadTextExceptionComment.Text.Length <= 0)
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Comment ";
            }

        }

        //<%--                2022.05.27 add for change INC14831972, add for Validation-- end%>


        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            //if (salesGroup == "AH" && GetTypeValueAH().IsNullOrEmptyEx())
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "type";

            if (salesGroup != "AH" && GetTypeValue().IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "type";

            if (!radDatPeriodFrom.SelectedDate.HasValue)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Contract Period From(H <-> W)";

            if (!radDatPeriodTo.SelectedDate.HasValue)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Contract Period To(H <-> W)";


            if (!radDatPeriodFrom_W_BKL.SelectedDate.HasValue)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Contract Period From(W<-> BKL)";

            if (!radDatPeriodTo_W_BKL.SelectedDate.HasValue)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Contract Period To(W <-> BKL)";

            // Extension Agreement Check 가 선택되었을 경우에 필수항목인지 체크한다.
            if(radBtnExtension.Checked == true)
            {

                if (this.radTxtReDocNo.Text.Length <= 0)
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Re e-WORKFlow Document No.";

                if (!radDatPeriodFromExtention.SelectedDate.HasValue)
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Extension Period From(W<-> BKL)";

                if (!radDatPeriodToExtention.SelectedDate.HasValue)
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Extension Period To(W<-> BKL)";

                if (this.radTxtRemark.Text.Length <= 0)
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Remark";
            }


            //if (((List<DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT>)ViewState[VIEWSTATE_KEY]).Count < 1)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Product";
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
        DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT doc = new DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT();
        doc.PROCESS_ID = this.hddProcessID.Value;
        string selectedbu = GetSalesGroup();
        //string selectedProductFamily = GetProductFamily();
        string hospitalName = "";
        if (radAcomHospital.Entries.Count > 0)
        {
            AutoCompleteBoxEntry entry = radAcomHospital.Entries[0];
            hospitalName = entry.Text.Trim();
            //if (selectedbu == "AH")
            //    doc.SUBJECT = GetTypeTextAH() + "/" + hospitalName.Substring(0, hospitalName.LastIndexOf("(")) + "/" + GetSalesGroup();
            //else
                doc.SUBJECT = GetTypeText() + "/" + hospitalName.Substring(0, hospitalName.LastIndexOf("(")) + "/" + GetSalesGroup();
        }
        else
        {
            //if (selectedbu == "AH")
            //    doc.SUBJECT = GetTypeTextAH() + "/" + GetSalesGroup();
            //else
                doc.SUBJECT = GetTypeText() + "/" + GetSalesGroup();
        }



        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;

        //string typeAh = GetTypeValueAH();

        doc.BU = selectedbu;
        doc.TYPE = GetTypeValue();
        doc.EXCEPTION = GetExceptionValue();
        doc.SAMEDISCOUNT = GetSameDiscountValue();

        //<% --2022.05.27 add 使用填写的内容，来提交表單，for change INC14831972 start-- %>
        if (radBtnDiscountYes.Checked)
        {
            doc.SPPriceExcp = this.RadTextSPPriceExcp.Text;

            doc.BACKGROUND = string.Empty;
            doc.PROPOSAL = string.Empty;
            doc.PROCESS = string.Empty;
            doc.FINANCIAL_IMPACT = string.Empty;
            doc.COMMENT = string.Empty;

        }


        if (radBtnDiscountNo.Checked)
        {
            doc.SPPriceExcp = string.Empty;

            doc.BACKGROUND = this.RadTextBackground.Text;
            doc.PROPOSAL = this.RadTextProposal.Text;
            doc.PROCESS = this.RadTextProcess.Text;
            doc.FINANCIAL_IMPACT = this.RadTextFinancial.Text;
            doc.COMMENT = this.RadTextExceptionComment.Text;
        }

        //<% --2022.05.27 add for change INC14831972 end-- %>


        //if (selectedbu == "AH")
        //{
        //    doc.TYPE = GetTypeValueAH();
        //}
        //else if (selectedbu != "AH")
        //{
        //    doc.TYPE = GetTypeValue();
        //}

        //if (selectedbu == "HH")
        //    doc.PRODUCT_FAMILY = GetProductFamily();
        //else
        doc.PRODUCT_FAMILY = string.Empty;



        string AttachCheck = string.Empty;
        foreach (Control control in this.divCheck.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                    AttachCheck += (control as RadButton).Value + ";";
            }
        }

        doc.ATTACH_CHECK = AttachCheck;

        // 2nd Whloesaler
        doc.SECOND_WHOLESALER = this.radTxtSecondWholesaler.Text;

        doc.CONTRACT_FROM = this.radDatPeriodFrom.SelectedDate;
        doc.CONTRACT_TO = this.radDatPeriodTo.SelectedDate;
        doc.CONTRACT_FROM_W_BKL = this.radDatPeriodFrom_W_BKL.SelectedDate;
        doc.CONTRACT_TO_W_BKL = this.radDatPeriodTo_W_BKL.SelectedDate;

        // Extension Agreement Check Begin (2018.04.06: Youngwoo Lee)

        string ExtentionAgreementCheck = string.Empty;
        string Remark = string.Empty;
        string ReDocNo = string.Empty;

        if (this.radBtnExtension.Checked)
        {
            ExtentionAgreementCheck = this.radBtnExtension.Value;
            ReDocNo = this.radTxtReDocNo.Text;
            Remark = this.radTxtRemark.Text;

            if (this.radDatPeriodFromExtention.SelectedDate.HasValue)
                doc.EXTENSION_FROM_W_BKL = this.radDatPeriodFromExtention.SelectedDate.Value;
            else
                doc.EXTENSION_FROM_W_BKL = DateTime.Today;

            if (this.radDatPeriodToExtention.SelectedDate.HasValue)
                doc.EXTENSION_TO_W_BKL = this.radDatPeriodToExtention.SelectedDate.Value;
            else
                doc.EXTENSION_TO_W_BKL = DateTime.Today;

            doc.SUBJECT = "[EA] " + doc.SUBJECT; // ExtensionAgreement

        }
        else
        {
            ExtentionAgreementCheck = "N";
            ReDocNo = "";
            Remark = "";
            doc.EXTENSION_FROM_W_BKL = DateTime.Today;
            doc.EXTENSION_TO_W_BKL = DateTime.Today;

        }



        webMaster.Subject = doc.SUBJECT;




        doc.EXTENTION_AGREEMENT_CHECK = ExtentionAgreementCheck;
        doc.RE_EWORKFLOW_DOC_NO = ReDocNo;
        doc.REMARK = Remark;

        // Extension Agreement Check End

        if (radNumTotalAmount.Text.Length > 0)
            doc.TOTAL_AMOUNT = Convert.ToDecimal(radNumTotalAmount.Text);
        else doc.TOTAL_AMOUNT = 0;
        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;

        List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_CUSTOMER> customers = new List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_CUSTOMER>();
        foreach (AutoCompleteBoxEntry entry in this.radAcomWholesaler.Entries)
        {
            DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_CUSTOMER customer = new DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_CUSTOMER();
            customer.PROCESS_ID = this.hddProcessID.Value;
            customer.CUSTOMER_CODE = entry.Value;
            customer.CUSTOMER_NAME = entry.Text;
            customer.CREATOR_ID = Sessions.UserID;
            customers.Add(customer);
        }

        List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_HOSPITAL> hospitals = new List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_HOSPITAL>();
        foreach (AutoCompleteBoxEntry entry in this.radAcomHospital.Entries)
        {
            DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_HOSPITAL hospital = new DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_HOSPITAL();
            hospital.PROCESS_ID = this.hddProcessID.Value;
            hospital.HOSPITAL_CODE = entry.Value;
            hospital.HOSPITAL_NAME = entry.Text;
            hospital.CREATOR_ID = Sessions.UserID;
            hospitals.Add(hospital);
        }

        List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT> products = (List<DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT>)ViewState[VIEWSTATE_KEY];
        foreach (DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT product in products)
        {
            product.PROCESS_ID = this.hddProcessID.Value;
            product.CREATOR_ID = Sessions.UserID;
        }



        //<%--                2022.05.30 add for change INC14831972 TBD SPSPSP, need modify the SP --%>
        using (BKLSpecialPriceandDiscountMgr mgr = new BKLSpecialPriceandDiscountMgr())
        {
            return mgr.MergeBKLPriceAndMargin(doc, customers, hospitals, products);
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
}