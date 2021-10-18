using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Dto
{
    public class DTO_ATTACH_FILES
    {
        /// <summary>
        /// 
        /// </summary> 
        public int IDX { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PROCESS_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int SEQ { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ATTACH_FILE_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int COMMENT_IDX { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DISPLAY_FILE_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SAVED_FILE_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? FILE_SIZE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string FILE_PATH { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string FILE_URL { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CREATOR_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime CREATE_DATE { get; set; }
    }

    public class DTO_FILESTORAGE
    {
        /// <summary>
        /// 
        /// </summary> 
        public int StorageID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SubSystemTypeID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string FilePath { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ImagePath { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string FileURL { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ImageURL { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string Description { get; set; }
    }
     

}
