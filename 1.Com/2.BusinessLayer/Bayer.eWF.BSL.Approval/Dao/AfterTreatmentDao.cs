using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class AfterTreatmentDao  :DNSoft.eWF.FrameWork.Data.EF.DaoBase
    {
        public void MergeMembershipApplication(string processId)
        {
            try
            {
                using (context = new AfterTreatmentContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(AfterTreatmentContext.USP_MERGE_AFS_MEMBERSHIP_APPLICATION, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        #region After Free Goods

        public void UpdateFreeGoods(string processId, string idx, string howgrid, string Status, string userId)
        {
            try
            {
                using (context = new AfterTreatmentContext())
                {
                    SqlParameter[] parameters = new SqlParameter[5];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", idx);
                    parameters[2] = new SqlParameter("@HOWGRID", howgrid);
                    parameters[3] = new SqlParameter("@STATES", Status);
                    parameters[4] = new SqlParameter("@USERID", userId);                    
                    
                    context.Database.ExecuteSqlCommand(AfterTreatmentContext.USP_UPDATE_FREE_GOODS_STATUS, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        public void UpdateColletoralStatus(string processId)
        {
            try
            {
                using (context = new AfterTreatmentContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(AfterTreatmentContext.USP_UPDATE_MORTGAGE_MANAGEMENT_STATUS, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
