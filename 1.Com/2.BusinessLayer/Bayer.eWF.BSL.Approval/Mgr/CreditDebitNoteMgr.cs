using Bayer.eWF.BSL.Approval.Dao;
using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DNSoft.eW.FrameWork;
namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class CreditDebitNoteMgr : MgrBase
    {
        public string MergeCreditDebitNote(Dto.DTO_DOC_CREDIT_DEBIT_NOTE note, List<Dto.DTO_DOC_CREDIT_DEBIT_NOTE_ATTN> attns, List<Dto.DTO_DOC_CREDIT_DEBIT_NOTE_CC> ccs, List<Dto.DTO_DOC_CREDIT_DEBIT_NOTE_DESC> descriptions)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    string processId = note.PROCESS_ID;
                    if (processId.NullObjectToEmptyEx().Length == 0)
                    {
                        using (Dao.CommonDao dao = new Dao.CommonDao())
                        {
                            processId = dao.GetNewProcessID();
                            note.PROCESS_ID = processId;
                        }
                    }
                    using (CreditDebitNoteDao dao = new CreditDebitNoteDao())
                    {
                        dao.MergeCreditDebitNote(note);

                        dao.DeleteCreditDebitNoteAttn(processId);

                        foreach (Dto.DTO_DOC_CREDIT_DEBIT_NOTE_ATTN attn in attns)
                        {
                            attn.PROCESS_ID = processId;
                            dao.MergeCreditDebitNoteAttn(attn);
                        }

                        dao.DeleteCreditDebitNoteCc(processId);

                        foreach (Dto.DTO_DOC_CREDIT_DEBIT_NOTE_CC cc in ccs)
                        {
                            cc.PROCESS_ID = processId;
                            dao.MergeCreditDebitNoteCc(cc);
                        }

                        foreach (Dto.DTO_DOC_CREDIT_DEBIT_NOTE_DESC desc in descriptions)
                        {
                            desc.PROCESS_ID = processId;
                            //if (desc.DESCRIPTION.NullObjectToEmptyEx().Length < 1 && desc.AMOUNT == 0) continue;
                            dao.MergeCreditDebitNoteDesc(desc);
                        }
                    }
                    scope.Complete();
                }
                return note.PROCESS_ID;
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
                using (CreditDebitNoteDao dao = new CreditDebitNoteDao())
                {
                    return dao.SelectCreditDebitNote(processId);
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
                using (CreditDebitNoteDao dao = new CreditDebitNoteDao())
                {
                    return dao.SelectCreditDebitNoteAttn(processId);
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
                using (CreditDebitNoteDao dao = new CreditDebitNoteDao())
                {
                    return dao.SelectCreditDebitNoteCc(processId);
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
                using (CreditDebitNoteDao dao = new CreditDebitNoteDao())
                {
                    return dao.SelectCreditDebitNoteDesc(processId);
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
                using (CreditDebitNoteDao dao = new CreditDebitNoteDao())
                {
                    dao.DeleteCreditDebitNoteDescByIndex(processId, idx);
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
                using (CreditDebitNoteDao dao = new CreditDebitNoteDao())
                {

                    return dao.SelectCreditDebitNoteView(processId);
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
                using (CreditDebitNoteDao dao = new CreditDebitNoteDao())
                {
                    return dao.SelectRecipientList(processId);
                }

            }
            catch
            {
                throw;
            }
        }
    }
}
