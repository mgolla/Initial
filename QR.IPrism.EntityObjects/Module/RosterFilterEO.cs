using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class RosterFilterEO
    {
        public string StaffID { get; set; }
        public DateTime StartDate { get; set; }//"23-OCT-2015"
        public DateTime EndDate { get; set; }//"29-OCT-2015"
        public int RosterTypeID { get; set; }
        public int TimeFormat { get; set; }
        public int IsNext { get; set; }
        public Boolean IsLandling { get; set; }
        public int FlightDigits { get; set; }
    }
}
