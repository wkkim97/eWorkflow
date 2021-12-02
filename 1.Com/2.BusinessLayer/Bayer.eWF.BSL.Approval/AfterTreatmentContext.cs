using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval
{
    public class AfterTreatmentContext : WorkflowContext
    {
        public const string USP_MERGE_AFS_MEMBERSHIP_APPLICATION = "[dbo].[USP_MERGE_AFS_MEMBERSHIP_APPLICATION] @PROCESS_ID"; 


        #region After Free Goods

        public const string USP_UPDATE_FREE_GOODS_STATUS = "[dbo].[USP_UPDATE_FREE_GOODS_STATUS] @PROCESS_ID, @IDX, @HOWGRID, @STATES, @USERID";

        #endregion

        public const string USP_UPDATE_MORTGAGE_MANAGEMENT_STATUS = "[dbo].[USP_UPDATE_MORTGAGE_MANAGEMENT_STATUS] @PROCESS_ID";
    }
}
