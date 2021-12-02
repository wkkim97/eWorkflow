using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_BVS_SEED_IMPORT_PRODUCT    
    {
        
        public string PROCESS_ID { get; set; }

        public int IDX { get; set; }

        public string PRODUCT_CODE { get; set; }

        public string PRODUCT_NAME { get; set; }

        public string CROP { get; set; }

        public string VARIETY { get; set; }

        public string SF_ST { get; set; }

        public decimal? TP_PRICE { get; set; }

        public decimal? AMOUNT { get; set; }

        public decimal? QTY_EA { get; set; }
        
        public decimal? FACTOR { get; set; }

        public decimal? QTY_TH { get; set; }

        public string CREATOR_ID { get; set; }
    }
}
