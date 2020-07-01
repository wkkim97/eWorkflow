using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common
{
    public class SystemLogContext : WorkflowContext
    {
        public const string USP_INSERT_SYSTEM_LOG = "[eManage].[dbo].[USP_INSERT_SYSTEM_LOG] @TYPE, @EVENT_NAME, @MESSAGE, @CREATE_DATE, @CREATER_ID";

    }
}
