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

public partial class Approval_Document_IncidentReport : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 
    }

    private void InitPageInfo()
    {

        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0040";

        InitControls();

    }

    private void InitControls()
    {


        DTO_DOC_INCIDENT_REPORT doc;
        if (!hddProcessID.Value.Equals(String.Empty))
        {
            using (Bayer.eWF.BSL.Approval.Mgr.IncidentReportMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.IncidentReportMgr())
            {
                doc = mgr.SelectIncidentReport(hddProcessID.Value);
            }

            if (doc != null)
            {

                
                this.RadDateOccur.SelectedDate  = Convert.ToDateTime(doc.OCCUR_DATE);
                this.RadDateDetect.SelectedDate = Convert.ToDateTime(doc.DETECT_DATE);
                this.RadTextLocation.Text   = doc.LOCATION;
                this.RadTexDescription.Text = doc.DESCRIPTION;
                string encryptYN = doc.ENCRYPT_YN;
                this.RadTextExplain.Text = doc.EXPLAIN;
                this.RadTextOthers.Text  = doc.OTHERS;
                this.RadNumCost.Value = Convert.ToDouble(doc.COST);


                foreach (string DeviceType in doc.DEVICE_TYPE.Split(new char[] { '|' }))
                {
                    foreach (Control control in this.divDeviceType.Controls)
                    {
                        if (control is RadButton)
                        {
                            if ((control as RadButton).Value.Equals(DeviceType))
                            {
                                (control as RadButton).Checked = true; break;
                            }
                        }
                    }
                }

                //Encrypt_YN
                foreach (Control control in this.divEncrypt.Controls)
                {
                    if (control is RadButton)
                    {
                        if ((control as RadButton).Value.Equals(doc.ENCRYPT_YN))
                        {
                            (control as RadButton).Checked = true; break;
                        }
                    }
                }

                if (encryptYN == "Y")
                {
                    this.divExplain.Attributes.Add("style", "display: none; visibility: visible");
                }
                else
                {
                    this.divExplain.Attributes.Add("style", "display: inline; visibility: visible");
                }


                webMaster.DocumentNo = doc.DOC_NUM;
            }
        }
        else
        {
            RadDateOccur.SelectedDate = DateTime.Now.Date;
            RadDateDetect.SelectedDate = DateTime.Now.Date;
        }


    }

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

    private string GetSelectedEncryptedYN()
    {
        string Encrypt = string.Empty;
        foreach (Control control in divEncrypt.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    Encrypt = (control as RadButton).Value;
                    break;
                }
            }
        }
        return Encrypt;
    }

    private string GetSelectedMediaType()
    {
        string Duration = string.Empty;
        foreach (Control control in divDeviceType.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    Duration = (control as RadButton).Value;
                    break;
                }
            }
        }
        return Duration;
    }

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {

            //Occured Date
            if (!this.RadDateOccur.SelectedDate.HasValue)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Occured Date";

            //Detected Date
            if (!this.RadDateDetect.SelectedDate.HasValue)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Detected Date";

            //Location
            if (this.RadTextLocation.Text.IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Location";
            
            //Description
            if (this.RadTexDescription.Text.IsNullOrEmptyEx())
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "General description";
            }

            //Media Type
            if (GetSelectedMediaType().IsNullOrEmptyEx())
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Media/Device type";
            }

            // if (txtSubject.Text.Length <= 0) 
            //    message += "Subject";

        }
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;

    }


    private string DocumentSave(string processStstus)
    {
        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_INCIDENT_REPORT doc = null;

        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_INCIDENT_REPORT();
        doc.PROCESS_STATUS = processStstus;
        doc.PROCESS_ID = hddProcessID.Value;
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = Sessions.UserID;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.CREATE_DATE = DateTime.Now;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.CREATOR_ID = Sessions.UserID;
        doc.IS_DISUSED = "N";

        string occurDate  = this.RadDateOccur.SelectedDate.Value.ToString("yyyy-MM-dd");
        string detectDate = this.RadDateDetect.SelectedDate.Value.ToString("yyyy-MM-dd");   //Convert.ToString(this.RadDateDetect.SelectedDate);
        doc.OCCUR_DATE   = occurDate;
        doc.DETECT_DATE  = detectDate;

        string DeviceType = string.Empty;
        foreach (Control control in this.divDeviceType.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                    DeviceType += (control as RadButton).Value + "|";
            }
        }
        //this.informationMessage = DeviceType;
        doc.DEVICE_TYPE = DeviceType;

        doc.LOCATION    = this.RadTextLocation.Text;
        doc.DESCRIPTION = this.RadTexDescription.Text;

        string encryptYN = GetSelectedEncryptedYN();

        doc.ENCRYPT_YN  = encryptYN;
        doc.EXPLAIN     = this.RadTextExplain.Text;
        doc.OTHERS      = this.RadTextOthers.Text;
        doc.COST        = Convert.ToDecimal(this.RadNumCost.Value);

        doc.SUBJECT = "Occurred:" + occurDate + "/" + "Detected:" + detectDate + "/" + DeviceType;
        webMaster.Subject = doc.SUBJECT;

        using (Bayer.eWF.BSL.Approval.Mgr.IncidentReportMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.IncidentReportMgr())
        {
            processID = mgr.MergeIncidentReport(doc);
        }

        return processID;

    }


}