using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using Bayer.eWF.BSL.Common.Dto;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Telerik.Web.UI;

public partial class Approval_Document_SecondarySeal : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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
                InitPageInfo();
            }
            PageLoadInfo();
            this.UserID.Value = Sessions.UserID;
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    #region PageLoadInfo
    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현
        CallClientScript(); 
    }
    #endregion

    private void CallClientScript()
    {
        if ((this.radBtnSecondary.Checked) || (this.radBtnCorporate.Checked))
        {
            string type = GetSealTypeData(DataKind.Value);
            if (!ClientScript.IsStartupScriptRegistered("setSeal"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "setSeal", "setSeal('" + type + "');", true);
        }
    }

    #region InitPageInfo
    private void InitPageInfo()
    {
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0025";
        //hddProcessID.Value = "P000000488";

        InitControls();
    }
    #endregion

    #region DoSave
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

    private void InitControls()
    {
        DTO_DOC_SECONDARY_SEAL doc;
        if (!hddProcessID.Value.Equals(string.Empty))
        {
            using (SecondarySealMgr mgr = new SecondarySealMgr())
                doc = mgr.SelectSecondarySeal(hddProcessID.Value);
            if (doc != null)
            {

                // //Seal Holder
                //foreach (Control control in this.divSeal.Controls)
                //{
                //    if (control is RadButton)
                //    {
                //        if ((control as RadButton).Value.Equals(doc.SEAL_HOLDER_CODE))
                //        {
                //            (control as RadButton).Checked = true; break;
                //        }
                //    }
                //}

                if (radBtnSecondary.Value == doc.SEAL_TYPE)
                {
                    radBtnSecondary.Checked = true;
                    this.radDropSeal.SelectedValue = doc.SEAL_HOLDER_CODE;
                }
                else if (radBtnCorporate.Value == doc.SEAL_TYPE)
                {
                    radBtnCorporate.Checked = true;
                    this.radDropSeal.SelectedValue = doc.SEAL_HOLDER_CODE;
                }

                if (radBKL.Value == doc.SEAL_COMPANY)
                {
                    radBKL.Checked = true;
                    //this.radDropSeal.SelectedValue = doc.SEAL_HOLDER_CODE;
                }
                else if (radBCS.Value == doc.SEAL_COMPANY)
                {
                    radBCS.Checked = true;
                    //this.radDropSeal.SelectedValue = doc.SEAL_HOLDER_CODE;
                }

                if (RadElectronic.Value == doc.SIGN_TYPE)
                {
                    RadElectronic.Checked = true;
                    //this.radDropSeal.SelectedValue = doc.SEAL_HOLDER_CODE;
                }
                else if (RadPaper.Value == doc.SIGN_TYPE)
                {
                    RadPaper.Checked = true;
                    //this.radDropSeal.SelectedValue = doc.SEAL_HOLDER_CODE;
                }

                this.RadtxtRecipient.Text = doc.RECIPIENT;
                this.RadtxtPurpose.Text = doc.PURPOSE_DETAILS;
                webMaster.DocumentNo = doc.DOC_NUM;
            }
        }

    }

    #region DocumentSave
    private string DocumentSave(string ApprovalStatus)
    {
        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_SECONDARY_SEAL doc = null;
        string processId = string.Empty;
        
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_SECONDARY_SEAL();

        doc.PROCESS_ID = hddProcessID.Value;
        doc.PROCESS_STATUS = ApprovalStatus; //
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = this.Sessions.UserID.ToString();
        if (radBtnSecondary.Checked == true)  doc.SUBJECT = GetSealTypeData(DataKind.Value)+ "/" + this.radDropSeal.SelectedText + "/" + this.RadtxtRecipient.Text;  // Secondary Seal 을 선책했을 경우
        else if (radBtnCorporate.Checked == true) doc.SUBJECT = GetSealTypeData(DataKind.Value) + "/" + this.RadtxtRecipient.Text;  // Corporate Seal 을 선택했을 경우

        webMaster.Subject = doc.SUBJECT;
        doc.COMPANY_CODE = this.Sessions.CompanyCode.ToString();
        doc.ORGANIZATION_NAME = this.Sessions.OrgName.ToString();
        doc.LIFE_CYCLE = webMaster.LifeCycle.ToString();
        doc.IS_DISUSED = "N"; //보존연한 
        doc.CREATOR_ID = this.Sessions.UserID.ToString();
        doc.CREATE_DATE = DateTime.Now;

        if (this.radBtnSecondary.Checked)
        {
            doc.SEAL_TYPE = this.radBtnSecondary.Value;
            doc.SEAL_HOLDER_CODE = this.radDropSeal.SelectedValue;
        }
        else if (this.radBtnCorporate.Checked)
        {
            doc.SEAL_TYPE = this.radBtnCorporate.Value;
            doc.SEAL_HOLDER_CODE = null;
        }
        if (this.radBKL.Checked)
        {
            doc.SEAL_COMPANY = this.radBKL.Value;
            //doc.SEAL_HOLDER_CODE = this.radDropSeal.SelectedValue;
        }
        else if (this.radBCS.Checked)
        {
            doc.SEAL_COMPANY = this.radBCS.Value;
            //doc.SEAL_HOLDER_CODE = null;
        }
        if(this.RadElectronic.Checked)
            doc.SIGN_TYPE = this.RadElectronic.Value;

        if (this.RadPaper.Checked)
            doc.SIGN_TYPE = this.RadPaper.Value;


        doc.RECIPIENT = this.RadtxtRecipient.Text;
        doc.PURPOSE_DETAILS = this.RadtxtPurpose.Text;

        using (SecondarySealMgr mgr = new SecondarySealMgr())
        {
            processId = mgr.MergeSecondarySeal(doc);
        }
        return processId;
    }
    #endregion


    #region ValidationCheck
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus approvalStatus)
    {
        string message = string.Empty;

        if (GetSealTypeData(DataKind.Text).IsNullOrEmptyEx())
        {
            message = "Seal Type";
        }
        if (GetSignTypeData(DataKind.Text).IsNullOrEmptyEx())
        {
            message += (message.IsNullOrEmptyEx() ? "" : ",") + "날인 Type";
        }

        

        if (approvalStatus == ApprovalUtil.ApprovalStatus.Request)
        {
            if (this.radBtnSecondary.Checked) //Secondary Seal 선택시 
            {
                if (this.radDropSeal.SelectedIndex <= -1)
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Seal Holder";
            }

            if (RadtxtRecipient.Text.Length <= 0)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Recipient";
            
            if (RadtxtPurpose.Text.Length <= 0)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose & Details";

        }

        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;

    }
    #endregion

    enum DataKind
    {
        Text,
        Value,
    }

    private string GetSealTypeData(DataKind kind)
    {
        string rtnData = string.Empty; ;
        switch (kind)
        {
            case DataKind.Text:
                if (this.radBtnSecondary.Checked) rtnData = this.radBtnSecondary.Text;
                else if (this.radBtnCorporate.Checked) rtnData = this.radBtnCorporate.Text;                
                break;
            case DataKind.Value:
                if (this.radBtnSecondary.Checked) rtnData = this.radBtnSecondary.Value;
                else if (this.radBtnCorporate.Checked) rtnData = this.radBtnCorporate.Value;
                break;
        }

        return rtnData;
    }
    private string GetSignTypeData(DataKind kind)
    {
        string rtnData = string.Empty; ;
        switch (kind)
        {
            case DataKind.Text:
                if (this.RadElectronic.Checked) rtnData = this.RadElectronic.Text;
                else if (this.RadPaper.Checked) rtnData = this.RadPaper.Text;
                break;
            case DataKind.Value:
                if (this.RadElectronic.Checked) rtnData = this.RadElectronic.Value;
                else if (this.RadPaper.Checked) rtnData = this.RadPaper.Value;
                break;
        }

        return rtnData;
    }


    protected void RadCompany_Click(object sender, EventArgs e)
    {
        var COMPANY = "BCS";
        if (radBKL.Checked) COMPANY = "BKL";

        for(int i=0;i< radDropSeal.Items.Count; i++)
        {
            if (radDropSeal.Items[i].Text.IndexOf(COMPANY) >=0)
            {
                radDropSeal.Items[i].Enabled = true;
            }
            else
            {
                radDropSeal.Items[i].Enabled = false;
            }
        }
        //Grid_reset();
        //List<DTO_DOC_REBATE_POLICY_PRODUCT_NEW> list = (List<DTO_DOC_REBATE_POLICY_PRODUCT_NEW>)ViewState[VIEWSTATE_KEY_LIST_NEW];
        //list.Clear();
        //this.radGrdProduct_NEW.DataSource = list;
        //this.radGrdProduct_NEW.DataBind();


    }

}