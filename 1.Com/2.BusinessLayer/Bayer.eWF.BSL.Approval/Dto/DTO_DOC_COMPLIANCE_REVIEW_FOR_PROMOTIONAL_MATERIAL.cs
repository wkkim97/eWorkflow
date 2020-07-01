using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL
    {
        public DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL()
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
        public string PROMOTIONAL_MATERIAL { get; set; }

        /// <summary>
        ///
        /// </summary>
        public decimal? UNIT_PRICE { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int? QTY { get; set; }

        /// <summary>
        ///
        /// </summary>
        public decimal? AMT { get; set; }

        /// <summary>
        ///
        /// </summary>
        public decimal? TOTAL_AMOUNT { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string MATERIAL_TYPE { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string TARGET_AUDIENCE { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string PURPOSE { get; set; }

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
