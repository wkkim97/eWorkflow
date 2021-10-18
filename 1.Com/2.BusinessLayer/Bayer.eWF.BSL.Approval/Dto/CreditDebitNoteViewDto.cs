using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class CreditDebitNoteViewDto
    {
        public string TYPE { get; set; }

        public string TO_CODE { get; set; }

        public string TO_NAME { get; set; }

        public string COMPANY_ID { get; set; }

        public DateTime INVOICE_DATE { get; set; }

        public DateTime DUE_DATE { get; set; }

        public string CURRENCY { get; set; }

        public decimal TOTAL_AMOUNT { get; set; }

        public decimal LOCAL_AMOUNT { get; set; }

        public string TO_ADDRESS { get; set; }

        public string COMPANY_NAME { get; set; }

        public string COMPANY_ADDRESS { get; set; }

        public string ATTN_LIST { get; set; }

        public string CC_LIST { get; set; }

        public string DOC_NUM { get; set; }
    }
}
