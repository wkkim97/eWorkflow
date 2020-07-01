using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Dto
{
    public class DTO_SYSTEM_LOG
    {
        public DTO_SYSTEM_LOG()
        {
            CREATE_DATE = DateTime.Now;
        }
        /// <summary>
        /// 
        /// </summary> 
        public int IDX { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string EVENT_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string MESSAGE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? CREATE_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CREATER_ID { get; set; }


    }
}
