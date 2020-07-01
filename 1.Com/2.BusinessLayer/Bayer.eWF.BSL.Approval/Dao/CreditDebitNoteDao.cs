using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class CreditDebitNoteDao : DaoBase
    {
        public void MergeCreditDebitNote(Dto.DTO_DOC_CREDIT_DEBIT_NOTE data)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[21];

                    parameters[0] = new SqlParameter("@PROCESS_ID", data.PROCESS_ID);
                    parameters[1] = new SqlParameter("@SUBJECT", data.SUBJECT);
                    parameters[2] = new SqlParameter("@DOC_NUM", data.DOC_NUM);
                    parameters[3] = new SqlParameter("@PROCESS_STATUS", data.PROCESS_STATUS);
                    parameters[4] = new SqlParameter("@REQUESTER_ID", data.REQUESTER_ID);
                    parameters[5] = new SqlParameter("@REQUEST_DATE", data.REQUEST_DATE);
                    parameters[6] = new SqlParameter("@COMPANY_CODE", data.COMPANY_CODE);
                    parameters[7] = new SqlParameter("@ORGANIZATION_NAME", data.ORGANIZATION_NAME);
                    parameters[8] = new SqlParameter("@LIFE_CYCLE", data.LIFE_CYCLE);
                    parameters[9] = new SqlParameter("@TYPE", data.TYPE);
                    parameters[10] = new SqlParameter("@TO_CODE", data.TO_CODE);
                    parameters[11] = new SqlParameter("@TO_NAME", data.TO_NAME);
                    parameters[12] = new SqlParameter("@COMPANY_ID", data.COMPANY_ID);
                    parameters[13] = new SqlParameter("@INVOICE_DATE", data.INVOICE_DATE);
                    parameters[14] = new SqlParameter("@DUE_DATE", data.DUE_DATE);
                    parameters[15] = new SqlParameter("@CURRENCY", data.CURRENCY);
                    parameters[16] = new SqlParameter("@TOTAL_AMOUNT", data.TOTAL_AMOUNT);
                    parameters[17] = new SqlParameter("@LOCAL_AMOUNT", data.LOCAL_AMOUNT);
                    parameters[18] = new SqlParameter("@DESCRIPTION", data.DESCRIPTION);
                    parameters[19] = new SqlParameter("@IS_DISUSED", data.IS_DISUSED);
                    parameters[20] = new SqlParameter("@CREATOR_ID", data.CREATOR_ID);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CREDIT_DEBIT_NOTE, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void MergeCreditDebitNoteAttn(Dto.DTO_DOC_CREDIT_DEBIT_NOTE_ATTN data)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[6];

                    parameters[0] = new SqlParameter("@PROCESS_ID", data.PROCESS_ID);
                    parameters[1] = new SqlParameter("@ATTN_CODE", data.ATTN_CODE);
                    parameters[2] = new SqlParameter("@ATTN_NAME", data.ATTN_NAME);
                    parameters[3] = new SqlParameter("@ATTN_MAIL_ADDRESS", data.ATTN_MAIL_ADDRESS);
                    parameters[4] = new SqlParameter("@ATTN_TYPE", data.ATTN_TYPE);
                    parameters[5] = new SqlParameter("@CREATOR_ID", data.CREATOR_ID);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CREDIT_DEBIT_NOTE_ATTN, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void MergeCreditDebitNoteCc(Dto.DTO_DOC_CREDIT_DEBIT_NOTE_CC data)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[6];

                    parameters[0] = new SqlParameter("@PROCESS_ID", data.PROCESS_ID);
                    parameters[1] = new SqlParameter("@CC_CODE", data.CC_CODE);
                    parameters[2] = new SqlParameter("@CC_NAME", data.CC_NAME);
                    parameters[3] = new SqlParameter("@CC_MAIL_ADDRESS", data.CC_MAIL_ADDRESS);
                    parameters[4] = new SqlParameter("@CC_TYPE", data.CC_TYPE);
                    parameters[5] = new SqlParameter("@CREATOR_ID", data.CREATOR_ID);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CREDIT_DEBIT_NOTE_CC, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void MergeCreditDebitNoteDesc(Dto.DTO_DOC_CREDIT_DEBIT_NOTE_DESC data)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[5];

                    parameters[0] = new SqlParameter("@PROCESS_ID", data.PROCESS_ID);
                    parameters[1] = new SqlParameter("@IDX", data.IDX);
                    parameters[2] = new SqlParameter("@DESCRIPTION", data.DESCRIPTION);
                    parameters[3] = new SqlParameter("@AMOUNT", data.AMOUNT);
                    parameters[4] = new SqlParameter("@CREATOR_ID", data.CREATOR_ID);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CREDIT_DEBIT_NOTE_DESC, parameters);

                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_CREDIT_DEBIT_NOTE SelectCreditDebitNote(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    var result = context.Database.SqlQuery<Dto.DTO_DOC_CREDIT_DEBIT_NOTE>(ApprovalContext.USP_SELECT_CREDIT_DEBIT_NOTE, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_CREDIT_DEBIT_NOTE_ATTN> SelectCreditDebitNoteAttn(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_CREDIT_DEBIT_NOTE_ATTN>(ApprovalContext.USP_SELECT_CREDIT_DEBIT_NOTE_ATTN, parameters);
                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_CREDIT_DEBIT_NOTE_CC> SelectCreditDebitNoteCc(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_CREDIT_DEBIT_NOTE_CC>(ApprovalContext.USP_SELECT_CREDIT_DEBIT_NOTE_CC, parameters);
                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_CREDIT_DEBIT_NOTE_DESC> SelectCreditDebitNoteDesc(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_CREDIT_DEBIT_NOTE_DESC>(ApprovalContext.USP_SELECT_CREDIT_DEBIT_NOTE_DESC, parameters);
                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteCreditDebitNoteAttn(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_CREDIT_DEBIT_NOTE_ATTN, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteCreditDebitNoteCc(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_CREDIT_DEBIT_NOTE_CC, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteCreditDebitNoteDescByIndex(string processId, int idx)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", idx);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_CREDIT_DEBIT_NOTE_DESC_IDX, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.CreditDebitNoteViewDto SelectCreditDebitNoteView(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.CreditDebitNoteViewDto>(ApprovalContext.USP_SELECT_CREDIT_DEBIT_NOTE_VIEW, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_PROCESS_APPROVAL_LIST> SelectRecipientList(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_PROCESS_APPROVAL_LIST>(ApprovalContext.USP_SELECT_PROCESS_RECIPIENT_LIST, parameters);

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
