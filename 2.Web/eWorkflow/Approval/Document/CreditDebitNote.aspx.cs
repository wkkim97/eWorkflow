using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using Bayer.eWF.BSL.Common.Dto;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Approval_Document_CreditDebitNote : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_KEY = "VIEWSTATE_KEY_CREDIT_DEBIT_NOTE";

    private enum DocType
    {
        CREDIT,
        DEBIT,
    }

    protected override void OnPreInit(EventArgs e)
    {
        var masterPage = Master;
        this.webMaster = (masterPage as Master_eWorks_Document);
        base.OnPreInit(e);
    }

    protected override void OnPreRender(EventArgs e)
    {
        if (!webMaster.DocumentNo.Equals(string.Empty))
        {
            if (!(hddReuse.Value.Equals(ApprovalUtil.ApprovalStatus.Completed.ToString()) ||
                hddReuse.Value.Equals(ApprovalUtil.ApprovalStatus.Reject.ToString())))
                divLink.Style.Remove("display");
        }

        base.OnPreRender(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_CREDIT_DEBIT_NOTE_DESC>();
                this.radGrdDescription.DataSource = (List<DTO_DOC_CREDIT_DEBIT_NOTE_DESC>)ViewState[VIEWSTATE_KEY];
                this.radGrdDescription.DataBind();
                InitPageInfo();
            }
            PageLoadInfo();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }


    #region InitPageInfo
    private void InitPageInfo()
    {
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0004";
        //hddProcessID.Value = "";


        InitControls();
    }

    private void InitControls()
    {
        this.radBtnCredit.Value = DocType.CREDIT.ToString();
        this.radBtnDebit.Value = DocType.DEBIT.ToString();
        using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
        {
            this.radDdlCompany.DataSource = mgr.SelectBayerCompanyList();
            this.radDdlCompany.DataBind();
        }

        //기등록 문서 조회
        if (!hddProcessID.Value.IsNullOrEmptyEx())
        {
            string processId = this.hddProcessID.Value;
            using (CreditDebitNoteMgr mgr = new CreditDebitNoteMgr())
            {

                DTO_DOC_CREDIT_DEBIT_NOTE note = mgr.SelectCreditDebitNote(processId);
                if (note == null) return;
                this.hddProcessStatus.Value = note.PROCESS_STATUS;
                if (note.TYPE.Equals(DocType.CREDIT.ToString()))
                    this.radBtnCredit.Checked = true;
                else
                    this.radBtnDebit.Checked = true;

                this.radDdlCompany.SelectedValue = note.COMPANY_ID;
                this.radTxtCompany.Text = note.TO_NAME;
                this.hddCompanyId.Value = note.TO_CODE;

                this.radDateInvoice.SelectedDate = note.INVOICE_DATE;
                this.radDateDue.SelectedDate = note.DUE_DATE;
                this.radTxtDescription.Text = note.DESCRIPTION;

                this.radDdlCompany.SelectedValue = note.COMPANY_ID;
                this.radDdlCurrency.SelectedValue = note.CURRENCY;
                this.radNumLocalAmount.Value = (double?)note.LOCAL_AMOUNT;
                webMaster.DocumentNo = note.DOC_NUM;

                List<DTO_DOC_CREDIT_DEBIT_NOTE_ATTN> attns = mgr.SelectCreditDebitNoteAttn(processId);
                foreach (DTO_DOC_CREDIT_DEBIT_NOTE_ATTN attn in attns)
                {
                    AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                    entry.Text = attn.ATTN_NAME;
                    if (attn.ATTN_TYPE.Equals(CommonEnum.MailAddressType.Internal.ToString().Substring(0, 1)))
                    {
                        SmallUserInfoDto value = new SmallUserInfoDto();
                        value.USER_ID = attn.ATTN_CODE;
                        value.FULL_NAME = attn.ATTN_NAME;
                        value.MAIL_ADDRESS = attn.ATTN_MAIL_ADDRESS;
                        entry.Value = JsonConvert.toJson<SmallUserInfoDto>(value);
                    }
                    this.radAcomAttn.Entries.Add(entry);
                }

                List<DTO_DOC_CREDIT_DEBIT_NOTE_CC> ccs = mgr.SelectCreditDebitNoteCc(processId);
                foreach (DTO_DOC_CREDIT_DEBIT_NOTE_CC cc in ccs)
                {
                    AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                    entry.Text = cc.CC_NAME;
                    if (cc.CC_TYPE.Equals(CommonEnum.MailAddressType.Internal.ToString().Substring(0, 1)))
                    {
                        SmallUserInfoDto value = new SmallUserInfoDto();
                        value.USER_ID = cc.CC_CODE;
                        value.FULL_NAME = cc.CC_NAME;
                        value.MAIL_ADDRESS = cc.CC_MAIL_ADDRESS;
                        entry.Value = JsonConvert.toJson<SmallUserInfoDto>(value);
                    }
                    this.radAcomCC.Entries.Add(entry);
                }


                List<DTO_DOC_CREDIT_DEBIT_NOTE_DESC> descriptions = mgr.SelectCreditDebitNoteDesc(processId);
                ViewState[VIEWSTATE_KEY] = descriptions;

                this.radGrdDescription.DataSource = descriptions;
                this.radGrdDescription.DataBind();

                string documentUri = DNSoft.eW.FrameWork.eWBase.GetConfig("//SmtpMailInfo/DocumentUrl");
                documentUri = documentUri.Replace("Document", "Link");
                string link = documentUri + "/CreditDebitNoteView.aspx?processid={0}";
                link = string.Format(link, processId);

                string linkHtml = "<a href='{0}' target='_blank'>Invoice Link</a>";
                linkHtml = string.Format(linkHtml, link);

                this.divLink.Controls.Add(new LiteralControl(linkHtml));

            }
        }
        else
        {
            this.radDateInvoice.SelectedDate = DateTime.Today;
            this.radDateDue.SelectedDate = DateTime.Today.AddDays(60);
        }
    }

    #endregion

    #region PageLoadInfo
    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 

        UpdateGridData();

        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), radGrdDescription);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.hddAddRow);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_CreditDebitNote_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";

    }

    void Approval_Document_CreditDebitNote_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.Equals("Rebind"))
        {
            UpdateGridData();
            this.hddAddRow.Value = "Y";
        }
        else
        {
            this.hddAddRow.Value = "N";
            UpdateGridData();
        }
    }
    #endregion

    #region [ 문서상단 버튼 ]

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        UpdateGridData();

        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {

            //if (this.radTxtCompany.Text.IsNullOrEmptyEx())
            //    message += "To";

            //if (this.radAcomAttn.Entries.Count < 1)
            //    message += message.IsNullOrEmptyEx() ? "Attn" : ",Attn";

            //if (this.radDateInvoice.SelectedDate == null)
            //    message += message.IsNullOrEmptyEx() ? "Invoice Date" : ",Invoice Date";

            //if (this.radDateDue.SelectedDate == null)
            //    message += message.IsNullOrEmptyEx() ? "Due Date" : ",Due Date";

            //if (this.radGrdDescription.Items.Count < 1)
            //    message += message.IsNullOrEmptyEx() ? "Description" : ",Description";

            //if (!this.radNumLocalAmount.Value.HasValue || this.radNumLocalAmount.Value == 0)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Local Amount";
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

        DTO_DOC_CREDIT_DEBIT_NOTE note = new DTO_DOC_CREDIT_DEBIT_NOTE();
        note.PROCESS_ID = this.hddProcessID.Value;
        note.SUBJECT = (this.radBtnCredit.Checked ? DocType.CREDIT.ToString() : DocType.DEBIT.ToString()) + "/" + radDdlCompany.SelectedText;
        webMaster.Subject = (this.radBtnCredit.Checked ? DocType.CREDIT.ToString() : DocType.DEBIT.ToString()) + "/" + radDdlCompany.SelectedText + "/" + this.radTxtCompany.Text + "/" + this.radTxtDescription.Text;
        note.DOC_NUM = string.Empty;
        note.PROCESS_STATUS = status.ToString();
        note.REQUESTER_ID = Sessions.UserID;
        note.REQUEST_DATE = DateTime.Now;
        note.COMPANY_CODE = Sessions.CompanyCode;
        note.ORGANIZATION_NAME = Sessions.OrgName;
        note.LIFE_CYCLE = webMaster.LifeCycle;
        note.TYPE = (this.radBtnCredit.Checked ? "CREDIT" : "DEBIT");
        note.TO_CODE = this.hddCompanyId.Value;
        note.TO_NAME = this.radTxtCompany.Text;
        note.COMPANY_ID = this.radDdlCompany.SelectedValue;
        note.INVOICE_DATE = this.radDateInvoice.SelectedDate;
        note.DUE_DATE = this.radDateDue.SelectedDate;
        note.CURRENCY = this.radDdlCurrency.SelectedValue;
        GridFooterItem itemTotal = (GridFooterItem)this.radGrdDescription.MasterTableView.GetItems(GridItemType.Footer)[0];
        if (itemTotal["AMOUNT"].Text == "")
            note.TOTAL_AMOUNT = 0;
        else
            note.TOTAL_AMOUNT = Convert.ToDecimal((itemTotal["AMOUNT"].Text.Replace(",", "")));
        if (!this.radNumLocalAmount.Value.HasValue || this.radNumLocalAmount.Value == 0)
            note.LOCAL_AMOUNT = 0;
        else
            note.LOCAL_AMOUNT = Convert.ToDecimal(this.radNumLocalAmount.Value);
        note.DESCRIPTION = this.radTxtDescription.Text;
        note.IS_DISUSED = "N";
        note.CREATOR_ID = Sessions.UserID;

        List<DTO_DOC_CREDIT_DEBIT_NOTE_ATTN> attns = new List<DTO_DOC_CREDIT_DEBIT_NOTE_ATTN>();
        foreach (AutoCompleteBoxEntry entry in this.radAcomAttn.Entries)
        {
            DTO_DOC_CREDIT_DEBIT_NOTE_ATTN attn = new DTO_DOC_CREDIT_DEBIT_NOTE_ATTN();
            attn.PROCESS_ID = this.hddProcessID.Value;

            if (entry.Value.NullObjectToEmptyEx().Length > 0)
            {
                SmallUserInfoDto dto = JsonConvert.JsonDeserialize<SmallUserInfoDto>(entry.Value);
                attn.ATTN_CODE = dto.USER_ID;
                attn.ATTN_NAME = dto.FULL_NAME;
                attn.ATTN_MAIL_ADDRESS = dto.MAIL_ADDRESS;
                attn.ATTN_TYPE = CommonEnum.MailAddressType.Internal.ToString().Substring(0, 1);
            }
            else
            {
                attn.ATTN_CODE = entry.Text;
                attn.ATTN_NAME = entry.Text;
                attn.ATTN_MAIL_ADDRESS = entry.Text;
                attn.ATTN_TYPE = CommonEnum.MailAddressType.External.ToString().Substring(0, 1);
            }

            attn.CREATOR_ID = Sessions.UserID;
            attns.Add(attn);
        }

        List<DTO_DOC_CREDIT_DEBIT_NOTE_CC> ccs = new List<DTO_DOC_CREDIT_DEBIT_NOTE_CC>();
        foreach (AutoCompleteBoxEntry entry in this.radAcomCC.Entries)
        {
            DTO_DOC_CREDIT_DEBIT_NOTE_CC cc = new DTO_DOC_CREDIT_DEBIT_NOTE_CC();
            cc.PROCESS_ID = this.hddProcessID.Value;

            if (entry.Value.NullObjectToEmptyEx().Length > 0)
            {
                SmallUserInfoDto dto = JsonConvert.JsonDeserialize<SmallUserInfoDto>(entry.Value);
                cc.CC_CODE = dto.USER_ID;
                cc.CC_NAME = dto.FULL_NAME;
                cc.CC_MAIL_ADDRESS = dto.MAIL_ADDRESS;
                cc.CC_TYPE = CommonEnum.MailAddressType.Internal.ToString().Substring(0, 1);
            }
            else
            {
                cc.CC_CODE = entry.Text;
                cc.CC_NAME = entry.Text;
                cc.CC_MAIL_ADDRESS = entry.Text;
                cc.CC_TYPE = CommonEnum.MailAddressType.External.ToString().Substring(0, 1);
            }

            cc.CREATOR_ID = Sessions.UserID;
            ccs.Add(cc);
        }

        List<DTO_DOC_CREDIT_DEBIT_NOTE_DESC> descriptions = (List<DTO_DOC_CREDIT_DEBIT_NOTE_DESC>)ViewState[VIEWSTATE_KEY];

        foreach (DTO_DOC_CREDIT_DEBIT_NOTE_DESC desc in descriptions)
        {
            desc.PROCESS_ID = this.hddProcessID.Value;
            desc.CREATOR_ID = Sessions.UserID;
        }

        using (CreditDebitNoteMgr mgr = new CreditDebitNoteMgr())
        {
            return mgr.MergeCreditDebitNote(note, attns, ccs, descriptions);
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

    #region [ Add Button Event ]

    private void UpdateGridData()
    {
        if (this.hddGridItems.Value.IsNullOrEmptyEx()) return;
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_CREDIT_DEBIT_NOTE_DESC> items = (List<DTO_DOC_CREDIT_DEBIT_NOTE_DESC>)serializer.Deserialize<List<DTO_DOC_CREDIT_DEBIT_NOTE_DESC>>(this.hddGridItems.Value);


        ViewState[VIEWSTATE_KEY] = items;
        this.radGrdDescription.DataSource = items;
        this.radGrdDescription.DataBind();

    }

    #endregion

    #region [ Grid Event ]

    protected void radGrdDescription_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            UpdateGridData();
            int index = Convert.ToInt32(e.CommandArgument);

            using (CreditDebitNoteMgr mgr = new CreditDebitNoteMgr())
            {
                mgr.DeleteCreditDebitNoteDescByIndex(this.hddProcessID.Value, index);
            }

            List<DTO_DOC_CREDIT_DEBIT_NOTE_DESC> list = (List<DTO_DOC_CREDIT_DEBIT_NOTE_DESC>)ViewState[VIEWSTATE_KEY];

            list.RemoveAll(p => p.IDX == index);


            this.radGrdDescription.DataSource = list;
            this.radGrdDescription.DataBind();
        }
    }

    #endregion

}