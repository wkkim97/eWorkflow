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
    public class MortgageManagementMgr : MgrBase
    {
        #region MergeMortgageManagement
        public string MergeMortgageManagement(Dto.DTO_DOC_MORTGAGE_MANAGMENT doc)
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
                    using (Dao.MortgageManagementDao dao = new MortgageManagementDao())
                    {
                        dao.MergeMortgageManagement(doc);
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

        #region SelectMortgageManagement
        public Dto.DTO_DOC_MORTGAGE_MANAGMENT SelectMortgageManagement(string processId)
        {
            try
            {
                using (Dao.MortgageManagementDao dao = new MortgageManagementDao())
                {
                    return dao.SelectMortgageManagement(processId);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        public decimal SelectNewCreditLimit(string customer, string bg, string status, decimal amount)
        {
            try
            {
                using (Dao.MortgageManagementDao dao = new MortgageManagementDao())
                {
                    return dao.SelectNewCreditLimit(customer, bg, status, amount);
                }
            }
            catch
            {
                throw;
            }
        }

        public void UpdateCreditLimit(string processId, string customer, string bg, string status, string updater, decimal amount, string curreltlyId)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (Dao.MortgageManagementDao dao = new MortgageManagementDao())
                    {
                        dao.UpdateCreditLimitStatus(processId, status, updater, curreltlyId);
                        dao.UpdateCreditLimit(customer, bg, amount);
                    }
                    scope.Complete();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
