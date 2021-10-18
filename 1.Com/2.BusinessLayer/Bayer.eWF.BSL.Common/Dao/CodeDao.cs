using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Dao
{
    public class CodeDao : DaoBase
    {
        #region [ SUB CODE ]

        public List<Dto.DTO_CODE_SUB> SelectCodeSubList(string classCode)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@CLASS_CODE", classCode);

                    var result = context.Database.SqlQuery<Dto.DTO_CODE_SUB>(CommonContext.USP_SELECT_CODE_SUB, parameters);

                    return result.ToList();
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
