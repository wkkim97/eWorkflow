using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNSoft.eWF.FrameWork.Web;
using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Dao;
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Mgr;
using Bayer.eWF.BSL.Common.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using Telerik.Web.UI;

public partial class Approval_Document_BHCReturnGoods : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 
    } 
    #endregion

    #region InitPageInfo
    private void InitPageInfo()
    {
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0039";
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();

        InitControls();
    } 
    #endregion

    #region InitControl (Select)
    private void InitControls()
    {
         DTO_DOC_BHC_RETURN_GOODS doc;
         if (!hddProcessID.Value.Equals(String.Empty))
         {
             using (Bayer.eWF.BSL.Approval.Mgr.BhcReturnGoodsMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.BhcReturnGoodsMgr())
             {
                 doc = mgr.SelectBhcReturnGoods(hddProcessID.Value);
             }

             if (doc != null)
             {

                 //BU
                 if (doc.BU == "CH")
                 {
                     //radBtnCH.Checked = true;
                     radRdoBuCH.Visible = true;

                     //radBtnCC.Checked = false;
                     radRdoBuCC.Visible = false;
                 }
                 else if (doc.BU == "CC")
                 {
                     //radBtnCH.Checked = false;
                     radRdoBuCH.Visible = false;

                     //radBtnCC.Checked = true;
                     radRdoBuCC.Visible = true;
                 }

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

                 this.radNumAmount.Text = Convert.ToString(doc.COST_AMOUNT);
                 this.radtxtComment.Text = doc.REMARK;
                 this.RadtxtMonth.Text = doc.RETURN_GOODS_CODE; //Month

                 webMaster.DocumentNo = doc.DOC_NUM;
             }
         }
    } 
    #endregion

    protected override void OnPreRender(EventArgs e)
    {      
        webMaster.SetEnableControls(this.divReport,true);
        base.OnPreRender(e);
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

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            //if (GetSelectedBu().IsNullOrEmptyEx())
            //    message = "BU";
            //if (GetSelectedBu() != "AH" && GetSelectLocation().IsNullOrEmptyEx())
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Location";
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
        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_BHC_RETURN_GOODS doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_BHC_RETURN_GOODS();
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

        string selectedBU = GetSelectedBu();

        doc.BU = selectedBU;
        doc.COST_AMOUNT = Convert.ToDecimal(this.radNumAmount.Text);
        doc.REMARK = this.radtxtComment.Text;
        doc.RETURN_GOODS_CODE = this.RadtxtMonth.Text;

        doc.SUBJECT = selectedBU + "/" + this.radNumAmount.Text + "/" + this.RadtxtMonth.Text;
        webMaster.Subject = doc.SUBJECT;

        using (Bayer.eWF.BSL.Approval.Mgr.BhcReturnGoodsMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.BhcReturnGoodsMgr())
        {
            processID = mgr.MergeBhcReturnGoods(doc);
        }


        return processID;

    }


    #region Radio Select
    private string GetSelectedBu()
    {
        string Bu = string.Empty;
        foreach (Control control in divBU.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    Bu = (control as RadButton).Value;
                    break;
                }
            }
        }
        return Bu;
    }
    #endregion

}