using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class FlightLoadEO
    {
        public string AircraftDesc { get; set; }//aircraft_desc
        public string AircraftRegNo { get; set; }//aircraftregno
        public DateTime? SchedeptDate { get; set; }//schedeptdate
        public DateTime? ActualArrDate { get; set; }//actarrdate
        public string FirstClassSeatCount { get; set; }//f_seat_count 
        public string FirstClassBookedLoad { get; set; }//f_booked_load
        public string BusinessClassSeatCount { get; set; }//j_seat_count 
        public string BusinessClassBookedLoad { get; set; }//j_booked_load
        public string EconomyClassSeatCount { get; set; }//y_seat_count 
        public string EconomyClassBookedLoad { get; set; }//y_booked_load

        public string DepartureName { get; set; }
        public string ArrivalName { get; set; }
        public string RouteFlight { get; set; }

        public DateTime? LastBatchTime { get; set; }//schedeptdate
    }
}
