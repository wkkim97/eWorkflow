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
    public class ListPriceMgr : MgrBase
    {
        #region MergeListPrice
        public string MergeListPrice(Dto.DTO_DOC_LIST_PRICE doc, List<Dto.DTO_DOC_LIST_PRICE_PRODUCT> products)
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
                    using (Dao.ListPriceDao dao = new ListPriceDao())
                    {
                        dao.MergeListPrice(doc);
                        dao.DeleteListPriceProductAll(doc.PROCESS_ID);

                        foreach (Dto.DTO_DOC_LIST_PRICE_PRODUCT product in products)
                        {
                            product.PROCESS_ID = doc.PROCESS_ID;
                            dao.InsertListPriceProduct(product);
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

        #region [SelectListPrice]
        public Dto.DTO_DOC_LIST_PRICE SelectListPrice(string processId)
        {
            try
            {
                using (Dao.ListPriceDao dao = new Dao.ListPriceDao())
                {
                    return dao.SelectListPrice(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region[SelectListPriceProduct]
        public List<Dto.DTO_DOC_LIST_PRICE_PRODUCT> SelectListPriceProduct(string processId)
        {
            try
            {
                using (Dao.ListPriceDao dao = new Dao.ListPriceDao())
                {
                    return dao.SelectListPriceProduct(processId);
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
