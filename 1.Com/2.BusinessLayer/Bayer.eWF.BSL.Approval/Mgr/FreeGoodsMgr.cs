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
    public class FreeGoodsMgr : MgrBase
    {
        #region Merge
        public string MergeFreeGoodsInfo(Dto.DTO_DOC_FREE_GOODS doc, List<Dto.DTO_DOC_FREE_GOODS_INFO> infos)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (doc.PROCESS_ID.NullObjectToEmptyEx().Length == 0)
                    {
                        using (Dao.CommonDao dao = new Dao.CommonDao())
                        {
                            doc.PROCESS_ID = dao.GetNewProcessID();
                        }
                    }
                    using (Dao.FreeGoodsDao dao = new FreeGoodsDao())
                    {
                        dao.MergerFreeGoods(doc);
                        dao.DeleteFreeGoodsInfoAll(doc.PROCESS_ID);

                        foreach (Dto.DTO_DOC_FREE_GOODS_INFO info in infos)
                        {
                            info.PROCESS_ID = doc.PROCESS_ID;
                            dao.InsertFreeGoodsInfo(info);
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

        public string MergeFreeGoodsCustomer(Dto.DTO_DOC_FREE_GOODS doc, List<Dto.DTO_DOC_FREE_GOODS_CUSTOMER> customers)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (doc.PROCESS_ID.NullObjectToEmptyEx().Length == 0)
                    {
                        using (Dao.CommonDao dao = new Dao.CommonDao())
                        {
                            doc.PROCESS_ID = dao.GetNewProcessID();
                        }
                    }
                    using (Dao.FreeGoodsDao dao = new FreeGoodsDao())
                    {
                        dao.MergerFreeGoods(doc);
                        dao.DeleteFreeGoodsCustomerAll(doc.PROCESS_ID);

                        foreach (Dto.DTO_DOC_FREE_GOODS_CUSTOMER customer in customers)
                        {
                            customer.PROCESS_ID = doc.PROCESS_ID;
                            dao.InsertFreeGoodsCustomer(customer);
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

        #region Select
        public Dto.DTO_DOC_FREE_GOODS SelectFreeGoods(string processId)
        {
            try
            {
                using (Dao.FreeGoodsDao dao = new Dao.FreeGoodsDao())
                {
                    return dao.SelectFreeGoods(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dto.DTO_DOC_FREE_GOODS_INFO> SelectFreeGoodsInfo(string processId)
        {
            try
            {
                using (Dao.FreeGoodsDao dao = new Dao.FreeGoodsDao())
                {
                    return dao.SelectFreeGoodsInfo(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dto.DTO_DOC_FREE_GOODS_CUSTOMER> SelectFreeGoodsCustomer(string processId)
        {
            try
            {
                using (Dao.FreeGoodsDao dao = new Dao.FreeGoodsDao())
                {
                    return dao.SelectFreeGoodsCustomer(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Free Goods Check Duplicate
        public void InsertFreeGoodsCheckDuplicate(List<Dto.DTO_DOC_FREE_GOODS_CHECK_DUPLICATE> duplicates)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (Dao.FreeGoodsDao dao = new FreeGoodsDao())
                    {
                        foreach (Dto.DTO_DOC_FREE_GOODS_CHECK_DUPLICATE duplicate in duplicates)
                        {
                            dao.InsertFreeGoodsCheckDuplicate(duplicate);
                        }
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dto.DTO_DOC_FREE_GOODS_CHECK_DUPLICATE> SelectFreeGoodsCheckDuplicate(string userId,string purpose, string bu)
        {
            try
            {
                using (Dao.FreeGoodsDao dao = new Dao.FreeGoodsDao())
                {
                    return dao.SelectFreeGoodsCheckDuplicate(userId,purpose,bu);
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
