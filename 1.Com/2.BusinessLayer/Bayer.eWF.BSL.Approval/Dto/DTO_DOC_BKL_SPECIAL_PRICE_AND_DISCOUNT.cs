using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT
    {
        public DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT()
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
        public string PRODUCT_FAMILY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ATTACH_CHECK { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TYPE { get; set; }
        /// <summary>
        /// 
        /// </summary> 
        public string CHECK_CONFIG { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string WHOLESALER_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string WHOLESALER_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SECOND_WHOLESALER { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? CONTRACT_FROM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CONTRACT_TO { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? CONTRACT_FROM_W_BKL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CONTRACT_TO_W_BKL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EXTENTION_AGREEMENT_CHECK { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RE_EWORKFLOW_DOC_NO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EXTENSION_FROM_W_BKL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EXTENSION_TO_W_BKL { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string REMARK { get; set; }
        //INC0003399184 EXCEPTION SAMEDISCOUNT
        public string EXCEPTION { get; set; }
        public string SAMEDISCOUNT { get; set; }


        /// <summary>
        /// 
        /// </summary> 
        public decimal? TOTAL_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DEAL_NO { get; set; }

        public string IS_DISUSED { get; set; }

        //2022.05.27 add new members for class, for change INC14831972 start add memeber for classs, rebuild Bayer.eWF.BSL.Approval.dll
        public string BACKGROUND { get; set; }
        public string PROPOSAL { get; set; }
        public string PROCESS { get; set; }
        public string FINANCIAL_IMPACT { get; set; }
        public string COMMENT { get; set; }
        public string SPPriceExcp { get; set; }
        //2022.05.27 add for change INC14831972 end


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
