using DNSoft.eWF.FrameWork.Data.EF;
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
    public class CsFmToNhSaleMgr : MgrBase
    {
        #region [MergeCsFmToNhSale]
        public string MergeCsFmToNhSale(Dto.DTO_DOC_CS_FM_TO_NH_SALE doc, List<Dto.DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT> products)
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
                    using (Dao.CsFmToNhSaleDao dao = new Dao.CsFmToNhSaleDao())
                    {
                        dao.MergeCsFmToNhSale(doc);
                        dao.DeleteCsFmToNhSaleProductAll(processId);

                        foreach (Dto.DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT product in products)
                        {
                            product.PROCESS_ID = processId;
                            dao.MergeCsFmToNhSaleProduct(product);
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


        public Dto.DTO_DOC_CS_FM_TO_NH_SALE SelectCsFmToNhSale(string processId)
        {
            try
            {
                using (CsFmToNhSaleDao dao = new CsFmToNhSaleDao())
                {
                    return dao.SelectCsFmToNhSale(processId);
                }
            }
            catch
            {
                throw;
            }
        }


        #region[SelectCsFmToNhSaleProduct]
        public List<Dto.DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT> SelectCsFmToNhSaleProduct(string processId)
        {
            try
            {
                using (Dao.CsFmToNhSaleDao dao = new Dao.CsFmToNhSaleDao())
                {
                    return dao.SelectCsFmToNhSaleProduct(processId);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteCsFmToNhSlaeProductsByIndex]
        public void DeleteCsFmToNhSlaeProductsByIndex(string processId, int idx)
        {
            try
            {
                using (Dao.CsFmToNhSaleDao dao = new Dao.CsFmToNhSaleDao())
                {
                    dao.DeleteCsFmToNhSaleProductsByIndex(processId, idx);
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
