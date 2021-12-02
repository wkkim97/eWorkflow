using Bayer.eWF.BSL.Approval.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DNSoft.eW.FrameWork;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class ITResourceMgr : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public string MergeITResource(Dto.DTO_DOC_IT_RESOURCE doc)
        {
            try
            {
                string processId = doc.PROCESS_ID;
                if (processId.NullObjectToEmptyEx().Length == 0)
                {
                    using (Dao.CommonDao dao = new Dao.CommonDao())
                    {
                        processId = dao.GetNewProcessID();
                        doc.PROCESS_ID = processId;
                    }
                }
                using (ITResourceDao dao = new ITResourceDao())
                {
                    dao.MergeITResource(doc);
                }
                return processId;
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_IT_RESOURCE SelectITResource(string processId)
        {
            try
            {
                using (ITResourceDao dao = new ITResourceDao())
                {
                    return dao.SelectITResource(processId);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
