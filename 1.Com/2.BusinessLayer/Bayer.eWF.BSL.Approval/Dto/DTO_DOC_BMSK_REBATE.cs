﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_BMSK_REBATE
    {
        public DTO_DOC_BMSK_REBATE()
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
        public string TITLE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string BU { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? VALIDITY_FROM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? VALIDITY_TO { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TARGET_PERFORM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ESTIMATED_PERFORM { get; set; }

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
