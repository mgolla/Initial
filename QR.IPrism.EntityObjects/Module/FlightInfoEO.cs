using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class FlightInfoEO
    {    
        public String FlightNumber { get; set; }
        public String Sector { get; set; }
        public String SectorFrom { get; set; }
        public String SectorTo { get; set; }
        public String AirCraftType { get; set; }
        public String AirCraftTypeId { get; set; }
        public DateTime StandardTimeDepartureUtc { get; set; }
        public DateTime StandardTimeArrivalUtc { get; set; }
        public DateTime? ReportingTime { get; set; }
        public String Route { get; set; }
        public String FlightDelayRptId { get; set; }
        public String FlightDetsID { get; set; }
        public String DelayReportStatus { get; set; }
        public String CrewDetail { get; set; }
        public String AirCraftRegNo { get; set; }

        public DateTime? ScheduledDeptTime { get; set; }
        public DateTime? ScheduledArrTime { get; set; }
        public DateTime? ActualDeptTime { get; set; }
        public DateTime? ActualArrTime { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsFromAIMS { get; set; }
        public bool IsManuallyEntered { get; set; }
        public bool IsFerry { get; set; }
        public bool IsActive { get; set; }
        public int WinWebNumber { get; set; }
        
        public FlightDelayEO FlightDelay { get; set; }
        public List<FlightCrewsEO> FlightCrewsDetail { get; set; }
    }
}