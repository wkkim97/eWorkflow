using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Dto
{
    public class UserInfoDto
    {
        /// <summary>
        /// 
        /// </summary> 
        public string USER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CREATED { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SUPERVISOR_USER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string IT_STATUS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SPONSOR_USER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PRIMARY_USER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SOURCE_HR { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string LAST_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string FIRST_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string FULL_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string NAME_ALIAS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ORG_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ORG_ACRONYM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMPANY_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PHONE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string MOBILE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string FAX { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string MAIL_ADDRESS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string MODIFIED { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string FORM_OF_ADDRESS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMPANY_CODE { get; set; }


    }

    public class SmallUserInfoDto
    {
        /// <summary>
        /// 
        /// </summary> 
        public string USER_ID { get; set; } 

        /// <summary>
        /// 
        /// </summary> 
        public string FULL_NAME { get; set; }
         

        /// <summary>
        /// 
        /// </summary> 
        public string COMPANY_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string MAIL_ADDRESS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ORG_ACRONYM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IPIN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string COST_CENTER { get; set; }

        /// <summary>
        /// 성별
        /// </summary>
        public string FORM_OF_ADDRESS { get; set; }
    }
    
}
