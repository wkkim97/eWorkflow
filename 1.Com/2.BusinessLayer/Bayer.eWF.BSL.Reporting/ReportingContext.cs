using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Reporting
{
    public class ReportingContext : WorkflowContext
    {
        public const string USP_SELECT_REPORTING_PROGRAM = "[dbo].[USP_SELECT_REPORTING_PROGRAM] @USER_ID, @LOCATION ";
    }
}
