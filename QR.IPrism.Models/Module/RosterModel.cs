using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class RosterModel
    {
        public int TempID { get; set; }
        public string StaffID { get; set; }
        public string Cc { get; set; }
        public string Flight { get; set; }
        public string FlightDisplay { get; set; }
        public string DutyDate { get; set; }
        public string DutyDateActual { get; set; }
        public string DutyDateActualDate { get; set; }
        public string DutyDateActualDateName { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
       
        public string BTime { get; set; }
        public int CountOfExtraRosters { get; set; }
        public bool IsOff { get; set; }
        public bool IsOnFlight { get; set; }
        public string ActiveClass { get; set; }
        public DateTime StandardTimeDeparture { get; set; }
        public DateTime StandardTimeArrival { get; set; }

        public string StandardTimeDepartureUtc { get; set; }
        public string StandardTimeArrivalUtc { get; set; }
        public string StandardTimeDepartureLocal { get; set; }
        public string StandardTimeArrivalLocal { get; set; }
        public string StandardTimeDepartureDoha { get; set; }
        public string StandardTimeArrivalDoha { get; set; }

        public string DohaDate { get; set; }
        public string LocalDate { get; set; }

        public string Address1Training { get; set; }
        public string Address2Training { get; set; }
        public string Address3Training { get; set; }
        public string FlightTravellingTime { get; set; } 
        
        public List<RosterModel> Rosters { get; set; }

        public bool AssessmentAllowed { get; set; }
    }
}
