using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Configuration.Dto
{
    public class DTO_CONFIG
    {
        /// <summary>
        /// 결재문서 ID
        /// </summary> 
        public string DOCUMENT_ID { get; set; }

        /// <summary>
        /// Table Name
        /// </summary> 
        public string TABLE_NAME { get; set; }

        /// <summary>
        /// 결재문서 명
        /// </summary> 
        public string DOC_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DATA_OWNER { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DATA_OWNER_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PREFIX_DOC_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FORM_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string READERS_GROUP_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CATEGORY_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string RETENTION_PERIOD { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RETENTION_PERIOD_TEXT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string FORWARD_YN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AFTER_TREATMENT_SERVICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CLASSIFICATION_INFO { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string APPROVAL_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int APPROVAL_LEVEL { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string JOB_TITLE_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ADD_ADDTIONAL_APPROVER { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ADD_APPROVER_POSITION { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ADD_REVIEWER { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ADD_REVIEWER_DESCRIPTION { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DOC_DESCRIPTION { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DOC_IMAGE_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DOC_IMAGE_PATH { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DISPLAY_DOC_LIST { get; set; }

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
