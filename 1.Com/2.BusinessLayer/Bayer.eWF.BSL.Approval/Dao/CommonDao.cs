using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNSoft.eWF.FrameWork.Data.EF;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class CommonDao : DaoBase
    {

        #region GetNewProcessID
        public string GetNewProcessID()
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    return context.Database.SqlQuery<string>(ApprovalContext.USP_CREATE_PROCESS_ID).First().ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region InsertProcessApproveAddtional [TB_PROCESS_APPROVER_ADDTIONAL 테이블에 Recipient 를 추가]
        /// <summary>
        /// TB_PROCESS_APPROVER_ADDTIONAL 테이블에 Recipient 를 추가
        /// </summary>
        /// <param name="add">추가할 Recipient Item</param>
        public void InsertProcessApproveAddtional(Dto.DTO_PROCESS_APPROVER_ADDTIONAL add)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[5];
                    parameters[0] = new SqlParameter("@PROCESS_ID", add.PROCESS_ID);
                    parameters[1] = new SqlParameter("@IDX", add.IDX);
                    parameters[2] = new SqlParameter("@APPROVAL_TYPE", add.APPROVAL_TYPE);
                    parameters[3] = new SqlParameter("@USER_ID", add.USER_ID);
                    parameters[4] = new SqlParameter("@CREATE_DATE", add.CREATE_DATE);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_PROCESS_APPROVER_ADDTIONAL, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DeleteProcessApproveAddtional [TB_PROCESS_APPROVER_ADDTIONAL 테이블에 Additional Approer(A)/Recipient(T) 를 삭제]
        /// <summary>
        /// TB_PROCESS_APPROVER_ADDTIONAL 테이블에 Recipient 를 삭제
        /// </summary>
        /// <param name="add">삭제할 PROCESS ID</param>
        public void DeleteProcessApproveAddtional(string processID, string approvalTYPE)
        {

            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processID);
                    parameters[1] = new SqlParameter("@APPROVAL_TYPE", approvalTYPE);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_PROCESS_APPROVER_ADDTIONAL, parameters);
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

            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@DOCUMENT_ID", documentID);
                parameters[1] = new SqlParameter("@USER_ID", userID);
                parameters[2] = new SqlParameter("@PROCESS_ID", processID);
                var result = context.Database.SqlQuery<Dto.DTO_APPROVAL>(ApprovalContext.USP_SELECT_APPROVAL_LINE, parameters);

                return result.ToList();
            }
        }
        #endregion

        #region UpdateDocumentProcessStatus 문서테이블별 문서상태 변경
        /// <summary>
        /// 각 문서테이불에 프로세스ID별로 문서상태를 변경한다.<br/>
        /// 각 테이블에 상태, 변경일자, 변경자 업데이트 처리<br/>
        /// </summary>
        /// <param name="documentid">문서ID</param>
        /// <param name="processid">프로세스ID</param>
        /// <param name="processstatus">문서상태</param>
        /// <param name="userid">결재자ID</param>
        public void UpdateDocumentProcessStatus(string documentid, string processid, string processstatus, string userid)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[4];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentid);
                    parameters[1] = new SqlParameter("@PROCESS_ID", processid);
                    parameters[2] = new SqlParameter("@PROCESS_STATUS", processstatus);
                    parameters[3] = new SqlParameter("@APPROVER_ID", userid);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_UPDATE_DOCUMENT_PROCESS_STATUS, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region InsertProcessApprove 결재선 추가
        public void InsertProcessApprove(Dto.DTO_PROCESS_APPROVER add)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[14];
                    parameters[0] = new SqlParameter("@PROCESS_ID", add.PROCESS_ID);
                    parameters[1] = new SqlParameter("@APPROVAL_TYPE", add.APPROVAL_TYPE);
                    parameters[2] = new SqlParameter("@APPROVAL_SEQ", add.APPROVAL_SEQ);
                    parameters[3] = new SqlParameter("@APPROVER_TYPE", add.APPROVER_TYPE);
                    parameters[4] = new SqlParameter("@APPROVER_ID", add.APPROVER_ID);
                    parameters[5] = new SqlParameter("@APPROVER_ORG_NAME", add.APPROVER_ORG_NAME);
                    parameters[6] = new SqlParameter("@ABSENCE_APPROVER_ID", add.ABSENCE_APPROVER_ID);
                    parameters[7] = new SqlParameter("@ABSENCE_APPROVER_ORG_NAME", add.ABSENCE_APPROVER_ORG_NAME);
                    parameters[8] = new SqlParameter("@STATUS", add.STATUS);
                    parameters[9] = new SqlParameter("@SENT_MAIL", add.SENT_MAIL);
                    parameters[10] = new SqlParameter("@APPROVAL_DATE", (object)add.APPROVAL_DATE ?? DBNull.Value);
                    parameters[11] = new SqlParameter("@COMMENT", add.COMMENT);
                    parameters[12] = new SqlParameter("@CREATE_DATE", add.CREATE_DATE);
                    parameters[13] = new SqlParameter("@UPDATE_DATE", (object)add.UPDATE_DATE ?? DBNull.Value);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_PROCESS_APPROVER, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region InsertProcessDocument [ TB_PROCESS_DOCUMENT 추가 ]
        /// <summary>
        /// 결재 문서 목록을 추가한다( TB_PROCESS_DOCUMENT )
        /// </summary>
        /// <param name="add"></param>
        public void InsertProcessDocument(Dto.DTO_PROCESS_DOCUMENT add)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[14];
                    parameters[0] = new SqlParameter("@PROCESS_ID", add.PROCESS_ID);
                    parameters[1] = new SqlParameter("@DOCUMENT_ID", add.DOCUMENT_ID);
                    parameters[2] = new SqlParameter("@DOC_NAME", add.DOC_NAME);
                    parameters[3] = new SqlParameter("@SUBJECT", add.SUBJECT);
                    parameters[4] = new SqlParameter("@DOC_NUM", (object)add.DOC_NUM ?? DBNull.Value);
                    parameters[5] = new SqlParameter("@PROCESS_STATUS", add.PROCESS_STATUS);
                    parameters[6] = new SqlParameter("@REQUEST_DATE", (object)add.REQUEST_DATE ?? DBNull.Value);
                    parameters[7] = new SqlParameter("@COMPANY_CODE", add.COMPANY_CODE);
                    parameters[8] = new SqlParameter("@REQUESTER_ID", add.REQUESTER_ID);
                    parameters[9] = new SqlParameter("@CURRENT_APPROVER", add.CURRENT_APPROVER);
                    parameters[10] = new SqlParameter("@FINAL_APPROVER", add.FINAL_APPROVER);
                    parameters[11] = new SqlParameter("@REJECTED_PROCESS_ID", add.REJECTED_PROCESS_ID);
                    parameters[12] = new SqlParameter("@CREATE_DATE", add.CREATE_DATE);
                    parameters[13] = new SqlParameter("@UPDATE_DATE", (object)add.UPDATE_DATE ?? DBNull.Value);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_PROCESS_DOCUMENT, parameters);
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

            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@SEARCH_TYPE", searchType);
                parameters[1] = new SqlParameter("@SEARCH_TEXT", searchText);
                parameters[2] = new SqlParameter("@DATE_USEYN", chkDateUseYN);
                parameters[3] = new SqlParameter("@START_DATE", (object)startDate ?? DBNull.Value);
                parameters[4] = new SqlParameter("@END_DATE", (object)endDate ?? DBNull.Value);
                parameters[5] = new SqlParameter("@CURRENT_APPROVER", currentApprover);

                var result = context.Database.SqlQuery<Dto.DTO_PROCESS_TODO_LIST>(ApprovalContext.USP_SELECT_APPROVAL_TODO_LIST, parameters);

                return result.ToList();
            }
        }
        #endregion

        #region SelectProcessApprovalStatusByUser
        public List<Dto.DTO_PROCESS_APPROVAL_USER_STATUS> SelectProcessApprovalStatusByUser(string processID, string userID)
        {

            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@PROCESS_ID", processID);
                parameters[1] = new SqlParameter("@APPROVER_ID", userID);

                var result = context.Database.SqlQuery<Dto.DTO_PROCESS_APPROVAL_USER_STATUS>(ApprovalContext.USP_SELECT_PROCESS_APPROVER_USER, parameters);

                return result.ToList();
            }
        }
        #endregion

        #region SelectProcessApproverList
        public List<Dto.DTO_PROCESS_APPROVAL_LIST> SelectProcessApproverList(string processID)
        {

            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processID);

                var result = context.Database.SqlQuery<Dto.DTO_PROCESS_APPROVAL_LIST>(ApprovalContext.USP_SELECT_PROCESS_APPROVER_LIST, parameters);

                return result.ToList();
            }
        }
        #endregion

        #region SelectDocLogList
        public List<Dto.DTO_DOC_LOG_LIST> SelectDocLogList(string processID)
        {

            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processID);

                var result = context.Database.SqlQuery<Dto.DTO_DOC_LOG_LIST>(ApprovalContext.USP_SELECT_DOC_LOG_LIST, parameters);

                return result.ToList();
            }
        }
        #endregion

        #region UpdateDocumentProcessStatus [결재문서목록 테이블(TB_PROCESS_DOCUMENT) 상태변경]
        /// <summary>
        /// 결재문서목록 테이블(TB_PROCESS_DOCUMENT)에 문서상태와 다음결재자 정보를 변경
        /// </summary>
        /// <param name="processid">프로세스아이디</param>
        /// <param name="userid">현결재자아이디</param>
        /// <param name="processstatus">문서상태</param>
        public void UpdateProcessDocumentStatus(string processid, string userid, string processstatus)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processid);
                    parameters[1] = new SqlParameter("@APPROVER_ID", userid);
                    parameters[2] = new SqlParameter("@PROCESS_STATUS", processstatus);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_UPDATE_PROCESS_DOCUMENT_STATUS, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ChangeProcessDocumentFinalApprover [결재문서목록 테이블(TB_PROCESS_DOCUMENT) Final Approver를 변경한다.]
        /// <summary>
        /// 결재문서목록 테이블(TB_PROCESS_DOCUMENT)에 문서상태와 다음결재자 정보를 변경
        /// </summary>
        /// <param name="processid">프로세스아이디</param>
        /// <param name="newFinalApproverid">현결재자아이디</param>
        public void ChangeProcessDocumentFinalApprover(string processid, string newFinalApproverid)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processid);
                    parameters[1] = new SqlParameter("@APPROVER_ID", newFinalApproverid);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_UPDATE_PROCESS_DOCUMENT_FINAL_APPROVER, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region UpdateDocumentProcessStatus [결재선변경(TB_PROCESS_APPROVER) APPROVER STATUS 변경]
        /// <summary>
        /// 결재선에 현결재자 프로세스상태를 변경한다.
        /// </summary>
        /// <param name="processid">프로세스아이디</param>
        /// <param name="userid">현결재자</param>
        /// <param name="status">프로세스상태</param>
        /// <param name="comment">커멘트</param>
        public void UpdateProcessApproverStatus(string processid, string userid, string status, string comment)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[4];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processid);
                    parameters[1] = new SqlParameter("@APPROVER_ID", userid);
                    parameters[2] = new SqlParameter("@STATUS", status);
                    parameters[3] = new SqlParameter("@COMMENT", comment);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_UPDATE_PROCESS_APPROVER, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region InsertDocLog
        public int InsertDocLog(Dto.DTO_DOC_LOG log)
        {
            int retValue = 0;
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[5];
                    parameters[0] = new SqlParameter("@PROCESS_ID", log.PROCESS_ID);
                    parameters[1] = new SqlParameter("@REGISTER_ID", log.REGISTER_ID);
                    parameters[2] = new SqlParameter("@LOG_TYPE", log.LOG_TYPE);
                    parameters[3] = new SqlParameter("@COMMENT", log.COMMENT);
                    parameters[4] = new SqlParameter("@CREATE_DATE", log.CREATE_DATE);

                    retValue = context.Database.SqlQuery<int>(ApprovalContext.USP_INSERT_DOC_LOG, parameters).First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }
        #endregion

        #region InsertForwardApprovalAddUser [TB_PROCESS_APPROVER Foward Approver Append]
        /// <summary>
        /// 결재선에 현결재자 다음으로 결재선에 추가
        /// </summary>
        /// <param name="processid"></param>
        /// <param name="currentApproverid"></param>
        /// <param name="forwardUserid"></param>
        public void InsertForwardApprovalAddUser(string processid, string currentApproverid, string forwardUserid)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processid);
                    parameters[1] = new SqlParameter("@USER_ID", currentApproverid);
                    parameters[2] = new SqlParameter("@APPROVER_ID", forwardUserid);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_PROCESS_APPROVER_FORWARDAPPROVAL_USER, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetFinalApprovalID [ TB_PROCESS_DOCUMENT에서 FINAL_APPROVER USERID를 조회한다 ]
        /// <summary>
        ///  TB_PROCESS_DOCUMENT에서 FINAL_APPROVER USERID를 조회한다
        /// </summary>
        /// <param name="processid">프로세스 ID</param>
        /// <returns>FINAL_APPROVER ID</returns>
        public string GetFinalApprovalID(string processid)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processid);

                    return context.Database.SqlQuery<string>(ApprovalContext.USP_SELECT_DOCUMENT_FINAL_APPROVER_ID, parameters).First().ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetFinalApprovalIDLine [ TB_PROCESS_APPROVER에서 FINAL_APPROVER USERID를 조회한다 ]
        /// <summary>
        ///  TB_PROCESS_APPROVER에서 FINAL_APPROVER USERID를 조회한다
        /// </summary>
        /// <param name="processid">프로세스 ID</param>
        /// <returns>FINAL_APPROVER ID</returns>
        public string GetFinalApprovalIDLine(string processid)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processid);

                    return context.Database.SqlQuery<string>(ApprovalContext.USP_SELECT_DOCUMENT_FINAL_APPROVER_ID_LINE, parameters).First().ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region UpdateProcessCompleted [최종결재완료시 수행 프로세스]
        /// <summary>
        /// 최종 결재자가 결재완료를 수행했을 경우 업데이트를 수행한다.
        /// </summary>
        /// <param name="documentid">문서ID</param>
        /// <param name="processid">프로세스ID</param>
        /// <param name="processstatus">문서상태</param>
        /// <param name="approverid">결재자ID</param>
        public void UpdateProcessCompleted(string documentid, string processid, string processstatus, string approverid)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[4];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentid);
                    parameters[1] = new SqlParameter("@PROCESS_ID", processid);
                    parameters[2] = new SqlParameter("@PROCESS_STATUS", processstatus);
                    parameters[3] = new SqlParameter("@APPROVER_ID", approverid);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_UPDATE_PROCESS_COMPLETED, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region InsertProcessApproveCompletedAddReviewer [완료된 결재선에 Reviewer 추가]
        /// <summary>
        /// 완료된 결재선에 Reviewer 추가
        /// </summary>
        /// <param name="add"></param>
        public void InsertProcessApproveCompletedAddReviewer(Dto.DTO_PROCESS_APPROVER_COMPLETED add)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[12];
                    parameters[0] = new SqlParameter("@PROCESS_ID", add.PROCESS_ID);
                    parameters[1] = new SqlParameter("@APPROVAL_TYPE", add.APPROVAL_TYPE);
                    parameters[2] = new SqlParameter("@APPROVAL_SEQ", add.APPROVAL_SEQ);
                    parameters[3] = new SqlParameter("@APPROVER_TYPE", add.APPROVER_TYPE);
                    parameters[4] = new SqlParameter("@APPROVER_ID", add.APPROVER_ID);
                    parameters[5] = new SqlParameter("@APPROVER_ORG_NAME", add.APPROVER_ORG_NAME);
                    parameters[6] = new SqlParameter("@ABSENCE_APPROVER_ID", add.ABSENCE_APPROVER_ID);
                    parameters[7] = new SqlParameter("@ABSENCE_APPROVER_ORG_NAME", add.ABSENCE_APPROVER_ORG_NAME);
                    parameters[8] = new SqlParameter("@STATUS", add.STATUS);
                    parameters[9] = new SqlParameter("@SENT_MAIL", add.SENT_MAIL);
                    parameters[10] = new SqlParameter("@APPROVAL_DATE", (object)add.APPROVAL_DATE ?? DBNull.Value);
                    parameters[11] = new SqlParameter("@COMMENT", add.COMMENT);


                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_PROCESS_APPROVER_COMPLETED_ADD_REVIEWER, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DeleteProcessApprove [ReCall시에 TB_PROCESS_APPROVE를 제거한다]
        /// <summary>
        /// ReCall시 기존 결재선(TB_PROCESS_APPROVE)에 있는 정보를 모두 제거한다.
        /// </summary>
        /// <param name="processid">프로세스ID</param>
        public void DeleteProcessApprove(string processid)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processid);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_PROCESS_APPROVER, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SelectApprovalSavedList
        public List<Dto.DTO_PROCESS_TODO_LIST> SelectApprovalSavedList(string searchType, string searchText, string chkDateUseYN, DateTime? startDate, DateTime? endDate, string requesterID)
        {

            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@SEARCH_TYPE", searchType);
                parameters[1] = new SqlParameter("@SEARCH_TEXT", searchText);
                parameters[2] = new SqlParameter("@DATE_USEYN", chkDateUseYN);
                parameters[3] = new SqlParameter("@START_DATE", (object)startDate ?? DBNull.Value);
                parameters[4] = new SqlParameter("@END_DATE", (object)endDate ?? DBNull.Value);
                parameters[5] = new SqlParameter("@REQUESTER_ID", requesterID);

                var result = context.Database.SqlQuery<Dto.DTO_PROCESS_TODO_LIST>(ApprovalContext.USP_SELECT_APPROVAL_SAVED_LIST, parameters);

                return result.ToList();
            }
        }
        #endregion

        #region SelectApprovalProcessList
        public List<Dto.DTO_PROCESS_PROECESSING_LIST> SelectApprovalProcessList(string searchType, string searchText, string chkDateUseYN, DateTime? startDate, DateTime? endDate, string requesterID)
        {

            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@SEARCH_TYPE", searchType);
                parameters[1] = new SqlParameter("@SEARCH_TEXT", searchText);
                parameters[2] = new SqlParameter("@DATE_USEYN", chkDateUseYN);
                parameters[3] = new SqlParameter("@START_DATE", (object)startDate ?? DBNull.Value);
                parameters[4] = new SqlParameter("@END_DATE", (object)endDate ?? DBNull.Value);
                parameters[5] = new SqlParameter("@REQUESTER_ID", requesterID);

                var result = context.Database.SqlQuery<Dto.DTO_PROCESS_PROECESSING_LIST>(ApprovalContext.USP_SELECT_APPROVAL_PROECESSING_LIST, parameters);

                return result.ToList();
            }
        }
        #endregion

        #region SelectApprovalCompletedList
        public List<Dto.DTO_PROCESS_COMPLETED_LIST> SelectApprovalCompletedList(string searchType, string searchText, string chkDateUseYN, DateTime? startDate, DateTime? endDate, string userid)
        {

            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@SEARCH_TYPE", searchType);
                parameters[1] = new SqlParameter("@SEARCH_TEXT", searchText);
                parameters[2] = new SqlParameter("@DATE_USEYN", chkDateUseYN);
                parameters[3] = new SqlParameter("@START_DATE", (object)startDate ?? DBNull.Value);
                parameters[4] = new SqlParameter("@END_DATE", (object)endDate ?? DBNull.Value);
                parameters[5] = new SqlParameter("@USER_ID", userid);

                var result = context.Database.SqlQuery<Dto.DTO_PROCESS_COMPLETED_LIST>(ApprovalContext.USP_SELECT_APPROVAL_COMPLETED_LIST, parameters);

                return result.ToList();
            }
        }
        #endregion

        #region SelectCollateralCompletedList
        public List<Dto.DTO_PROCESS_COMPLETED_COLLATERAL_LIST> SelectCollateralCompletedList(string bg, string type, string keyword, string userid)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[4];                
                parameters[0] = new SqlParameter("@BG", bg);
                parameters[1] = new SqlParameter("@TYPE", type);
                parameters[2] = new SqlParameter("@KEYWORD", keyword);
                parameters[3] = new SqlParameter("@USER_ID", userid);

                var result = context.Database.SqlQuery<Dto.DTO_PROCESS_COMPLETED_COLLATERAL_LIST>(ApprovalContext.USP_SELECT_APPROVAL_COMPLETED_COLLATERAL_LIST, parameters);

                return result.ToList();
            }
        }

        public List<string> SelectCollateralBg()
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    var result = context.Database.SqlQuery<string>(ApprovalContext.USP_SELECT_COLLATERAL_BG);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region SelectSendMailTargetList
        public List<Dto.DTO_SENDMAIL_TO_ADDRESS_LIST> SelectSendMailTargetList(string processid, string mailsendType)
        {

            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@PROCESS_ID", processid);
                parameters[1] = new SqlParameter("@MAILSENDTYPE", mailsendType);


                var result = context.Database.SqlQuery<Dto.DTO_SENDMAIL_TO_ADDRESS_LIST>(ApprovalContext.USP_SELECT_SENDMAIL_TO_ADDRESS_LIST, parameters);

                return result.ToList();
            }
        }
        #endregion

        #region SelectSendMailRetrunGoodList
         public List<Dto.DTO_SENDMAIL_TO_ADDRESS_LIST> SelectSendMailRetrunGoodList(string searchdata)
        {

            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@USER_ID", searchdata);
                var result = context.Database.SqlQuery<Dto.DTO_SENDMAIL_TO_ADDRESS_LIST>(ApprovalContext.USP_SELECT_RETURN_GOODS_MAILADDRESS, parameters);

                return result.ToList();
            }
        }
        #endregion

        #region UpdateApproverSentMail
        public void UpdateApproverSentMail(string processid, string approver, string split)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processid);
                    parameters[1] = new SqlParameter("@APPROVER_ID", approver);
                    parameters[2] = new SqlParameter("@SPLIT", split);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_UPDATE_PROCESS_APPROVER_SENT_MAIL, parameters);
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
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[4];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@STATUS", status);
                    parameters[2] = new SqlParameter("@APPROVER_ID", approver);
                    parameters[3] = new SqlParameter("@SPLIT", split);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_UPDATE_PROCESS_APPROVER_SENT_MAIL_COMMENT, parameters);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region SelectApprovalRejectList
        public List<Dto.DTO_PROCESS_PROECESSING_LIST> SelectApprovalRejectList(string searchType, string searchText, string chkDateUseYN, DateTime? startDate, DateTime? endDate, string requesterID)
        {

            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@SEARCH_TYPE", searchType);
                parameters[1] = new SqlParameter("@SEARCH_TEXT", searchText);
                parameters[2] = new SqlParameter("@DATE_USEYN", chkDateUseYN);
                parameters[3] = new SqlParameter("@START_DATE", (object)startDate ?? DBNull.Value);
                parameters[4] = new SqlParameter("@END_DATE", (object)endDate ?? DBNull.Value);
                parameters[5] = new SqlParameter("@REQUESTER_ID", requesterID);

                var result = context.Database.SqlQuery<Dto.DTO_PROCESS_PROECESSING_LIST>(ApprovalContext.USP_SELECT_APPROVAL_REJECT_LIST, parameters);

                return result.ToList();
            }
        }
        #endregion

        #region SelectApprovalWithDrawList
        public List<Dto.DTO_PROCESS_PROECESSING_LIST> SelectApprovalWithDrawList(string searchType, string searchText, string chkDateUseYN, DateTime? startDate, DateTime? endDate, string requesterID)
        {

            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@SEARCH_TYPE", searchType);
                parameters[1] = new SqlParameter("@SEARCH_TEXT", searchText);
                parameters[2] = new SqlParameter("@DATE_USEYN", chkDateUseYN);
                parameters[3] = new SqlParameter("@START_DATE", (object)startDate ?? DBNull.Value);
                parameters[4] = new SqlParameter("@END_DATE", (object)endDate ?? DBNull.Value);
                parameters[5] = new SqlParameter("@REQUESTER_ID", requesterID);

                var result = context.Database.SqlQuery<Dto.DTO_PROCESS_PROECESSING_LIST>(ApprovalContext.USP_SELECT_APPROVAL_WITHDRAW_LIST, parameters);

                return result.ToList();
            }
        }
        #endregion


        #region GetNoticeMailList
        public List<Dto.DTO_SENDMAIL_TO_ADDRESS_LIST> GetNoticeMailList(DateTime basicdate)
        {

            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@BASICDATE", basicdate);
                var result = context.Database.SqlQuery<Dto.DTO_SENDMAIL_TO_ADDRESS_LIST>(ApprovalContext.USP_SELECT_SEND_NOTICEMAIL_LIST, parameters);

                return result.ToList();
            }
        }
        #endregion

        #region IsCompletedApproval
        public string IsCompletedApproval(string processid)
        {
            string retValue = string.Empty;
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processid);

                retValue = context.Database.SqlQuery<string>(ApprovalContext.USP_SELECT_APPROVAL_IS_COMPLETED, parameters).First();

                return retValue;
            }
        }
        #endregion

        #region GetApprovalCount
        public int GetApprovalCount(string userid, string viewCntType)
        {
            int retValue = 0;
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@REQUESTER_ID", userid);
                parameters[1] = new SqlParameter("@CNT_TYPE", viewCntType);

                retValue = context.Database.SqlQuery<Int32>(ApprovalContext.USP_SELECT_APPROVAL_CNT, parameters).First();

                return retValue;
            }
        }
        #endregion

        #region [ GetProcessDocumentStatus ]

        public string SelectProcessDocumentStatus(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<string>(ApprovalContext.USP_SELECT_PROCESS_DOCUMENT_STATUS, parameters);

                    return result.First();
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region [ InsertProcessApproverCompleted ]

        public void InsertProcessApproverCompleted(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_PROCESS_APPROVER_COMPLETED, parameters);
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
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.RejectDocumentMailCommentDto>(ApprovalContext.USP_SELECT_PROCESS_REJECT_USER, parameters);

                    return result.First();
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
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID_LIST", processIds);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_DOCUMENT_PROCESS, parameters);
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
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<string>(ApprovalContext.USP_SELECT_INPUT_COMMENT, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        } 
        #endregion

        #region [ Process Auth User Check ]
        public string IsApprovalProcessUser(string processId, string userid)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@USER_ID", userid);

                    var result = context.Database.SqlQuery<string>(ApprovalContext.USP_SELECT_DOCUMENT_AUTH_USER, parameters);

                    return result.First();
                }
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
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@SEARCH_TYPE", searchType);
                parameters[1] = new SqlParameter("@SEARCH_TEXT", searchText);
                parameters[2] = new SqlParameter("@DATE_USEYN", chkDateUseYN);
                parameters[3] = new SqlParameter("@START_DATE", (object)startDate ?? DBNull.Value);
                parameters[4] = new SqlParameter("@END_DATE", (object)endDate ?? DBNull.Value);

                var result = context.Database.SqlQuery<Dto.DTO_ADMIN_DOCUMENT_LIST>(ApprovalContext.USP_SELECT_APPROVAL_LIST, parameters);
                return result.ToList();
            }
        }

        public List<Dto.DTO_ADMIN_DOCUMENT_LIST> SelectAdminViewDocumentList(string searchType, string searchText, string chkDateUseYN, DateTime? startDate, DateTime? endDate, string userId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@SEARCH_TYPE", searchType);
                parameters[1] = new SqlParameter("@SEARCH_TEXT", searchText);
                parameters[2] = new SqlParameter("@DATE_USEYN", chkDateUseYN);
                parameters[3] = new SqlParameter("@START_DATE", (object)startDate ?? DBNull.Value);
                parameters[4] = new SqlParameter("@END_DATE", (object)endDate ?? DBNull.Value);
                parameters[5] = new SqlParameter("@USER_ID", userId);

                var result = context.Database.SqlQuery<Dto.DTO_ADMIN_DOCUMENT_LIST>(ApprovalContext.USP_SELECT_APPROVAL_ADMIN_VIEW_LIST, parameters);
                return result.ToList();
            }
        }
            
        #endregion

        #region SelectWithHolding
        public List<Dto.DTO_WITHHOLDING_LIST> SelectWithHolding(string searchText, DateTime? startDate, DateTime? endDate)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@SEARCH_TEXT", searchText);
                parameters[1] = new SqlParameter("@START_DATE", (object)startDate ?? DBNull.Value);
                parameters[2] = new SqlParameter("@END_DATE", (object)endDate ?? DBNull.Value);

                var result = context.Database.SqlQuery<Dto.DTO_WITHHOLDING_LIST>(ApprovalContext.USP_SELECT_WITHHOLDING, parameters);
                return result.ToList();
            }
        }
        #endregion
        #region SelectAdminReportingDocumentList
        public List<Dto.DTO_ADMIN_REPORTING_DOCUMENT_LIST> SelectAdminReportingDocumentList(string DocumentID)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@DOCUMNET_ID", DocumentID);
              

                var result = context.Database.SqlQuery<Dto.DTO_ADMIN_REPORTING_DOCUMENT_LIST>(ApprovalContext.USP_REPORTING_DOCUMENT, parameters);
                return result.ToList();
            }
        }
        public List<Dto.DTO_ADMIN_REPORTING_DOCUMENT_LIST> SelectAdminReportingDocumentList(string DocumentID,DateTime? FROM_DATE,DateTime? TO_DATE)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@DOCUMNET_ID", DocumentID);
                parameters[1] = new SqlParameter("@FROM_DATE", FROM_DATE);
                parameters[2] = new SqlParameter("@TO_DATE", TO_DATE);



                var result = context.Database.SqlQuery<Dto.DTO_ADMIN_REPORTING_DOCUMENT_LIST>(ApprovalContext.USP_REPORTING_DOCUMENT, parameters);
                return result.ToList();
            }
        }

        #endregion

    }
}
