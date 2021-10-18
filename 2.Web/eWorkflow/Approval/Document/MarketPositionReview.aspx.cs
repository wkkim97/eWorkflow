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

public partial class Approval_Document_MarketPositionReview : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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

    #region Page_Load
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

    #region InitPageInfo
    private void InitPageInfo()
    {
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0020";
        //hddProcessID.Value = "P000000391";

        InitControls();
    }
    #endregion

    #region InitControls
    private void InitControls()
    {
        DTO_DOC_MARKET_POSITION_REVIEW doc;

        if (!hddProcessID.Value.Equals(string.Empty))
        {
            using (MarketPositionReviewMgr mgr = new MarketPositionReviewMgr())
                doc = mgr.SelectMarketPositionReview(hddProcessID.Value);
            if (doc != null)
            {

                if (RadrdoBHP.Value == doc.BU) { RadrdoBHP.Checked = true; RadrdoBHP.Visible = true; }
                else if (RadrdoRI.Value == doc.BU) RadrdoRI.Checked = true;
                else if (RadrdoCC.Value == doc.BU) RadrdoCC.Checked = true;
                else if (RadrdoAH.Value == doc.BU) RadrdoAH.Checked = true;
                else if (RadrdoCP.Value == doc.BU) RadrdoCP.Checked = true;
                else if (RadrdoES.Value == doc.BU) RadrdoES.Checked = true;
                else if (RadrdoBVS.Value == doc.BU) RadrdoBVS.Checked = true;
                else if (RadrdoIS.Value == doc.BU) RadrdoIS.Checked = true;

                else if (RadrdoDC.Value == doc.BU) { RadrdoDC.Checked = true; RadrdoDC.Visible = true; }

                else if (RadrdoHH.Value == doc.BU) RadrdoHH.Checked = true;
                else if (RadrdoWH.Value == doc.BU) RadrdoWH.Checked = true;
                else if (RadrdoSM.Value == doc.BU) RadrdoSM.Checked = true;

                //BMS BU 필드는 사용하지 않음.(2017.07.19)
                //if (RadrdoPUR.Value == doc.BU) RadrdoPUR.Checked = true;
                //if (RadrdoPCS.Value == doc.BU) RadrdoPCS.Checked = true;
                //if (RadrdoCAS.Value == doc.BU) RadrdoCAS.Checked = true;

                Radtxtyear.Text = doc.YEAR;
                RadtxtProdut.Text = doc.PRODUCT;
                RadtxtApplication.Text = doc.APPLICATION_INDICATION;
                
                //1,2,3위작성 필드를 추가함과 동시에 Competitor 필드는 사용하지 않음.(2017.07.19)
                //RadtxtCompetitor.Text = doc.COMPETITOR;

                RadtxtShare.Value = (double?)doc.MARKET_SHARE;
                RadtxtVolume.Text = doc.MARKET_SHARE_VOLUME;
                RadtxtValue.Text = doc.MARKET_SHARE_VALUE;

                RadtxtRank_1.Value = (double?)doc.FIRST_MARKET_SHARE;
                RadtxtRank_2.Value = (double?)doc.SECOND_MARKET_SHARE;
                RadtxtRank_3.Value = (double?)doc.THIRD_MARKET_SHARE;
                RadtxtRank_Total.Value = (double?)doc.TOTAL_MARKET_SHARE;

                RadtxtProductCompany_1.Text = doc.FIRST_PRODUCT_COMPANY_NAME;
                RadtxtProductCompany_2.Text = doc.SECOND_PRODUCT_COMPANY_NAME;
                RadtxtProductCompany_3.Text = doc.THIRD_PRODUCT_COMPANY_NAME;

                RadtxtDataSource.Text = doc.DATA_SOURCE;
                webMaster.DocumentNo = doc.DOC_NUM;

                if (doc.CHECKED_DISCRIMINATION == "Y") RadchkDis.Checked = true;
                if (doc.CHECKED_EXCESSIVE_PRICING == "Y") RadchkExcess.Checked = true;
                if (doc.CHECKED_PREDATORY_PRICING == "Y") RadchkPre.Checked = true;
                if (doc.CHECKED_MARGIN_SQUEEZE == "Y") RadchkMargin.Checked = true;
                if (doc.CHECKED_TRYING_AND_BUNDING == "Y") RadchkTying.Checked = true;
                if (doc.CHECKED_LOYALTY_AND_DISCOUNT == "Y") RadchkLoyaly.Checked = true;
                if (doc.CHECKED_ABUSIVE_EXCLUSIVE_DEALING == "Y") RadchkDealing.Checked = true;
                if (doc.CHECKED_ABUSIVE_REFUSAL_TO_SUPPLY == "Y") RadchkSupply.Checked = true;
            }
        }
    }


    private void RadtxtRead()
    {        
    }
    #endregion

    #region PageLoadInfo
    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현         


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

    private string GetBUCode()
    {
        string buCode = string.Empty;
        if (this.RadrdoBHP.Checked) buCode = this.RadrdoBHP.Value;
        else if (this.RadrdoRI.Checked) buCode = this.RadrdoRI.Value;
        else if (this.RadrdoHH.Checked) buCode = this.RadrdoHH.Value;
        else if (this.RadrdoWH.Checked) buCode = this.RadrdoWH.Value;
        else if (this.RadrdoSM.Checked) buCode = this.RadrdoSM.Value;
        else if (this.RadrdoCC.Checked) buCode = this.RadrdoCC.Value;
        else if (this.RadrdoAH.Checked) buCode = this.RadrdoAH.Value;
        else if (this.RadrdoDC.Checked) buCode = this.RadrdoDC.Value;
        else if (this.RadrdoCP.Checked) buCode = this.RadrdoCP.Value;
        else if (this.RadrdoES.Checked) buCode = this.RadrdoES.Value;
        else if (this.RadrdoBVS.Checked) buCode = this.RadrdoBVS.Value;
        else if (this.RadrdoIS.Checked) buCode = this.RadrdoIS.Value;

        //BMS BU 는 더 이상 사용하지 않음. (2018.07.19)
        //else if (this.RadrdoPUR.Checked) buCode = this.RadrdoPUR.Value;
        //else if (this.RadrdoPCS.Checked) buCode = this.RadrdoPCS.Value;
        //else if (this.RadrdoCAS.Checked) buCode = this.RadrdoCAS.Value;
        return buCode;
    }

    #region DocumentSave
    private string DocumentSave(string ApprovalStatus)
    {
        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_MARKET_POSITION_REVIEW doc = null;
        string processId = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_MARKET_POSITION_REVIEW();
        doc.PROCESS_ID = hddProcessID.Value;
        doc.PROCESS_STATUS = ApprovalStatus; 
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = this.Sessions.UserID.ToString();
        doc.SUBJECT = GetBUCode() + "/" + this.RadtxtProdut.Text;          
        webMaster.Subject = doc.SUBJECT;
        doc.COMPANY_CODE = this.Sessions.CompanyCode.ToString();
        doc.ORGANIZATION_NAME = this.Sessions.OrgName.ToString();
        doc.LIFE_CYCLE = webMaster.LifeCycle.ToString();
        doc.IS_DISUSED = "N";                                                       //보존연한 
        doc.CREATOR_ID = this.Sessions.UserID.ToString();
        doc.CREATE_DATE = DateTime.Now;

        if (RadrdoBHP.Checked)
            doc.BU = RadrdoBHP.Value;
        else if (RadrdoHH.Checked)
            doc.BU = RadrdoHH.Value;
        else if (RadrdoWH.Checked)
            doc.BU = RadrdoWH.Value;
        else if (RadrdoSM.Checked)
            doc.BU = RadrdoSM.Value;
        else if (RadrdoRI.Checked)
            doc.BU = RadrdoRI.Value;
        else if (RadrdoCC.Checked)
            doc.BU = RadrdoCC.Value;
        else if (RadrdoAH.Checked)
            doc.BU = RadrdoAH.Value;
        else if (RadrdoCP.Checked)
            doc.BU = RadrdoCP.Value;
        else if (RadrdoES.Checked)
            doc.BU = RadrdoES.Value;
        else if (RadrdoBVS.Checked)
            doc.BU = RadrdoBVS.Value;
        else if (RadrdoIS.Checked)
            doc.BU = RadrdoIS.Value;

        //BMS BU 는 더 이상 사용하지 않음. (2018.07.19)
        //else if (RadrdoPUR.Checked)
        //    doc.BU = RadrdoPUR.Value;
        //else if (RadrdoPCS.Checked)
        //    doc.BU = RadrdoPCS.Value;
        //else if (RadrdoCAS.Checked)
        //    doc.BU = RadrdoCAS.Value;

        doc.YEAR = Radtxtyear.Text;
        doc.PRODUCT = RadtxtProdut.Text;
        doc.APPLICATION_INDICATION = RadtxtApplication.Text;

        //1,2,3위작성 필드를 추가함과 동시에 Competitor 필드는 사용하지 않음.(2017.07.19_
        //doc.COMPETITOR = RadtxtCompetitor.Text;

        if (RadtxtShare.Text.Equals(string.Empty)) doc.MARKET_SHARE = 0; else doc.MARKET_SHARE = (float)RadtxtShare.Value;        
        doc.MARKET_SHARE_VOLUME = RadtxtVolume.Text;
        doc.MARKET_SHARE_VALUE = RadtxtValue.Text;

        if (RadtxtRank_1.Text.Equals(string.Empty)) doc.FIRST_MARKET_SHARE = 0; else doc.FIRST_MARKET_SHARE = (float)RadtxtRank_1.Value;
        if (RadtxtRank_2.Text.Equals(string.Empty)) doc.SECOND_MARKET_SHARE = 0; else doc.SECOND_MARKET_SHARE = (float)RadtxtRank_2.Value;
        if (RadtxtRank_3.Text.Equals(string.Empty)) doc.THIRD_MARKET_SHARE = 0; else doc.THIRD_MARKET_SHARE = (float)RadtxtRank_3.Value;
        if (RadtxtRank_Total.Text.Equals(string.Empty)) doc.TOTAL_MARKET_SHARE = 0; else doc.TOTAL_MARKET_SHARE = (float)RadtxtRank_Total.Value;


        doc.FIRST_PRODUCT_COMPANY_NAME = RadtxtProductCompany_1.Text;
        doc.SECOND_PRODUCT_COMPANY_NAME = RadtxtProductCompany_2.Text;
        doc.THIRD_PRODUCT_COMPANY_NAME = RadtxtProductCompany_3.Text;


        doc.DATA_SOURCE = RadtxtDataSource.Text;
        doc.CHECKED_DISCRIMINATION = RadchkDis.Checked ? "Y" : "N";
        doc.CHECKED_EXCESSIVE_PRICING = RadchkExcess.Checked ? "Y" : "N";
        doc.CHECKED_PREDATORY_PRICING = RadchkPre.Checked ? "Y" : "N";
        doc.CHECKED_MARGIN_SQUEEZE = RadchkMargin.Checked ? "Y" : "N";
        doc.CHECKED_TRYING_AND_BUNDING = RadchkTying.Checked ? "Y" : "N";
        doc.CHECKED_LOYALTY_AND_DISCOUNT = RadchkLoyaly.Checked ? "Y" : "N";
        doc.CHECKED_ABUSIVE_EXCLUSIVE_DEALING = RadchkDealing.Checked ? "Y" : "N";
        doc.CHECKED_ABUSIVE_REFUSAL_TO_SUPPLY = RadchkSupply.Checked ? "Y" : "N";

        using (MarketPositionReviewMgr mgr = new MarketPositionReviewMgr())
        {
            processId = mgr.MergeMarketPositionReview(doc);
        }
        return processId;
    }
    #endregion

    #region ValidationCheck
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus approvalStatus)
    {
        string message = string.Empty;
        if (approvalStatus == ApprovalUtil.ApprovalStatus.Request)
        {
            //BMS BU 는 더 이상 사용하지 않음. (2018.07.19)
            //if (RadrdoBHP.Checked == false && RadrdoRI.Checked == false && RadrdoCC.Checked == false && RadrdoAH.Checked == false && RadrdoPUR.Checked == false && RadrdoPCS.Checked == false && RadrdoCAS.Checked == false && RadrdoCP.Checked == false && RadrdoES.Checked == false && RadrdoBVS.Checked == false)
            //    message += "Division/BU";

            if (RadrdoHH.Checked == false && RadrdoWH.Checked == false && RadrdoSM.Checked == false && RadrdoRI.Checked == false && RadrdoCC.Checked == false && RadrdoAH.Checked == false && RadrdoCP.Checked == false && RadrdoES.Checked == false && RadrdoBVS.Checked == false)
                message += "Division/BU";
            if (Radtxtyear.Text.Length <= 0)
                message += message.IsNullOrEmptyEx()?  "Year" : ",Year";
            if(RadtxtProdut.Text.Length <=0)
                message += message.IsNullOrEmptyEx() ? "Product" : ",Product";
            //if(RadtxtApplication.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "Application/Indication" : ",Application/Indication";
            //if(RadtxtCompetitor.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "Competitor" : ",Competitor";
            if(RadtxtShare.Text.Length <= 0 )
                message += message.IsNullOrEmptyEx() ? "Market Share" : ",Market Share";
            //if(RadtxtVolume.Text.Length <= 0 )
            //    message += message.IsNullOrEmptyEx() ? "Market Share-Volume" : ",Market Share-Volume";
            //if(RadtxtValue.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "Market Share-Value" : ",Market Share-Value";
            //if(RadtxtDataSource.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "Data Source" : ",Data Source";            

            if(RadtxtRank_1.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Market Ranking(%) - 1st" : ",Market Ranking(%) - 1st";
            if (RadtxtRank_2.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Market Ranking(%) - 2nd" : ",Market Ranking(%) - 2nd";
            if (RadtxtRank_3.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Market Ranking(%) - 3rd" : ",Market Ranking(%) - 3rd";

            if (RadtxtProductCompany_1.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Product/Company name - 1st" : ",Product/Company name -1st";
            if (RadtxtProductCompany_2.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Product/Company name - 2nd" : ",Product/Company name - 2nd";
            if (RadtxtProductCompany_3.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Product/Company name - 3rd" : ",Product/Company name - 3rd";

        }
        if (approvalStatus == ApprovalUtil.ApprovalStatus.Saved)
        {
            
        }
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;

    }
    #endregion
}