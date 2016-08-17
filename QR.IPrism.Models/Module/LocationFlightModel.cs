using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class LocationFlightModel
    {
        public string Location { get; set; }
        public string FlightNo { get; set; }
        public string Sectors { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public int FlightCrewCount { get; set; }
       
    }
}
