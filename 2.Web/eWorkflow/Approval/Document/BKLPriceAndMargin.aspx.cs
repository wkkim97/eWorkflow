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

public partial class Approval_Document_BKLPriceAndMargin : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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
        hddDocumentID.Value = "D0056";
        //hddProcessID.Value = "";


        InitControls();
    }

    private void InitControls()
    {
        if (!this.hddProcessID.Value.IsNullOrEmptyEx())
        {
            using (BKLPriceAndMargin mgr = new BKLPriceAndMargin())
            {

                Tuple<DTO_DOC_BKL_PRICE_AND_MARGIN
                , List<DTO_DOC_BKL_PRICE_AND_MARGIN_CUSTOMER>
                , List<DTO_DOC_BKL_PRICE_AND_MARGIN_HOSPITAL>
                , List<DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT>> doc = mgr.SelectBKLPriceAndMargin(this.hddProcessID.Value);

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
                    this.radBtnDirect.Visible = false;
                    if (doc.Item1.TYPE.Equals(this.radBtnTender.Value)) this.radBtnTender.Checked = true;
                    else if (doc.Item1.TYPE.Equals(this.radBtnNormal.Value)) this.radBtnNormal.Checked = true;
                    else if (doc.Item1.TYPE.Equals(this.radBtnDirect.Value))
                    {
                        this.radBtnDirect.Checked = true;
                        this.radBtnDirect.Visible = true;
                        this.radBtnSuEui.Visible = false;
                    }
                    else if (doc.Item1.TYPE.Equals(this.radBtnSuEui.Value))
                    {
                        this.radBtnSuEui.Checked = true;
                        this.radBtnDirect.Visible = false;
                        this.radBtnSuEui.Visible = true;
                    }
                    else if (doc.Item1.TYPE.Equals(this.radBtnGateKeeper.Value)) this.radBtnGateKeeper.Checked = true;
                    else if (doc.Item1.TYPE.Equals(this.radBtnCustomer.Value)) this.radBtnCustomer.Checked = true;
                }
                else if (doc.Item1.BU == "AH")
                {
                    foreach (Control control in this.divAH.Controls)
                    {
                        if (control is RadButton)
                        {
                            if ((control as RadButton).Value == doc.Item1.TYPE)
                            {
                                (control as RadButton).Visible = true;
                                (control as RadButton).Checked = true; break;
                            }
                        }
                    }
                }

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

                List<DTO_DOC_BKL_PRICE_AND_MARGIN_CUSTOMER> customers = doc.Item2;
                foreach (DTO_DOC_BKL_PRICE_AND_MARGIN_CUSTOMER customer in customers)
                {
                    AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                    entry.Value = customer.CUSTOMER_CODE;
                    entry.Text = customer.CUSTOMER_NAME;
                    this.radAcomWholesaler.Entries.Add(entry);
                }

                List<DTO_DOC_BKL_PRICE_AND_MARGIN_HOSPITAL> hospitals = doc.Item3;
                foreach (DTO_DOC_BKL_PRICE_AND_MARGIN_HOSPITAL hospital in hospitals)
                {
                    AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                    entry.Value = hospital.HOSPITAL_CODE;
                    entry.Text = hospital.HOSPITAL_NAME;
                    this.radAcomHospital.Entries.Add(entry);
                }

                List<DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT> products = doc.Item4;
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
            this.radBtnDirect.Visible = false;
            ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT>();
            this.radGrdProduct.DataSource = (List<DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT>)ViewState[VIEWSTATE_KEY];
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

        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnTender, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnNormal, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnDirect, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnSuEui, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnGateKeeper, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnSM, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnAH, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnHH, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnRI, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnSM, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnWH, this.radGrdProduct);

        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnMax, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnSpc, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnVol, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnQC, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnCustomer, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnCustomerA, this.radGrdProduct);        
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnVirKon, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdProduct);
        
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_BKLPriceAndMargin_AjaxRequest;
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

    void Approval_Document_BKLPriceAndMargin_AjaxRequest(object sender, AjaxRequestEventArgs e)
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
        List<DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT> items
            = (List<DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT>)serializer.Deserialize<List<DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT>>(this.hddGridItems.Value);

        foreach (DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT product in items)
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

            using (BKLPriceAndMargin mgr = new BKLPriceAndMargin())
            {
                mgr.DeleteBKLPriceAndMarginProduct(this.hddProcessID.Value, idx);
            }

            List<DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT> list = (List<DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT>)ViewState[VIEWSTATE_KEY];

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
        
        using (Bayer.eWF.BSL.Approval.Mgr.BKLPriceAndMargin mgr = new Bayer.eWF.BSL.Approval.Mgr.BKLPriceAndMargin())
        {
            mgr.Update_Dealno(this.hddProcessID.Value, this.txtDealno.Text, Sessions.UserID);
           
            
          
            //excel.DataSource = mgr.Download_Detail_Report(DOCUMENT_ID_SE, FROM_DATE, TO_DATE);
            //excel.DataSource = mgr.SelectAdminReportingDocumentList("D0036");
        }

    }
    protected void radBtnSalesGroup_Click(object sender, EventArgs e)
    {
        radRadioCheck();
        List<DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT> list = (List<DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT>)ViewState[VIEWSTATE_KEY];
        list.Clear();
        this.radGrdProduct.DataSource = list;
        this.radGrdProduct.DataBind();
    }

    protected void radBtnSalesGroup_Click_1(object sender, EventArgs e)
    {
        //radRadioCheck();
       
    }

    private void radRadioCheck()
    {
        string typeValue = GetTypeValue();
        string salesGroup = GetSalesGroup();

        this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("MARGIN_TO_BE").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("BASE_PRICE").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("NORMAL_DISCOUNT").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("CONDITIONAL_PRODUCT_DISCOUNT").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("PARTNER_BASED_DISCOUNT").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("TRANSACTION_DISCOUNT").Display = false; 
        this.radGrdProduct.MasterTableView.GetColumn("DISCOUNT_AMOUNT").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("QTY").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("SUPPLY_PRICE").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("PASS_THROUGH_MARGIN").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("WHOLESALER_MARGIN").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("TOTAL_MARGIN").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("VOLUME").Display = false;
        this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS").Display = false;
        //(this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS") as GridTemplateColumn).ReadOnly = true;


        if (salesGroup.Equals("CH"))
        {
            this.radGrdProduct.MasterTableView.GetColumn("DISCOUNT_AMOUNT").Display = true;
            (this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS") as GridTemplateColumn).ReadOnly = true;
        }
        else if(salesGroup.Equals("AH"))
        {
            this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS").Display = true;
            this.radGrdProduct.MasterTableView.GetColumn("MARGIN_TO_BE").Display = true;
            (this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS") as GridTemplateColumn).ReadOnly = false;
            (this.radGrdProduct.MasterTableView.GetColumn("MARGIN_TO_BE") as GridTemplateColumn).ReadOnly = false;

            typeValue = GetTypeValueAH();
            if (typeValue.Equals("CustomerA"))
            {
                this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS").Display = false;
                this.radGrdProduct.MasterTableView.GetColumn("MARGIN_TO_BE").Display = false;
                this.radGrdProduct.MasterTableView.GetColumn("DISCOUNT_AMOUNT").Display = false;
                this.radGrdProduct.MasterTableView.GetColumn("QTY").Display = false;
                this.radGrdProduct.MasterTableView.GetColumn("SUPPLY_PRICE").Display = false;
                this.radGrdProduct.MasterTableView.GetColumn("PASS_THROUGH_MARGIN").Display = false;
                this.radGrdProduct.MasterTableView.GetColumn("WHOLESALER_MARGIN").Display = false;
                this.radGrdProduct.MasterTableView.GetColumn("TOTAL_MARGIN").Display = false;
                this.radGrdProduct.MasterTableView.GetColumn("VOLUME").Display = false;
            }
        }
        else
        {
            this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS").Display = false;
            this.radGrdProduct.MasterTableView.GetColumn("MARGIN_TO_BE").Display = false;
            this.radGrdProduct.MasterTableView.GetColumn("NORMAL_DISCOUNT").Display = false;
            this.radGrdProduct.MasterTableView.GetColumn("CONDITIONAL_PRODUCT_DISCOUNT").Display = false;
            this.radGrdProduct.MasterTableView.GetColumn("PARTNER_BASED_DISCOUNT").Display = false;
            this.radGrdProduct.MasterTableView.GetColumn("TRANSACTION_DISCOUNT").Display = false; 
            this.radGrdProduct.MasterTableView.GetColumn("DISCOUNT_AMOUNT").Display = false;
            this.radGrdProduct.MasterTableView.GetColumn("QTY").Display = false;
            this.radGrdProduct.MasterTableView.GetColumn("SUPPLY_PRICE").Display = false;
            this.radGrdProduct.MasterTableView.GetColumn("PASS_THROUGH_MARGIN").Display = false;
            this.radGrdProduct.MasterTableView.GetColumn("WHOLESALER_MARGIN").Display = false;
            this.radGrdProduct.MasterTableView.GetColumn("TOTAL_MARGIN").Display = false;
            this.radGrdProduct.MasterTableView.GetColumn("VOLUME").Display = false;

            if (typeValue.Equals("Special Supply w R.P.") || typeValue.Equals("SuEui Contract"))
            {
                this.radGrdProduct.MasterTableView.GetColumn("SUPPLY_PRICE").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("PASS_THROUGH_MARGIN").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("WHOLESALER_MARGIN").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("TOTAL_MARGIN").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("VOLUME").Display = true;
                (this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS") as GridTemplateColumn).ReadOnly = false;
            }
            else if (typeValue.Equals("Direct Contract"))
            {
                this.radGrdProduct.MasterTableView.GetColumn("SUPPLY_PRICE").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("PASS_THROUGH_MARGIN").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS").Display = false;
                this.radGrdProduct.MasterTableView.GetColumn("WHOLESALER_MARGIN").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("TOTAL_MARGIN").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("VOLUME").Display = true;
            }
            else if (typeValue.Equals("Normal"))
            {
                this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS").Display = true;                
                this.radGrdProduct.MasterTableView.GetColumn("MARGIN_TO_BE").Display = true;
                (this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS") as GridTemplateColumn).ReadOnly = true;
                (this.radGrdProduct.MasterTableView.GetColumn("MARGIN_TO_BE") as GridTemplateColumn).ReadOnly = false;
            }
            else if (typeValue.Equals("Special Supply w.o. R.P."))
            {
                this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS").Display = true;                
                this.radGrdProduct.MasterTableView.GetColumn("MARGIN_TO_BE").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("NORMAL_DISCOUNT").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("CONDITIONAL_PRODUCT_DISCOUNT").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("PARTNER_BASED_DISCOUNT").Display = true;
                this.radGrdProduct.MasterTableView.GetColumn("TRANSACTION_DISCOUNT").Display = true; 
                this.radGrdProduct.MasterTableView.GetColumn("VOLUME").Display = true;
                (this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS") as GridTemplateColumn).ReadOnly = false;
                (this.radGrdProduct.MasterTableView.GetColumn("MARGIN_TO_BE") as GridTemplateColumn).ReadOnly = true;
            }
            else if (typeValue.Equals("Customer"))
            {
                this.radGrdProduct.MasterTableView.GetColumn("DISCOUNT_AMOUNT").Display = true;
               
            }

        }
        this.radGrdProduct.Rebind();
    }

    #endregion

    #region [ 문서 상단 버튼 ]

    private string GetTypeText()
    {
        string type = string.Empty;
        if (this.radBtnTender.Checked) type = this.radBtnTender.Text;
        else if (this.radBtnNormal.Checked) type = this.radBtnNormal.Text;
        else if (this.radBtnGateKeeper.Checked) type = this.radBtnGateKeeper.Text;
        else if (this.radBtnDirect.Checked) type = this.radBtnDirect.Text;
        else if (this.radBtnSuEui.Checked) type = this.radBtnSuEui.Text;
        else if (this.radBtnCustomer.Checked) type = this.radBtnCustomer.Text;
        return type;
    }

    private string GetTypeValue()
    {
        string type = string.Empty;
        if (this.radBtnTender.Checked) type = this.radBtnTender.Value;
        else if (this.radBtnNormal.Checked) type = this.radBtnNormal.Value;
        else if (this.radBtnGateKeeper.Checked) type = this.radBtnGateKeeper.Value;
        else if (this.radBtnDirect.Checked) type = this.radBtnDirect.Value;
        else if (this.radBtnSuEui.Checked) type = this.radBtnSuEui.Value;
        else if (this.radBtnCustomer.Checked) type = this.radBtnCustomer.Value;
        return type;
    }

    private string GetTypeValueAH()
    {
        string selectedtype = string.Empty;
        foreach (Control control in this.divAH.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    selectedtype = (control as RadButton).Value;
                    break;
                }
            }
        }
        return selectedtype;
    }

    private string GetTypeTextAH()
    {
        string selectedtype = string.Empty;
        foreach (Control control in this.divAH.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    selectedtype = (control as RadButton).Text;
                    break;
                }
            }
        }
        return selectedtype;
    }

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

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            if (salesGroup == "AH" && GetTypeValueAH().IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "type";

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
        DTO_DOC_BKL_PRICE_AND_MARGIN doc = new DTO_DOC_BKL_PRICE_AND_MARGIN();
        doc.PROCESS_ID = this.hddProcessID.Value;
        string selectedbu = GetSalesGroup();
        //string selectedProductFamily = GetProductFamily();
        string hospitalName = "";
        if (radAcomHospital.Entries.Count > 0)
        {
            AutoCompleteBoxEntry entry = radAcomHospital.Entries[0];
            hospitalName = entry.Text.Trim();
            if (selectedbu == "AH")
                doc.SUBJECT = GetTypeTextAH() + "/" + hospitalName.Substring(0, hospitalName.LastIndexOf("(")) + "/" + GetSalesGroup();
            else
                doc.SUBJECT = GetTypeText() + "/" + hospitalName.Substring(0, hospitalName.LastIndexOf("(")) + "/" + GetSalesGroup();
        }
        else
        {
            if (selectedbu == "AH")
                doc.SUBJECT = GetTypeTextAH() + "/" + GetSalesGroup();
            else
                doc.SUBJECT = GetTypeText() + "/" + GetSalesGroup();
        }



        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;

        string typeAh = GetTypeValueAH();

        doc.BU = selectedbu;
        if (selectedbu == "AH")
        {
            doc.TYPE = GetTypeValueAH();
        }
        else if (selectedbu != "AH")
        {
            doc.TYPE = GetTypeValue();
        }

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

        List<DTO_DOC_BKL_PRICE_AND_MARGIN_CUSTOMER> customers = new List<DTO_DOC_BKL_PRICE_AND_MARGIN_CUSTOMER>();
        foreach (AutoCompleteBoxEntry entry in this.radAcomWholesaler.Entries)
        {
            DTO_DOC_BKL_PRICE_AND_MARGIN_CUSTOMER customer = new DTO_DOC_BKL_PRICE_AND_MARGIN_CUSTOMER();
            customer.PROCESS_ID = this.hddProcessID.Value;
            customer.CUSTOMER_CODE = entry.Value;
            customer.CUSTOMER_NAME = entry.Text;
            customer.CREATOR_ID = Sessions.UserID;
            customers.Add(customer);
        }

        List<DTO_DOC_BKL_PRICE_AND_MARGIN_HOSPITAL> hospitals = new List<DTO_DOC_BKL_PRICE_AND_MARGIN_HOSPITAL>();
        foreach (AutoCompleteBoxEntry entry in this.radAcomHospital.Entries)
        {
            DTO_DOC_BKL_PRICE_AND_MARGIN_HOSPITAL hospital = new DTO_DOC_BKL_PRICE_AND_MARGIN_HOSPITAL();
            hospital.PROCESS_ID = this.hddProcessID.Value;
            hospital.HOSPITAL_CODE = entry.Value;
            hospital.HOSPITAL_NAME = entry.Text;
            hospital.CREATOR_ID = Sessions.UserID;
            hospitals.Add(hospital);
        }

        List<DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT> products = (List<DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT>)ViewState[VIEWSTATE_KEY];
        foreach (DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT product in products)
        {
            product.PROCESS_ID = this.hddProcessID.Value;
            product.CREATOR_ID = Sessions.UserID;
        }




        using (BKLPriceAndMargin mgr = new BKLPriceAndMargin())
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