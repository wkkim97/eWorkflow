using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_IT_RESOURCE
    {
        public DTO_DOC_IT_RESOURCE()
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
        public string CATEGORY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string RESOURCE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string LOSS_REASON { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SECURITY_REPORT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string STATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMPUTER_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMPUTER_MODEL { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string MOBILE_MODEL { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string MOBILE_COLOR { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string NEW_MODEL { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string NEW_PURPOSE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SOFTWARE_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SOFTWARE_PURPOSE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IS_PERMANENT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? SOFTWARE_FROM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? SOFTWARE_TO { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SERVICE_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SERVICE_ACCOUNT_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SERVICE_MDM_MODEL { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SERVICE_MDM_MAC { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SERVICE_MDM_SERIAL_NUMBER { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SERVICE_MDM_PHONE_NUMBER { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SERVICE_IPT_PHONE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SERVICE_IPT_MAC { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SERVICE_IPT_MODEL { get; set; }

        public string SERVICE_BYOS_AGREE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SERVICE_PURPOSE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? SERVICE_FROM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? SERVICE_TO { get; set; }

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
