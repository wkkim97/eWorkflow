using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Dto
{
    public class DTO_CUSTOMER
    {
        public DTO_CUSTOMER()
        {
            this.CREATE_DATE = DateTime.Now;
            this.UPDATE_DATE = DateTime.Now;
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
        public string COMPANY_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BU { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PARVW { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? CREDIT_LIMIT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? MORTAGE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LEVEL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? SALES_RATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? SCORING_RATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CHANNEL { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string CREATOR_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CREATE_DATE { get; set; }

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


    }
    public class DTO_CUSTOMER_KPIS
    {
        public DTO_CUSTOMER_KPIS()
        {
            this.CREATE_DATE = DateTime.Now;
            this.UPDATE_DATE = DateTime.Now;
        }
        public String BCNC_CODE {get; set;}
        public string CUSTOMER_NAME { get; set; }
            
        public String BIZR_NO {get; set;}
        public String BCNC_NM {get; set;}
        public String BCNC_RPRSNTV {get; set;}
        public String BCNC_SE {get; set;}
        public String TELNO {get; set;}
        public String POST_CODE {get; set;}
        public String POST_ADRES { get; set; }



        public string CREATOR_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CREATE_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UPDATER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? UPDATE_DATE { get; set; }
        public string Delete { get; set; }


    }
}
