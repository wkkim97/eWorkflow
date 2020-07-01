using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_RX_VOLUME_DATA_COLLECTION
    {
        public DTO_DOC_RX_VOLUME_DATA_COLLECTION()
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
        public string COLLECTED_PRODUCT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DATA_SOURCE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DETAILS_OF_RX_VOLUME_DATA { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DETAILS_OF_RX_VOLUME_DATA_YN { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string INCLUDING_COMPETITIVE_DATA_AND_DETAILS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string INCLUDING_COMPETITIVE_DATA_AND_DETAILS_YN { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PURPOSE_OF_COLLECTION { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COLLECTION_METHOD { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime VALIDITY_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ARCHIVING_OF_DATA { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ACCESS_RIGHT { get; set; }

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
