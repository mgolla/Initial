using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class FlightDelayEO
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

        public string ArrivalDelay { get; set; }
        public string DepartureDelay { get; set; }
        public string SubmittedVRs { get; set; }

        public string DraftVRs { get; set; }
        public string NoVR { get; set; }
        public string VrNotSubmitted { get; set; }
        public string NoVrSubmittedStatus { get; set; }
        public string SubmittedVRsbyUser { get; set; }            
    }
}