using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Mgr;

public partial class Approval_Document_ItResource : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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

    #region PreRender
    protected override void OnPreRender(EventArgs e)
    {
        if (!webMaster.DocumentNo.Equals(string.Empty))
        {
            this.webMaster.SetEnableControls(radDatSoftFrom, false);
            this.webMaster.SetEnableControls(radDatSoftTo, false);
            //this.radDatSoftFrom.Enabled = false;
            //this.radDatSoftTo.Enabled = false;
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
        hddDocumentID.Value = "D0029";
        //HddProcessID.Value = "P000000606";

        InitControls();
    }

    private void InitControls()
    {
        //기등록 문서 조회
        if (!hddProcessID.Value.IsNullOrEmptyEx())
        {
            using (ITResourceMgr mgr = new ITResourceMgr())
            {
                DTO_DOC_IT_RESOURCE doc = mgr.SelectITResource(this.hddProcessID.Value);
                webMaster.DocumentNo = doc.DOC_NUM;
                if (doc.CATEGORY == "Hardware") //Hardware인 경우
                {
                    radBtnHardware.Visible = true;
                    radBtnHardware.Checked = true;

                    this.radBtnHardware.Checked = true;
                    //Type
                    if (doc.TYPE.Equals(this.radBtnLoss.Value)) this.radBtnLoss.Checked = true;
                    else if (doc.TYPE.Equals(this.radBtnTrouble.Value)) this.radBtnTrouble.Checked = true;
                    else if (doc.TYPE.Equals(this.radBtnNew.Value)) this.radBtnNew.Checked = true;
                    if (this.radBtnNew.Checked)
                    {
                        this.radBtnHardwareComputer.Visible = true;
                        this.radBtnHardwareMobile.Visible = false;
                        this.radBtnHardwareETC.Visible = true;
                        this.radBtnHardwareIpad.Visible = false;
                    }
                    else
                    {
                        this.radBtnHardwareComputer.Visible = true;
                        this.radBtnHardwareMobile.Visible = true;
                        this.radBtnHardwareETC.Visible = false;
                        this.radBtnHardwareIpad.Visible = true;
                    }
                    //Resource
                    if (doc.RESOURCE.Equals(this.radBtnHardwareMobile.Value)) this.radBtnHardwareMobile.Checked = true;
                    else if (doc.RESOURCE.Equals(this.radBtnHardwareComputer.Value)) this.radBtnHardwareComputer.Checked = true;
                    else if (doc.RESOURCE.Equals(this.radBtnHardwareETC.Value)) this.radBtnHardwareETC.Checked = true;
                    else if (doc.RESOURCE.Equals(this.radBtnHardwareIpad.Value)) this.radBtnHardwareIpad.Checked = true;

                    if (this.radBtnLoss.Checked && this.radBtnHardwareComputer.Checked) //컴퓨터 분실
                    {
                        //정보보안
                        //this.radBtnComReport.Checked = doc.SECURITY_REPORT.Equals("Y") ? true : false;
                        this.radTxtComLossNum.Text = doc.SECURITY_REPORT;
                        //분실사유
                        //if (doc.LOSS_REASON.Equals(this.radBtnComCareless.Value)) this.radBtnComCareless.Checked = true;
                        //else if (doc.LOSS_REASON.Equals(this.radBtnComStolen.Value)) this.radBtnComStolen.Checked = true;
                        //모델
                        if (doc.COMPUTER_MODEL.Equals(this.radBtnComSmallNotebook.Value)) this.radBtnComSmallNotebook.Checked = true;
                        else if (doc.COMPUTER_MODEL.Equals(this.radBtnComBigNotebook.Value)) this.radBtnComBigNotebook.Checked = true;
                        else if (doc.COMPUTER_MODEL.Equals(this.radBtnComDesktop.Value)) this.radBtnComDesktop.Checked = true;
                    }
                    else if (this.radBtnLoss.Checked && this.radBtnHardwareIpad.Checked) //아이패드 분실
                    {
                        //this.radBtnIpadReport.Checked = doc.SECURITY_REPORT.Equals("Y") ? true : false;
                        this.radTxtIpadLossNum.Text = doc.SECURITY_REPORT;
                        //if (doc.LOSS_REASON.Equals(this.radBtnIpadCareless.Value)) this.radBtnIpadCareless.Checked = true;
                        //else if (doc.LOSS_REASON.Equals(this.radBtnIpadStolen.Value)) this.radBtnIpadStolen.Checked = true;

                        this.radTxtIpadModel.Text = doc.MOBILE_MODEL;
                    }
                    else if (this.radBtnLoss.Checked && this.radBtnHardwareMobile.Checked) //모바일 분실
                    {
                        //정보보안
                       // this.radBtnMobileReport.Checked = doc.SECURITY_REPORT.Equals("Y") ? true : false;
                       this.radTxtMobileLossNum.Text = doc.SECURITY_REPORT;
                        //분실사유
                        //if (doc.LOSS_REASON.Equals(this.radBtnComCareless.Value)) this.radBtnMobCareless.Checked = true;
                        //else if (doc.LOSS_REASON.Equals(this.radBtnComStolen.Value)) this.radBtnMobStolen.Checked = true;
                        //모델
                        this.radTxtMobileModel.Text = doc.MOBILE_MODEL;
                        //this.radTxtMobileColor.Text = doc.MOBILE_COLOR;

                    }
                    else if (this.radBtnTrouble.Checked && this.radBtnHardwareComputer.Checked)
                    {
                        this.radTxtComState.Text = doc.STATE;
                    }
                    else if (this.radBtnTrouble.Checked && this.radBtnHardwareMobile.Checked)
                    {
                        this.radTxtMobileState.Text = doc.STATE;
                    }
                    else if (this.radBtnTrouble.Checked && this.radBtnHardwareIpad.Checked)
                    {
                        this.radTxtIpadState.Text = doc.STATE;
                    }
                    else if (this.radBtnNew.Checked && this.radBtnHardwareETC.Checked) //신규 기타
                    {
                        //모델
                        if (doc.NEW_MODEL.Equals(this.radBtnNewModelMouse.Value)) this.radBtnNewModelMouse.Checked = true;
                        else if (doc.NEW_MODEL.Equals(this.radBtnNewModelAdaptor.Value)) this.radBtnNewModelAdaptor.Checked = true;
                        else if (doc.NEW_MODEL.Equals(this.radBtnNewModelKeyboard.Value)) this.radBtnNewModelKeyboard.Checked = true;
                        else if (doc.NEW_MODEL.Equals(this.radBtnNewModelMonitor.Value)) this.radBtnNewModelMonitor.Checked = true;
                        else if (doc.NEW_MODEL.Equals(this.radBtnNewModelDocking.Value)) this.radBtnNewModelDocking.Checked = true;
                        else if (doc.NEW_MODEL.Equals(this.radBtnNewModelDVD.Value)) this.radBtnNewModelDVD.Checked = true;
                        else if (doc.NEW_MODEL.Equals(this.radBtnNewModelBattery.Value)) this.radBtnNewModelBattery.Checked = true;
                        //사유
                        this.radTxtNewETCPurpose.Text = doc.NEW_PURPOSE;
                    }
                    else if (this.radBtnNew.Checked && this.radBtnHardwareComputer.Checked) //신규 컴퓨터
                    {
                        //타입
                        if (doc.COMPUTER_TYPE.Equals(this.radBtnComTypeStandard.Value)) this.radBtnComTypeStandard.Checked = true;
                        else if (doc.COMPUTER_TYPE.Equals(this.radBtnComTypeFunctional.Value)) this.radBtnComTypeFunctional.Checked = true;
                        //모델
                        if (doc.COMPUTER_MODEL.Equals(this.radBtnNewComSmallNotebook.Value)) this.radBtnNewComSmallNotebook.Checked = true;
                        else if (doc.COMPUTER_MODEL.Equals(this.radBtnNewComBigNotebook.Value)) this.radBtnNewComBigNotebook.Checked = true;
                        else if (doc.COMPUTER_MODEL.Equals(this.radBtnNewComDesktop.Checked)) this.radBtnNewComDesktop.Checked = true;
                        //사유
                        this.radTxtNewComPurpose.Text = doc.NEW_PURPOSE;
                    }

                }
                else if (doc.CATEGORY == "Software") //Software인 경우
                {
                    this.radBtnSoftware.Checked = true;
                    //타입
                    if (doc.SOFTWARE_TYPE.Equals(this.radBtnSoftHwp.Value)) this.radBtnSoftHwp.Checked = true;
                    else if (doc.SOFTWARE_TYPE.Equals(this.radBtnSoftAcrobatSTD.Value)) this.radBtnSoftAcrobatSTD.Checked = true;
                    else if (doc.SOFTWARE_TYPE.Equals(this.radBtnSoftAcrobatPRO.Value)) this.radBtnSoftAcrobatPRO.Checked = true;
                    else if (doc.SOFTWARE_TYPE.Equals(this.radBtnSoftETC.Value)) this.radBtnSoftETC.Checked = true;
                    //Purpose
                    this.radTxtSoftPurpose.Text = doc.SOFTWARE_PURPOSE;
                    //사용기한
                    if (doc.IS_PERMANENT.Equals("Y"))
                    {
                        this.radRdoPermanent.Checked = true;
                        this.radRdoPeriod.Checked = false;
                        this.radDatSoftFrom.SelectedDate = null;
                        this.radDatSoftTo.SelectedDate = null;
                    }
                    
                    else if (doc.IS_PERMANENT.Equals("N"))
                    {
                        this.radRdoPeriod.Checked = true;
                        this.radRdoPermanent.Checked = false;
                        this.radDatSoftFrom.SelectedDate = doc.SOFTWARE_FROM;
                        this.radDatSoftTo.SelectedDate = doc.SOFTWARE_TO;
                    }
                }
                else if (doc.CATEGORY == "BYOS") //BYOS인 경우
                {
                    radBtnBYOS.Visible = true;
                    radBtnBYOS.Checked = true;
                }
                else if (doc.CATEGORY == "ITService") //IT Service인 경우
                {
                    this.radBtnITService.Checked = true;
                    //타입
                    if (doc.SERVICE_TYPE.Equals(this.radBtnSvcTypeEmail.Value)) this.radBtnSvcTypeEmail.Checked = true;
                    else if (doc.SERVICE_TYPE.Equals(this.radBtnSvcTypeMailBox.Value)) this.radBtnSvcTypeMailBox.Checked = true;
                    else if (doc.SERVICE_TYPE.Equals(this.radBtnSvcTypeRetal.Value)) this.radBtnSvcTypeRetal.Checked = true;
                    //else if (doc.SERVICE_TYPE.Equals(this.radBtnSvcTypeMDM.Value)) this.radBtnSvcTypeMDM.Checked = true;
                    else if (doc.SERVICE_TYPE.Equals("MDM")) { this.radBtnSvcTypeMDM.Visible = true; this.radBtnSvcTypeMDM.Checked = true; }
                    else if (doc.SERVICE_TYPE.Equals("IPT")) { this.radBtnSvcTypeIPT.Visible = true; this.radBtnSvcTypeIPT.Checked = true; }

                    if (this.radBtnSvcTypeEmail.Checked)
                    {
                        this.radTxtSvcAccountName.Text = doc.SERVICE_ACCOUNT_NAME;
                        this.radDatSvcFrom.SelectedDate = doc.SERVICE_FROM;
                        this.radDatSvcTo.SelectedDate = doc.SERVICE_TO;
                    }
                    else if (this.radBtnSvcTypeMDM.Checked)
                    {
                        if (doc.SERVICE_MDM_MODEL.Equals(this.radBtnSvcMDMIPhone.Value)) this.radBtnSvcMDMIPhone.Checked = true;
                        else if (doc.SERVICE_MDM_MODEL.Equals(this.radBtnSvcMDMIPad.Value)) this.radBtnSvcMDMIPad.Checked = true;

                        this.radTxtSvcMDMMac.Text = doc.SERVICE_MDM_MAC;
                        this.radTxtSvcMDMSerial.Text = doc.SERVICE_MDM_SERIAL_NUMBER;
                        this.radTxtSvcMDMPhone.Text = doc.SERVICE_MDM_PHONE_NUMBER;
                    }
                    else if (this.radBtnSvcTypeIPT.Checked)
                    {
                        this.radBtnSvcExistIPT.Checked = doc.SERVICE_IPT_PHONE.Equals("Y") ? true : false;
                        this.radTxtSvcIPTMac.Text = doc.SERVICE_IPT_MAC;
                        if (doc.SERVICE_IPT_MODEL.Equals(this.radBtnSvcIPTNormal.Value)) this.radBtnSvcIPTNormal.Checked = true;
                        else if (doc.SERVICE_IPT_MODEL.Equals(this.radBtnSvcIPTManager.Value)) this.radBtnSvcIPTManager.Checked = true;
                    }
                    else if (this.radBtnSvcTypeRetal.Checked)
                    {
                        this.radDatSvcFrom.SelectedDate = doc.SERVICE_FROM;
                        this.radDatSvcTo.SelectedDate = doc.SERVICE_TO;
                    }
                    this.radTxtSvcPurpose.Text = doc.SERVICE_PURPOSE;


                }
            }
        }
    }

    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 
        CallClientScript();
    }

    private void CallClientScript()
    {
        if (this.radBtnHardware.Checked)
        {
            string type = GetHardwareTypeData(DataKind.Value);
            string res = GetHardwareResource(DataKind.Value);
            if (!ClientScript.IsStartupScriptRegistered("setHardware"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "setHardware", "setHardware('" + type + "','" + res + "');", true);
        }
        else if (this.radBtnSoftware.Checked)
        {
            string type = GetSoftwareType(DataKind.Value);
            if (!ClientScript.IsStartupScriptRegistered("setSoftware"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "setSoftware", "setSoftware('" + type + "');", true);
        }
        else if (this.radBtnITService.Checked)
        {
            string type = GetITServiceType(DataKind.Value);
            if (!ClientScript.IsStartupScriptRegistered("setITService"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "setITService", "setITService('" + type + "');", true);
        }

    }

    #region [ 문서상단 버튼 ]

    enum DataKind
    {
        Text,
        Value,
    }

    private string GetCategoryData(DataKind kind)
    {
        string rtnData = string.Empty; ;
        switch (kind)
        {
            case DataKind.Text:
                if (this.radBtnHardware.Checked) rtnData = this.radBtnHardware.Text;
                else if (this.radBtnSoftware.Checked) rtnData = this.radBtnSoftware.Text;
                else if (this.radBtnITService.Checked) rtnData = this.radBtnITService.Text;
                else if (this.radBtnBYOS.Checked) rtnData = this.radBtnBYOS.Text;
                break;
            case DataKind.Value:
                if (this.radBtnHardware.Checked) rtnData = this.radBtnHardware.Value;
                else if (this.radBtnSoftware.Checked) rtnData = this.radBtnSoftware.Value;
                else if (this.radBtnITService.Checked) rtnData = this.radBtnITService.Value;
                else if (this.radBtnBYOS.Checked) rtnData = this.radBtnBYOS.Value; 
                break;
        }

        return rtnData;
    }

    private string GetHardwareTypeData(DataKind kind)
    {
        string rtnData = string.Empty;
        switch (kind)
        {
            case DataKind.Text:
                if (this.radBtnLoss.Checked) rtnData = this.radBtnLoss.Text;
                else if (this.radBtnTrouble.Checked) rtnData = this.radBtnTrouble.Text;
                else if (this.radBtnNew.Checked) rtnData = this.radBtnNew.Text;
                break;
            case DataKind.Value:
                if (this.radBtnLoss.Checked) rtnData = this.radBtnLoss.Value;
                else if (this.radBtnTrouble.Checked) rtnData = this.radBtnTrouble.Value;
                else if (this.radBtnNew.Checked) rtnData = this.radBtnNew.Value;
                break;
        }
        return rtnData;
    }

    private string GetHardwareResource(DataKind kind)
    {
        string rtnData = string.Empty;
        switch (kind)
        {
            case DataKind.Text:
                if (this.radBtnHardwareComputer.Checked) rtnData = this.radBtnHardwareComputer.Text;
                else if (this.radBtnHardwareMobile.Checked) rtnData = this.radBtnHardwareMobile.Text;
                else if (this.radBtnHardwareETC.Checked) rtnData = this.radBtnHardwareETC.Text;
                else if (this.radBtnHardwareIpad.Checked) rtnData = this.radBtnHardwareIpad.Text;
                break;
            case DataKind.Value:
                if (this.radBtnHardwareComputer.Checked) rtnData = this.radBtnHardwareComputer.Value;
                else if (this.radBtnHardwareMobile.Checked) rtnData = this.radBtnHardwareMobile.Value;
                else if (this.radBtnHardwareETC.Checked) rtnData = this.radBtnHardwareETC.Value;
                else if (this.radBtnHardwareIpad.Checked) rtnData = this.radBtnHardwareIpad.Value;
                break;
        }
        return rtnData;
    }

    /// <summary>
    /// Softwrae Type반환
    /// </summary>
    /// <param name="kind"></param>
    /// <returns></returns>
    private string GetSoftwareType(DataKind kind)
    {
        string rtnData = string.Empty;
        switch (kind)
        {
            case DataKind.Text:
                if (this.radBtnSoftHwp.Checked) rtnData = this.radBtnSoftHwp.Text;
                else if (this.radBtnSoftAcrobatSTD.Checked) rtnData = this.radBtnSoftAcrobatSTD.Text;
                else if (this.radBtnSoftAcrobatPRO.Checked) rtnData = this.radBtnSoftAcrobatPRO.Text;
                else if (this.radBtnSoftETC.Checked) rtnData = this.radBtnSoftETC.Text;
                break;
            case DataKind.Value:
                if (this.radBtnSoftHwp.Checked) rtnData = this.radBtnSoftHwp.Value;
                else if (this.radBtnSoftAcrobatSTD.Checked) rtnData = this.radBtnSoftAcrobatSTD.Value;
                else if (this.radBtnSoftAcrobatPRO.Checked) rtnData = this.radBtnSoftAcrobatPRO.Value;
                else if (this.radBtnSoftETC.Checked) rtnData = this.radBtnSoftETC.Value;
                break;
        }
        return rtnData;
    }

    /// <summary>
    /// IT Service Type 반환
    /// </summary>
    /// <param name="kind"></param>
    /// <returns></returns>
    private string GetITServiceType(DataKind kind)
    {
        string rtnData = string.Empty;
        switch (kind)
        {
            case DataKind.Text:
                if (this.radBtnSvcTypeEmail.Checked) rtnData = this.radBtnSvcTypeEmail.Text;
                else if (this.radBtnSvcTypeMailBox.Checked) rtnData = this.radBtnSvcTypeMailBox.Text;
                else if (this.radBtnSvcTypeRetal.Checked) rtnData = this.radBtnSvcTypeRetal.Text;
                else if (this.radBtnSvcTypeMDM.Checked) rtnData = radBtnSvcTypeMDM.Text;
                else if (this.radBtnSvcTypeIPT.Checked) rtnData = radBtnSvcTypeIPT.Text;
                break;
            case DataKind.Value:
                if (this.radBtnSvcTypeEmail.Checked) rtnData = this.radBtnSvcTypeEmail.Value;
                else if (this.radBtnSvcTypeMailBox.Checked) rtnData = this.radBtnSvcTypeMailBox.Value;
                else if (this.radBtnSvcTypeRetal.Checked) rtnData = this.radBtnSvcTypeRetal.Value;
                else if (this.radBtnSvcTypeMDM.Checked) rtnData = radBtnSvcTypeMDM.Value;
                else if (this.radBtnSvcTypeIPT.Checked) rtnData = radBtnSvcTypeIPT.Value;
                break;
        }
        return rtnData;
    }

    private string GetNewModel(DataKind kind)
    {
        string rtnData = string.Empty;
        switch (kind)
        {
            case DataKind.Text:
                if (this.radBtnNewModelMouse.Checked) rtnData = this.radBtnNewModelMouse.Text;
                else if (this.radBtnNewModelAdaptor.Checked) rtnData = this.radBtnNewModelAdaptor.Text;
                else if (this.radBtnNewModelKeyboard.Checked) rtnData = this.radBtnNewModelKeyboard.Text;
                else if (this.radBtnNewModelMonitor.Checked) rtnData = this.radBtnNewModelMonitor.Text;
                else if (this.radBtnNewModelDocking.Checked) rtnData = this.radBtnNewModelDocking.Text;
                else if (this.radBtnNewModelDVD.Checked) rtnData = this.radBtnNewModelDVD.Text;
                else if (this.radBtnNewModelBattery.Checked) rtnData = this.radBtnNewModelBattery.Text;
                break;
            case DataKind.Value:
                if (this.radBtnNewModelMouse.Checked) rtnData = this.radBtnNewModelMouse.Value;
                else if (this.radBtnNewModelAdaptor.Checked) rtnData = this.radBtnNewModelAdaptor.Value;
                else if (this.radBtnNewModelKeyboard.Checked) rtnData = this.radBtnNewModelKeyboard.Value;
                else if (this.radBtnNewModelMonitor.Checked) rtnData = this.radBtnNewModelMonitor.Value;
                else if (this.radBtnNewModelDocking.Checked) rtnData = this.radBtnNewModelDocking.Value;
                else if (this.radBtnNewModelDVD.Checked) rtnData = this.radBtnNewModelDVD.Value;
                else if (this.radBtnNewModelBattery.Checked) rtnData = this.radBtnNewModelBattery.Value;
                break;

        }
        return rtnData;
    }

    private string SaveDocument(ApprovalUtil.ApprovalStatus status)
    {
        DTO_DOC_IT_RESOURCE doc = new DTO_DOC_IT_RESOURCE();
        doc.PROCESS_ID = this.hddProcessID.Value;
        doc.SUBJECT = GetCategoryData(DataKind.Text);
        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.CATEGORY = GetCategoryData(DataKind.Value);
        if (this.radBtnHardware.Checked)
        {
            doc.TYPE = GetHardwareTypeData(DataKind.Value);
            doc.RESOURCE = GetHardwareResource(DataKind.Value);
            doc.SUBJECT = doc.SUBJECT + " / " + doc.TYPE + " / " + doc.RESOURCE;
            if (this.radBtnLoss.Checked)
            {
                if (this.radBtnHardwareComputer.Checked) //컴퓨터 분실
                {
                    //정보보안
                   // doc.SECURITY_REPORT = this.radBtnComReport.Checked ? "Y" : "N";
                    doc.SECURITY_REPORT = this.radTxtComLossNum.Text;
                    //분실사유
                   // if (this.radBtnComCareless.Checked) doc.LOSS_REASON = this.radBtnComCareless.Value;
                   // else if (this.radBtnComStolen.Checked) doc.LOSS_REASON = this.radBtnComStolen.Value;
                    //모델
                    if (this.radBtnComSmallNotebook.Checked) doc.COMPUTER_MODEL = this.radBtnComSmallNotebook.Value;
                    else if (this.radBtnComBigNotebook.Checked) doc.COMPUTER_MODEL = this.radBtnComBigNotebook.Value;
                    else if (this.radBtnComDesktop.Checked) doc.COMPUTER_MODEL = this.radBtnComDesktop.Value;
                }
                else if (this.radBtnHardwareMobile.Checked) //모바일 분실
                {
                    //정보보안
                    //doc.SECURITY_REPORT = this.radBtnMobileReport.Checked ? "Y" : "N";
                    doc.SECURITY_REPORT = this.radTxtMobileLossNum.Text;
                    //분실사유
                   // if (this.radBtnMobCareless.Checked) doc.LOSS_REASON = this.radBtnMobCareless.Value;
                   // else if (this.radBtnMobStolen.Checked) doc.LOSS_REASON = this.radBtnMobStolen.Value;
                    //모델 및 색상
                    doc.MOBILE_MODEL = this.radTxtMobileModel.Text;
                }
                else if (this.radBtnHardwareIpad.Checked)
                {
                    //정보보안
                    //doc.SECURITY_REPORT = this.radBtnIpadReport.Checked ? "Y" : "N";
                    doc.SECURITY_REPORT = this.radTxtIpadLossNum.Text;
                    //분실사유
                    //if (this.radBtnIpadCareless.Checked) doc.LOSS_REASON = this.radBtnIpadCareless.Value;
                   // else if (this.radBtnIpadStolen.Checked) doc.LOSS_REASON = this.radBtnIpadStolen.Value;
                    //모델
                    doc.MOBILE_MODEL = this.radTxtIpadModel.Text;


                }
            }
            else if (this.radBtnTrouble.Checked)
            {
                if (this.radBtnHardwareComputer.Checked) //컴퓨터 파손
                {
                    doc.STATE = this.radTxtComState.Text;
                }
                else if (this.radBtnHardwareMobile.Checked)
                {
                    doc.STATE = this.radTxtMobileState.Text;
                }
                else if (this.radBtnHardwareIpad.Checked)
                {
                    doc.STATE = this.radTxtIpadState.Text;
                }
            }
            else if (this.radBtnNew.Checked)
            {
                if (this.radBtnHardwareComputer.Checked) //컴퓨터 신규
                {
                    //Type
                    if (this.radBtnComTypeStandard.Checked) doc.COMPUTER_TYPE = this.radBtnComTypeStandard.Value;
                    else if (this.radBtnComTypeFunctional.Checked) doc.COMPUTER_TYPE = this.radBtnComTypeFunctional.Value;
                    //모델
                    if (this.radBtnNewComSmallNotebook.Checked) doc.COMPUTER_MODEL = this.radBtnNewComSmallNotebook.Value;
                    else if (this.radBtnNewComBigNotebook.Checked) doc.COMPUTER_MODEL = this.radBtnNewComBigNotebook.Value;
                    else if (this.radBtnNewComDesktop.Checked) doc.COMPUTER_MODEL = this.radBtnNewComDesktop.Value;
                    //Purpose
                    doc.NEW_PURPOSE = this.radTxtNewComPurpose.Text;
                }
                else if (this.radBtnHardwareETC.Checked) //기타 신규
                {
                    doc.NEW_MODEL = GetNewModel(DataKind.Value);

                    doc.NEW_PURPOSE = this.radTxtNewETCPurpose.Text;
                }
            }
        }
        else if (this.radBtnSoftware.Checked) //Software
        {
            doc.SOFTWARE_TYPE = GetSoftwareType(DataKind.Value);
            doc.SUBJECT = doc.SUBJECT + " / " + doc.SOFTWARE_TYPE;

            doc.SOFTWARE_PURPOSE = this.radTxtSoftPurpose.Text;
            if (this.radRdoPermanent.Checked)
            {
                doc.IS_PERMANENT = "Y";
                doc.SOFTWARE_FROM = null;
                doc.SOFTWARE_TO = null;
            }
            else
            {
                doc.IS_PERMANENT = "N";
                doc.SOFTWARE_FROM = this.radDatSoftFrom.SelectedDate;
                doc.SOFTWARE_TO = this.radDatSoftTo.SelectedDate;
            }
        }
        else if (this.radBtnITService.Checked) //IT Service
        {
            doc.SERVICE_TYPE = GetITServiceType(DataKind.Value);
            doc.SUBJECT = doc.SUBJECT + " / " + doc.SERVICE_TYPE;

            doc.SERVICE_PURPOSE = this.radTxtSvcPurpose.Text;
            if (this.radBtnSvcTypeEmail.Checked)
            {
                doc.SERVICE_ACCOUNT_NAME = this.radTxtSvcAccountName.Text; //Full Name
                doc.SERVICE_FROM = this.radDatSvcFrom.SelectedDate;
                doc.SERVICE_TO = this.radDatSvcTo.SelectedDate;
            }
            else if (this.radBtnSvcTypeMDM.Checked)
            {
                if (this.radBtnSvcMDMIPhone.Checked) doc.SERVICE_MDM_MODEL = this.radBtnSvcMDMIPhone.Value;
                else if (this.radBtnSvcMDMIPad.Checked) doc.SERVICE_MDM_MODEL = this.radBtnSvcMDMIPad.Value;

                doc.SERVICE_MDM_MAC = this.radTxtSvcMDMMac.Text;
                doc.SERVICE_MDM_SERIAL_NUMBER = this.radTxtSvcMDMSerial.Text;
                doc.SERVICE_MDM_PHONE_NUMBER = this.radTxtSvcMDMPhone.Text;
            }
            else if (this.radBtnSvcTypeIPT.Checked)
            {
                doc.SERVICE_IPT_PHONE = this.radBtnSvcExistIPT.Checked ? "Y" : "N";
                if (this.radBtnSvcExistIPT.Checked) doc.SERVICE_IPT_MAC = this.radTxtSvcIPTMac.Text;
                else doc.SERVICE_IPT_MAC = string.Empty;
                if (this.radBtnSvcIPTNormal.Checked) doc.SERVICE_IPT_MODEL = this.radBtnSvcIPTNormal.Value;
                else if (this.radBtnSvcIPTManager.Checked) doc.SERVICE_IPT_MODEL = this.radBtnSvcIPTManager.Value;
            }

            else if (this.radBtnSvcTypeRetal.Checked)
            {
                doc.SERVICE_FROM = this.radDatSvcFrom.SelectedDate;
                doc.SERVICE_TO = this.radDatSvcTo.SelectedDate;
            }

        }
        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;
        webMaster.Subject = doc.SUBJECT;

        using (ITResourceMgr mgr = new ITResourceMgr())
        {
            return mgr.MergeITResource(doc);
        }
    }

    /// <summary>
    /// 유효성 체크
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;

        if (GetCategoryData(DataKind.Text).IsNullOrEmptyEx())
        {
            message = "Category";
        }

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {

            if (this.radBtnHardware.Checked) //Hardware
            {
                if (GetHardwareTypeData(DataKind.Text).IsNullOrEmptyEx())
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Type";

                if (GetHardwareResource(DataKind.Text).IsNullOrEmptyEx())
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Resource";
                if (this.radBtnLoss.Checked) //분실
                {
                    if (this.radBtnHardwareComputer.Checked) //컴퓨터 분실
                    {
                        //if (!this.radBtnComReport.Checked)
                        if (this.radTxtComLossNum.Text.IsNullOrEmptyEx())
                           message += (message.IsNullOrEmptyEx() ? "" : ",") + "정보보안사고경위서";
                      //  if (!(this.radBtnComCareless.Checked || this.radBtnComStolen.Checked))
                      //      message += (message.IsNullOrEmptyEx() ? "" : ",") + "분실사유";
                        if (!(this.radBtnComSmallNotebook.Checked || this.radBtnComBigNotebook.Checked || this.radBtnComDesktop.Checked))
                            message += (message.IsNullOrEmptyEx() ? "" : ",") + "신규모델";
                    }
                    else if (this.radBtnHardwareMobile.Checked) //모바일 분실
                    {
                        //if (!this.radBtnMobileReport.Checked)
                        if (this.radTxtMobileLossNum.Text.IsNullOrEmptyEx())
                            message += (message.IsNullOrEmptyEx() ? "" : ",") + "정보보안사고경위서";
                      //  if (!(this.radBtnMobCareless.Checked || this.radBtnMobStolen.Checked))
                       //     message += (message.IsNullOrEmptyEx() ? "" : ",") + "분실사유";
                        if (this.radTxtMobileModel.Text.Trim().IsNullOrEmptyEx())
                            message += (message.IsNullOrEmptyEx() ? "" : ",") + "신규모델";
                        //if (this.radTxtMobileColor.Text.Trim().IsNullOrEmptyEx())
                        //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "색상";
                    }
                    else if (this.radBtnHardwareIpad.Checked)
                    {
                        //if (!this.radBtnIpadReport.Checked)
                        if (this.radTxtIpadLossNum.Text.IsNullOrEmptyEx())
                            message += (message.IsNullOrEmptyEx() ? "" : ",") + "정보보안사고경위서";
                        if (this.radTxtIpadModel.Text.Trim().IsNullOrEmptyEx())
                            message += (message.IsNullOrEmptyEx() ? "" : ",") + "신규모델";
                       // if (!(this.radBtnIpadCareless.Checked || this.radBtnIpadStolen.Checked))
                       //     message += (message.IsNullOrEmptyEx() ? "" : ",") + "분실사유";
                    }
                }
                else if (this.radBtnTrouble.Checked) //판소
                {
                    if (this.radBtnHardwareComputer.Checked) //컴퓨터 파손
                    {
                        if (this.radTxtComState.Text.Trim().IsNullOrEmptyEx())
                            message += (message.IsNullOrEmptyEx() ? "" : ",") + "증상";
                    }
                    else if (this.radBtnHardwareMobile.Checked)
                    {
                        if (this.radTxtMobileState.Text.Trim().IsNullOrEmptyEx())
                            message += (message.IsNullOrEmptyEx() ? "" : ",") + "증상";
                    }
                    else if (this.radBtnHardwareIpad.Checked)
                    {
                        if (this.radTxtIpadState.Text.Trim().IsNullOrEmptyEx())
                            message += (message.IsNullOrEmptyEx() ? "" : ",") + "증상";
                    }
                }
                else if (this.radBtnNew.Checked) //신규
                {
                    if (this.radBtnHardwareETC.Checked) //컴퓨터 신규
                    {
                        if (GetNewModel(DataKind.Text).IsNullOrEmptyEx())
                            message += (message.IsNullOrEmptyEx() ? "" : ",") + "모델";
                        if (this.radTxtNewETCPurpose.Text.Trim().IsNullOrEmptyEx())
                            message += (message.IsNullOrEmptyEx() ? "" : ",") + "사유";
                    }
                    else if (this.radBtnHardwareComputer.Checked) //기타 신규
                    {
                        if (!(this.radBtnComTypeStandard.Checked || this.radBtnComTypeFunctional.Checked))
                            message += (message.IsNullOrEmptyEx() ? "" : ",") + "Type";
                        if (!(this.radBtnNewComSmallNotebook.Checked || this.radBtnNewComBigNotebook.Checked || this.radBtnNewComDesktop.Checked))
                            message += (message.IsNullOrEmptyEx() ? "" : ",") + "모델";
                        if (this.radTxtNewComPurpose.Text.Trim().IsNullOrEmptyEx())
                            message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose";
                    }
                }
            }
            else if (this.radBtnSoftware.Checked) //Software
            {
                if (GetSoftwareType(DataKind.Text).IsNullOrEmptyEx())
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Software";
                if (this.radTxtSoftPurpose.Text.Trim().IsNullOrEmptyEx())
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose";
            }
            else if (this.radBtnBYOS.Checked) //BYOS
            {
                if (!this.radBtnSvcBYOS_AGREE.Checked)
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "BYOS 이용 약관에 동의";
            }
            else if (this.radBtnITService.Checked) //IT Service
            {
                if (GetITServiceType(DataKind.Text).IsNullOrEmptyEx())
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Type";
                if (this.radBtnSvcTypeEmail.Checked)
                {
                    if (this.radTxtSvcAccountName.Text.Trim().IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Full Name";
                    if (this.radTxtSvcPurpose.Text.Trim().IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose";
                    if (!this.radDatSvcFrom.SelectedDate.HasValue || !this.radDatSvcTo.SelectedDate.HasValue)
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "사용기한";
                }
                else if (this.radBtnSvcTypeMailBox.Checked)
                {
                    if (this.radTxtSvcPurpose.Text.Trim().IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose";
                }
                else if (this.radBtnSvcTypeMDM.Checked)
                {
                    if (!(this.radBtnSvcMDMIPhone.Checked || this.radBtnSvcMDMIPad.Checked))
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Model";
                    if (this.radTxtSvcMDMMac.Text.Trim().IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "MAC Address";
                    if (this.radTxtSvcMDMSerial.Text.Trim().IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Serial Number";
                    if (this.radTxtSvcMDMPhone.Text.Trim().IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Phone Number";
                    if (this.radTxtSvcPurpose.Text.Trim().IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose";

                }
                else if (this.radBtnSvcTypeRetal.Checked)
                {
                    if (this.radTxtSvcPurpose.Text.Trim().IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose";
                    if (!this.radDatSvcFrom.SelectedDate.HasValue || !this.radDatSvcTo.SelectedDate.HasValue)
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "사용기한";
                }
                else if (this.radBtnSvcTypeIPT.Checked)
                {
                    if (this.radBtnSvcExistIPT.Checked && this.radTxtSvcIPTMac.Text.Trim().IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "MAC Address";

                    if (!(this.radBtnSvcIPTNormal.Checked || this.radBtnSvcIPTManager.Checked))
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "모델";
                    if (this.radTxtSvcPurpose.Text.Trim().IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose";
                }

            }
        }

        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + message;
            return false;
        }
        else return true;
    }

    protected override void DoSave()
    {
        try
        {
            if (ValidationCheck(ApprovalUtil.ApprovalStatus.Saved))
            {
               // string ApprovalStatus = hddProcessID.Value.Length > 0 ? ApprovalUtil.ApprovalStatus.Saved.ToString() : ApprovalUtil.ApprovalStatus.Temp.ToString();
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