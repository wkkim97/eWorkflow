using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Dto
{
    public class DTO_LOGIN_HISTORY
    {
        /// <summary>
        /// 
        /// </summary> 
        public string FULL_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string USER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string MAILADDRESS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ORG_ACRONYM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CLIENTIP { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? CREATE_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string WINDOWUSERNAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string WINDOWDOMAINNAME { get; set; }


    }
}
