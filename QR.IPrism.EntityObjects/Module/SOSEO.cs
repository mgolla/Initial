using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class SOSEO
    {
        public System.String FlightNo { get; set; }
        public System.String SectorFrom { get; set; }
        public System.String SectorTo { get; set; }

        public int IsDataLoaded { get; set; }

        public List<SummaryOfServiceEO> EconomySOS { get; set; }      
        public List<SummaryOfServiceEO> PremiumSOS { get; set; }
    }
}
