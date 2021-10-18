using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Dto
{
    public class DTO_PRODUCT
    {
        /// <summary>
        /// 
        /// </summary> 
        public string PRODUCT_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PRODUCT_NAME { get; set; }

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
        public decimal? BASE_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SAMPLE_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SAMPLE_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string USE_SAMPLE_DC { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string MRP_FLAG { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double MARGIN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal INVOICE_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? NET1_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? NET1_PRICE_NH { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? NET2_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? NET2_PRICE_NH { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? INVOICE_PRICE_NH { get; set; }


        public decimal TP_PRICE { get; set; }
        public string CROP { get; set; }
        public string VARIETY { get; set; }
        public string SF_ST { get; set; }
        public decimal FACTOR { get; set; }


        /// <summary>
        /// 
        /// </summary> 
        public string CREATOR_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? CREATE_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string UPDATER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? UPDATE_DATE { get; set; }

        public string KPIS { get; set; }

        public string VISIBILITY { get; set; }

        public string PRODUCT_NAME_KR { get; set; }

    }
    public class DTO_PRODUCT_KPIS
    {
        public string PRDUCT_CODE { get; set; }
        public string PRODUCT_NAME_KR { get; set; }

        public string STD_CODE { get; set; }
        public string REPRSNT_CODE { get; set; }
        public string REPRSNT_NM { get; set; }
        public string REPORT_YN { get; set; }
        public decimal? PACKNG_QY { get; set; }
        public string C_STD_CODE { get; set; }
        public string C_PRODUCT_KOR_NM { get; set; }
        public string C_REPRSNT_CODE { get; set; }
        public string C_PEPRSNT_NM { get; set; }
        public decimal? C_PACKNG_QY { get; set; }
        public decimal? MULTIPLICATION { get; set; }

        public string CREATOR_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? CREATE_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string UPDATER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? UPDATE_DATE { get; set; }


    }


}
