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
    public class MaterialPriceMgr : MgrBase
    {
         #region MergeMaterialPrice
        public string MergeMaterialPrice(Dto.DTO_DOC_MATERIAL_PRICE doc, List<Dto.DTO_DOC_MATERIAL_PRICE_LIST> materialGrid)
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
                    using (Dao.MaterialPriceDao dao = new MaterialPriceDao())
                    {
                        dao.MergeMaterialPrice(doc);                        
                        dao.DeleteMaterialPriceListAll(doc.PROCESS_ID);
                        dao.DeleteMaterialPriceProductAll(doc.PROCESS_ID);

                        foreach (Dto.DTO_DOC_MATERIAL_PRICE_LIST material in materialGrid)
                        {
                            material.PROCESS_ID = doc.PROCESS_ID;
                            dao.InsertMaterialPriceList(material);
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

        #region SelectMaterialPrice
        public Dto.DTO_DOC_MATERIAL_PRICE SelectMaterialPrice(string processId)
        {
            try
            {
                using (Dao.MaterialPriceDao dao = new Dao.MaterialPriceDao())
                {
                    return dao.SelectMaterialPrice(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  SelectMaterialPriceList
        public List<Dto.DTO_DOC_MATERIAL_PRICE_LIST> SelectMaterialPriceList(string processId)
        {
            try
            {
                using (Dao.MaterialPriceDao dao = new Dao.MaterialPriceDao())
                {
                    return dao.SelectMaterialPriceList(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public List<Dto.DTO_DOC_MATERIAL_PRICE_PRODUCT> SelectMaterialPriceProduct(string processId)
        {
            try
            {
                using (Dao.MaterialPriceDao dao = new MaterialPriceDao())
                {
                    return dao.SelectMaterialPriceProduct(processId);
                }
            }
            catch
            {
                throw;
            }
        }
    }
    
}
