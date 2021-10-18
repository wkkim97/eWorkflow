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
    public class BasePriceMgr : MgrBase
    {
        #region MergeBasePrice
        public string MergeBasePrice(Dto.DTO_DOC_BASE_PRICE doc, List<Dto.DTO_DOC_BASE_PRICE_PRODUCT> products)
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
                    using (Dao.BasePriceDao dao = new BasePriceDao())
                    {
                        dao.MergeBasePrice(doc);
                        dao.DeleteBasePriceProductAll(doc.PROCESS_ID);

                        foreach (Dto.DTO_DOC_BASE_PRICE_PRODUCT product in products)
                        {
                            product.PROCESS_ID = doc.PROCESS_ID;
                            dao.InsertBasePriceProduct(product);
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

        #region [SelectBasePrice]
        public Dto.DTO_DOC_BASE_PRICE SelectBasePrice(string processId)
        {
            try
            {
                using (Dao.BasePriceDao dao = new Dao.BasePriceDao())
                {
                    return dao.SelectBasePrice(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region[SelectBasePriceProduct]
        public List<Dto.DTO_DOC_BASE_PRICE_PRODUCT> SelectBasePriceProduct(string processId)
        {
            try
            {
                using (Dao.BasePriceDao dao = new Dao.BasePriceDao())
                {
                    return dao.SelectBasePriceProduct(processId);
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
