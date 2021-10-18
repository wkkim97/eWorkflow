using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT
    {
        public DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT()
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

        public string PRODUCT_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PRODUCT_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DEMAND_CODE { get; set; }


        /// <summary>
        /// 
        /// </summary> 
        public string DEMAND_NAME { get; set; }


        /// <summary>
        /// 
        /// </summary> 
        public string POPULATION_NO { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int ISSUE_QTY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ISSUE_UNIT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ISSUE_TEXT { get; set; }

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
