using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class SOSModel
    {
        public int IsDataLoaded { get; set; }//local_time
        public List<SummaryOfServiceModel> EconomySOS { get; set; }
        public List<SummaryOfServiceModel> PremiumSOS { get; set; }

    }
}
