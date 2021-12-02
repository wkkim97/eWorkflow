using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    #region DTO_APPROVAL
    public class DTO_APPROVAL
    {
        public Int64? IDX { get; set; }
        public string APPROVAL_TYPE { get; set; }
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string ABSENCE_USER_ID { get; set; }
        public string ABSENCE_USER_NAME { get; set; }
        public string SUPERVISOR_USER_ID { get; set; }
        public int APPROVAL_LEVEL { get; set; }
        public int APPROVAL_SEQ { get; set; }
        public string APPROVER_TYPE { get; set; }
        public string ORG_NAME { get; set; }
        public string ABSENCE_ORG_NAME { get; set; }
        public string JOB_TITLE { get; set; }
        public int? JOB_TITLE_NUM { get; set; }
        public decimal? LIMIT_AMOUNT { get; set; }
        public string PHONE { get; set; }
        public string MAIL_ADDRESS { get; set; }
    } 
    #endregion

    #region DTO_PROCESS_APPROVER
    public class DTO_PROCESS_APPROVER
    {
        /// <summary>
        /// 
        /// </summary> 
        public string PROCESS_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string APPROVAL_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int APPROVAL_SEQ { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string APPROVER_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string APPROVER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string APPROVER_ORG_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ABSENCE_APPROVER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ABSENCE_APPROVER_ORG_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string STATUS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SENT_MAIL { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? APPROVAL_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMMENT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime CREATE_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? UPDATE_DATE { get; set; }


    } 
    #endregion

    #region DTO_PROCESS_APPROVER_COMPLETED
    public class DTO_PROCESS_APPROVER_COMPLETED
    {
        /// <summary>
        /// 
        /// </summary> 
        public string PROCESS_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string APPROVAL_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int APPROVAL_SEQ { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string APPROVER_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string APPROVER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string APPROVER_ORG_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ABSENCE_APPROVER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ABSENCE_APPROVER_ORG_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string STATUS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SENT_MAIL { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? APPROVAL_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMMENT { get; set; }


    } 
    #endregion

    #region DTO_PROCESS_DOCUMENT
    public class DTO_PROCESS_DOCUMENT
    {
        /// <summary>
        /// 
        /// </summary> 
        public string PROCESS_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DOCUMENT_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DOC_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SUBJECT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DOC_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PROCESS_STATUS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime REQUEST_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMPANY_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string REQUESTER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CURRENT_APPROVER { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string FINAL_APPROVER { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string REJECTED_PROCESS_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime CREATE_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? UPDATE_DATE { get; set; }


    } 
    #endregion
 
    #region DTO_PROCESS_TODO_LIST
    public class DTO_PROCESS_TODO_LIST
    {
        public string PROCESS_ID { get; set; }
        public string DOCUMENT_ID { get; set; }
        public string DOC_NAME { get; set; }
        public string SUBJECT { get; set; }
        public string DOC_NUM { get; set; }
        public string PROCESS_STATUS { get; set; }
        public string REQUESTER_ID { get; set; }
        public string REQUESTER { get; set; }
        public string CURRENT_APPROVER { get; set; }
        public string NEXT_APPROVER_ID { get; set; }
        public string NEXT_APPROVER_NAME { get; set; }
        public DateTime REQUEST_DATE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string COMMENT { get; set; }
        public string ATTACHFILEYN { get; set; }
        public string FORM_NAME { get; set; }
    } 
    #endregion

    #region DTO_PROCESS_PROECESSING_LIST
    [Serializable]
    public class DTO_PROCESS_PROECESSING_LIST
    {
        public string PROCESS_ID { get; set; }
        public string DOCUMENT_ID { get; set; }
        public string DOC_NAME { get; set; }
        public string SUBJECT { get; set; }
        public string DOC_NUM { get; set; }
        public string PROCESS_STATUS { get; set; }
        public string REQUESTER_ID { get; set; }
        public string REQUESTER { get; set; }
        public string CURRENT_APPROVER { get; set; } 
        public string CURRENT_APPROVER_NAME { get; set; }
        public DateTime REQUEST_DATE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string COMMENT { get; set; }
        public string ATTACHFILEYN { get; set; }
        public string FORM_NAME { get; set; }
    }
    #endregion

    #region DTO_PROCESS_WITHDRAW_LIST
    [Serializable]
    public class DTO_PROCESS_WITHDRAW_LIST
    {
        public string PROCESS_ID { get; set; }
        public string DOCUMENT_ID { get; set; }
        public string DOC_NAME { get; set; }
        public string SUBJECT { get; set; }
        public string DOC_NUM { get; set; }
        public string PROCESS_STATUS { get; set; }
        public string REQUESTER_ID { get; set; }
        public string REQUESTER { get; set; }
        public string WITHDRAW_ID { get; set; }
        public string WITHDRAW_NAME { get; set; }
        public DateTime REQUEST_DATE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string COMMENT { get; set; }
        public string ATTACHFILEYN { get; set; }
        public string FORM_NAME { get; set; }
    }
    #endregion

    #region DTO_PROCESS_COMPLETED_LIST
    [Serializable]
    public class DTO_PROCESS_COMPLETED_LIST
    {
        public string PROCESS_ID { get; set; }
        public string DOCUMENT_ID { get; set; }
        public string DOC_NAME { get; set; }
        public string SUBJECT { get; set; }
        public string DOC_NUM { get; set; }
        public string PROCESS_STATUS { get; set; }
        public string REQUESTER_ID { get; set; }
        public string REQUESTER { get; set; }
        public string FINAL_APPROVER { get; set; }
        public string FINAL_APPROVER_NAME { get; set; }
        public DateTime REQUEST_DATE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string COMMENT { get; set; }
        public string ATTACHFILEYN { get; set; }
        public string FORM_NAME { get; set; }
    }
    #endregion

    #region DTO_PROCESS_COMPLEDTED_COLLATERAL_LIST
    public class DTO_PROCESS_COMPLETED_COLLATERAL_LIST
    {
        public string PROCESS_ID { get; set; }
        public string DOCUMENT_ID { get; set; }
        public string FORM_NAME { get; set; }
        public string CUSTOMER_CODE { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public decimal? CREDIT_LIMIT { get; set; }
        public string BG_CODE { get; set; }
        public string STATUS_CODE { get; set; }
        public string MORTGAGE_TYPE { get; set; }
        public decimal? BOOK_VALUE { get; set; }
        public decimal? REVALUTION_VALUE { get; set; }
        public DateTime? RECEIVED_DATE { get; set; }
        public DateTime? DUE_DATE { get; set; }
        public DateTime? ISSUE_DATE { get; set; }
        public DateTime? RETURN_DATE { get; set; }
        public string DOC_NUM { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string PUBLISHER { get; set; }
        public string PUBLISHED_NUM { get; set; }
    }
    #endregion

    #region DTO_PROCESS_APPROVAL_USER_STATUS
    public class DTO_PROCESS_APPROVAL_USER_STATUS
    {
        public string PROCESS_ID { get; set; }
        public string DOCUMENT_ID { get; set; }
        public string PROCESS_STATUS { get; set; }
        public string APPROVAL_TYPE { get; set; }
        public string APPROVER_STATUS { get; set; }
    } 
    #endregion

    #region DTO_PROCESS_APPROVAL_LIST
    public class DTO_PROCESS_APPROVAL_LIST
    { 
        public string PROCESS_ID { get; set; }
        public Int64 IDX { get; set; }
        public string APPROVAL_TYPE { get; set; }
        public string APPROVER_ID { get; set; }
        public string APPROVER { get; set; }
        public string MAIL_ADDRESS { get; set; }
        public string COMPANY_NAME { get; set; }
        public string APPROVER_ORG_NAME { get; set; }
        public string STATUS { get; set; }
        public string APPROVER_STATUS { get; set; }
        public DateTime? APPROVAL_DATE { get; set; }
        public string COMMENT { get; set; }
        public DateTime? UPDATE_DATE { get; set; }
    }
    #endregion

    #region DTO_DOC_LOG_LIST
    public class DTO_DOC_LOG_LIST
    { 
        public string REGISTER_ID { get; set; }
        public string REGISTER { get; set; }
        public string LOG_TYPE { get; set; }
        public string COMMENT { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public int FILE_IDX { get; set; } 
    }
    #endregion

    #region DTO_DOC_LOG
    public class DTO_DOC_LOG
    {
        /// <summary>
        /// 
        /// </summary> 
        public int IDX { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PROCESS_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string REGISTER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string LOG_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMMENT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime CREATE_DATE { get; set; }


    } 
    #endregion

    #region DTO_SENDMAIL_TO_ADDRESS_LIST
    public class DTO_SENDMAIL_TO_ADDRESS_LIST
    {
        public string PROCESS_ID { get; set; }
        public string DOCUMENT_ID { get; set; }
        public string DOC_NAME { get; set; }
        public string APPROVER_ID { get; set; }
        public string APPROVER_NAME { get; set; }
        public string MAIL_ADDRESS { get; set; }
        public string FORM_NAME { get; set; }
        public string COMMENT { get; set; }
        public string APPROVAL_TYPE { get; set; }
        public string REJECTED_PROCESS_ID { get; set; }
        public string SENDER_ID { get; set; }
        public string SENDER_NAME { get; set; }
        public string SENDER_MAIL_ADDRESS { get; set; }
        public string REQUEST_DATE { get; set; }
        
    }
    #endregion

    #region DTO_ADMIN_DOCUMENT_LIST
    [Serializable]
    public class DTO_ADMIN_DOCUMENT_LIST
    {
        public string PROCESS_ID { get; set; }
        public string DOCUMENT_ID { get; set; }
        public string DOC_NAME { get; set; }
        public string SUBJECT { get; set; }
        public string DOC_NUM { get; set; }
        public string PROCESS_STATUS { get; set; }
        public string REQUESTER_ID { get; set; }
        public string REQUESTER { get; set; }
        public string CURRENT_APPROVER { get; set; }
        public string CURRENT_APPROVER_NAME { get; set; }
        public string FINAL_APPROVER { get; set; }
        public string FINAL_APPROVER_NAME { get; set; }
        public DateTime REQUEST_DATE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string COMMENT { get; set; }
        public string FORM_NAME { get; set; }
    }
    #endregion  
    #region DTO_WITHHOLDING_LIST
    [Serializable]
    public class DTO_WITHHOLDING_LIST
    {
        public DateTime? TRANS_DATE { get; set; }
        public string ID_NUM { get; set; }
        public string NAME { get; set; }
        public decimal? AMOUNT { get; set; }
        public long IDX { get; set; }
        public string VOUNO { get; set; }
    }
    #endregion  

    #region DTO_ADMIN_DOCUMENT_LIST
    [Serializable]
    public class DTO_ADMIN_REPORTING_DOCUMENT_LIST
    {
        public string DOCUMENT_ID                        { get; set; }
        public string PROCESS_ID                         { get; set; }
        public DateTime? REQUEST_DATE                    { get; set; }
        public string REQUEST_ID                         { get; set; }
        public string REQUESTER                          { get; set; }
        public string COMPANY                            { get; set; }
        public string ORGANIZATION                       { get; set; }
        public string RETENTION_PERIOD                   { get; set; }
        public string DOCUMENT_NO                        { get; set; }
        public string SUBJECT                            { get; set; }
        public string BG                                 { get; set; }
        public string ACTIVITY_TYPE                      { get; set; }
        public string INCENTIVE_AGREEMENT                { get; set; }
        public string INCENTIVE_CHECKLIST                { get; set; }
        public string COST_CENTER_AND_APPROVAL_BUDGET    { get; set; }
        public string MEETING_VENUE                      { get; set; }
        public DateTime? FROM_DATE                       { get; set; }
        public DateTime? TO_DATE                         { get; set; }
        public string ADDRESS_VENUE                      { get; set; }
        public string PURPOSE_OBJECTIVE                  { get; set; }
        public string APPROVED_E_WORKFLOW_NUMBER         { get; set; }
        public int    GOVERNMENTAL_OFFICER               { get; set; }
        public int    NON_GOVERNMENTAL_OFFICER           { get; set; }
        public int    FARMER                             { get; set; }
        public int    TOTAL_PARTICIPANTS                 { get; set; }
        public string COST_DETAIL                        { get; set; }
        public decimal TOTAL_AMOUNT                      { get; set; }
        public string LAST_APPROVER                      { get; set; }
        public string LAST_APPROVER_DATE                 { get; set; }
        public string LOG                                { get; set; }
    }
    #endregion  
}
