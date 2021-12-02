using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_FREE_GOODS_INFO
    {
        public DTO_DOC_FREE_GOODS_INFO()
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
        public int IDX { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string IS_DOCTOR_PHARMACY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string INSTITUE_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string INSTITUE_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string HCP_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string HCP_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SPECIALTY_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SPECIALTY_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SAMPLE_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SAMPLE_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int QTY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PURPOSE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SAP_PRODUCT_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string STATES { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string OLD_DOC_NUMBER { get; set; }

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
