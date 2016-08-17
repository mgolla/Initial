using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class FlightDelayFilterModel
    {
        public string FlightNumber { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public String SectorFrom { get; set; }
        public String SectorTo { get; set; }
        public String DelayType { get; set; }
        public String DelayComment { get; set; }
        public String DelayReason { get; set; }
    }
}
