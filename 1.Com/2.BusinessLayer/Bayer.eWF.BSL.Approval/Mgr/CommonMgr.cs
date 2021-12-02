using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DNSoft.eWF.FrameWork.Web;


namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class CommonMgr : MgrBase
    {
        #region InsertProcessApproveAddtional
        /// <summary>
        /// 결재선(Recipient List 추가) 등록처리
        /// </summary>
        /// <param name="add"></param>
        public void InsertProcessApproveAddtional(string processID, List<Dto.DTO_PROCESS_APPROVER_ADDTIONAL> add)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (Dao.CommonDao dao = new Dao.CommonDao())
                    {

                        foreach (Dto.DTO_PROCESS_APPROVER_ADDTIONAL item in add)
                        {
                            dao.InsertProcessApproveAddtional(item);
                        }
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DeleteProcessApproveAddtional
        /// <summary>
        /// 결재선(AdditionalApproer(A)/Recipient(T) List 삭제) 삭제처리
        /// </summary>
        /// <param name="add"></param>
        public void DeleteProcessApproveAddtional(string processID, string approvalTYPE)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (Dao.CommonDao dao = new Dao.CommonDao())
                    {

                        dao.DeleteProcessApproveAddtional(processID, approvalTYPE);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region InsertProcessApprove [결재요청 처리]
        /// <summary>
        /// 결재요청 처리 실행
        /// </summary>
        /// <param name="apprList">결재자리스트</param>
        /// <param name="docProc">문서테이블</param>
        /// <param name="documentid"></param>
        /// <param name="processstatus">문서상태</param>
        public void InsertProcessApprove(List<Dto.DTO_PROCESS_APPROVER> apprList, Dto.DTO_PROCESS_DOCUMENT docProc, string documentid, string processstatus, string userid)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (Dao.CommonDao dao = new Dao.CommonDao())
                    {
                        dao.DeleteProcessApprove(apprList[0].PROCESS_ID);
                        foreach (Dto.DTO_PROCESS_APPROVER item in apprList)
                        {
                            dao.InsertProcessApprove(item);

                        }
                        dao.InsertProcessDocument(docProc);
                        dao.UpdateDocumentProcessStatus(documentid, apprList[0].PROCESS_ID, processstatus, userid);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 문서리스트에 추가
        /// </summary>
        /// <param name="docProc"></param>
        public void InsertProcessDocument(Dto.DTO_PROCESS_DOCUMENT docProc)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (Dao.CommonDao dao = new Dao.CommonDao())
                    {
                        dao.InsertProcessDocument(docProc);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region UpdateProcessStatus [ Approval, Reject 처리]
        /// <summary>
        /// Approval, Reject 처리
        /// </summary>
        /// <param name="documentid">문서ID</param>
        /// <param name="processid">프로세스ID</param>
        /// <param name="comment">커멘트</param>
        /// <param name="processstatus">문서상태</param>
        /// <param name="userid">현결재자ID</param>
        /// <param name="approverstatus">결재자상태</param>
        public void UpdateProcessStatus(string documentid, string processid, string comment, string processstatus, string userid, string approverstatus)
        {
            string finalApprover = string.Empty;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (Dao.CommonDao dao = new Dao.CommonDao())
                    {
                        dao.UpdateProcessDocumentStatus(processid, userid, processstatus);
                        dao.UpdateDocumentProcessStatus(documentid, processid, processstatus, userid);
                        dao.UpdateProcessApproverStatus(processid, userid, approverstatus, comment);

                        if (approverstatus.Equals(ApprovalUtil.ProcessStatus.ACEPTER))
                        {
                            finalApprover = dao.GetFinalApprovalIDLine(processid);
                            // 현결재자가 최종결재자일 경우 결재문서 상태를 Completed로 변경한다.
                            if (userid.Equals(finalApprover))
                            {
                                dao.UpdateProcessCompleted(documentid, processid, ApprovalUtil.ApprovalStatus.Completed.ToString(), userid);
                            }
                        }
                        else if (approverstatus.Equals(ApprovalUtil.ProcessStatus.REJECTER))
                        {
                            dao.InsertProcessApproverCompleted(processid);
                        }
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region InsertWithdraw [ Withdraw 처리]
        /// <summary>
        /// Withdraw 처리
        /// </summary>
        /// <param name="documentid"></param>
        /// <param name="processid"></param>
        /// <param name="comment"></param>
        /// <param name="processstatus"></param>
        /// <param name="userid"></param>
        /// <param name="logtype"></param>
        public void InsertWithdraw(string documentid, string processid, string comment, string processstatus, string userid, string logtype, string approverstatus)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (Dao.CommonDao dao = new Dao.CommonDao())
                    {
                        Dto.DTO_DOC_LOG log = new Dto.DTO_DOC_LOG();
                        log.PROCESS_ID = processid;
                        log.REGISTER_ID = userid;
                        log.LOG_TYPE = logtype;
                        log.COMMENT = comment;
                        log.CREATE_DATE = DateTime.Now;

                        dao.InsertDocLog(log);
                        dao.UpdateProcessDocumentStatus(processid, userid, processstatus);
                        dao.UpdateDocumentProcessStatus(documentid, processid, processstatus, userid);
                        dao.UpdateProcessApproverStatus(processid, userid, approverstatus, comment);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region InsertForwardApproval [ ForwardApproval 처리]
        /// <summary>
        /// ForwardApproval 처리
        /// </summary>
        /// <param name="documentid">문서ID</param>
        /// <param name="processid">프로세스ID</param>
        /// <param name="currentUserid">현결재자ID</param>
        /// <param name="forwardApproverId">Forward Approval Target UserID</param>
        /// <param name="processstatus">문서상태</param>
        /// <param name="approverstatus">결재자상태</param>
        //--eWorkflow Optimization 2020 

        //public void InsertForwardApproval(string documentid, string processid, string currentUserid, string forwardApproverId, string processstatus, string approverstatus)
        public void InsertForwardApproval(string documentid, string processid, string currentUserid, string forwardApproverId, string processstatus, string approverstatus,string comment)
        {
            string finalApprover = string.Empty;
            try
            {
                //using (TransactionScope scope = new TransactionScope())
                {
                    using (Dao.CommonDao dao = new Dao.CommonDao())
                    {
                        dao.InsertForwardApprovalAddUser(processid, currentUserid, forwardApproverId);
                        dao.UpdateProcessDocumentStatus(processid, currentUserid, processstatus);
                        dao.UpdateDocumentProcessStatus(documentid, processid, processstatus, currentUserid);
                        //comment 추가 되어야함 0404 - eWorkflow Optimization
                        dao.UpdateProcessApproverStatus(processid, currentUserid, approverstatus, comment);
                        finalApprover = dao.GetFinalApprovalIDLine(processid);

                        // 현결재자가 최종결재자와 동일ID일 경우 최종결재자를 forwardApprover로 변경한다.
                        if (currentUserid.Equals(finalApprover))
                        {
                            dao.ChangeProcessDocumentFinalApprover(processid, forwardApproverId);
                        }
                    }
                    //scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region InsertForward [ Forward 처리]
        /// <summary>
        /// ForwardApproval 처리
        /// </summary>
        /// <param name="documentid">문서ID</param>
        /// <param name="processid">프로세스ID</param>
        /// <param name="currentUserid">현결재자ID</param>
        /// <param name="forwardApproverId">Forward Approval Target UserID</param>
        /// <param name="processstatus">문서상태</param>
        /// <param name="approverstatus">결재자상태</param>
        public void InsertForward(List<Dto.DTO_PROCESS_APPROVER_COMPLETED> reviewers, string comment, string userId, string logtype)
        {
            string finalApprover = string.Empty;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (Dao.CommonDao dao = new Dao.CommonDao())
                    {
                        if (reviewers.Count > 0)
                        {
                            Dto.DTO_DOC_LOG log = new Dto.DTO_DOC_LOG();
                            log.PROCESS_ID = reviewers[0].PROCESS_ID;
                            log.REGISTER_ID = userId;
                            log.LOG_TYPE = logtype;
                            log.COMMENT = comment;
                            log.CREATE_DATE = DateTime.Now;
                            dao.InsertDocLog(log);
                        }
                        foreach (Dto.DTO_PROCESS_APPROVER_COMPLETED item in reviewers)
                        {
                            dao.InsertProcessApproveCompletedAddReviewer(item);
                        }
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetApprovalLine
        public List<Dto.DTO_APPROVAL> GetApprovalLine(string documentID, string userID, string processID)
        {
            try
            {
                using (Dao.CommonDao dao = new Dao.CommonDao())
                {
                    return dao.GetApprovalLine(documentID, userID, processID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SelectApprovalTodoList
        public List<Dto.DTO_PROCESS_TODO_LIST> SelectApprovalTodoList(string searchType, string searchText, string chkDateUseYN, DateTime? startDate, DateTime? endDate, string currentApprover)
        {

            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.SelectApprovalTodoList(searchType, searchText, chkDateUseYN, startDate, endDate, currentApprover);
            }
        }
        #endregion

        #region SelectProcessApprovalStatusByUser
        public List<Dto.DTO_PROCESS_APPROVAL_USER_STATUS> SelectProcessApprovalStatusByUser(string processID, string userID)
        {
            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.SelectProcessApprovalStatusByUser(processID, userID);
            }
        }
        #endregion

        #region SelectProcessApproverList
        public List<Dto.DTO_PROCESS_APPROVAL_LIST> SelectProcessApproverList(string processID)
        {
            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.SelectProcessApproverList(processID);
            }
        }
        #endregion

        #region SelectDocLogList
        public List<Dto.DTO_DOC_LOG_LIST> SelectDocLogList(string processID)
        {
            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.SelectDocLogList(processID);
            }
        }
        #endregion

        #region SelectApprovalTodoList
        public List<Dto.DTO_PROCESS_TODO_LIST> SelectApprovalSavedList(string searchType, string searchText, string chkDateUseYN, DateTime? startDate, DateTime? endDate, string requesterID)
        {

            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.SelectApprovalSavedList(searchType, searchText, chkDateUseYN, startDate, endDate, requesterID);
            }
        }
        #endregion

        #region SelectApprovalProcessList
        public List<Dto.DTO_PROCESS_PROECESSING_LIST> SelectApprovalProcessList(string searchType, string searchText, string chkDateUseYN, DateTime? startDate, DateTime? endDate, string requesterID)
        {

            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.SelectApprovalProcessList(searchType, searchText, chkDateUseYN, startDate, endDate, requesterID);
            }
        }
        #endregion

        #region SelectApprovalCompletedList
        public List<Dto.DTO_PROCESS_COMPLETED_LIST> SelectApprovalCompletedList(string searchType, string searchText, string chkDateUseYN, DateTime? startDate, DateTime? endDate, string userid)
        {

            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.SelectApprovalCompletedList(searchType, searchText, chkDateUseYN, startDate, endDate, userid);
            }
        }
        #endregion

        #region SelectCollateralCompletedList
         public List<Dto.DTO_PROCESS_COMPLETED_COLLATERAL_LIST> SelectCollateralCompletedList(string bg, string type, string keyword, string userid)
        {
            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.SelectCollateralCompletedList(bg, type, keyword, userid);
            }
        }

         public List<string> SelectCollateralBg()
         {
             try
             {
                 using (Dao.CommonDao dao = new Dao.CommonDao())
                 {
                     return dao.SelectCollateralBg();
                 }
             }
             catch
             {
                 throw;
             }
         }
        #endregion

        #region InsertInputComment [InputComment 처리]
        /// <summary>
        /// InputComment 처리
        /// </summary>
        /// <param name="processid">프로세스아이디</param>
        /// <param name="comment">커민트</param>
        /// <param name="userid">사용자아이디</param>
        /// <param name="logtype">로그타입</param>
        /// <param name="uploadTempPath">임시 첨부폴더</param>
        /// <param name="file">첨부파일정보</param>
        public void InsertInputComment(string processid, string comment, string userid, string logtype, string uploadTempPath, Bayer.eWF.BSL.Common.Dto.DTO_ATTACH_FILES file)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (Bayer.eWF.BSL.Common.Mgr.FileMgr mgr = new Common.Mgr.FileMgr())
                    using (Dao.CommonDao dao = new Dao.CommonDao())
                    {
                        Dto.DTO_DOC_LOG log = new Dto.DTO_DOC_LOG();
                        log.PROCESS_ID = processid;
                        log.REGISTER_ID = userid;
                        log.LOG_TYPE = logtype;
                        log.COMMENT = comment;
                        log.CREATE_DATE = DateTime.Now;

                        int idx = dao.InsertDocLog(log);

                        if (file != null)
                        {
                            file.COMMENT_IDX = idx;
                            mgr.AddAttachFile(processid, userid, uploadTempPath, file);
                        }

                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region UpdateRecall [Recall 처리]
        /// <summary>
        /// 진행중인 결재를 취소하고 Saved 상태로 변경한다.
        /// 결재선 제거
        /// </summary>
        /// <param name="documentid">문서ID</param>
        /// <param name="processid">프로세스아이디</param>
        /// <param name="comment">커멘트</param>
        /// <param name="userid">기안자</param>
        /// <param name="logtype">로그타입</param>
        /// <param name="processstatus">문서상태</param>
        public void UpdateRecall(string documentid, string processid, string comment, string userid, string logtype, string processstatus)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (Dao.CommonDao dao = new Dao.CommonDao())
                    {

                        dao.UpdateProcessDocumentStatus(processid, userid, processstatus);
                        dao.UpdateDocumentProcessStatus(documentid, processid, processstatus, userid);
                        dao.DeleteProcessApprove(processid);

                        Dto.DTO_DOC_LOG log = new Dto.DTO_DOC_LOG();
                        log.PROCESS_ID = processid;
                        log.REGISTER_ID = userid;
                        log.LOG_TYPE = logtype;
                        log.COMMENT = comment;
                        log.CREATE_DATE = DateTime.Now;

                        dao.InsertDocLog(log);

                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SelectSendMailTargetList [ 결재 후처리로 메일 발송 대상자를 조회해 온다 ]
        public List<Dto.DTO_SENDMAIL_TO_ADDRESS_LIST> SelectSendMailTargetList(string processid, string mailsendType)
        {

            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.SelectSendMailTargetList(processid, mailsendType);
            }
        }
        #endregion

        #region SelectSendMailRetrunGoodList [Return Good Approver 메일 발송 대상사를 조회]
        public List<Dto.DTO_SENDMAIL_TO_ADDRESS_LIST> SelectSendMailRetrunGoodList(string searchdata)
        {

            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.SelectSendMailRetrunGoodList(searchdata);
            }
        }
        

        #endregion

        #region UpdateApproverSentMail [ TB_PROCESS_APPROVER 메일 발송 완료 업데이트]
        /// <summary>
        /// TB_PROCESS_APPROVER 메일 발송 완료 업데이트
        /// </summary>
        /// <param name="processid">프로세스아이디</param>
        /// <param name="approver">approver id list. ex) abc,def,dwf</param>
        /// <param name="split">approver split type ex) [,]</param>
        public void UpdateApproverSentMail(string processid, string approver, string split)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (Dao.CommonDao dao = new Dao.CommonDao())
                    {
                        dao.UpdateApproverSentMail(processid, approver, split);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateApproverSentMailComment(string processId, string status, string approver, string split)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (Dao.CommonDao dao = new Dao.CommonDao())
                    {
                        dao.UpdateApproverSentMailComment(processId, status, approver, split);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SelectApprovalRejectList
        public List<Dto.DTO_PROCESS_PROECESSING_LIST> SelectApprovalRejectList(string searchType, string searchText, string chkDateUseYN, DateTime? startDate, DateTime? endDate, string requesterID)
        {

            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.SelectApprovalRejectList(searchType, searchText, chkDateUseYN, startDate, endDate, requesterID);
            }
        }
        #endregion

        #region SelectApprovalWithDrawList
        public List<Dto.DTO_PROCESS_PROECESSING_LIST> SelectApprovalWithDrawList(string searchType, string searchText, string chkDateUseYN, DateTime? startDate, DateTime? endDate, string requesterID)
        {

            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.SelectApprovalWithDrawList(searchType, searchText, chkDateUseYN, startDate, endDate, requesterID);
            }
        }
        #endregion


        #region GetNoticeMailList [ 장기간 미 결재자 알림 메일 발송할 리스트를 조회해 온다 ]
        public List<Dto.DTO_SENDMAIL_TO_ADDRESS_LIST> GetNoticeMailList(DateTime basicdate)
        {

            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.GetNoticeMailList(basicdate);
            }
        }
        #endregion

        #region IsCompletedApproval
        /// <summary>
        /// 문서의 결재 완료 상태 여부를 조회
        /// </summary>
        /// <param name="processid"></param>
        /// <returns></returns>
        public bool IsCompletedApproval(string processid)
        {
            bool retValue = false;
            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                string val = dao.IsCompletedApproval(processid);
                retValue = val.Equals("Y") ? true : false;
            }
            return retValue;
        }
        #endregion


        #region GetApprovalCount
        /// <summary>
        /// 문서의 결재 건수 조회
        /// </summary>
        /// <param name="userid">아이디</param>
        /// <param name="viewCntType">건수타입(Saved:임시저장건수, Todo:결재대기건수, Processing:진행건수)</param>
        /// <returns></returns>
        public int GetApprovalCount(string userid, string viewCntType)
        {
            int retValue = 0;
            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                retValue = dao.GetApprovalCount(userid, viewCntType);
            }
            return retValue;
        }
        #endregion

        #region [ SelectProcessDocumentStatus ]

        public string SelectProcessDocumentStatus(string processId)
        {
            try
            {
                using (Dao.CommonDao dao = new Dao.CommonDao())
                {
                    return dao.SelectProcessDocumentStatus(processId);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region [ SelectProcessRejectUser ]

        public Dto.RejectDocumentMailCommentDto SelectProcessRejectUser(string processId)
        {
            try
            {
                using (Dao.CommonDao dao = new Dao.CommonDao())
                {
                    return dao.SelectProcessRejectUser(processId);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region [ DeleteDocumentProcess ]

        public void DeleteDocumentProcess(string processIds)
        {
            try
            {
                using (Dao.CommonDao dao = new Dao.CommonDao())
                {
                    dao.DeleteDocumentProcess(processIds);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region [ SelectInputComment ]
        public string SelectInputComment(string processId)
        {
            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.SelectInputComment(processId);
            }
        } 
        #endregion

        #region InsertDocLog
        public int InsertDocLog(Dto.DTO_DOC_LOG log)
        {
            int retValue = 0;
            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                retValue = dao.InsertDocLog(log);
            }
            return retValue;
        }
        #endregion

        #region [ Process Auth User Check ]
        public bool IsApprovalProcessUser(string processId, string userid)
        {
            string retValue = string.Empty;
            bool bretValue = false;
            try
            {
                using (Dao.CommonDao dao = new Dao.CommonDao())
                {
                     retValue =  dao.IsApprovalProcessUser(processId, userid);
                     if (retValue.ToUpper().Equals("TRUE"))
                         bretValue = true;
                     else
                         bretValue = false;
                }
                return bretValue;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region SelectAdminDocumentList
        public List<Dto.DTO_ADMIN_DOCUMENT_LIST> SelectAdminDocumentList(string searchType, string searchText, string chkDateUseYN, DateTime? startDate, DateTime? endDate)
        {

            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.SelectAdminDocumentList(searchType, searchText, chkDateUseYN, startDate, endDate);
            }
        }

        public List<Dto.DTO_ADMIN_DOCUMENT_LIST> SelectAdminViewDocumentList(string searchType, string searchText, string chkDateUseYN, DateTime? startDate, DateTime? endDate, string userId)
        {

            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.SelectAdminViewDocumentList(searchType, searchText, chkDateUseYN, startDate, endDate, userId);
            }
        }

        #endregion

        #region SelectWithHolding
        public List<Dto.DTO_WITHHOLDING_LIST> SelectWithHolding(string searchText, DateTime? startDate, DateTime? endDate)
        {

            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.SelectWithHolding(searchText, startDate, endDate);
            }
        }
        #endregion

        #region SelectAdminReportingDocumentList
        //public List<Dto.DTO_ADMIN_REPORTING_DOCUMENT_LIST> SelectAdminReportingDocumentList(string DocumentID)
        //{

        //    using (Dao.CommonDao dao = new Dao.CommonDao())
        //    {
        //        return dao.SelectAdminReportingDocumentList(DocumentID);
        //    }
        //}
        public List<Dto.DTO_ADMIN_REPORTING_DOCUMENT_LIST> SelectAdminReportingDocumentList(string DocumentID,DateTime? FROM_DATE,DateTime? TO_DATE)
        {

            using (Dao.CommonDao dao = new Dao.CommonDao())
            {
                return dao.SelectAdminReportingDocumentList(DocumentID, FROM_DATE, TO_DATE);
            }
        }

        #endregion


    }
}
