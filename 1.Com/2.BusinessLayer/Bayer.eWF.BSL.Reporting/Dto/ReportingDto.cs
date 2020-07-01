using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Reporting.Dto
{
    public class DetailSearch
    {
        public string DOCUMENT_ID { get; set; }
        public string SUBJECT { get; set; }
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }

    }

    public class ReportingDto
    {
    }
}
