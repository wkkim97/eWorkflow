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
    public class BvsSeedImportMgr : MgrBase
    {
        #region MergeBvsSeedImport
        public string MergeBvsSeedImport(Dto.DTO_DOC_BVS_SEED_IMPORT doc, List<Dto.DTO_DOC_BVS_SEED_IMPORT_PRODUCT> products)
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
                    using (Dao.BvsSeedImportDao dao = new BvsSeedImportDao())
                    {
                        dao.MergeBvsSeedImport(doc);
                        dao.DeleteBvsSeedImportProductAll(doc.PROCESS_ID);

                        foreach (Dto.DTO_DOC_BVS_SEED_IMPORT_PRODUCT product in products)
                        {
                            product.PROCESS_ID = doc.PROCESS_ID;
                            dao.MergeBvsSeedImportProduct(product);
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

        #region [SelectBvsSeedImport]
        public Dto.DTO_DOC_BVS_SEED_IMPORT SelectBvsSeedImport(string processId)
        {
            try
            {
                using (Dao.BvsSeedImportDao dao = new Dao.BvsSeedImportDao())
                {
                    return dao.SelectBvsSeedImport(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region[SelectBvsSeedImportProduct]
        public List<Dto.DTO_DOC_BVS_SEED_IMPORT_PRODUCT> SelectBvsSeedImportProduct(string processId)
        {
            try
            {
                using (Dao.BvsSeedImportDao dao = new Dao.BvsSeedImportDao())
                {
                    return dao.SelectBvsSeedImportProduct(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [DeleteSampleRequestItemsByIndex]
        public void DeleteBvsSeedImportItemsByIndex(string processId, int idx)
        {
            try
            {
                using (Dao.BvsSeedImportDao dao = new Dao.BvsSeedImportDao())
                {
                    dao.DeleteBvsSeedImportItemsByIndex(processId, idx);
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
