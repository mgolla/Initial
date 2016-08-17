using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Shared
{
    public class AnalyticEO
    {
        public string Category { get; set; }
        public string Action { get; set; }
        public string OptionLabel { get; set; }
        public string OptionValue { get; set; }
        public string Resolution { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }
        public string UserAgent { get; set; }
        public string IsDesktop { get; set; }
        public string IsTablet { get; set; }
        public string IpAddress { get; set; }
        public string StaffNumber { get; set; }
        public string DateTime { get; set; }
    }
}
