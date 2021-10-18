using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Dao
{
    public class UserDao : DaoBase
    {

        public List<Dto.SmallUserInfoDto> SelectUserList(string UserName)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@KEYWOARD", UserName);
                    var result = context.Database.SqlQuery<Dto.SmallUserInfoDto>(UserContext.USP_SELECT_USER_LIST, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.SmallUserInfoDto> SelectUserGlobalList(string userName)
        {
            try
            {
                using (context = new UserContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];

                    parameters[0] = new SqlParameter("@KEYWOARD", userName);
                    var result = context.Database.SqlQuery<Dto.SmallUserInfoDto>(UserContext.USP_SELECT_USER_GLOBAL_LIST, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }


        public List<Dto.SmallUserInfoDto> SelectApprovalUserList(string UserName)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@KEYWOARD", UserName);
                    var result = context.Database.SqlQuery<Dto.SmallUserInfoDto>(UserContext.USP_SELECT_APPROVAL_TARGET_USER_LIST, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.SmallUserInfoDto> SelectDelegationUserList(string userid)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@USER_ID", userid);
                    var result = context.Database.SqlQuery<Dto.SmallUserInfoDto>(UserContext.USP_SELECT_USER_LIST_DELEGATION, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
