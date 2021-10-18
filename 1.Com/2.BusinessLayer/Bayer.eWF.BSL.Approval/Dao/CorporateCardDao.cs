using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class CorporateCardDao : DaoBase
    {
        #region [ selectCorporateCard ]
        public Dto.DTO_DOC_CORPORATE_CARD selectCorporateCard(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_CORPORATE_CARD>(ApprovalContext.USP_SELECT_CORPORATE_CARD, parameters);

                return result.First();
            }
        }
        #endregion

        #region [ mergeCorporateCard ]
        public void mergeCorporateCard(Dto.DTO_DOC_CORPORATE_CARD doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[31];
                    parameters[0] = new SqlParameter("@PROCESS_ID", doc.PROCESS_ID);
                    parameters[1] = new SqlParameter("@SUBJECT", doc.SUBJECT);
                    parameters[2] = new SqlParameter("@DOC_NUM", (object)doc.DOC_NUM ?? DBNull.Value);
                    parameters[3] = new SqlParameter("@PROCESS_STATUS", doc.PROCESS_STATUS);
                    parameters[4] = new SqlParameter("@REQUESTER_ID", doc.REQUESTER_ID);
                    parameters[5] = new SqlParameter("@REQUEST_DATE", doc.REQUEST_DATE);
                    parameters[6] = new SqlParameter("@COMPANY_CODE", doc.COMPANY_CODE);
                    parameters[7] = new SqlParameter("@ORGANIZATION_NAME", doc.ORGANIZATION_NAME);
                    parameters[8] = new SqlParameter("@LIFE_CYCLE", doc.LIFE_CYCLE);
                    parameters[9] = new SqlParameter("@CATEGORY_CODE", (object)doc.CATEGORY_CODE ?? DBNull.Value);
                    parameters[10] = new SqlParameter("@BIRTHDAY", (object)doc.BIRTHDAY ?? DBNull.Value);
                    parameters[11] = new SqlParameter("@REASON", (object)doc.REASON ?? DBNull.Value);
                    parameters[12] = new SqlParameter("@CARD_NUMBER", (object)doc.CARD_NUMBER ?? DBNull.Value);
                    parameters[13] = new SqlParameter("@PERIOD_FOR_INCREASE_CODE", (object)doc.PERIOD_FOR_INCREASE_CODE ?? DBNull.Value);
                    parameters[14] = new SqlParameter("@STARTING_DATE", (object)doc.STARTING_DATE ?? DBNull.Value);
                    parameters[15] = new SqlParameter("@PERIOD", (object)doc.PERIOD ?? DBNull.Value);
                    parameters[16] = new SqlParameter("@INCREASE_AMOUNT", (object)doc.INCREASE_AMOUNT ?? DBNull.Value);
                    parameters[17] = new SqlParameter("@REASON_CODE", (object)doc.REASON_CODE ?? DBNull.Value);
                    parameters[18] = new SqlParameter("@REASON_OTHERS", (object)doc.REASON_OTHERS ?? DBNull.Value);
                    parameters[19] = new SqlParameter("@CHANGEMENT_ITEM", (object)doc.CHANGEMENT_ITEM ?? DBNull.Value);
                    parameters[20] = new SqlParameter("@BANK_NAME", (object)doc.BANK_NAME ?? DBNull.Value);
                    parameters[21] = new SqlParameter("@BANK_ACOUNT_NUMBER", (object)doc.BANK_ACOUNT_NUMBER ?? DBNull.Value);
                    parameters[22] = new SqlParameter("@CELL_PHONE_NUMBER", (object)doc.CELL_PHONE_NUMBER ?? DBNull.Value);
                    parameters[23] = new SqlParameter("@ADDRESS", (object)doc.ADDRESS ?? DBNull.Value);
                    parameters[24] = new SqlParameter("@NAME", (object)doc.NAME ?? DBNull.Value);
                    parameters[25] = new SqlParameter("@NEW_PASSWORD", (object)doc.NEW_PASSWORD ?? DBNull.Value);
                    parameters[26] = new SqlParameter("@IS_DISUSED", doc.IS_DISUSED );
                    parameters[27] = new SqlParameter("@CREATOR_ID", doc.CREATOR_ID );
                    parameters[28] = new SqlParameter("@CREATE_DATE", doc.CREATE_DATE);
                    parameters[29] = new SqlParameter("@UPDATER_ID", (object)doc.UPDATER_ID ?? DBNull.Value);
                    parameters[30] = new SqlParameter("@UPDATE_DATE", (object)doc.UPDATE_DATE ?? DBNull.Value);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CORPORATE_CARD, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion
    }
}
