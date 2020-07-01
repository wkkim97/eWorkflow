using Bayer.eWF.BSL.Common.Dao;
using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Bayer.eWF.BSL.Common.Mgr
{
    public class UserConfigMgr : MgrBase
    {
        #region [ SelectUserConfigDocList ]

        public List<Dto.DTO_USER_CONFIG_DOC_SORT> SelectUserConfigDocList(string userid)
        {
            try
            {
                using (UserConfigDao dao = new UserConfigDao())
                {
                    return dao.SelectUserConfigDocList(userid);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region [ UpdateMainViewType ]

        public void UpdateMainViewType(string userid, string type)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (UserConfigDao dao = new UserConfigDao())
                    {
                        dao.UpdateMainViewType(userid, type);
                    }
                    scope.Complete();
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion


        #region [ SelectUserConfig ]

        public Dto.DTO_USER_CONFIG SelectUserConfig(string userid)
        {
            try
            {
                using (UserConfigDao dao = new UserConfigDao())
                {
                    return dao.SelectUserConfig(userid);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion



        #region [ UpdateOrderingDocument ]

        public void UpdateOrderingDocument(List<Dto.DTO_USER_CONFIG_DOC_SORT> sort)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (UserConfigDao dao = new UserConfigDao())
                    {
                        foreach (Dto.DTO_USER_CONFIG_DOC_SORT item in sort)
                        {
                            dao.UpdateOrderingDocument(item);
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

        #endregion
    }
}
