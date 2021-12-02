using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common
{
    public class UserContext : WorkflowContext
    {
        public const string USP_SELECT_USER_LIST = "[eManage].[dbo].[USP_SELECT_USER_LIST] @KEYWOARD";

        public const string USP_SELECT_USER_GLOBAL_LIST = "[eManage].[dbo].[USP_SELECT_USER_GLOBAL_LIST] @KEYWOARD";

        public const string USP_SELECT_APPROVAL_TARGET_USER_LIST = "[eManage].[dbo].[USP_SELECT_APPROVAL_TARGET_USER_LIST] @KEYWOARD";

        public const string USP_SELECT_USER_LIST_DELEGATION = "[eManage].[dbo].[USP_SELECT_USER_LIST_DELEGATION] @USER_ID";
        
    }
}
