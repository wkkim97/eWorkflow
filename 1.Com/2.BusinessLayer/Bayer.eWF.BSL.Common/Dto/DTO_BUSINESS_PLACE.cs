using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Dto
{
    public class DTO_BUSINESS_PLACE
    {
        /// <summary>
        /// 
        /// </summary> 
        public int BUSINESS_PLACE_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string BUSINESS_PLACE_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ENG_ADDRESS1 { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ENG_ADDRESS2 { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string KOR_ADDRESS1 { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string KOR_ADDRESS2 { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CREATOR_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime CREATE_DATE { get; set; }

    }
}
