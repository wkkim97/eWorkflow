using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class BusinessCardDao : DaoBase
    {
        #region MergeBusinessCard
        public void MergeBusinessCard(Dto.DTO_DOC_BUSINESS_CARD doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[31];
                    parameters[0] = new SqlParameter("@PROCESS_ID", doc.PROCESS_ID);
                    parameters[1] = new SqlParameter("@SUBJECT", doc.SUBJECT);
                    parameters[2] = new SqlParameter("@DOC_NUM", (object)doc.DOC_NUM??DBNull.Value);
                    parameters[3] = new SqlParameter("@PROCESS_STATUS", doc.PROCESS_STATUS);
                    parameters[4] = new SqlParameter("@REQUESTER_ID", doc.REQUESTER_ID);
                    parameters[5] = new SqlParameter("@REQUEST_DATE", doc.REQUEST_DATE);
                    parameters[6] = new SqlParameter("@COMPANY_CODE", doc.COMPANY_CODE);
                    parameters[7] = new SqlParameter("@ORGANIZATION_NAME", doc.ORGANIZATION_NAME);
                    parameters[8] = new SqlParameter("@LIFE_CYCLE", doc.LIFE_CYCLE);
                    parameters[9] = new SqlParameter("@ENG_NAME", doc.ENG_NAME);
                    parameters[10] = new SqlParameter("@ENG_TITLE", doc.ENG_TITLE);
                    parameters[11] = new SqlParameter("@ENG_DIVISION_CODE", doc.ENG_DIVISION_CODE);
                    parameters[12] = new SqlParameter("@ENG_DIVISION_NAME", doc.ENG_DIVISION_NAME);
                    parameters[13] = new SqlParameter("@ENG_DEPARTMENT", doc.ENG_DEPARTMENT);
                    parameters[14] = new SqlParameter("@KOR_NAME", doc.KOR_NAME);
                    parameters[15] = new SqlParameter("@KOR_TITLE", doc.KOR_TITLE);
                    parameters[16] = new SqlParameter("@KOR_TITLE_NAME", doc.KOR_TITLE_NAME);
                    parameters[17] = new SqlParameter("@KOR_DIVISION_CODE", doc.KOR_DIVISION_CODE);
                    parameters[18] = new SqlParameter("@KOR_DIVISION_NAME", doc.KOR_DIVISION_NAME);
                    parameters[19] = new SqlParameter("@KOR_DEPARTMENT", doc.KOR_DEPARTMENT);
                    parameters[20] = new SqlParameter("@TEL_OFFICE", doc.TEL_OFFICE);
                    parameters[21] = new SqlParameter("@MOBILE", doc.MOBILE);
                    parameters[22] = new SqlParameter("@FAX", doc.FAX);
                    parameters[23] = new SqlParameter("@E_MAIL", doc.E_MAIL);
                    parameters[24] = new SqlParameter("@ADDRESS_CODE", doc.ADDRESS_CODE);
                    parameters[25] = new SqlParameter("@ADDRESS", doc.ADDRESS);
                    parameters[26] = new SqlParameter("@QUANTITY", doc.QUANTITY);
                    parameters[27] = new SqlParameter("@KOR_JOB_TITLE", doc.KOR_JOB_TITLE);
                    parameters[28] = new SqlParameter("@COLOR_CODE", doc.COLOR_CODE);
                    parameters[29] = new SqlParameter("@IS_DISUSED", doc.IS_DISUSED);
                    parameters[30] = new SqlParameter("@CREATOR_ID", doc.CREATOR_ID);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BUSINESS_CARD, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SelectBusinessCard
        public Dto.DTO_DOC_BUSINESS_CARD SelectBusinessCard(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_BUSINESS_CARD>(ApprovalContext.USP_SELECT_BUSINESS_CARD, parameters);

                return result.First();
            }
        }
        #endregion        

        #region SelectBusinessCardDisplayNameCard
        public Dto.DTO_DOC_BUSINESS_CARD SelectBusinessCardDisplayNameCard(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_BUSINESS_CARD>(ApprovalContext.USP_SELECT_BUSINESS_CARD_DISPLAY_NAME_CARD, parameters);

                return result.First();
            }
        }
        #endregion

        public List<Dto.SendToAgencyBusicessCardDto> SelectRequesterAndRecipient(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.SendToAgencyBusicessCardDto>(ApprovalContext.USP_SELECT_BUSINESS_CARD_AGENCY, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
