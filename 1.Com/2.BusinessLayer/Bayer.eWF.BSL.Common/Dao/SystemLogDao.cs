using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Dao
{
    public class SystemLogDao : DaoBase
    {
        #region InsertSystemLog
        public void InsertSystemLog(Dto.DTO_SYSTEM_LOG log)
        {
            try
            {
                using (context = new SystemLogContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(log);
                    context.Database.ExecuteSqlCommand(SystemLogContext.USP_INSERT_SYSTEM_LOG, parameters);
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
