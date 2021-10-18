using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_RESALE_DATA_COLLECTION
    {
        public DTO_DOC_RESALE_DATA_COLLECTION()
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
        public string DATA_SOURCE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string RESALE_DATA_DETAILS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? RESALE_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int OTHERS { get; set; }
        //LPC-forMonsanto User- start
        public string PRICEVALUE { get; set; }
        public string Employee { get; set; }
        //LPC-forMonsanto User- End
        /// <summary>
        /// 
        /// </summary> 
        public string COLLECTION_PURPOSE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DATA_ARCHIVING { get; set; }

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
