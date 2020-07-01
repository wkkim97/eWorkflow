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

public partial class Approval_Document_WholesalerMarginAgreement : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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
        hddDocumentID.Value = "D0012";
        //hddProcessID.Value = "";


        InitControls();
    }

    private void InitControls()
    {
        if (!this.hddProcessID.Value.IsNullOrEmptyEx())
        {
            using (WholesalerMarginAgreementMgr mgr = new WholesalerMarginAgreementMgr())
            {

                Tuple<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT
                , List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER>
                , List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL>
                , List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT>> doc = mgr.SelectWholesalerMarginAgreement(this.hddProcessID.Value);

                this.HddProcessStatus.Value = doc.Item1.PROCESS_STATUS;
                //BU
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
                //TYPE
                if (doc.Item1.BU != "AH")
                {
                    if (doc.Item1.TYPE.Equals(this.radBtnTender.Value)) this.radBtnTender.Checked = true;
                    else if (doc.Item1.TYPE.Equals(this.radBtnNormal.Value)) this.radBtnNormal.Checked = true;
                    else if (doc.Item1.TYPE.Equals(this.radBtnBidding.Value)) this.radBtnBidding.Checked = true;
                }
                else if (doc.Item1.BU == "AH")
                {
                    foreach (Control control in this.divAH.Controls)
                    {
                        if (control is RadButton)
                        {
                            if ((control as RadButton).Value == doc.Item1.TYPE)
                            {
                                (control as RadButton).Checked = true; break;
                            }
                        }
                    }
                }

                this.radDatPeriodFrom.SelectedDate = doc.Item1.CONTRACT_FROM;
                this.radDatPeriodTo.SelectedDate = doc.Item1.CONTRACT_TO;
                this.radNumTotalAmount.Value = (double?)doc.Item1.TOTAL_AMOUNT;
                webMaster.DocumentNo = doc.Item1.DOC_NUM;

                List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER> customers = doc.Item2;
                foreach (DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER customer in customers)
                {
                    AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                    entry.Value = customer.CUSTOMER_CODE;
                    entry.Text = customer.CUSTOMER_NAME;
                    this.radAcomWholesaler.Entries.Add(entry);
                }

                List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL> hospitals = doc.Item3;
                foreach (DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL hospital in hospitals)
                {
                    AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                    entry.Value = hospital.HOSPITAL_CODE;
                    entry.Text = hospital.HOSPITAL_NAME;
                    this.radAcomHospital.Entries.Add(entry);
                }

                List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT> products = doc.Item4;
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
            ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT>();
            this.radGrdProduct.DataSource = (List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT>)ViewState[VIEWSTATE_KEY];
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
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnBidding, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnSM, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnAH, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnCC, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnDC, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnHH, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnRI, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnSM, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnWH, this.radGrdProduct);

        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnMax, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnSpc, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnVol, this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnQC, this.radGrdProduct);

        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_WholesalerMarginAgreement_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";

        if (this.hddGridItems.Value.IsNotNullOrEmptyEx())
        {
            UpdateGridData();
        }

        if (!ClientScript.IsStartupScriptRegistered("setVisible"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisible", "setVisible('" + GetSalesGroup() + "');", true);
        
    }

    void Approval_Document_WholesalerMarginAgreement_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.Equals("Rebind"))
        {
            UpdateGridData();
        }
    }
    #endregion

    #region [ Product Grid ]

    private void UpdateGridData()
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT> items
            = (List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT>)serializer.Deserialize<List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT>>(this.hddGridItems.Value);

        foreach (DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT product in items)
        {
            product.AMOUNT = product.DISCOUNT_AMOUNT * product.QTY;
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

            using (WholesalerMarginAgreementMgr mgr = new WholesalerMarginAgreementMgr())
            {
                mgr.DeleteWholesalerMarginAgreementProduct(this.hddProcessID.Value, idx);
            }

            List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT> list = (List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT>)ViewState[VIEWSTATE_KEY];

            list.RemoveAll(p => p.IDX == idx);


            this.radGrdProduct.DataSource = list;
            this.radGrdProduct.DataBind();
        }

    }

    #endregion

    #region [ Sales Group Event ]

    protected void radBtnSalesGroup_Click(object sender, EventArgs e)
    {
        radRadioCheck();
        List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT> list = (List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT>)ViewState[VIEWSTATE_KEY];
        list.Clear();
        this.radGrdProduct.DataSource = list;
        this.radGrdProduct.DataBind();
    }

    private void radRadioCheck()
    {
        string typeValue = GetTypeValue();
        string salesGroup = GetSalesGroup();
        if (salesGroup.Equals("CC") || salesGroup.Equals("DC"))
        {
            this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS").Display = false;
            this.radGrdProduct.MasterTableView.GetColumn("MARGIN_TO_BE").Display = false;
            this.radGrdProduct.MasterTableView.GetColumn("BASE_PRICE").Display = false;
            this.radGrdProduct.MasterTableView.GetColumn("DISCOUNT_AMOUNT").Display = true;
            this.radGrdProduct.MasterTableView.GetColumn("QTY").Display = false;
            this.radGrdProduct.MasterTableView.GetColumn("AMOUNT").Display = false;

            if (typeValue.Equals("Tender") || typeValue.Equals("Bidding"))
                (this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS") as GridTemplateColumn).ReadOnly = true;
            else if (typeValue.Equals("Normal"))
                (this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS") as GridTemplateColumn).ReadOnly = false;

        }
        else
        {
            this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS").Display = true;
            (this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS") as GridTemplateColumn).ReadOnly = false;
            this.radGrdProduct.MasterTableView.GetColumn("MARGIN_TO_BE").Display = true;
            this.radGrdProduct.MasterTableView.GetColumn("DISCOUNT_AMOUNT").Display = false;
            this.radGrdProduct.MasterTableView.GetColumn("QTY").Display = false;
            this.radGrdProduct.MasterTableView.GetColumn("AMOUNT").Display = false;

            if (typeValue.Equals("Tender") || typeValue.Equals("Bidding"))
                (this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS") as GridTemplateColumn).ReadOnly = false;
            else if (typeValue.Equals("Normal"))
                (this.radGrdProduct.MasterTableView.GetColumn("MARGIN_AS_IS") as GridTemplateColumn).ReadOnly = true;
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
        else if (this.radBtnBidding.Checked) type = this.radBtnBidding.Text;
        //else if (this.radBtnOthers.Checked) type = this.radBtnOthers.Text;
        return type;
    }

    private string GetTypeValue()
    {
        string type = string.Empty;
        if (this.radBtnTender.Checked) type = this.radBtnTender.Value;
        else if (this.radBtnNormal.Checked) type = this.radBtnNormal.Value;
        else if (this.radBtnBidding.Checked) type = this.radBtnBidding.Value;
        //else if (this.radBtnOthers.Checked) type = this.radBtnOthers.Value;
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

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        UpdateGridData();
        string salesGroup = GetSalesGroup();
        string message = string.Empty;

        if (salesGroup.IsNullOrEmptyEx())
            message += "BU";

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            if (salesGroup == "AH" && GetTypeValueAH().IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "type";

            if (salesGroup != "AH" && GetTypeValue().IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "type";

            if (!radDatPeriodFrom.SelectedDate.HasValue)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Contract Period";

            //if (!radDatPeriodTo.SelectedDate.HasValue)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Contract Period";

            //if (((List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT>)ViewState[VIEWSTATE_KEY]).Count < 1)
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
        DTO_DOC_WHOLESALER_MARGIN_AGREEMENT doc = new DTO_DOC_WHOLESALER_MARGIN_AGREEMENT();
        doc.PROCESS_ID = this.hddProcessID.Value;
        string selectedbu = GetSalesGroup();
        string hospitalName = "";
        if (radAcomHospital.Entries.Count > 0)
        {
            AutoCompleteBoxEntry entry = radAcomHospital.Entries[0];
            hospitalName = entry.Text;
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
        



        webMaster.Subject = doc.SUBJECT;


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

        doc.CONTRACT_FROM = this.radDatPeriodFrom.SelectedDate;
        doc.CONTRACT_TO = this.radDatPeriodTo.SelectedDate;
        if (radNumTotalAmount.Text.Length > 0)
            doc.TOTAL_AMOUNT = Convert.ToDecimal(radNumTotalAmount.Text);
        else doc.TOTAL_AMOUNT = 0;
        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;

        List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER> customers = new List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER>();
        foreach (AutoCompleteBoxEntry entry in this.radAcomWholesaler.Entries)
        {
            DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER customer = new DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER();
            customer.PROCESS_ID = this.hddProcessID.Value;
            customer.CUSTOMER_CODE = entry.Value;
            customer.CUSTOMER_NAME = entry.Text;
            customer.CREATOR_ID = Sessions.UserID;
            customers.Add(customer);
        }

        List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL> hospitals = new List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL>();
        foreach (AutoCompleteBoxEntry entry in this.radAcomHospital.Entries)
        {
            DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL hospital = new DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL();
            hospital.PROCESS_ID = this.hddProcessID.Value;
            hospital.HOSPITAL_CODE = entry.Value;
            hospital.HOSPITAL_NAME = entry.Text;
            hospital.CREATOR_ID = Sessions.UserID;
            hospitals.Add(hospital);
        }

        List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT> products = (List<DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT>)ViewState[VIEWSTATE_KEY];
        foreach (DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT product in products)
        {
            product.PROCESS_ID = this.hddProcessID.Value;
            product.CREATOR_ID = Sessions.UserID;
        }

      


        using (WholesalerMarginAgreementMgr mgr = new WholesalerMarginAgreementMgr())
        {
            return mgr.MergeWholesalerMarginAgreement(doc, customers, hospitals, products);
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