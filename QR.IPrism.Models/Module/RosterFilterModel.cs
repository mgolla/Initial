using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class RosterFilterModel
    {
        public string StaffID { get; set; }
        public DateTime StartDate { get; set; }//"23-OCT-2015"
        public DateTime EndDate { get; set; }//"29-OCT-2015"
        public int RosterTypeID { get; set; }
        public int TimeFormat { get; set; }
        public int IsNext { get; set; }
        public int LeftValue { get; set; }
        public int RightValue { get; set; }
        public Boolean IsLanding { get; set; }
        public int FlightDigits { get; set; }

        public DateTime? LastSelectedDate { get; set; }
    }
}
