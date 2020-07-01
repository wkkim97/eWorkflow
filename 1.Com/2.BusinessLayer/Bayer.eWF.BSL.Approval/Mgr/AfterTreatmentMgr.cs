using Bayer.eWF.BSL.Approval.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class AfterTreatmentMgr : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public void MergeMembershipApplication(string processId)
        {
            try
            {
                using (AfterTreatmentDao dao = new AfterTreatmentDao())
                {
                    dao.MergeMembershipApplication(processId);
                }
            }
            catch
            {
                throw;
            }
        }


        #region AFTER FREE GOODS 
        public void UpdateFreeGoods(string processId, string idx, string howgrid, string Status, string userId)
        {
            try
            {
                using (AfterTreatmentDao dao = new AfterTreatmentDao())
                {
                    dao.UpdateFreeGoods(processId, idx, howgrid, Status, userId);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        public void UpdateCollateralStatus(string processId)
        {
            try
            {
                using (AfterTreatmentDao dao = new AfterTreatmentDao())
                {
                    dao.UpdateColletoralStatus(processId);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
