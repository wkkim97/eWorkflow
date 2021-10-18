using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Bayer.eWF.BSL.Approval.Dao;
using DNSoft.eWF.FrameWork.Data.EF;
using DNSoft.eW.FrameWork;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class BCSReturnGoodsMgr : MgrBase
    {
        #region [MergeBcsReturnGoods]
        public string MergeBcsReturnGoods(Dto.DTO_DOC_BCS_RETURN_GOODS doc, List<Dto.DTO_DOC_BCS_RETURN_GOODS_LIST> lists)
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
                    using (Dao.BCSReturnGoodsDao dao = new Dao.BCSReturnGoodsDao())
                    {
                        dao.MergeBcsReturnGoods(doc);
                        dao.DeleteBcsReturnGoodsListAll(processId);

                        foreach (Dto.DTO_DOC_BCS_RETURN_GOODS_LIST list in lists)
                        {
                            //if (list.CUSTOMER_CODE.IsNullOrEmptyEx() || list.CUSTOMER_NAME.IsNullOrEmptyEx() || list.CUR_PRODUCT_CODE.IsNullOrEmptyEx() || list.CUR_PRODUCT_NAME.IsNullOrEmptyEx() || list.REP_PRODUCT_CODE.IsNullOrEmptyEx() || list.REP_PRODUCT_NAME.IsNullOrEmptyEx()) continue;
                            list.PROCESS_ID = processId;
                            dao.MergeBcsReturnGoodsList(list);
                        }
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

        #region [SelectBcsReturnGoods]
        public Dto.DTO_DOC_BCS_RETURN_GOODS SelectBcsReturnGoods(string processId)
        {
            try
            {
                using (Dao.BCSReturnGoodsDao dao = new Dao.BCSReturnGoodsDao())
                {
                    return dao.SelectBcsReturnGoods(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region[SelectBcsReturnGoodsList]
        public List<Dto.DTO_DOC_BCS_RETURN_GOODS_LIST> SelectBcsReturnGoodsList(string processId)
        {
            try
            {
                using (Dao.BCSReturnGoodsDao dao = new Dao.BCSReturnGoodsDao())
                {
                    return dao.SelectBcsReturnGoodsList(processId);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteBcsReturnGoodsListByIndex]
        public void DeleteBcsReturnGoodsListByIndex(string processId, int idx)
        {
            try
            {
                using (Dao.BCSReturnGoodsDao dao = new Dao.BCSReturnGoodsDao())
                {
                    dao.DeleteBcsReturnGoodsListByIndex(processId, idx);
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
