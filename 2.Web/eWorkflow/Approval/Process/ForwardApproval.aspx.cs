using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using System.Text;
using Bayer.eWF.BSL.Common.Dto;

public partial class Approval_Process_ForwardApproval : DNSoft.eWF.FrameWork.Web.PageBase
{
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PageLoadInfo();
            if (!IsPostBack)
            {
                InitControls();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    } 
    #endregion

    #region InitControls
    private void InitControls()
    {
        GridSource();
        grdUserList.DataBind();
    } 
    #endregion

    #region GridSource
    private void GridSource()
    {
        List<SmallUserInfoDto> users;
        using (Bayer.eWF.BSL.Common.Mgr.UserMgr mgr = new Bayer.eWF.BSL.Common.Mgr.UserMgr())
        {
            users = mgr.SelectApprovalUserList(txtSearchName.Text);
        }
        grdUserList.DataSource = users;

    } 
    #endregion

    #region PageLoadInfo
    private void PageLoadInfo()
    {
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
    } 
    #endregion

    #region Forward Approval Event
    protected void btnForward_Click(object sender, EventArgs e)
    {
        string script = string.Empty;
        try
        {
            DoFowardApproval();
            script = AfterTreatmentScript();
            ClientWindowClose("true", script);
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
        finally
        {
            if (script != null)
                script = null;
        }
    } 
    #endregion

    #region AfterTreatmentScript 후처리 호출

    /// <summary>
    /// 후처리 호출
    /// </summary>
    /// <returns></returns>
    private string AfterTreatmentScript()
    {
        StringBuilder script = new StringBuilder(128);
        string mailSendType = string.Empty;
        try
        {
            #region SendMail Process

            mailSendType = ApprovalUtil.SendMailType.CurrentApprover.ToString();

            if (mailSendType.Length > 0 && hddProcessID.Value.Length > 0)
            {
                script.AppendFormat("fn_sendMail('{0}','{1}', '{2}');", hddProcessID.Value, mailSendType, Sessions.MailAddress);
            }
            #endregion

            return script.ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (script != null)
            {
                script.Clear();
                script = null;
            }
        }
    }
    #endregion

    #region Grid Event
    protected void grdUserList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            GridSource();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    } 
    #endregion

    #region btnSearch Event
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            GridSource();
            grdUserList.DataBind();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    } 
    #endregion

    #region DoFowardApproval
    private void DoFowardApproval()
    {
        try
        {
            using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
            {
                mgr.InsertForwardApproval(hddDocumentID.Value, hddProcessID.Value, Sessions.UserID, hddUserID.Value, ApprovalUtil.ApprovalStatus.Processing.ToString(), ApprovalUtil.ProcessStatus.ACEPTER, string.Format("Forward : {0}",txtComment.Text));
            }
            Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, "DoFowardApproval"), string.Format("PROCESS_ID : {0}", hddProcessID.Value), Sessions.UserID);
        }
        catch( Exception ex)
        {
            Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Error", string.Format("{0}.{1}", this.GetType().Name, "DoFowardApproval"), string.Format("PROCESS_ID : {0}, DOCUMENT_ID", hddProcessID.Value, hddDocumentID.Value), Sessions.UserID);
            throw ex;
        }
    } 
    #endregion
}