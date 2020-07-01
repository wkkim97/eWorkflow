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
    public class BCSProcessChangeMgr : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public string MergeBCSProcessChange (Dto.DTO_DOC_BCS_PROCESS_CHANGE doc)
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
                    using (BCSProcessChangeDao dao = new BCSProcessChangeDao())
                    {
                        dao.MergeBCSProcessChange(doc);

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

        public Dto.DTO_DOC_BCS_PROCESS_CHANGE SelectBCSProcessChange(string processId)
        {
            try
            {
                using (BCSProcessChangeDao dao = new BCSProcessChangeDao())
                {
                    return dao.SelectBCSProcessChange(processId);
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
