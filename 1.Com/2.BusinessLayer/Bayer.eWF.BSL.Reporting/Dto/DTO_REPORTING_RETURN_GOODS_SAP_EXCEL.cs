using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Reporting.Dto
{
    public class DTO_REPORTING_RETURN_GOODS_SAP_EXCEL
    {
        /// <summary>
        /// 
        /// </summary> 
        public int IDX { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SHIPTO_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PRODUCT_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string BATCH { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SN { get; set; }


        public double QTY { get; set; }
        /// <summary>
        /// 
        /// </summary> 
        public decimal SAP_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal UNIT_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime CREATE_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CREATE_ID { get; set; }


    }
}
