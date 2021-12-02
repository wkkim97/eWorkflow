using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Dto
{
    public class DTO_BOARD_NOTICE
    {
        public DTO_BOARD_NOTICE()
        {
            CREATE_DATE = DateTime.Now;
            UPDATE_DATE = DateTime.Now;
        }
        /// <summary>
        /// 
        /// </summary> 
        public int IDX { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SUBJECT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string BODY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CREATE_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string  CREATE_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime CREATE_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string UPDATE_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime UPDATE_DATE { get; set; }


        public int FILE_IDX { get; set; }

        public string DISPLAY_FILE_NAME { get; set; }

    }
}
