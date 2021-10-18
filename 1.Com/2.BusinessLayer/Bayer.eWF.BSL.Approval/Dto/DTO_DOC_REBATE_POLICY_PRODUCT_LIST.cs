using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_REBATE_POLICY_PRODUCT_LIST
    {        

        public string PROCESS_ID { get; set; }

        public string AS_TO { get; set; }

        public string PRODUCT_CODE { get; set; }

        public string PRODUCT_NAME { get; set; }

        public string CHANNEL_CODE { get; set; }

        public string CHANNEL_NAME { get; set; }

        public string DISTRIBUTION { get; set; }

        public decimal? LIST { get; set; }

        public decimal? INVOICE { get; set; }

        public decimal? NET1 { get; set; }

        public decimal? NET2 { get; set; }

        public string CREATOR_ID { get; set; }
    }
    
}
