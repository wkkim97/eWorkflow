using Bayer.eWF.BSL.Approval.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Data.EF;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class CompetitorContractMgr : MgrBase
    {
        public string MergeCompetitorContract(Dto.DTO_DOC_COMPETITOR_CONTRACT doc, List<Dto.DTO_DOC_COMPETITOR_CONTRACT_PARTICIPANT> participants)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
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
                    using (CompetitorContractDao dao = new CompetitorContractDao())
                    {
                        dao.MergeCompetitorContract(doc);

                        dao.DeletCompetitorContractParticipants(processId);

                        foreach (Dto.DTO_DOC_COMPETITOR_CONTRACT_PARTICIPANT participant in participants)
                        {
                            participant.PROCESS_ID = processId;
                            dao.MergeCompetitorContractParticipants(participant);
                        }
                    }

                    scope.Complete();
                    return processId;
                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_COMPETITOR_CONTRACT SelectCompetitorContract(string processId)
        {
            try
            {
                using (CompetitorContractDao dao = new CompetitorContractDao())
                {
                    return dao.SelectCompetitorContract(processId);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_COMPETITOR_CONTRACT_PARTICIPANT> SelectCompetitorContractParticipants(string processId)
        {
            try
            {
                using (CompetitorContractDao dao = new CompetitorContractDao())
                {
                    return dao.SelectCompetitorContractParticipants(processId);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
