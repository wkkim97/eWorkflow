using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common
{
    public class UserConfigContext : WorkflowContext
    {
        public const string USP_SELECT_USER_CONFIG_DOC_LIST = "[dbo].[USP_SELECT_USER_CONFIG_DOC_LIST] @USER_ID";

        public const string USP_UPDATE_USER_CONFIG = "[eManage].[dbo].[USP_UPDATE_USER_CONFIG] @USER_ID, @MAIN_VIEWTYPE";

        public const string USP_SELECT_USER_CONFIG = "[eManage].[dbo].[USP_SELECT_USER_CONFIG] @USER_ID";

        public const string USP_UPDATE_USER_CONFIG_DOC = "[dbo].[USP_UPDATE_USER_CONFIG_DOC] @USER_ID, @DOCUMENT_ID, @SORT"; 
    }
}
