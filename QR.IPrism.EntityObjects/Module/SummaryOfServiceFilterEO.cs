
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class SummaryOfServiceFilterEO
    {
        public System.String FlightNo { get; set; }
        public System.String SectorFrom { get; set; }
        public System.String SectorTo { get; set; }
        public int economyFlag { get; set; }
        public int nfEconomyFlag { get; set; }
        public int piFlag { get; set; }
        public int ngFlag { get; set; }
        public int nfPremium { get; set; }
    }
}



