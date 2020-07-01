using Bayer.eWF.BSL.Approval.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DNSoft.eW.FrameWork;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class CancelationMortgageMgr : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public string MergeCancelationMortgage(Dto.DTO_DOC_CANCELATION_MORTGAGE doc)
        {
            try
            {
                if (doc.PROCESS_ID.NullObjectToEmptyEx().Length == 0)
                {
                    using (Dao.CommonDao dao = new Dao.CommonDao())
                    {
                        doc.PROCESS_ID = dao.GetNewProcessID();
                    }
                }
                using (CancelationMortgageDao dao = new CancelationMortgageDao())
                {
                    dao.MergeCancelationMortgage(doc);
                }
                return doc.PROCESS_ID;
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_CANCELATION_MORTGAGE SelectCancelationMortgage(string processId)
        {
            try
            {
                using (CancelationMortgageDao dao = new CancelationMortgageDao())
                {
                    return dao.SelectCancelationMortgage(processId);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
