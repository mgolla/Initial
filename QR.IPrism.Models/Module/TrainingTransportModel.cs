
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class TrainingTransportModel
    {
        public string StaffNo { get; set; }//staffno
        public string FlightNumber { get; set; }
        public string TripId { get; set; }//trip_id
        public string PickupLocation { get; set; }//pickup_location
        public DateTime PickupTime { get; set; }//pickup_time
        public string DutyCode { get; set; }//duty_code
        public string DutyType { get; set; }//duty_type
        public DateTime ReportingTime { get; set; }//reporting_time					
    }
}


