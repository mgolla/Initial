using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class StationInfoEO
    {
        public string StationCode { get; set; }//station_code
        public string CountryName { get; set; }//country_name
        public string AirportName { get; set; }//airport_name
        public string Language { get; set; }//language
        public string HandlingAgent { get; set; }//handling_agents
        public string Catering { get; set; }//catering
        public string StationContact { get; set; }//hotel_name
        public string CountryCode { get; set; }//country_code
        public string Currency { get; set; }//currency
        public string CrewInformation { get; set; }//crew_information
        public string CustomerInformation { get; set; }//customer_information
        public string CityName { get; set; }//city_name
        public string IprismId { get; set; }//iprism_id
        public string LocalTime { get; set; }//local_time

        public int IsDataLoaded { get; set; }//local_time
    }
}
