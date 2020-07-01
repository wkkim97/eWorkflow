using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Dao
{
    public class UserConfigDao : DaoBase
    {

        #region SelectUserConfigDocList
        public List<Dto.DTO_USER_CONFIG_DOC_SORT> SelectUserConfigDocList(string userid)
        {
            try
            {
                using (context = new UserConfigContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@USER_ID", userid);
                    var result = context.Database.SqlQuery<Dto.DTO_USER_CONFIG_DOC_SORT>(UserConfigContext.USP_SELECT_USER_CONFIG_DOC_LIST, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        } 
        #endregion
         
        #region UpdateMainViewType
        public void UpdateMainViewType(string userid, string type)
        {
            try
            {
                using (context = new UserConfigContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@USER_ID", userid);
                    parameters[1] = new SqlParameter("@MAIN_VIEWTYPE", type);

                    context.Database.ExecuteSqlCommand(UserConfigContext.USP_UPDATE_USER_CONFIG, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion


        #region SelectUserConfig
        public  Dto.DTO_USER_CONFIG  SelectUserConfig(string userid)
        {
            try
            {
                using (context = new UserConfigContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@USER_ID", userid);
                    var result = context.Database.SqlQuery<Dto.DTO_USER_CONFIG>(UserConfigContext.USP_SELECT_USER_CONFIG, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region UpdateOrderingDocument
        public void UpdateOrderingDocument(Dto.DTO_USER_CONFIG_DOC_SORT sort)
        {
            try
            {
                using (context = new UserConfigContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(sort);
                    context.Database.ExecuteSqlCommand(UserConfigContext.USP_UPDATE_USER_CONFIG_DOC, parameters);
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
