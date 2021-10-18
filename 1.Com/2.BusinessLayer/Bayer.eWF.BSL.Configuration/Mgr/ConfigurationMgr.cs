using Bayer.eWF.BSL.Configuration.Dao;
using Bayer.eWF.BSL.Configuration.Dto;
using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Bayer.eWF.BSL.Configuration.Mgr
{
    public class ConfigurationMgr : MgrBase
    {
        //그룹 목록 저장
        public void MergeReadersGroup(List<Dto.DTO_READERS_GROUP_USER_LIST> users)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (ConfigurationDao dao = new ConfigurationDao())
                    {

                        foreach (DTO_READERS_GROUP_USER_LIST user in users)
                        {
                            dao.InsertReadersGroupUser(user);
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
        //Readers Group 전체삭제
        public void DeleteReadersGroup(string group_code, string user_id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (ConfigurationDao dao = new ConfigurationDao())
                    {
                        dao.DeleteReadersGroup(group_code);

                        dao.DeleteReadersGroupUser(group_code, user_id);

                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // 그룹코드 생성
        public string CreateGroupCode()
        {
            try
            {
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    return dao.CreateGroupCode();
                }
            }
            catch
            {
                throw;
            }
        }

        public int SelectReadersGroupCount(string userId)
        {
            try
            {
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    return dao.SelectReadersGroupCount(userId);
                }
            }
            catch
            {
                throw;
            }
        }

        #region [ Configuration ]

        public List<DbTableColumnDto> SelectDbTableColumn(string tableName)
        {
            try
            {
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    return dao.SelectDbTableColumn(tableName);
                }
            }
            catch
            {
                throw;
            }
        }

        public string CreateDocumentId()
        {
            try
            {
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    return dao.CreateDocumentId();
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
        public void MergeConfiguration(Dto.DTO_CONFIG config, List<Dto.DTO_CONFIG_COMPANY> companies)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (ConfigurationDao dao = new ConfigurationDao())
                    {
                        dao.MergeConfiguration(config);

                        if (companies != null)
                        {
                            if (companies.Count > 0)
                                dao.DeleteConfigurationCompany(companies[0].DOCUMENT_ID);
                            foreach (Dto.DTO_CONFIG_COMPANY company in companies)
                            {
                                dao.InsertConfigurationCompany(company);
                            }
                        }
                    }
                    scope.Complete();
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
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    dao.DeleteConfiguration(documentId);
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
        public void MergeConfigurationApprover(Dto.DTO_CONFIG_APPROVER approver, List<Dto.DTO_CONFIG_APPROVER_CONDITION> conditions)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (ConfigurationDao dao = new ConfigurationDao())
                    {
                        dao.MergeConfigurationApprover(approver);

                        dao.DeleteConfigurationApproverConditionAll(approver.DOCUMENT_ID, approver.APPROVAL_LOCATION, approver.CONDITION_INDEX);

                        foreach (Dto.DTO_CONFIG_APPROVER_CONDITION condition in conditions)
                        {
                            dao.InsertConfigurationApproverCondition(condition);
                        }

                        scope.Complete();
                    }
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
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    dao.DeleteConfigurationApprover(documentId, location, index);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Additional Approver의 선택된 조건을 삭제한다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="location"></param>
        /// <param name="index"></param>
        /// <param name="seq"></param>
        public void DeleteConfigurationApproverCondition(DTO_CONFIG_APPROVER approver, string documentId, string location, int index, int seq)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (ConfigurationDao dao = new ConfigurationDao())
                    {
                        dao.DeleteConfigurationApproverCondition(documentId, location, index, seq);
                        dao.MergeConfigurationApprover(approver);
                    }
                    scope.Complete();
                }
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 결재문서 Additional recipient정보를 저장한다.
        /// </summary>
        /// <param name="approver"></param>
        public void MergeConfigurationRecipient(Dto.DTO_CONFIG_RECIPIENT recipient, List<Dto.DTO_CONFIG_RECIPIENT_CONDITION> conditions)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (ConfigurationDao dao = new ConfigurationDao())
                    {
                        dao.MergeConfigurationRecipient(recipient);

                        dao.DeleteConfigurationRecipientConditionAll(recipient.DOCUMENT_ID, recipient.CONDITION_INDEX);

                        foreach (Dto.DTO_CONFIG_RECIPIENT_CONDITION condition in conditions)
                        {
                            dao.InsertConfigurationRecipientCondition(condition);
                        }
                    }

                    scope.Complete();
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
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    dao.DeleteConfigurationRecipient(documentId, index);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Additional Recipient의 선택된 조건을 삭제한다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="index"></param>
        /// <param name="seq"></param>
        public void DeleteConfigurationRecipientCondition(DTO_CONFIG_RECIPIENT recipient, string documentId, int index, int seq)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (ConfigurationDao dao = new ConfigurationDao())
                    {
                        dao.DeleteConfigurationRecipientCondition(documentId, index, seq);

                        dao.MergeConfigurationRecipient(recipient);
                    }
                    scope.Complete();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Additional reviewer정보를 저장한다.
        /// </summary>
        /// <param name="approver"></param>
        public void MergeConfigurationReviewer(Dto.DTO_CONFIG_REVIEWER reviewer, List<Dto.DTO_CONFIG_REVIEWER_CONDITION> conditions)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (ConfigurationDao dao = new ConfigurationDao())
                    {
                        dao.MergeConfigurationReviewer(reviewer);

                        dao.DeleteConfigurationReviewerConditionAll(reviewer.DOCUMENT_ID, reviewer.CONDITION_INDEX);

                        foreach (Dto.DTO_CONFIG_REVIEWER_CONDITION condition in conditions)
                        {
                            dao.InsertConfigurationReviewerCondition(condition);
                        }
                    }

                    scope.Complete();
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
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    dao.DeleteConfigurationReviewer(documentId, index);
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
        public void DeleteConfigurationReviewerCondition(DTO_CONFIG_REVIEWER reviewer, string documentId, int index, int seq)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (ConfigurationDao dao = new ConfigurationDao())
                    {
                        dao.DeleteConfigurationReviewerCondition(documentId, index, seq);
                        dao.MergeConfigurationReviewer(reviewer);
                    }
                    scope.Complete();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 목록을 조회한다.
        /// </summary>
        /// <returns></returns>
        public List<Dto.DTO_CONFIG> SelectConfigurationList()
        {
            try
            {
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    return dao.SelectConfigurationList();
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
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    return dao.SelectConfiguration(documentId);
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
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    return dao.SelectConfigurationCompany(documentId);
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
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    return dao.SelectConfigurationApprover(documentId, location);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 결재자 조건목록을 가져온다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="location"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<Dto.DTO_CONFIG_APPROVER_CONDITION> SelectConfigApproverCondition(string documentId, string location, int index)
        {
            try
            {
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    return dao.SelectConfigApproverCondition(documentId, location, index);
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
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    return dao.SelectConfigurationRecipient(documentId);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 결재문서 Recipient 조건목록을 가져온다.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<Dto.DTO_CONFIG_RECIPIENT_CONDITION> SelectConfigRecipientCondition(string documentId, int index)
        {
            try
            {
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    return dao.SelectConfigRecipientCondition(documentId, index);
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
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    return dao.SelectConfigurationReviewer(documentId);
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
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    return dao.SelectConfigReviewerCondition(documentId, index);
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
        public bool SelectExistsTable(string tableName)
        {
            try
            {
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    if (dao.SelectExistsTable(tableName) > 0) return true;
                    else return false;
                }
            }
            catch
            {
                throw;
            }
        }

        public string GetAfterTreatementServiceName(string documentid)
        {
            try
            {
                using (ConfigurationDao dao = new ConfigurationDao())
                {
                    return dao.GetAfterTreatementServiceName(documentid);
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
