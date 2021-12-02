using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNSoft.eW.FrameWork;
using System.Data;
using System.Data.SqlClient;
using Bayer.eWF.BSL.Approval.Dto;
using DNSoft.eWF.FrameWork.Web;
using System.Web.UI.HtmlControls;

namespace DNSoft.eWF.FrameWork.Web.WebBase
{
    /// <summary>
    /// Aspx.cs파일에 공통적인 작업이 이루어지는 파생클래스
    /// </summary>
    [Serializable]
    public class DocBase : DNSoft.eWF.FrameWork.Web.PageBase 
    {
        #region DocMasterBase webMaster
        protected DocMasterBase webMaster = null;
        #endregion

        #region GetProcessApproveByUser
        public List<DTO_PROCESS_APPROVAL_USER_STATUS> GetProcessApproveByUser(string processid)
        {
            using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
            {
                return mgr.SelectProcessApprovalStatusByUser(webMaster.ProcessID, Sessions.UserID);
            }
        }
        #endregion

        #region GetDocbtnStatus
        public void GetDocbtnStatus(string process_id)
        {
            if (process_id.Trim().Length == 0 )
            {
                webMaster.CommandAuthList = ApprovalUtil.GetApprovalButtonAuthList((int)ApprovalUtil.ApprovalViewStatus.NEW_REQUESTER);
                webMaster.ProcessStatus = ApprovalUtil.ApprovalStatus.Temp.ToString();
                return;
            }

            List<DTO_PROCESS_APPROVAL_USER_STATUS> retValue = GetProcessApproveByUser(process_id);
            if (retValue.Count > 0)
            {
                foreach (DTO_PROCESS_APPROVAL_USER_STATUS item in retValue)
                {
                    if ((item.APPROVER_STATUS.NullObjectToEmptyEx().Equals(ApprovalUtil.ProcessStatus.REJECTER) && item.APPROVER_STATUS.NullObjectToEmptyEx().Equals(ApprovalUtil.ProcessStatus.ACEPTER))
                        || item.PROCESS_STATUS.NullObjectToEmptyEx().Equals(ApprovalUtil.ApprovalStatus.Reject.ToString()))
                    {

                        webMaster.CommandAuthList = ApprovalUtil.GetApprovalButtonAuthList((int)ApprovalUtil.ApprovalViewStatus.DEFAULT);
                    }
                    else
                    {
                        // 문서가 완료되었을 경우
                        if (item.PROCESS_STATUS.NullObjectToEmptyEx().Equals(ApprovalUtil.ApprovalStatus.Completed.ToString()) || item.PROCESS_STATUS.NullObjectToEmptyEx().Equals(ApprovalUtil.ApprovalStatus.Withdraw.ToString()))
                        {
                            if (item.APPROVAL_TYPE.NullObjectToEmptyEx().Equals(ApprovalUtil.ApprovalType.RECIPIENT))
                            {
                                webMaster.CommandAuthList = ApprovalUtil.GetApprovalButtonAuthList((int)ApprovalUtil.ApprovalViewStatus.COMPLETED_RECIPIENT);
                                // Recipient가 Withdraw를 수행하였을 경우 버튼을 비활성화 하도록 처리
                                if (item.APPROVER_STATUS.NullObjectToEmptyEx().Equals(ApprovalUtil.ProcessStatus.ACEPTER))
                                {
                                    webMaster.CommandAuthList[(int)ApprovalUtil.ApprovalButtons.WITHDRAW] = 0;
                                }
                            }
                            if (item.APPROVAL_TYPE.NullObjectToEmptyEx().Equals(ApprovalUtil.ApprovalType.REVIEWER))
                            {
                                webMaster.CommandAuthList = ApprovalUtil.GetApprovalButtonAuthList((int)ApprovalUtil.ApprovalViewStatus.COMPLETED_REVIEWER);
                            }
                            if (item.APPROVAL_TYPE.NullObjectToEmptyEx().Equals(ApprovalUtil.ApprovalType.DRAFTER))
                            {
                                webMaster.CommandAuthList = ApprovalUtil.GetApprovalButtonAuthList((int)ApprovalUtil.ApprovalViewStatus.COMPLETED_REQUESTER);
                            }
                            if (item.APPROVAL_TYPE.NullObjectToEmptyEx().Equals(ApprovalUtil.ApprovalType.APPROVER))
                            {
                                webMaster.CommandAuthList = ApprovalUtil.GetApprovalButtonAuthList((int)ApprovalUtil.ApprovalViewStatus.COMPLETED_APPROVER);
                            }
                        }
                        // 문서가 거절되었을 경우
                        else if (item.PROCESS_STATUS.NullObjectToEmptyEx().Equals(ApprovalUtil.ApprovalStatus.Reject.ToString()))
                        {
                            if (item.APPROVAL_TYPE.NullObjectToEmptyEx().Equals(ApprovalUtil.ApprovalType.DRAFTER))
                            {
                                webMaster.CommandAuthList = ApprovalUtil.GetApprovalButtonAuthList((int)ApprovalUtil.ApprovalViewStatus.NEW_REQUESTER);
                            }
                            else
                            {
                                webMaster.CommandAuthList = ApprovalUtil.GetApprovalButtonAuthList((int)ApprovalUtil.ApprovalViewStatus.DEFAULT);
                                this.informationMessage = "This document has been rejected.";
                            }
                        }
                        else
                        {
                            // 결재 요청 및 결재승인을 수행하였을 경우는 이후 조회시 Exit 버튼만 표시하도록 처리
                            if (!item.APPROVAL_TYPE.NullObjectToEmptyEx().Equals(ApprovalUtil.ApprovalType.DRAFTER) 
                                && item.APPROVER_STATUS.NullObjectToEmptyEx().Equals(ApprovalUtil.ProcessStatus.ACEPTER))
                            {
                                webMaster.CommandAuthList = ApprovalUtil.GetApprovalButtonAuthList((int)ApprovalUtil.ApprovalViewStatus.EXIT);
                            }
                            else
                            {
                                if (!item.APPROVAL_TYPE.NullObjectToEmptyEx().Equals(ApprovalUtil.ApprovalType.REVIEWER))
                                {
                                    if (item.APPROVAL_TYPE.NullObjectToEmptyEx().Equals(ApprovalUtil.ApprovalType.APPROVER))
                                    {
                                        webMaster.CommandAuthList = ApprovalUtil.GetApprovalButtonAuthList((int)ApprovalUtil.ApprovalViewStatus.ON_GOING_APPROVER);
                                    }
                                    else if (item.APPROVAL_TYPE.NullObjectToEmptyEx().Equals(ApprovalUtil.ApprovalType.DRAFTER) || item.PROCESS_STATUS.NullObjectToEmptyEx().Equals(ApprovalUtil.ApprovalStatus.Processing.ToString()))
                                    {
                                        webMaster.CommandAuthList = ApprovalUtil.GetApprovalButtonAuthList((int)ApprovalUtil.ApprovalViewStatus.ON_GOING_REQUESTER);
                                    }
                                }
                            }

                        }
                    }
                    webMaster.ProcessStatus = item.PROCESS_STATUS;
                }
            }
            else
            {
                //Approver에 없는경우 저장상태인지 아니면 ReadersGroup에 포함되어 있는지를 구분하기 위해
                //문서 Status를 한번 가져와 비교한다.
                string processStatus = ApprovalUtil.ApprovalStatus.Temp.ToString();
                string requestid = null;

                using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
                {                    
                    // 2015. 02. 12 Recall 시 userid를 비교하기 위해 requestID를 추가적으로 가져온다.
                    string documentprocess = mgr.SelectProcessDocumentStatus(process_id);

                    string[] status = documentprocess.Split(new char[] { '/' });
                    processStatus = status[0];

                    if (processStatus == "Temp")
                    {
                        requestid = Sessions.UserID;
                    }
                    else
                    {
                        requestid = status[1];
                    }

                }

                if (processStatus == string.Empty) processStatus = ApprovalUtil.ApprovalStatus.Temp.ToString();
                if (processStatus.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || processStatus.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString()))
                {
                    // 2015. 02.12  
                    // 수정내용 : Recall 된 문서를 approver 가 열수 없도록 하기 위해 requestID 를 받아와 Session userID 와 비교한다.
                    if (requestid == Sessions.UserID)
                        webMaster.CommandAuthList = ApprovalUtil.GetApprovalButtonAuthList((int)ApprovalUtil.ApprovalViewStatus.NEW_REQUESTER);
                    else
                    {
                        string redirectUrl = DNSoft.eW.FrameWork.eWBase.GetConfig("//eWorkflowInfo/DocError");
                        Response.Redirect(redirectUrl);
                    }                       
                }
                else if (processStatus.Equals(ApprovalUtil.ApprovalStatus.Request.ToString()) || processStatus.Equals(ApprovalUtil.ApprovalStatus.Processing.ToString()))
                {
                    webMaster.CommandAuthList = ApprovalUtil.GetApprovalButtonAuthList((int)ApprovalUtil.ApprovalViewStatus.EXIT);
                }
                else
                {
                    webMaster.CommandAuthList = ApprovalUtil.GetApprovalButtonAuthList((int)ApprovalUtil.ApprovalViewStatus.COMPLETED_REVIEWER);
                }
                webMaster.ProcessStatus = processStatus;
            }

        }
        #endregion

        #region InitMasterPageInfo
        /// <summary>
        /// 마스터 페이지 설정
        /// </summary>
        /// <param name="title">문서타이틀</param>
        /// <param name="docID">문서ID</param>
        /// <param name="processID">프로세스ID</param>
        /// <param name="docProcessStatus">문서상태</param>
        /// <param name="auth">버튼권한</param>
        protected virtual void InitMasterPageInfo(string docID, string processID, string docProcessStatus, DNSoft.eWF.FrameWork.Web.ApprovalUtil.ApprovalButtons auth)
        {
            if (webMaster != null)
            {
                webMaster.ProcessID = processID;
                webMaster.Requester = Sessions.UserName;
                webMaster.Company = Sessions.CompanyName;
                webMaster.CompanyCode = Sessions.CompanyCode;
                webMaster.Organization = Sessions.OrgName;
                webMaster.DocumentID = docID;

                webMaster.ProcessStatus = docProcessStatus;
                webMaster.CommandAuthList = ApprovalUtil.GetApprovalButtonAuthList((int)auth);
                webMaster.RequestID = Sessions.UserID;
                var htmlMailAddress = webMaster.FindControl("hddUserMailAddress"); //Remind를 위해
                if (htmlMailAddress != null && htmlMailAddress is HtmlInputHidden)
                    (htmlMailAddress as HtmlInputHidden).Value = Sessions.MailAddress;

            }

        }

        protected virtual void InitMasterPageInfo(string docID, string processID, string reuse)
        {
            if (reuse == ApprovalUtil.ApprovalStatus.Completed.ToString() || reuse == ApprovalUtil.ApprovalStatus.Reject.ToString() || reuse == ApprovalUtil.ApprovalStatus.Withdraw.ToString())
            {
                //TODO : Reject
                webMaster.ProcessID = "";
            }
            else
            {
                webMaster.ProcessID = processID;
            }
            webMaster.Requester = Sessions.UserName;
            webMaster.Company = Sessions.CompanyName;
            webMaster.CompanyCode = Sessions.CompanyCode;
            webMaster.Organization = Sessions.OrgName;
            webMaster.DocumentID = docID;
            webMaster.RequestID = Sessions.UserID;
            var htmlMailAddress = webMaster.FindControl("hddUserMailAddress"); //Remind를 위해
            if (htmlMailAddress != null && htmlMailAddress is HtmlInputHidden)
                (htmlMailAddress as HtmlInputHidden).Value = Sessions.MailAddress;
            GetDocbtnStatus(webMaster.ProcessID);
            if (processID.Length > 0 && !(webMaster.ProcessStatus.Equals("Temp") || webMaster.ProcessStatus.Equals("Saved")))
                GetCheckApprovalProcessUser(processID);


            //}

        }
        #endregion

        #region GetCheckApprovalProcessUser
        private void GetCheckApprovalProcessUser(string processID)
        {
            bool retValue = false;
            using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
            {
                retValue = mgr.IsApprovalProcessUser(processID, Sessions.UserID);
            }
            if (!retValue)
            {
                string redirectUrl = DNSoft.eW.FrameWork.eWBase.GetConfig("//eWorkflowInfo/DocError");
                Response.Redirect(redirectUrl);
            }
        }
        #endregion

        #region CallClientScript
        protected void CallClientScript(string strFnc)
        {
            string radconfirmscript = "<script language='javascript'>function f(){ " + strFnc + "; Sys.Application.remove_load(f);}; Sys.Application.add_load(f);</script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "radconfirm", radconfirmscript);
        }
        #endregion

        #region OnPreInit
        protected override void OnPreInit(EventArgs e)
        {
            try
            {

                if (webMaster != null)
                {
                    this.webMaster.btnRequestClicked += new EventHandler(RequestClick);
                    this.webMaster.btnApprovalClicked += new EventHandler(ApprovalClick);
                    this.webMaster.btnFowardApprovalClicked += new EventHandler(FowardApprovalClick);
                    this.webMaster.btnRejectClicked += new EventHandler(RejectClick);
                    this.webMaster.btnFowardClicked += new EventHandler(FowardClick);
                    this.webMaster.btnRecallClicked += new EventHandler(RecallClick);
                    this.webMaster.btnWithdrawClicked += new EventHandler(WithdrawClick);
                    this.webMaster.btnExitClicked += new EventHandler(ExitClick);
                    this.webMaster.btnSaveClicked += new EventHandler(SaveClick);
                    this.webMaster.btnInputCommandClick += new EventHandler(InputCommentClick);
                    this.webMaster.btnRemindClicked += new EventHandler(RemindClick);
                    this.webMaster.btnReUseClicked += new EventHandler(ReUseClick);

                }

            }
            catch (Exception ex)
            {
                this.informationMessage = ex.Message;
            }
            finally
            {
                base.OnPreInit(e);
            }
        }
        #endregion

        #region Document Command Event
        private void RemindClick(object sender, EventArgs e)
        {
            DoRemind();
        }

        protected virtual void DoRemind()
        {
            // TODO : 
            webMaster.MasterProcessing(System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void InputCommentClick(object sender, EventArgs e)
        {
            DoInputComment();
        }

        protected virtual void DoInputComment()
        {
            // TODO : 
            webMaster.MasterProcessing(System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void SaveClick(object sender, EventArgs e)
        {
            DoSave();
        }

        protected virtual void DoSave()
        {
            // TODO :
            // TODO : 
            webMaster.MasterProcessing(System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void ExitClick(object sender, EventArgs e)
        {
            DoExit();
        }

        protected virtual void DoExit()
        {
            // TODO :
        }

        private void WithdrawClick(object sender, EventArgs e)
        {
            DoWithdraw();
        }

        protected virtual void DoWithdraw()
        {
            // TODO : 
            webMaster.MasterProcessing(System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void RecallClick(object sender, EventArgs e)
        {
            DoRecall();
        }

        protected virtual void DoRecall()
        {
            // TODO : 
            webMaster.MasterProcessing(System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void FowardClick(object sender, EventArgs e)
        {
            DoForward();
        }

        protected virtual void DoForward()
        {
            // TODO :
            // TODO : 
            webMaster.MasterProcessing(System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void RejectClick(object sender, EventArgs e)
        {
            DoReject();
        }

        protected virtual void DoReject()
        {
            // TODO : 
            webMaster.MasterProcessing(System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }


        private void FowardApprovalClick(object sender, EventArgs e)
        {
            DoForwardApproval();
        }

        protected virtual void DoForwardApproval()
        {
            // TODO : 
            webMaster.MasterProcessing(System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void ApprovalClick(object sender, EventArgs e)
        {
            DoApproval();
        }

        protected virtual void DoApproval()
        {
            // TODO : 
            webMaster.MasterProcessing(System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        /// <summary>
        /// 결재 요청버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RequestClick(Object sender, EventArgs e)
        {
            DoRequest();
        }

        protected virtual void DoRequest()
        {
            // TODO : 
            webMaster.MasterProcessing(System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void ReUseClick(object sender, EventArgs e)
        {
            DoReUse();
        }

        protected virtual void DoReUse()
        {
            //TODO :
            webMaster.MasterProcessing(System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
        #endregion
    }
}
