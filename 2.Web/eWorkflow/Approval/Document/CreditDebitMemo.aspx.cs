using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Approval.Dto;
using Telerik.Web.UI;
using Bayer.eWF.BSL.Approval.Mgr;

public partial class Approval_Document_CreditDebitMemo : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
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
        hddDocumentID.Value = "D0006";
        //hddProcessID.Value = "P000000356";

        //hddProcessStatus.Value = ApprovalUtil.ApprovalStatus.Temp.ToString();

        InitControls();
    }

    private void InitControls()
    {
        if (this.hddProcessID.Value.IsNotNullOrEmptyEx())
        {
            using (CreditDebitMemoMgr mgr = new CreditDebitMemoMgr())
            {
                DTO_DOC_CREDIT_DEBIT_MEMO doc = mgr.SelectCreditDebitMemo(this.hddProcessID.Value);
                //Title / Purpose
                if (doc.TITLE_CODE.Equals("CREDIT"))
                {
                    this.radRdoCredit.Checked = true;
                    if (doc.PURPOSE_CODE.Equals("0001")) this.radRdoCrePur1.Checked = true;
                    else if (doc.PURPOSE_CODE.Equals("0002")) this.radRdoCrePur2.Checked = true;
                    else if (doc.PURPOSE_CODE.Equals("0003")) this.radRdoCrePur3.Checked = true;
                }
                else
                {
                    this.radRdoDebit.Checked = true;
                    if (doc.PURPOSE_CODE.Equals("0002")) this.radRdoDebPur1.Checked = true;
                    else if (doc.PURPOSE_CODE.Equals("0003")) this.radRdoDebPur2.Checked = true;
                }

                //Sales Group
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
                this.radNumAmount.Value = (double?)doc.ADJUST_AMOUNT;
                this.radTxtReason.Text = doc.REASON_DESC;
                webMaster.DocumentNo = doc.DOC_NUM;

                //Product
                List<DTO_DOC_CREDIT_DEBIT_MEMO_PRODUCT> products = mgr.SelectCreditDebitMemoProduct(this.hddProcessID.Value);
                foreach (DTO_DOC_CREDIT_DEBIT_MEMO_PRODUCT product in products)
                {
                    AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                    entry.Value = product.PRODUCT_CODE;
                    entry.Text = product.PRODUCT_NAME;
                    this.radAcomProduct.Entries.Add(entry);
                }


                List<DTO_DOC_CREDIT_DEBIT_MEMO_WHOLESALER> wholesalers = mgr.SelectCreditDebitMemoWholeSaler(this.hddProcessID.Value);
                foreach (DTO_DOC_CREDIT_DEBIT_MEMO_WHOLESALER wholesaler in wholesalers)
                {
                    AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                    entry.Value = wholesaler.WHOLESALER_CODE;
                    entry.Text = wholesaler.WHOLESALER_NAME;
                    this.radAcomWholesaler.Entries.Add(entry);
                }

                if (!(doc.PROCESS_STATUS.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || doc.PROCESS_STATUS.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString())))
                {
                    if (products.Count < 1)
                    {
                        this.radAcomProduct.Visible = false;
                        this.lblNotProduct.Visible = true;
                    }

                    if (wholesalers.Count < 1)
                    {
                        this.radAcomWholesaler.Visible = false;
                        this.lblNotCustomer.Visible = true;
                    }
                }
            }
        }
    }

    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 
        if (!ClientScript.IsStartupScriptRegistered("setVisibleControl"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleControl", "setVisibleControl('" + GetTitleCode() + "');", true);
    }

    #region [ 문서상단 버튼 ]
    private bool IsCheckedTitle()
    {
        if (this.radRdoCredit.Checked)
            return true;
        else if (this.radRdoDebit.Checked)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Sales Group반환
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

    private string GetTitleCode()
    {
        string titleCode = string.Empty;
        if (this.radRdoCredit.Checked) titleCode = this.radRdoCredit.Value;
        else if (this.radRdoDebit.Checked) titleCode = this.radRdoDebit.Value;
        return titleCode;

    }

    private string GetCreditPurpose()
    {
        string creditPurpose = string.Empty;
        foreach (Control control in divCreditPurpose.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    creditPurpose = (control as RadButton).Text;
                    break;
                }
            }
        }
        return creditPurpose;
    }

    private string GetDebitPurpose()
    {
        string debitPurpose = string.Empty;
        foreach (Control control in divDebitPurpose.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    debitPurpose = (control as RadButton).Text;
                    break;
                }
            }
        }
        return debitPurpose;
    }

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;
        if (!IsCheckedTitle())
            message = "Title";

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            //purpose
            if (this.radRdoCredit.Checked)
            {
                if (!(this.radRdoCrePur1.Checked || this.radRdoCrePur2.Checked || this.radRdoCrePur3.Checked))
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose";
            }
            else
            {
                if (!(this.radRdoDebPur1.Checked || this.radRdoDebPur2.Checked))
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose";
            }

            if (GetSelectedBU().IsNullOrEmptyEx())
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "BU";
            }

            //if (radAcomWholesaler.Entries.Count < 1)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "WholeSaler Name / Number";

            //if (radAcomProduct.Entries.Count < 1)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Product Name / Number";

            if (!radNumAmount.Value.HasValue || radNumAmount.Value == 0)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Adjustment Amount";
        }
        else if (status == ApprovalUtil.ApprovalStatus.Saved)
        {
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
        DTO_DOC_CREDIT_DEBIT_MEMO doc = new DTO_DOC_CREDIT_DEBIT_MEMO();
        doc.PROCESS_ID = this.hddProcessID.Value;
        string subject = string.Empty;
        string title = string.Empty;
        string subpurpose = string.Empty;
        if (this.radRdoCredit.Checked)
        {
            title = "Credit Memo";
            subpurpose = GetCreditPurpose();
        }
        else if (this.radRdoDebit.Checked)
        {
            title = "Debit Memo";
            subpurpose = GetDebitPurpose();
        }
        string salesGroup = GetSelectedBU();
        doc.SUBJECT = title + "/" + subpurpose + "/" + salesGroup;
        webMaster.Subject = doc.SUBJECT;
        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.TITLE_CODE = GetTitleCode();
        string purpose = string.Empty;
        if (this.radRdoCredit.Checked)
        {
            if (radRdoCrePur1.Checked) purpose = radRdoCrePur1.Value;
            else if (radRdoCrePur2.Checked) purpose = radRdoCrePur2.Value;
            else if (radRdoCrePur3.Checked) purpose = radRdoCrePur3.Value;
        }
        else
        {
            if (radRdoDebPur1.Checked) purpose = radRdoDebPur1.Value;
            else if (radRdoDebPur2.Checked) purpose = radRdoDebPur2.Value;
        }
        doc.PURPOSE_CODE = purpose;
        doc.BU = salesGroup;
        doc.ADJUST_AMOUNT = (decimal?)this.radNumAmount.Value;
        doc.REASON_DESC = this.radTxtReason.Text;
        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;


        List<DTO_DOC_CREDIT_DEBIT_MEMO_PRODUCT> products = new List<DTO_DOC_CREDIT_DEBIT_MEMO_PRODUCT>();
        foreach (AutoCompleteBoxEntry entry in this.radAcomProduct.Entries)
        {
            DTO_DOC_CREDIT_DEBIT_MEMO_PRODUCT product = new DTO_DOC_CREDIT_DEBIT_MEMO_PRODUCT();
            product.PROCESS_ID = this.hddProcessID.Value;
            product.PRODUCT_CODE = entry.Value;
            product.PRODUCT_NAME = entry.Text;
            product.CREATOR_ID = Sessions.UserID;
            products.Add(product);
        }

        List<DTO_DOC_CREDIT_DEBIT_MEMO_WHOLESALER> wholesalers = new List<DTO_DOC_CREDIT_DEBIT_MEMO_WHOLESALER>();
        foreach (AutoCompleteBoxEntry entry in this.radAcomWholesaler.Entries)
        {
            DTO_DOC_CREDIT_DEBIT_MEMO_WHOLESALER wholesaler = new DTO_DOC_CREDIT_DEBIT_MEMO_WHOLESALER();
            wholesaler.PROCESS_ID = this.hddProcessID.Value;
            wholesaler.WHOLESALER_CODE = entry.Value;
            wholesaler.WHOLESALER_NAME = entry.Text;
            wholesaler.CREATOR_ID = Sessions.UserID;
            wholesalers.Add(wholesaler);
        }

        using (CreditDebitMemoMgr mgr = new CreditDebitMemoMgr())
        {
            return mgr.MergeCreditDebitMemo(doc, products, wholesalers);
        }

    }
    protected override void DoSave()
    {
        try
        {
            if (ValidationCheck(ApprovalUtil.ApprovalStatus.Saved))
            {
                hddProcessID.Value = SaveDocument(ApprovalUtil.ApprovalStatus.Saved);
                webMaster.ProcessID = this.hddProcessID.Value;
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