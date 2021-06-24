using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_CONTRACT_MANAGEMENT
    {
        public DTO_DOC_CONTRACT_MANAGEMENT()
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
        public string TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ESSENTIAL_CONTRACT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string INTRAGROUP { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CROSS_BORDER { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CONTRACT_TOTAL_VALUE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TITLE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? TERM_FROM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? TERM_TO { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CONTRACT_PARTNER { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? CONTRACT_VALUE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CONTRACT_CATEGORY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PRIVACY_INFORMATION { get; set; }

        public string PIPA_PURPOSE { get; set; }
        public string PIPA_EVENT { get; set; }
        public string PIPA_CONTRACT { get; set; }
        public string PIPA_PURPOSE_PI { get; set; }
        public string PIPA_PURPOSE_PI_OTHER { get; set; }
        public string PIPA_TARGET { get; set; }
        public string PIPA_COLLECTION { get; set; }
        public string PIPA_ARCHIVING { get; set; }
        public string PIPA_ARCHIVING_OTHER { get; set; }
        public string PIPA_PERMISSION { get; set; }
        public string PIPA_VOLUMN { get; set; }
        public string PIPA_RETENTION { get; set; }
        public string PIPA_3RDPARTY { get; set; }
        
        public string PIPA_OVERSEA { get; set; }
        

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
