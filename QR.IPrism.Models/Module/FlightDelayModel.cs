using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class FlightDelayModel
    {
        public String FlightDetsID { get; set; }
        public string CrewCount { get; set; }
        public string FlightRoute { get; set; }
        public string PassengerLoadFC { get; set; }
        public string PassengerLoadJC { get; set; }
        public string PassengerLoadYC { get; set; }
        public string InfantLoadFC { get; set; }
        public string InfantLoadJC { get; set; }
        public string InfantLoadYC { get; set; }
        public string SeatCapacityFC { get; set; }
        public string SeatCapacityJC { get; set; }
        public string SeatCapacityYC { get; set; }

        public string IsGroomingCheck { get; set; }
        public string GroomingCheckComment { get; set; }
        public string IsCsdCsBriefed { get; set; }
        public string CsdCsBriefedComment { get; set; }
        public string StaffName { get; set; }
        public string StaffId { get; set; }
        public string DelayType { get; set; }
        public string DelayId { get; set; }
        public string DelayComment { get; set; }
        public string FlightDelayCatName { get; set; }
        public string FlightDelayCatId { get; set; }
        public bool IsSelected { get; set; }

        public String ArrivalDelay { get; set; }
        public String DepartureDelay { get; set; }
    }
}
