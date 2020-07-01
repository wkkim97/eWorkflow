using Bayer.eWF.BSL.Approval.Dao;
using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DNSoft.eW.FrameWork;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class MembershipApplicationMgr : MgrBase
    {
        public string MergeMembershipApplication(Dto.DTO_DOC_MEMBERSHIP_APPLICATION doc)
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
                using (MembershipApplicationDao dao = new MembershipApplicationDao())
                {
                    dao.MergeMembershipApplication(doc);
                }

                return processId;
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_MEMBERSHIP_APPLICATION SelectMembershipApplication(string processId)
        {
            try
            {
                using (MembershipApplicationDao dao = new MembershipApplicationDao())
                {
                    return dao.SelectMembershipApplication(processId);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
