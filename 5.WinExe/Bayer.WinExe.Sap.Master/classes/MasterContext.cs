using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.WinExe.Sap.Master
{
    public class MasterContext : WorkflowContext
    {
        public const string USP_INSERT_SAP_CUSTOMER = "[eManage].[dbo].[USP_INSERT_SAP_CUSTOMER] @CUSTOMER_CODE, @CUSTOMER_NAME, @CUSTOMER_NAME_KR, @COMPANY_CODE, @PARVW, @BU";

        public const string USP_DELETE_SAP_CUSTOMER = "[eManage].[dbo].[USP_DELETE_SAP_CUSTOMER]";
    }
}
