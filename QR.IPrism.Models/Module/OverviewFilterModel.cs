using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class OverviewFilterModel
    {
        public string StaffNo { get; set; }
        public string FlightNo { get; set; }
        public string Sectorfrom { get; set; }
        public string SectorTo { get; set; }
        public string STD { get; set; }
    }

    public class BackgroundFilterModel
    {
        public bool IsMobile { get; set; }
        public string SectorTo { get; set; }
        public string ImagePath { get; set; }
    }
}



