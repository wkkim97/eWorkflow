using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_BCS_RETURN_GOODS_GRID
    {        
        public string PROCESS_ID { get; set; }

        public string CU_RE { get; set; }

        public int GRID_INDEX { get; set; }

        public int IDX { get; set; }

        public string CUSTOMER_CODE { get; set; }

        public string CUSTOMER_NAME { get; set; }

        public string CUSTOMER_NAME_NEW { get; set; }

        public string PARVW { get; set; }


        public string PRODUCT_CODE { get; set; }

        public string PRODUCT_NAME { get; set; }

        public int? QTY { get; set; }
        //eWorkflow Optimization 2020 START
        public decimal? RETURN_PRICE { get; set; }
        public decimal? RETURN_PRICE_NEW { get; set; }

        public decimal? DIFFERENCE { get; set; }
        public decimal? TOTAL_AMOUNT { get; set; }
        //eWorkflow Optimization 2020 END

        public string REASON { get; set; }

        public decimal? INVOICE_PRICE { get; set; }

        public decimal? AMOUNT { get; set; }

        public string CREATOR_ID { get; set; }
    }
}
