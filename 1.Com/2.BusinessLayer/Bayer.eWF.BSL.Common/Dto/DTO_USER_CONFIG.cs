using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Dto
{
    public class DTO_USER_CONFIG_DOC_SORT
    {
        public string USER_ID { get; set; }
        public string DOCUMENT_ID { get; set; }
        public string DOC_NAME { get; set; }
        public Int32 SORT { get; set; }
        public string FORM_NAME { get; set; }
        public string DOC_DESCRIPTION { get; set; }
    }

    public class DTO_USER_CONFIG
    {
         public string USER_ID { get; set; }
         public string MAIN_VIEWTYPE { get; set; }
    }
}
