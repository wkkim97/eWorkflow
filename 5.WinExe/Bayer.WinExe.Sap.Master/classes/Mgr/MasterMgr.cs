using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bayer.WinExe.Sap.Master.Mgr;
using System.Transactions;

namespace Bayer.WinExe.Sap.Master.Mgr
{
    public class MasterMgr : MgrBase
    {
        public void InsertSapCustomer(List<Dto.DTO_SAP_CUSTOMER> customers)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (Dao.MasterDao dao = new Dao.MasterDao())
                    {
                        dao.DeleteSapCustomer();

                        foreach (Dto.DTO_SAP_CUSTOMER customer in customers)
                        {
                            dao.InsertSapCustomer(customer);
                        }
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
