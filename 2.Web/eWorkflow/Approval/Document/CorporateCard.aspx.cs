using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

using DNSoft.eWF.FrameWork.Web;
using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Approval.Dto;


public partial class Approval_Document_CorporateCard : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private enum ChangementItem
    {
        Bank,
        CellPhone,
        Addr,
        Name,
        Pwd
    }
    string Bank = "N";
    string cell = "N";
    string address = "N";
    string name = "N";
    string passwod = "N";

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

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
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

    #region PageLoadInfo
    private void PageLoadInfo()
    {
        string Period = null;
        string Other = null;
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 

        if (RadDbCategory.SelectedValue == "Re-issue")
        {
            if (!ClientScript.IsStartupScriptRegistered("fn_ReissueCategory"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "fn_ReissueCategory", "fn_ReissueCategory('" + RadDbCategory.SelectedValue + "');", true);
        }
        else if (RadDbCategory.SelectedValue == "Increase")
        {
            if (RadrdoTemporary.Checked) Period = RadrdoTemporary.Value; else Period = RadrdoPermanent.Value;
            if (RadrdoReason3.Checked) Other = RadrdoReason3.Value;

            if (!ClientScript.IsStartupScriptRegistered("fn_IncreaseCategory"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "fn_IncreaseCategory", "fn_IncreaseCategory('" + RadDbCategory.SelectedValue + "','" + Period + "','" + Other + "');", true);
        }
        else if (RadDbCategory.SelectedValue == "Change")
        {
            if (RadrdoBank.Checked) Bank = "Y";
            if (RadrdoCellPhone.Checked) cell = "Y";
            if (RadrdoAddr.Checked) address = "Y";
            if (RadrdoName.Checked) name = "Y";
            if (RadrdoPwd.Checked) passwod = "Y";

            if (!ClientScript.IsStartupScriptRegistered("fn_ChangeCategory"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "fn_ChangeCategory", "fn_ChangeCategory('" + RadDbCategory.SelectedValue + "','" + Bank + "','" + cell + "','" + address + "','" + name + "','" + passwod + "');", true);
        }
    }
    #endregion

    #region InitPageInfo
    private void InitPageInfo()
    {
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0015"; //문서번호
        //hddProcessID.Value = "P000000720";

        RaddpStardata.SelectedDate = DateTime.Now.Date;
        RadDatePicker1.SelectedDate = DateTime.Now.Date;

        InitControls();
    }

    private void InitControls()
    {
        DTO_DOC_CORPORATE_CARD doc;
        if (!hddProcessID.Value.Equals(string.Empty))
        {
            string processId = this.hddProcessID.Value;
            using (Bayer.eWF.BSL.Approval.Mgr.CorporateCardMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CorporateCardMgr())
            {
                doc = mgr.selectCorporateCard(processId);
                this.hddProcessStatus.Value = doc.PROCESS_STATUS;

            }
            if (doc != null)
            {
                webMaster.DocumentNo = doc.DOC_NUM;
                RadDbCategory.SelectedValue = doc.CATEGORY_CODE;
                string categoryCode = RadDbCategory.SelectedValue;
                if (categoryCode == "Re-issue")
                {
                    this.RadDatePicker1.SelectedDate = DateTime.Parse(doc.BIRTHDAY.ToString());
                    this.RadReasonValue.Text = doc.REASON;

                    if (!ClientScript.IsStartupScriptRegistered("fn_ReissueCategory"))
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "fn_ReissueCategory", "fn_ReissueCategory('" + doc.CATEGORY_CODE + "');", true);
                }
                else if (categoryCode == "Increase")
                {
                    this.RadtxtCardNumber.ReadOnly = true;
                    this.RadtxtCardNumber.Text = doc.CARD_NUMBER;

                    if (doc.PERIOD_FOR_INCREASE_CODE.Length > 1 && doc.PERIOD_FOR_INCREASE_CODE.Substring(0, 1) == "T")
                    {
                        //Read only

                        this.RadrdoTemporary.Checked = true;
                        this.RaddpStardata.SelectedDate = DateTime.Parse(doc.STARTING_DATE.ToString());
                        this.RadcbPeriod.SelectedValue = doc.PERIOD;
                        this.RadtxtInAmount.Value = (double?)doc.INCREASE_AMOUNT;
                    }
                    else if (doc.PERIOD_FOR_INCREASE_CODE.Length > 1 && doc.PERIOD_FOR_INCREASE_CODE.Substring(0, 1) == "P")
                    {

                        this.RadrdoPermanent.Checked = true;
                        if (doc.INCREASE_AMOUNT.HasValue)
                        {
                            if (doc.INCREASE_AMOUNT == 10000000)
                                this.RadrdoAmount2.Checked = true;
                            else if (doc.INCREASE_AMOUNT == 7500000)
                                this.RadrdoAmount1.Checked = true;
                        }
                    }

                    if (doc.REASON_CODE != null && doc.REASON_CODE.Length > 1)
                    {
                        if (doc.REASON_CODE.Substring(0, 1) == "B")
                            this.RadrdoReason1.Checked = true;
                        else if (doc.REASON_CODE.Substring(0, 1) == "S")
                            this.RadrdoReason2.Checked = true;
                        else if (doc.REASON_CODE.Substring(0, 1) == "O")
                        {
                            this.RadrdoReason3.Checked = true;
                        }
                    }
                    this.RadtxtOthers.Text = doc.REASON_OTHERS;
                    string strIncreaseCode = doc.PERIOD_FOR_INCREASE_CODE;
                    string strReasonCode = doc.REASON_CODE;
                    
                    
                    if (!ClientScript.IsStartupScriptRegistered("fn_IncreaseCategory"))
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "fn_IncreaseCategory", "fn_IncreaseCategory('" + doc.CATEGORY_CODE + "','" + strIncreaseCode + "','" + strReasonCode + "');", true);

                }
                else if (categoryCode == "Change")
                {
                    this.RadTextCardNumber2.Text = doc.CARD_NUMBER;
                    //string cItem; 
                    string[] item = doc.CHANGEMENT_ITEM.Split(new string[] { "/" }, StringSplitOptions.None);
                    for (int i = 0; i < item.Length; i++)
                    {
                        if (item[i].ToString().Equals(ChangementItem.Bank.ToString()))
                        {
                            this.RadrdoBank.Checked = true;
                            this.RadtxtBankName.Text = doc.BANK_NAME;
                            this.RadtxtBankNumber.Text = doc.BANK_ACOUNT_NUMBER;
                            Bank = "Y";
                        }
                        else if (item[i].ToString().Equals(ChangementItem.CellPhone.ToString()))
                        {
                            this.RadrdoCellPhone.Checked = true;
                            this.RadtxtCell.Text = doc.CELL_PHONE_NUMBER;
                            cell = "Y";
                        }
                        else if (item[i].ToString().Equals(ChangementItem.Addr.ToString()))
                        {
                            this.RadrdoAddr.Checked = true;
                            this.RadtxtAddr.Text = doc.ADDRESS;
                            address = "Y";
                        }
                        else if (item[i].ToString().Equals(ChangementItem.Name.ToString()))
                        {
                            this.RadrdoName.Checked = true;
                            this.RadtxtName.Text = doc.NAME;
                            name = "Y";
                        }
                        else if (item[i].ToString().Equals(ChangementItem.Pwd.ToString()))
                        {
                            this.RadrdoPwd.Checked = true;
                            this.RadtxtNewpwd.Text = doc.NEW_PASSWORD;
                            passwod = "Y";
                        }
                    }
                    if (!ClientScript.IsStartupScriptRegistered("fn_ChangeCategory"))
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "fn_ChangeCategory", "fn_ChangeCategory('" + doc.CATEGORY_CODE + "','" + Bank + "','" + cell + "','" + address + "','" + name + "','" + passwod + "');", true);
                }
            }
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
    #endregion

    #region Do Save
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
    #endregion

    #region Document Save
    private string DocumentSave(string processState)
    {
        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_CORPORATE_CARD doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_CORPORATE_CARD();
        doc.PROCESS_ID = hddProcessID.Value;
        doc.PROCESS_STATUS = processState; //
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = this.Sessions.UserID.ToString();
        doc.SUBJECT = subject(RadDbCategory.SelectedText);   // category:Type
        webMaster.Subject = doc.SUBJECT;
        doc.COMPANY_CODE = this.Sessions.CompanyCode.ToString();
        doc.ORGANIZATION_NAME = this.Sessions.OrgName.ToString();
        doc.LIFE_CYCLE = webMaster.LifeCycle.ToString();
        doc.IS_DISUSED = "N"; //보존연한 
        doc.CREATOR_ID = this.Sessions.UserID.ToString();
        doc.CREATE_DATE = DateTime.Now;

        doc.CATEGORY_CODE = subject(this.RadDbCategory.SelectedText);

        if (doc.SUBJECT == "Re-issue")
        {
            doc.BIRTHDAY = Convert.ToDateTime(this.RadDatePicker1.SelectedDate.Value.ToShortDateString());
            doc.REASON = this.RadReasonValue.Text;
        }
        else if (doc.SUBJECT == "Increase")
        {
            doc.CARD_NUMBER = this.RadtxtCardNumber.Text;
            if (this.RadrdoTemporary.Checked)
            {
                doc.PERIOD_FOR_INCREASE_CODE = this.RadrdoTemporary.Text;
                this.hddPeriod.Value = this.RadrdoTemporary.Value;
            }
            else if (this.RadrdoPermanent.Checked)
            {
                doc.PERIOD_FOR_INCREASE_CODE = this.RadrdoPermanent.Text;
                this.hddPeriod.Value = this.RadrdoPermanent.Value;
            }
            else
                doc.PERIOD_FOR_INCREASE_CODE = string.Empty;
            doc.STARTING_DATE = Convert.ToDateTime(this.RaddpStardata.SelectedDate.Value.ToShortDateString());
            doc.PERIOD = this.RadcbPeriod.SelectedValue;

            if (this.hddPeriod.Value == "Temporary")
                doc.INCREASE_AMOUNT = (decimal?)this.RadtxtInAmount.Value;

            else if (this.hddPeriod.Value == "Permanent")
            {
                if (this.RadrdoAmount1.Checked)
                    doc.INCREASE_AMOUNT = Convert.ToDecimal(this.RadrdoAmount1.Value);
                else if (this.RadrdoAmount2.Checked)
                    doc.INCREASE_AMOUNT = Convert.ToDecimal(this.RadrdoAmount2.Value);
            }

            if (this.RadrdoReason1.Checked)
                doc.REASON_CODE = this.RadrdoReason1.Text;
            else if (this.RadrdoReason2.Checked)
                doc.REASON_CODE = this.RadrdoReason2.Text;
            else if (this.RadrdoReason3.Checked)
                doc.REASON_CODE = this.RadrdoReason3.Text;
            else
                doc.REASON_CODE = string.Empty;
            doc.REASON_OTHERS = this.RadtxtOthers.Text;
        }
        else if (doc.SUBJECT == "Change")
        {
            doc.CARD_NUMBER = this.RadTextCardNumber2.Text;
            if (this.RadrdoBank.Checked)
            {
                doc.CHANGEMENT_ITEM = this.RadrdoBank.Value + "/";
                doc.BANK_NAME = this.RadtxtBankName.Text;
                doc.BANK_ACOUNT_NUMBER = this.RadtxtBankNumber.Text;
                this.hddChangement.Value = doc.CHANGEMENT_ITEM;
            }
            if (this.RadrdoCellPhone.Checked)
            {
                doc.CHANGEMENT_ITEM = doc.CHANGEMENT_ITEM + this.RadrdoCellPhone.Value + "/";
                doc.CELL_PHONE_NUMBER = this.RadtxtCell.Text;
                this.hddChangement.Value = doc.CHANGEMENT_ITEM;
            }
            if (this.RadrdoAddr.Checked)
            {
                doc.CHANGEMENT_ITEM = doc.CHANGEMENT_ITEM + this.RadrdoAddr.Value + "/";
                doc.ADDRESS = this.RadtxtAddr.Text;
            }
            if (this.RadrdoName.Checked)
            {
                doc.CHANGEMENT_ITEM = doc.CHANGEMENT_ITEM + this.RadrdoName.Value + "/";
                doc.NAME = this.RadtxtName.Text;
            }
            if (this.RadrdoPwd.Checked)
            {
                doc.CHANGEMENT_ITEM = doc.CHANGEMENT_ITEM + this.RadrdoPwd.Value + "/";
                doc.NEW_PASSWORD = this.RadtxtNewpwd.Text;
            }
        }
        using (Bayer.eWF.BSL.Approval.Mgr.CorporateCardMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CorporateCardMgr())
        {
            processID = mgr.mergeCorporateCard(doc);
        }

        return processID;

    }
    #endregion

    #region Validation Check
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;
        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            if (RadDbCategory.SelectedIndex == -1)
                message += "Category";
            //if (subject(RadDbCategory.SelectedText) == "Re-issue")
            //{
            //    if (RadReasonValue.Text.Length <= 0)
            //        message += message.IsNullOrEmptyEx() ? "Reason" : ",Reason";
            //}
            //else if (subject(RadDbCategory.SelectedText) == "Increase")
            //{
            //    if (RadtxtCardNumber.Text.Length <= 0)
            //        message += message.IsNullOrEmptyEx() ? "CardNumber" : ",CardNumber";

            //    if (RadrdoTemporary.Checked == false && RadrdoPermanent.Checked == false)
            //        message += message.IsNullOrEmptyEx() ? "Period for increase" : ",Period for increase";

            //    if (RadrdoTemporary.Checked == true)
            //    {
            //        if (RadtxtInAmount.Text.Length <= 0)
            //            message += message.IsNullOrEmptyEx() ? "increase Amount" : ",increase Amount";
            //    }
            //    else if (RadrdoPermanent.Checked == true)
            //    {
            //        if (RadrdoAmount1.Checked == false && RadrdoAmount2.Checked == false)
            //            message += message.IsNullOrEmptyEx() ? "increase Amount" : ",increase Amount";
            //    }

            //    if (RadrdoReason1.Checked == false && RadrdoReason2.Checked == false && RadrdoReason3.Checked == false)
            //        message += message.IsNullOrEmptyEx() ? "Reason" : ",Reason";

            //    if (RadrdoReason3.Checked == true)
            //    {
            //        if (RadtxtOthers.Text.Length <= 0)
            //            message += message.IsNullOrEmptyEx() ? "Others" : ",Others";
            //    }

            //    if (this.RadtxtOthers.Text.IsNullOrEmptyEx())
            //        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Detailed description";
            //}
            //else if (subject(RadDbCategory.SelectedText) == "Change")
            //{
            //    if (RadTextCardNumber2.Text.Length <= 0)
            //        message += message.IsNullOrEmptyEx() ? "Card Number" : ",Card Number";

            //    if (RadrdoBank.Checked == true && RadtxtBankName.Text.Length <= 0)
            //        message += message.IsNullOrEmptyEx() ? "Bank Name" : ",Bank Name";

            //    if (RadrdoBank.Checked == true && RadTextCardNumber2.Text.Length <= 0)
            //        message += message.IsNullOrEmptyEx() ? "Bank Account Number" : ",Bank Account Number";

            //    if (RadrdoCellPhone.Checked == true && RadtxtCell.Text.Length <= 0)
            //        message += message.IsNullOrEmptyEx() ? "Cell Phone Number" : ",Cell Phone Number";

            //    if (RadrdoAddr.Checked == true && RadtxtAddr.Text.Length <= 0)
            //        message += message.IsNullOrEmptyEx() ? "Address" : ",Address";

            //    if (RadrdoName.Checked == true && RadtxtName.Text.Length <= 0)
            //        message += message.IsNullOrEmptyEx() ? "Name" : ",Name";

            //    if (RadrdoPwd.Checked == true && RadtxtNewpwd.Text.Length <= 0)
            //        message += message.IsNullOrEmptyEx() ? "New Password" : ",New Password";
            //}
        }
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;

    }
    #endregion

    private string subject(string SubjectText)
    {
        string[] result;
        result = SubjectText.Split(new string[] { " " }, StringSplitOptions.None);

        return result[0];
    }


}