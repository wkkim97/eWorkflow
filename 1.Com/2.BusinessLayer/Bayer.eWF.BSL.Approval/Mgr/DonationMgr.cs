using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DNSoft.eW.FrameWork;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class DonationMgr : MgrBase
    {
        #region [MergeDonation]
        public string MergeDonation(Dto.DTO_DOC_DONATION doc, List<Dto.DTO_DOC_DONATION_PRODUCT> products)
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
                    using (Dao.DonationDao dao = new Dao.DonationDao())
                    {
                        dao.MergeDonation(doc);
                        dao.DeleteDonationProduct(processId);

                        foreach (Dto.DTO_DOC_DONATION_PRODUCT product in products)
                        {
                            if (product.PRODUCT_CODE.IsNullOrEmptyEx()) continue;
                            product.PROCESS_ID = processId;
                            dao.MergeDonationProduct(product);
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

        #region [SelectDonation]
        public Dto.DTO_DOC_DONATION SelectDonation(string processId)
        {
            try
            {
                using (Dao.DonationDao dao = new Dao.DonationDao())
                {
                    return dao.SelectDonation(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region[SelectDonationProduct]
        public List<Dto.DTO_DOC_DONATION_PRODUCT> SelectDonationProduct(string processId)
        {
            try
            {
                using (Dao.DonationDao dao = new Dao.DonationDao())
                {
                    return dao.SelectDonationProduct(processId);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteDonationProductByIndex]
        public void DeleteDonationProductByIndex(string processId, int idx)
        {
            try
            {
                using (Dao.DonationDao dao = new Dao.DonationDao())
                {
                    dao.DeleteDonationProductByIndex(processId, idx);
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
