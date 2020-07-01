using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bayer.eWF.BSL.Approval.Dao;
using DNSoft.eWF.FrameWork.Data.EF;
using DNSoft.eW.FrameWork;
using System.Transactions;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class BhcReturnGoodsMgr :MgrBase
    {
        #region [MergeBhcReturnGoods]
        public string MergeBhcReturnGoods(Dto.DTO_DOC_BHC_RETURN_GOODS doc)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    string processId = doc.PROCESS_ID;
                    if (doc.PROCESS_ID.NullObjectToEmptyEx().Length == 0)
                    {
                        using (Dao.CommonDao dao = new Dao.CommonDao())
                        {
                            processId = dao.GetNewProcessID();
                            doc.PROCESS_ID = processId;
                        }
                    }
                    using (Dao.BHCReturnGoodsDao dao = new Dao.BHCReturnGoodsDao())
                    {
                        dao.MergeBhcReturnGoods(doc);
                    }
                    scope.Complete();
                    return doc.PROCESS_ID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [SelectBHCReturnGoods]
        public Dto.DTO_DOC_BHC_RETURN_GOODS SelectBhcReturnGoods(string processId)
        {
            try
            {
                using (Dao.BHCReturnGoodsDao dao = new Dao.BHCReturnGoodsDao())
                {
                    return dao.SelectBhcReturnGoods(processId);
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
