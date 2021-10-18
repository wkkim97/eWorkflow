using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_BCS_PROCESS_CHANGE
    {

        public DTO_DOC_BCS_PROCESS_CHANGE()
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

        /// <summary>  ///
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
        public string TITLE { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string CATEGORY { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string PURPOSE_TYPE { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string DURATION_TYPE { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime? FROM_DATE { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime? TO_DATE { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string BRIEF_DESCRIPTION { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string EFFECT_BEFORE_AFTER { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string AFFECTED_PRODUCT { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime? PROPOSED_DUE_DATE { get; set; }

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
