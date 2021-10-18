using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Dto;
using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Bayer.eWF.BSL.Common.Mgr
{
    public class UserMgr : MgrBase
    {
        public List<Dto.SmallUserInfoDto> SelectUserList(string UserName)
        {
            try
            {
                using (UserDao dao = new UserDao())
                {
                    return dao.SelectUserList(UserName);
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
                using (UserDao dao = new UserDao())
                {
                    return dao.SelectUserGlobalList(userName);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.SmallUserInfoDto> SelectApprovalUserList(string userName)
        {
            try
            {
                using (UserDao dao = new UserDao())
                {
                    return dao.SelectApprovalUserList(userName);
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
                using (UserDao dao = new UserDao())
                {
                    return dao.SelectDelegationUserList(userid);
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
