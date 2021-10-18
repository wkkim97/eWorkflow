using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.WinExe.Sap.Master.Dao
{
    public class MasterDao : DaoBase
    {
        public void InsertSapCustomer(Dto.DTO_SAP_CUSTOMER customer)
        {
            try
            {
                using (context = new MasterContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(customer);

                    context.Database.ExecuteSqlCommand(MasterContext.USP_INSERT_SAP_CUSTOMER, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteSapCustomer()
        {
            try
            {
                using (context = new MasterContext())
                {
                    context.Database.ExecuteSqlCommand(MasterContext.USP_DELETE_SAP_CUSTOMER);
                }
            }catch
            {
                throw;
            }
        }
    }
}
