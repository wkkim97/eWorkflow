using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Configuration.Dto
{
    public class DTO_READERS_GROUP_USER_LIST
    {
        /// <summary>
        /// 
        /// </summary> 
        public string GROUP_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string USER_ID { get; set; }

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
	public class DTO_READERS_GROUP_USER_NAME
	{       
		public string USER_ID { get; set; }
		public string FULL_NAME { get; set; }
        public string COST_CENTER { get; set; }
	}
   
}
