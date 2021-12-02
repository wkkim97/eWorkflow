using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bayer.eWF.BSL.Configuration.Dto;

namespace Bayer.eWF.BSL.Configuration.Dao
{
    public class ConfigurationDao : DaoBase
    {

        #region [ Readers Group ]

        // Group Code 코드를 생성한다.
        public string CreateGroupCode()
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    return context.Database.SqlQuery<string>(ConfigurationContext.USP_CREATE_READERS_GROUP_CODE).First();
                }
            }
            catch
            {
                throw;
            }
        }


        public void MergeReadersGroup(Dto.DTO_READERS_GROUP group)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@GROUP_CODE", group.GROUP_CODE);
                    parameters[1] = new SqlParameter("@GROUP_NAME", group.GROUP_NAME);
                    parameters[2] = new SqlParameter("@CREATOR_ID", group.CREATOR_ID);
                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_MERGE_READERS_GROUP, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertReadersGroupUser(Dto.DTO_READERS_GROUP_USER_LIST user)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@GROUP_CODE", user.GROUP_CODE);
                    parameters[1] = new SqlParameter("@USER_ID", user.USER_ID);
                    parameters[2] = new SqlParameter("@CREATOR_ID", user.CREATOR_ID);
                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_INSERT_READERS_GROUP_USER, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteReadersGroup(string group_code)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@GROUP_CODE", group_code);
                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_DELETE_READERS_GROUP, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteReadersGroupUser(string group_code, string user_id)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@GROUP_CODE", group_code);
                    parameters[1] = new SqlParameter("@USER_ID", user_id);
                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_DELETE_READERS_GROUP_USER, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // READERS GROUP NAME 단순 조회
        public List<Dto.DTO_READERS_GROUP> SelectReadersGroupList()
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    var result = context.Database.SqlQuery<Dto.DTO_READERS_GROUP>(ConfigurationContext.USP_SELECT_READERS_GROUP_LIST);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        // READERS GROUP 에 따른 USER LIST 조회
        public List<Dto.DTO_READERS_GROUP_USER_NAME> SelectReadersGroup(string group_code, string group_name)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@GROUP_CODE", group_code);
                    parameters[1] = new SqlParameter("@GROUP_NAME", group_name);
                    var result = context.Database.SqlQuery<Dto.DTO_READERS_GROUP_USER_NAME>(ConfigurationContext.USP_SELECT_READERS_GROUP, parameters);

                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //  READERS GROUP USER LIST 조회 : full_name이 있을시 search
        public List<Dto.DTO_READERS_GROUP_USER_NAME> SelectReadersGroupUser(string full_name)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@FULL_NAME", full_name);
                    var result = context.Database.SqlQuery<Dto.DTO_READERS_GROUP_USER_NAME>(ConfigurationContext.USP_SELECT_READERS_GROUP_USER, parameters);

                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //public List<Dto.DTO_READERS> SelectReadersGroupUser_D()
        //{
        //    try
        //    {
        //        using (context = new ConfigurationContext())
        //        {
        //            var result = context.Database.SqlQuery<Dto.DTO_READERS>(ConfigurationContext.USP_SELECT_READERS_GROUP_D);

        //            return result.ToList();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        public int SelectReadersGroupCount(string userId)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@USER_ID", userId);

                    var result = context.Database.SqlQuery<int>(ConfigurationContext.USP_SELECT_READER_GROUP_COUNT, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region [ Configuration ]

        public List<DbTableColumnDto> SelectDbTableColumn(string tableName)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@TABLE_NAME", tableName);
                    return context.Database.SqlQuery<DbTableColumnDto>(ConfigurationContext.USP_SELECT_TABLE_COLUMN_INFO, parameters).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Document Id를 생성해 가져온다.
        /// </summary>
        /// <returns></returns>
        public string CreateDocumentId()
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    return context.Database.SqlQuery<string>(ConfigurationContext.USP_CREATE_DOCUMENT_ID).First();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 설정정보를 저장한다.
        /// </summary>
        /// <param name="config"></param>
        public void MergeConfiguration(Dto.DTO_CONFIG config)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[24];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", config.DOCUMENT_ID);
                    parameters[1] = new SqlParameter("@TABLE_NAME", config.TABLE_NAME);
                    parameters[2] = new SqlParameter("@DOC_NAME", config.DOC_NAME);
                    parameters[3] = new SqlParameter("@DATA_OWNER", config.DATA_OWNER);
                    parameters[4] = new SqlParameter("@PREFIX_DOC_NUM", config.PREFIX_DOC_NUM);
                    parameters[5] = new SqlParameter("@FORM_NAME", config.FORM_NAME);
                    parameters[6] = new SqlParameter("@READERS_GROUP_CODE", (object)config.READERS_GROUP_CODE ?? DBNull.Value);
                    parameters[7] = new SqlParameter("@CATEGORY_CODE", config.CATEGORY_CODE);
                    parameters[8] = new SqlParameter("@RETENTION_PERIOD", config.RETENTION_PERIOD);
                    parameters[9] = new SqlParameter("@FORWARD_YN", config.FORWARD_YN);
                    parameters[10] = new SqlParameter("@CLASSIFICATION_INFO", config.CLASSIFICATION_INFO);
                    parameters[11] = new SqlParameter("@AFTER_TREATMENT_SERVICE", config.AFTER_TREATMENT_SERVICE);
                    parameters[12] = new SqlParameter("@APPROVAL_TYPE", config.APPROVAL_TYPE);
                    parameters[13] = new SqlParameter("@APPROVAL_LEVEL", (object)config.APPROVAL_LEVEL ?? DBNull.Value);
                    parameters[14] = new SqlParameter("@JOB_TITLE_CODE", (object)config.JOB_TITLE_CODE ?? DBNull.Value);
                    parameters[15] = new SqlParameter("@ADD_ADDTIONAL_APPROVER", config.ADD_ADDTIONAL_APPROVER);
                    parameters[16] = new SqlParameter("@ADD_APPROVER_POSITION", (object)config.ADD_APPROVER_POSITION ?? DBNull.Value);
                    parameters[17] = new SqlParameter("@ADD_REVIEWER", config.ADD_REVIEWER);
                    parameters[18] = new SqlParameter("@ADD_REVIEWER_DESCRIPTION", (object)config.ADD_REVIEWER_DESCRIPTION ?? DBNull.Value);
                    parameters[19] = new SqlParameter("@DOC_DESCRIPTION", (object)config.DOC_DESCRIPTION ?? DBNull.Value);
                    parameters[20] = new SqlParameter("@DOC_IMAGE_NAME", (object)config.DOC_IMAGE_NAME ?? DBNull.Value);
                    parameters[21] = new SqlParameter("@DOC_IMAGE_PATH", (object)config.DOC_IMAGE_PATH ?? DBNull.Value);
                    parameters[22] = new SqlParameter("@DISPLAY_DOC_LIST", config.DISPLAY_DOC_LIST);
                    parameters[23] = new SqlParameter("@CREATOR_ID", config.CREATOR_ID);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_MERGE_CONFIG, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 설정정보 전체를 삭제한다.
        /// </summary>
        /// <param name="documentId"></param>
        public void DeleteConfiguration(string documentId)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_DELETE_CONFIG, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서에 해당하는 Company를 저장한다.
        /// </summary>
        /// <param name="company"></param>
        public void InsertConfigurationCompany(Dto.DTO_CONFIG_COMPANY company)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", company.DOCUMENT_ID);
                    parameters[1] = new SqlParameter("@COMPANY_CODE", company.COMPANY_CODE);
                    parameters[2] = new SqlParameter("@CREATOR_ID", company.CREATOR_ID);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_INSERT_CONFIG_COMPANY, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 회사를 삭제한다.
        /// </summary>
        /// <param name="documentId"></param>
        public void DeleteConfigurationCompany(string documentId)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_DELETE_CONFIG_COMPANY, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Additional approver정보를 저장한다.
        /// </summary>
        /// <param name="approver"></param>
        public void MergeConfigurationApprover(Dto.DTO_CONFIG_APPROVER approver)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[8];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", approver.DOCUMENT_ID);
                    parameters[1] = new SqlParameter("@APPROVAL_LOCATION", approver.APPROVAL_LOCATION);
                    parameters[2] = new SqlParameter("@CONDITION_INDEX", approver.CONDITION_INDEX);
                    parameters[3] = new SqlParameter("@IS_MANDATORY", approver.IS_MANDATORY);
                    parameters[4] = new SqlParameter("@APPROVER_ID", approver.APPROVER_ID);
                    parameters[5] = new SqlParameter("@DISPLAY_CONDITION", approver.DISPLAY_CONDITION);
                    parameters[6] = new SqlParameter("@SQL_CONDITION", approver.SQL_CONDITION);
                    parameters[7] = new SqlParameter("@CREATOR_ID", approver.CREATOR_ID);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_MERGE_CONFIG_APPROVER, parameters);

                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Additional Approver정보를 삭제한다. Condition정보도 같이 삭제한다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="location"></param>
        /// <param name="index"></param>
        public void DeleteConfigurationApprover(string documentId, string location, int index)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);
                    parameters[1] = new SqlParameter("@APPROVAL_LOCATION", location);
                    parameters[2] = new SqlParameter("@CONDITION_INDEX", index);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_DELETE_CONFIG_APPROVER, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Addtional Approver의 선택된 조건을 삭제한다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="location"></param>
        /// <param name="index"></param>
        /// <param name="seq"></param>
        public void DeleteConfigurationApproverCondition(string documentId, string location, int index, int seq)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[4];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);
                    parameters[1] = new SqlParameter("@APPROVAL_LOCATION", location);
                    parameters[2] = new SqlParameter("@CONDITION_INDEX", index);
                    parameters[3] = new SqlParameter("@CONDITION_SEQ", seq);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_DELETE_CONFIG_APPROVER_CONDITION, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Additional Approver의 조건을 전체삭제한다. 등록시 전체삭제 후 저장을 위해
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="location"></param>
        /// <param name="index"></param>
        public void DeleteConfigurationApproverConditionAll(string documentId, string location, int index)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);
                    parameters[1] = new SqlParameter("@APPROVAL_LOCATION", location);
                    parameters[2] = new SqlParameter("@CONDITION_INDEX", index);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_DELETE_CONFIG_APPROVER_CONDITION_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Additional Approver의 조건을 저장한다.
        /// </summary>
        /// <param name="condition"></param>
        public void InsertConfigurationApproverCondition(Dto.DTO_CONFIG_APPROVER_CONDITION condition)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[9];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", condition.DOCUMENT_ID);
                    parameters[1] = new SqlParameter("@APPROVAL_LOCATION", condition.APPROVAL_LOCATION);
                    parameters[2] = new SqlParameter("@CONDITION_INDEX", condition.CONDITION_INDEX);
                    parameters[3] = new SqlParameter("@CONDITION_SEQ", condition.CONDITION_SEQ);
                    parameters[4] = new SqlParameter("@FIELD_NAME", condition.FIELD_NAME);
                    parameters[5] = new SqlParameter("@CONDITION", condition.CONDITION);
                    parameters[6] = new SqlParameter("@VALUE", condition.VALUE);
                    parameters[7] = new SqlParameter("@OPTION", condition.OPTION);
                    parameters[8] = new SqlParameter("@CREATOR_ID", condition.CREATOR_ID);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_INSERT_CONFIG_APPROVER_CONDITION, parameters);
                }
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 결재문서 Additional Recipient정보를 저장한다.
        /// </summary>
        /// <param name="recipient"></param>
        public void MergeConfigurationRecipient(Dto.DTO_CONFIG_RECIPIENT recipient)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[7];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", recipient.DOCUMENT_ID);
                    parameters[1] = new SqlParameter("@CONDITION_INDEX", recipient.CONDITION_INDEX);
                    parameters[2] = new SqlParameter("@IS_MANDATORY", recipient.IS_MANDATORY);
                    parameters[3] = new SqlParameter("@RECIPIENT_ID", recipient.RECIPIENT_ID);
                    parameters[4] = new SqlParameter("@DISPLAY_CONDITION", recipient.DISPLAY_CONDITION);
                    parameters[5] = new SqlParameter("@SQL_CONDITION", recipient.SQL_CONDITION);
                    parameters[6] = new SqlParameter("@CREATOR_ID", recipient.CREATOR_ID);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_MERGE_CONFIG_RECIPIENT, parameters);

                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Additional Recipient정보를 삭제한다. Condition정보도 같이 삭제한다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="index"></param>
        public void DeleteConfigurationRecipient(string documentId, int index)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);
                    parameters[1] = new SqlParameter("@CONDITION_INDEX", index);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_DELETE_CONFIG_RECIPIENT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Addtional Recipient의 선택된 조건을 삭제한다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="index"></param>
        /// <param name="seq"></param>
        public void DeleteConfigurationRecipientCondition(string documentId, int index, int seq)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);
                    parameters[1] = new SqlParameter("@CONDITION_INDEX", index);
                    parameters[2] = new SqlParameter("@CONDITION_SEQ", seq);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_DELETE_CONFIG_RECIPIENT_CONDITION, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Additional Recipient의 조건을 전체삭제한다. 등록시 전체삭제 후 저장을 위해
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="location"></param>
        /// <param name="index"></param>
        public void DeleteConfigurationRecipientConditionAll(string documentId, int index)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);
                    parameters[1] = new SqlParameter("@CONDITION_INDEX", index);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_DELETE_CONFIG_RECIPIENT_CONDITION_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }



        /// <summary>
        /// 결재문서 Additional Recipient의 조건을 저장한다.
        /// </summary>
        /// <param name="condition"></param>
        public void InsertConfigurationRecipientCondition(Dto.DTO_CONFIG_RECIPIENT_CONDITION condition)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[8];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", condition.DOCUMENT_ID);
                    parameters[1] = new SqlParameter("@CONDITION_INDEX", condition.CONDITION_INDEX);
                    parameters[2] = new SqlParameter("@CONDITION_SEQ", condition.CONDITION_SEQ);
                    parameters[3] = new SqlParameter("@FIELD_NAME", condition.FIELD_NAME);
                    parameters[4] = new SqlParameter("@CONDITION", condition.CONDITION);
                    parameters[5] = new SqlParameter("@VALUE", condition.VALUE);
                    parameters[6] = new SqlParameter("@OPTION", condition.OPTION);
                    parameters[7] = new SqlParameter("@CREATOR_ID", condition.CREATOR_ID);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_INSERT_CONFIG_RECIPIENT_CONDITION, parameters);
                }
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 결재문서 Additional Reviewer정보를 저장한다.
        /// </summary>
        /// <param name="reviewer"></param>
        public void MergeConfigurationReviewer(Dto.DTO_CONFIG_REVIEWER reviewer)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[7];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", reviewer.DOCUMENT_ID);
                    parameters[1] = new SqlParameter("@CONDITION_INDEX", reviewer.CONDITION_INDEX);
                    parameters[2] = new SqlParameter("@IS_MANDATORY", reviewer.IS_MANDATORY);
                    parameters[3] = new SqlParameter("@REVIEWER_ID", reviewer.REVIEWER_ID);
                    parameters[4] = new SqlParameter("@DISPLAY_CONDITION", reviewer.DISPLAY_CONDITION);
                    parameters[5] = new SqlParameter("@SQL_CONDITION", reviewer.SQL_CONDITION);
                    parameters[6] = new SqlParameter("@CREATOR_ID", reviewer.CREATOR_ID);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_MERGE_CONFIG_REVIEWER, parameters);

                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Additional Reviewer정보를 삭제한다. Condition정보도 같이 삭제한다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="index"></param>
        public void DeleteConfigurationReviewer(string documentId, int index)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);
                    parameters[1] = new SqlParameter("@CONDITION_INDEX", index);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_DELETE_CONFIG_REVIEWER, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Additional Reviewer의 선택된 조건을 삭제한다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="index"></param>
        /// <param name="seq"></param>
        public void DeleteConfigurationReviewerCondition(string documentId, int index, int seq)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);
                    parameters[1] = new SqlParameter("@CONDITION_INDEX", index);
                    parameters[2] = new SqlParameter("@CONDITION_SEQ", seq);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_DELETE_CONFIG_REVIEWER_CONDITION, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Additional Reviewer의 조건을 전체삭제한다. 등록시 전체삭제 후 저장을 위해
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="location"></param>
        /// <param name="index"></param>
        public void DeleteConfigurationReviewerConditionAll(string documentId, int index)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);
                    parameters[1] = new SqlParameter("@CONDITION_INDEX", index);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_DELETE_CONFIG_REVIEWER_CONDITION_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Additional Reviewer의 조건을 저장한다.
        /// </summary>
        /// <param name="condition"></param>
        public void InsertConfigurationReviewerCondition(Dto.DTO_CONFIG_REVIEWER_CONDITION condition)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[8];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", condition.DOCUMENT_ID);
                    parameters[1] = new SqlParameter("@CONDITION_INDEX", condition.CONDITION_INDEX);
                    parameters[2] = new SqlParameter("@CONDITION_SEQ", condition.CONDITION_SEQ);
                    parameters[3] = new SqlParameter("@FIELD_NAME", condition.FIELD_NAME);
                    parameters[4] = new SqlParameter("@CONDITION", condition.CONDITION);
                    parameters[5] = new SqlParameter("@VALUE", condition.VALUE);
                    parameters[6] = new SqlParameter("@OPTION", condition.OPTION);
                    parameters[7] = new SqlParameter("@CREATOR_ID", condition.CREATOR_ID);

                    context.Database.ExecuteSqlCommand(ConfigurationContext.USP_INSERT_CONFIG_REVIEWER_CONDITION, parameters);
                }
            }
            catch
            {
                throw;
            }
        }



        /// <summary>
        /// Configuration전체 목록을 조회한다.
        /// </summary>
        /// <returns></returns>
        public List<Dto.DTO_CONFIG> SelectConfigurationList()
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    return context.Database.SqlQuery<Dto.DTO_CONFIG>(ConfigurationContext.USP_SELECT_CONFIG_LIST).ToList();
                }
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// 결재문서 설정정보를 조회한다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public Dto.DTO_CONFIG SelectConfiguration(string documentId)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);

                    return context.Database.SqlQuery<Dto.DTO_CONFIG>(ConfigurationContext.USP_SELECT_CONFIG, parameters).First();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서에 해당하는 회사를 조회한다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public List<Dto.DTO_CONFIG_COMPANY> SelectConfigurationCompany(string documentId)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);

                    return context.Database.SqlQuery<Dto.DTO_CONFIG_COMPANY>(ConfigurationContext.USP_SELECT_CONFIG_COMPANY, parameters).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 결재자 목록을 가져온다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public List<Dto.DTO_CONFIG_APPROVER> SelectConfigurationApprover(string documentId, string location)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);
                    parameters[1] = new SqlParameter("@APPROVAL_LOCATION", location);

                    return context.Database.SqlQuery<Dto.DTO_CONFIG_APPROVER>(ConfigurationContext.USP_SELECT_CONFIG_APPROVER, parameters).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 결재자 조건 목록을 가져온다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="location"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<Dto.DTO_CONFIG_APPROVER_CONDITION> SelectConfigApproverCondition(string documentId, string location, int index)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);
                    parameters[1] = new SqlParameter("@APPROVAL_LOCATION", location);
                    parameters[2] = new SqlParameter("@CONDITION_INDEX", index);

                    return context.Database.SqlQuery<Dto.DTO_CONFIG_APPROVER_CONDITION>(ConfigurationContext.USP_SELECT_CONFIG_APPROVER_CONDITION, parameters).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Recipient 목록을 가져온다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public List<Dto.DTO_CONFIG_RECIPIENT> SelectConfigurationRecipient(string documentId)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);

                    return context.Database.SqlQuery<Dto.DTO_CONFIG_RECIPIENT>(ConfigurationContext.USP_SELECT_CONFIG_RECIPIENT, parameters).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Recipient 조건 목록을 가져온다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<Dto.DTO_CONFIG_RECIPIENT_CONDITION> SelectConfigRecipientCondition(string documentId, int index)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);
                    parameters[1] = new SqlParameter("@CONDITION_INDEX", index);

                    return context.Database.SqlQuery<Dto.DTO_CONFIG_RECIPIENT_CONDITION>(ConfigurationContext.USP_SELECT_CONFIG_RECIPIENT_CONDITION, parameters).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Reviewer 목록을 가져온다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public List<Dto.DTO_CONFIG_REVIEWER> SelectConfigurationReviewer(string documentId)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);

                    return context.Database.SqlQuery<Dto.DTO_CONFIG_REVIEWER>(ConfigurationContext.USP_SELECT_CONFIG_REVIEWER, parameters).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Reviewer 조건 목록을 가져온다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<Dto.DTO_CONFIG_REVIEWER_CONDITION> SelectConfigReviewerCondition(string documentId, int index)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentId);
                    parameters[1] = new SqlParameter("@CONDITION_INDEX", index);

                    return context.Database.SqlQuery<Dto.DTO_CONFIG_REVIEWER_CONDITION>(ConfigurationContext.USP_SELECT_CONFIG_REVIEWER_CONDITION, parameters).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Configuration 등록시 Table존재여부 확인
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public int SelectExistsTable(string tableName)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@TABLE_NAME", tableName);

                    return context.Database.SqlQuery<int>(ConfigurationContext.USP_SELECT_EXISTS_TABLE, parameters).First();
                }
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Document Id를 생성해 가져온다.
        /// </summary>
        /// <returns></returns>
        public string GetAfterTreatementServiceName(string documentid)
        {
            try
            {
                using (context = new ConfigurationContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@DOCUMENT_ID", documentid);
                    return context.Database.SqlQuery<string>(ConfigurationContext.USP_SELECT_CONFIG_AFTER_TREATEMENT_SERVICE, parameters).First();
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

    }
}
