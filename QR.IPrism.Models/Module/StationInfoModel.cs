using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class StationInfoModel
    {
        public string StationCode { get; set; }
        public string CountryName { get; set; }
        public string AirportName { get; set; }
        public string Language { get; set; }
        public string HandlingAgent { get; set; }
        public string Catering { get; set; }
        public string StationContact { get; set; }
        public string CountryCode { get; set; }
        public string Currency { get; set; }
        public string CrewInformation { get; set; }
        public string CustomerInformation { get; set; }
        public string CityName { get; set; }
        public string IprismId { get; set; }
        public string LocalTime { get; set; }//local_time
        public int IsDataLoaded { get; set; }//local_time
    }
}


