using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Shared
{
    public class LookupSearchModel
    {
        public string StaffNo { get; set; }
        public string LookupTypeCode { get; set; }
        public string SearchText { get; set; }
        public string[] ArrSearchText { get; set; }
    }
}
