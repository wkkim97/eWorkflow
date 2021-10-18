using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Approval.Dao;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class InteractionHealthcareProMgr : MgrBase
    {
        public string MergeInteractionHealthcarePro(Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO doc
               , List<Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO_COUNTRY> countrys
               , List<Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL> details)
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
                    using (InteractionHealthcareProDao dao = new InteractionHealthcareProDao())
                    {
                        dao.MergeInteractionHealthcarePro(doc);

                        dao.DeleteInteractionHealthcareProDetailAll(processId);

                        foreach (Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL detail in details)
                        {
                            detail.PROCESS_ID = processId;
                            dao.MergeInteractionHealthcareProDetail(detail);
                        }

                        dao.DeleteInteractionHealthcareProCountryAll(processId);

                        foreach (Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO_COUNTRY country in countrys)
                        {
                            country.PROCESS_ID = processId;
                            dao.MergeInteractionHealthcareProCountry(country);
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

        public void DeleteInteractionHealthcareProDetail(string processId, int index)
        {
            try
            {
                using (InteractionHealthcareProDao dao = new InteractionHealthcareProDao())
                {
                    dao.DeleteInteractionHealthcareProDetail(processId, index);
                }
            }
            catch
            {
                throw;
            }
        }

        public Tuple<Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO
            , List<Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO_COUNTRY>> SelectInteractionHealthcarePro(string processId)
        {
            try
            {
                using (InteractionHealthcareProDao dao = new InteractionHealthcareProDao())
                {
                    Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO doc = dao.SelectInteractionHealthcarePro(processId);

                    List<Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO_COUNTRY> countrys = dao.SelectInteractionHealthcareProCountry(processId);


                    return new Tuple<Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO
                    , List<Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO_COUNTRY>>(doc, countrys);
                }
            }
            catch
            {
                throw;
            }
        }


        public List<Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL> SelectInteractionHealthcareProDetails(string processId)
        {
            try
            {
                using(InteractionHealthcareProDao dao = new InteractionHealthcareProDao())
                {
                    return dao.SelectInteractionHealthcareProDetails(processId);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
