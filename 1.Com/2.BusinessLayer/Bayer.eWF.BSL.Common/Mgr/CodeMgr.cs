using Bayer.eWF.BSL.Common.Dao;
using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Mgr
{
    public class CodeMgr : MgrBase
    {
        #region [ SUB CODE ]

        public List<Dto.DTO_CODE_SUB> SelectCodeSubList(string classCode)
        {
            try
            {
                using (CodeDao dao = new CodeDao())
                {
                    return dao.SelectCodeSubList(classCode);
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
