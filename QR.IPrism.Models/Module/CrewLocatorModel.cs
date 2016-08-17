using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class CrewLocatorModel
    {
        public string LocationCode { get; set; }

        public string CityName { get; set; }

        public string AirportName { get; set; }

        public string CountryName { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public int StationCount { get; set; }

        public int FlightCount { get; set; }

        public int TotalCount { get; set; }

        public string CssClass { get; set; }
    }
}
