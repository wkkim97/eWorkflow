using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DNSoft.eW.FrameWork;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class CreditLimitSettingMgr : MgrBase
    {
        #region [MergeCreditLimitSetting]
        public string MergeCreditLimitSetting(Dto.DTO_DOC_CREDIT_LIMIT_SETTING doc)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (doc.PROCESS_ID.NullObjectToEmptyEx().Length == 0)
                    {
                        using (Dao.CommonDao dao = new Dao.CommonDao())
                        {
                            doc.PROCESS_ID = dao.GetNewProcessID();

                        }
                    }
                    using (Dao.CreditLimitSettingDao dao = new Dao.CreditLimitSettingDao())
                    {
                        dao.MergeCreditLimitSetting(doc);
                    }
                    scope.Complete();
                    return doc.PROCESS_ID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [SelectCreditLimitSetting]
        public Dto.DTO_DOC_CREDIT_LIMIT_SETTING SelectCreditLimitSetting(string processId)
        {
            try
            {
                using (Dao.CreditLimitSettingDao dao = new Dao.CreditLimitSettingDao())
                {
                    return dao.SelectCreditLimitSetting(processId);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        
    }
}
