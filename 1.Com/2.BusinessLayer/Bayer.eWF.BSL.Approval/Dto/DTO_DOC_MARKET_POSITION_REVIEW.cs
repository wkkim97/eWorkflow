using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_MARKET_POSITION_REVIEW
    {
        public DTO_DOC_MARKET_POSITION_REVIEW() 
        {
            this.CREATE_DATE = DateTime.Now;
            this.UPDATE_DATE = DateTime.Now;
        }
        /// <summary>
        /// 
        /// </summary> 
        public string PROCESS_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SUBJECT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DOC_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PROCESS_STATUS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string REQUESTER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime REQUEST_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMPANY_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ORGANIZATION_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string LIFE_CYCLE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string BU { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string YEAR { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PRODUCT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string APPLICATION_INDICATION { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMPETITOR { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public double? MARKET_SHARE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string MARKET_SHARE_VOLUME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string MARKET_SHARE_VALUE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public double? FIRST_MARKET_SHARE { get; set; }
        public double? SECOND_MARKET_SHARE { get; set; }
        public double? THIRD_MARKET_SHARE { get; set; }
        public double? TOTAL_MARKET_SHARE { get; set; }
        
        public string FIRST_PRODUCT_COMPANY_NAME { get; set; }
        public string SECOND_PRODUCT_COMPANY_NAME { get; set; }

        public string THIRD_PRODUCT_COMPANY_NAME { get; set; }



        /// <summary>
        /// 
        /// </summary> 
        public string DATA_SOURCE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CHECKED_DISCRIMINATION { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CHECKED_EXCESSIVE_PRICING { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CHECKED_PREDATORY_PRICING { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CHECKED_MARGIN_SQUEEZE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CHECKED_TRYING_AND_BUNDING { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CHECKED_LOYALTY_AND_DISCOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CHECKED_ABUSIVE_EXCLUSIVE_DEALING { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string CHECKED_ABUSIVE_REFUSAL_TO_SUPPLY { get; set; }


        /// <summary>
        /// 
        /// </summary> 
        public string IS_DISUSED { get; set; }

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


    }
}
