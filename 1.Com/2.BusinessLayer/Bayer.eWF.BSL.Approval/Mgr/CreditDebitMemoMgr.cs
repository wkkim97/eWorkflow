using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Approval.Dao;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class CreditDebitMemoMgr : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public string MergeCreditDebitMemo(Dto.DTO_DOC_CREDIT_DEBIT_MEMO doc, List<Dto.DTO_DOC_CREDIT_DEBIT_MEMO_PRODUCT> products, List<Dto.DTO_DOC_CREDIT_DEBIT_MEMO_WHOLESALER> salers)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    string processId = doc.PROCESS_ID;
                    if (processId.NullObjectToEmptyEx().Length == 0)
                    {
                        using (Dao.CommonDao dao = new Dao.CommonDao())
                        {
                            processId = dao.GetNewProcessID();
                            doc.PROCESS_ID = processId;
                        }
                    }
                    using (CreditDebitMemoDao dao = new CreditDebitMemoDao())
                    {
                        dao.MergeCreditDebitMemo(doc);

                        dao.DeleteCreditDebitMemoProductAll(processId);

                        dao.DeleteCreditDebitMemoWholeSalerAll(processId);

                        foreach (Dto.DTO_DOC_CREDIT_DEBIT_MEMO_PRODUCT product in products)
                        {
                            product.PROCESS_ID = processId;
                            dao.MergeCreditDebitMemoProduct(product);
                        }

                        foreach (Dto.DTO_DOC_CREDIT_DEBIT_MEMO_WHOLESALER saler in salers)
                        {
                            saler.PROCESS_ID = processId;
                            dao.MergeCreditDebitMemoWholeSaler(saler);
                        }
                    }

                    scope.Complete();
                    return processId;
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
                using (CreditDebitMemoDao dao = new CreditDebitMemoDao())
                {
                    return dao.SelectCreditDebitMemo(processId);
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
                using (CreditDebitMemoDao dao = new CreditDebitMemoDao())
                {
                    return dao.SelectCreditDebitMemoProduct(processId);
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
                using (CreditDebitMemoDao dao = new CreditDebitMemoDao())
                {
                    return dao.SelectCreditDebitMemoWholeSaler(processId);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
