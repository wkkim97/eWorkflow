using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.WinExe.Sap.Master.Dto
{
    public class DTO_SAP_CUSTOMER
    {
        public DTO_SAP_CUSTOMER()
        {
            this.CREATE_DATE = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary> 
        public string CUSTOMER_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CUSTOMER_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CUSTOMER_NAME_KR { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PARVW { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMPANY_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string BU { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime CREATE_DATE { get; set; }


    }
}
