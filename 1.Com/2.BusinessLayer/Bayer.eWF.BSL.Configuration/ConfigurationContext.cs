using DNSoft.eWF.FrameWork.Data.EF;
using Bayer.eWF.BSL.Configuration.Dao;
using Bayer.eWF.BSL.Configuration.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Configuration
{
    public class ConfigurationContext : WorkflowContext
    {
        /// <summary>
        /// readers 그룹 코드를 생성해 가져온다.
        /// </summary>
        public const string USP_CREATE_READERS_GROUP_CODE = "[dbo].[USP_CREATE_READERS_GROUP_CODE]";
        /// <summary>
        /// readers 그룹 생성, 수정
        /// </summary>
        public const string USP_MERGE_READERS_GROUP = "[dbo].[USP_MERGE_READERS_GROUP] @GROUP_CODE, @GROUP_NAME, @CREATOR_ID";

        public const string USP_INSERT_READERS_GROUP_USER = "[dbo].[USP_INSERT_READERS_GROUP_USER] @GROUP_CODE, @USER_ID, @CREATOR_ID";
        /// <summary>
        /// readers 그룹 삭제
        /// </summary>
        public const string USP_DELETE_READERS_GROUP = "[dbo].[USP_DELETE_READERS_GROUP] @GROUP_CODE";

        public const string USP_DELETE_READERS_GROUP_USER = "[dbo].[USP_DELETE_READERS_GROUP_USER] @GROUP_CODE, @USER_ID ";
        /// <summary>
        /// readers 그룹 조회
        /// </summary>
        public const string USP_SELECT_READERS_GROUP = "[dbo].[USP_SELECT_READERS_GROUP] @GROUP_CODE, @GROUP_NAME";

        public const string USP_SELECT_READERS_GROUP_D = "[dbo].[USP_SELECT_READERS_GROUP_D]";

        public const string USP_SELECT_READERS_GROUP_LIST = "[dbo].[USP_SELECT_READERS_GROUP_LIST]";
        /// <summary>
        /// readers Group에 넣기위한 유저를 가져온다.
        /// </summary>
        public const string USP_SELECT_READERS_GROUP_USER = "[dbo].[USP_SELECT_READERS_GROUP_USER] @FULL_NAME";

        /// <summary>
        /// 문서별 서비스 네임 가져오기
        /// </summary>
        public const string USP_SELECT_CONFIG_AFTER_TREATEMENT_SERVICE = "[dbo].[USP_SELECT_CONFIG_AFTER_TREATEMENT_SERVICE] @DOCUMENT_ID";

        /// <summary>
        /// 
        /// </summary>
        public const string USP_SELECT_READER_GROUP_COUNT = "[dbo].[USP_SELECT_READER_GROUP_COUNT] @USER_ID";



        public const string USP_SELECT_FACEBOOK = "[eManage].[dbo].[USP_SELECT_FACEBOOK] @KEYWOARD";

        public const string USP_SELECT_WITHHOLDING = "[eManage].[dbo].[USP_SELECT_WITHHOLDING] @KEYWOARD";

        #region [ Configuration ]

        /// <summary>
        /// Table Column목록을 가져온다.
        /// </summary>
        public const string USP_SELECT_TABLE_COLUMN_INFO = "[dbo].[USP_SELECT_TABLE_COLUMN_INFO] @TABLE_NAME";

        /// <summary>
        /// 결재문서 ID를 생성해 가져온다.
        /// </summary>
        public const string USP_CREATE_DOCUMENT_ID = "[dbo].[USP_CREATE_DOCUMENT_ID]";

        /// <summary>
        /// 결재문서 설정정보를 저장한다.
        /// </summary>
        public const string USP_MERGE_CONFIG = "[dbo].[USP_MERGE_CONFIG] @DOCUMENT_ID, @TABLE_NAME, @DOC_NAME, @DATA_OWNER, @PREFIX_DOC_NUM"
                                        + ", @FORM_NAME, @READERS_GROUP_CODE, @CATEGORY_CODE, @RETENTION_PERIOD, @FORWARD_YN, @CLASSIFICATION_INFO"
                                        + ", @AFTER_TREATMENT_SERVICE, @APPROVAL_TYPE, @APPROVAL_LEVEL, @JOB_TITLE_CODE, @ADD_ADDTIONAL_APPROVER, @ADD_APPROVER_POSITION"
                                        + ", @ADD_REVIEWER, @ADD_REVIEWER_DESCRIPTION, @DOC_DESCRIPTION, @DOC_IMAGE_NAME, @DOC_IMAGE_PATH"
                                        + ", @DISPLAY_DOC_LIST, @CREATOR_ID";

        /// <summary>
        /// 결재문서 설정정보 전체를 삭제한다.
        /// </summary>
        public const string USP_DELETE_CONFIG = "[dbo].[USP_DELETE_CONFIG] @DOCUMENT_ID";

        /// <summary>
        /// 결재문서 해당 Company를 등록한다.
        /// </summary>
        public const string USP_INSERT_CONFIG_COMPANY = "[dbo].[USP_INSERT_CONFIG_COMPANY] @DOCUMENT_ID, @COMPANY_CODE, @CREATOR_ID";

        /// <summary>
        /// 결재문서 해당 Company를 삭제한다. DocumentId기준으로 전체삭제
        /// </summary>
        public const string USP_DELETE_CONFIG_COMPANY = "[dbo].[USP_DELETE_CONFIG_COMPANY] @DOCUMENT_ID";

        /// <summary>
        /// 결재문서 Additional Approver정보를 저장한다.
        /// </summary>
        public const string USP_MERGE_CONFIG_APPROVER = "[dbo].[USP_MERGE_CONFIG_APPROVER] @DOCUMENT_ID, @APPROVAL_LOCATION, @CONDITION_INDEX, @IS_MANDATORY, @APPROVER_ID, @DISPLAY_CONDITION, @SQL_CONDITION, @CREATOR_ID";

        /// <summary>
        /// 결재문서 Additional Approver정보를 삭제한다. Condition정보도 같이 삭제한다.
        /// </summary>
        public const string USP_DELETE_CONFIG_APPROVER = "[dbo].[USP_DELETE_CONFIG_APPROVER] @DOCUMENT_ID, @APPROVAL_LOCATION, @CONDITION_INDEX";

        /// <summary>
        /// 결재문서 Additional Approver의 조건정보를 삭제한다. Grid의 Row삭제!
        /// </summary>
        public const string USP_DELETE_CONFIG_APPROVER_CONDITION = "[dbo].[USP_DELETE_CONFIG_APPROVER_CONDITION] @DOCUMENT_ID, @APPROVAL_LOCATION, @CONDITION_INDEX, @CONDITION_SEQ";

        /// <summary>
        /// 결재문서 Additional Approver의 조건정보를 전체 삭제한다. 등록시 삭제후 저장을 위해
        /// </summary>
        public const string USP_DELETE_CONFIG_APPROVER_CONDITION_ALL = "[dbo].[USP_DELETE_CONFIG_APPROVER_CONDITION_ALL] @DOCUMENT_ID, @APPROVAL_LOCATION, @CONDITION_INDEX";

        /// <summary>
        /// 결재문서 Additional Approver의 조건을 저장한다.
        /// </summary>
        public const string USP_INSERT_CONFIG_APPROVER_CONDITION = "[dbo].[USP_INSERT_CONFIG_APPROVER_CONDITION] @DOCUMENT_ID, @APPROVAL_LOCATION, @CONDITION_INDEX, @CONDITION_SEQ, @FIELD_NAME, @CONDITION, @VALUE, @OPTION, @CREATOR_ID";

        /// <summary>
        /// 결재문서 Additional Recipient정보를 저장한다.
        /// </summary>
        public const string USP_MERGE_CONFIG_RECIPIENT = "[dbo].[USP_MERGE_CONFIG_RECIPIENT] @DOCUMENT_ID, @CONDITION_INDEX, @IS_MANDATORY, @RECIPIENT_ID, @DISPLAY_CONDITION, @SQL_CONDITION, @CREATOR_ID";

        /// <summary>
        /// 결재문서 Additional Recipient정보를 삭제한다. Condition정보도 같이 삭제한다.
        /// </summary>
        public const string USP_DELETE_CONFIG_RECIPIENT = "[dbo].[USP_DELETE_CONFIG_RECIPIENT] @DOCUMENT_ID, @CONDITION_INDEX";

        /// <summary>
        /// 결재문서 Additional Recipient의 조건정보를 삭제한다. Grid의 Row삭제!
        /// </summary>
        public const string USP_DELETE_CONFIG_RECIPIENT_CONDITION = "[dbo].[USP_DELETE_CONFIG_RECIPIENT_CONDITION] @DOCUMENT_ID,@CONDITION_INDEX, @CONDITION_SEQ";

        /// <summary>
        /// 결재문서 Additional Recipient의 조건정보를 전체 삭제한다. 등록시 삭제후 저장을 위해
        /// </summary>
        public const string USP_DELETE_CONFIG_RECIPIENT_CONDITION_ALL = "[dbo].[USP_DELETE_CONFIG_RECIPIENT_CONDITION_ALL] @DOCUMENT_ID, @CONDITION_INDEX";

        /// <summary>
        /// 결재문서 Additional Recipient의 조건을 저장한다.
        /// </summary>
        public const string USP_INSERT_CONFIG_RECIPIENT_CONDITION = "[dbo].[USP_INSERT_CONFIG_RECIPIENT_CONDITION] @DOCUMENT_ID, @CONDITION_INDEX, @CONDITION_SEQ, @FIELD_NAME, @CONDITION, @VALUE, @OPTION, @CREATOR_ID";

        /// <summary>
        /// 결재문서 Additional Reviewer정보를 저장한다.
        /// </summary>
        public const string USP_MERGE_CONFIG_REVIEWER = "[dbo].[USP_MERGE_CONFIG_REVIEWER] @DOCUMENT_ID, @CONDITION_INDEX, @IS_MANDATORY, @REVIEWER_ID, @DISPLAY_CONDITION, @SQL_CONDITION, @CREATOR_ID";

        /// <summary>
        /// 결재문서 Additional Reviewer정보를 삭제한다. Condition정보도 같이 삭제한다.
        /// </summary>
        public const string USP_DELETE_CONFIG_REVIEWER = "[dbo].[USP_DELETE_CONFIG_REVIEWER] @DOCUMENT_ID, @CONDITION_INDEX";

        /// <summary>
        /// 결재문서 Additional Reviewer의 조건정보를 삭제한다. Grid의 Row삭제!
        /// </summary>
        public const string USP_DELETE_CONFIG_REVIEWER_CONDITION = "[dbo].[USP_DELETE_CONFIG_REVIEWER_CONDITION] @DOCUMENT_ID, @CONDITION_INDEX, @CONDITION_SEQ";

        /// <summary>
        /// 결재문서 Additional Recipient의 조건정보를 전체 삭제한다. 등록시 삭제후 저장을 위해
        /// </summary>
        public const string USP_DELETE_CONFIG_REVIEWER_CONDITION_ALL = "[dbo].[USP_DELETE_CONFIG_REVIEWER_CONDITION_ALL] @DOCUMENT_ID, @CONDITION_INDEX";

        /// <summary>
        /// 결재문서 Additional Reviewer의 조건을 저장한다.
        /// </summary>
        public const string USP_INSERT_CONFIG_REVIEWER_CONDITION = "[dbo].[USP_INSERT_CONFIG_REVIEWER_CONDITION] @DOCUMENT_ID, @CONDITION_INDEX, @CONDITION_SEQ, @FIELD_NAME, @CONDITION, @VALUE, @OPTION, @CREATOR_ID";

        /// <summary>
        /// 결재문서 목록을 가져온다.
        /// </summary>
        public const string USP_SELECT_CONFIG_LIST = "[dbo].[USP_SELECT_CONFIG_LIST]";

        /// <summary>
        /// 결재문서 설정정보를 조회한다.
        /// </summary>
        public const string USP_SELECT_CONFIG = "[dbo].[USP_SELECT_CONFIG]  @DOCUMENT_ID";

        /// <summary>
        /// 결재문서 Additional Approver정보를 조회한다.
        /// </summary>
        public const string USP_SELECT_CONFIG_APPROVER = "[dbo].[USP_SELECT_CONFIG_APPROVER] @DOCUMENT_ID, @APPROVAL_LOCATION";

        /// <summary>
        /// 결재문서 Additional Approver의 Condition을 조회한다.
        /// </summary>
        public const string USP_SELECT_CONFIG_APPROVER_CONDITION = "[dbo].[USP_SELECT_CONFIG_APPROVER_CONDITION] @DOCUMENT_ID, @APPROVAL_LOCATION, @CONDITION_INDEX";

        /// <summary>
        /// 결재문서 Additional Recipient정보를 조회한다.
        /// </summary>
        public const string USP_SELECT_CONFIG_RECIPIENT = "[dbo].[USP_SELECT_CONFIG_RECIPIENT] @DOCUMENT_ID ";

        /// <summary>
        /// 결재문서 Additional Recipient의 Condition을 조회한다.
        /// </summary>
        public const string USP_SELECT_CONFIG_RECIPIENT_CONDITION = "[dbo].[USP_SELECT_CONFIG_RECIPIENT_CONDITION] @DOCUMENT_ID,@CONDITION_INDEX";

        /// <summary>
        /// 결재문서 Additional Reviewer정보를 조회한다.
        /// </summary>
        public const string USP_SELECT_CONFIG_REVIEWER = "[dbo].[USP_SELECT_CONFIG_REVIEWER] @DOCUMENT_ID ";

        /// <summary>
        /// 결재문서 Additional Reviewer의 Condition을 조회한다.
        /// </summary>
        public const string USP_SELECT_CONFIG_REVIEWER_CONDITION = "[dbo].[USP_SELECT_CONFIG_REVIEWER_CONDITION] @DOCUMENT_ID,@CONDITION_INDEX";

        /// <summary>
        /// 결재문서 회사를 가져온다.
        /// </summary>
        public const string USP_SELECT_CONFIG_COMPANY = "[dbo].[USP_SELECT_CONFIG_COMPANY] @DOCUMENT_ID";

        /// <summary>
        /// Configuration등록시 테이블 존재여부 확인
        /// </summary>
        public const string USP_SELECT_EXISTS_TABLE = "[dbo].[USP_SELECT_EXISTS_TABLE] @TABLE_NAME";

        #endregion
    }
}
