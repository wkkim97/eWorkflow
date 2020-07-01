using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class CreditDebitMemoDao : DNSoft.eWF.FrameWork.Data.EF.DaoBase
    {
        public void MergeCreditDebitMemo(Dto.DTO_DOC_CREDIT_DEBIT_MEMO doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CREDIT_DEBIT_MEMO, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void MergeCreditDebitMemoProduct(Dto.DTO_DOC_CREDIT_DEBIT_MEMO_PRODUCT product)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(product);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CREDIT_DEBIT_MEMO_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void MergeCreditDebitMemoWholeSaler(Dto.DTO_DOC_CREDIT_DEBIT_MEMO_WHOLESALER saler)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(saler);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CREDIT_DEBIT_MEMO_WHOLESALER, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteCreditDebitMemoProductAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_CREDIT_DEBIT_MEMO_PRODUCT_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteCreditDebitMemoWholeSalerAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_CREDIT_DEBIT_MEMO_WHOLESALER_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_CREDIT_DEBIT_MEMO SelectCreditDebitMemo(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_CREDIT_DEBIT_MEMO>(ApprovalContext.USP_SELECT_CREDIT_DEBIT_MEMO, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_CREDIT_DEBIT_MEMO_PRODUCT> SelectCreditDebitMemoProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_CREDIT_DEBIT_MEMO_PRODUCT>(ApprovalContext.USP_SELECT_CREDIT_DEBIT_MEMO_PRODUCT, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_CREDIT_DEBIT_MEMO_WHOLESALER> SelectCreditDebitMemoWholeSaler(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_CREDIT_DEBIT_MEMO_WHOLESALER>(ApprovalContext.USP_SELECT_CREDIT_DEBIT_MEMO_WHOLESALER, parameters);

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
