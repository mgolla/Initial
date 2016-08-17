using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class OverviewEO
    {
        public TransportTripDetailEO TransportTripDetail { get; set; }//Bus Pickup
        public CurrencyDetailEO CurrencyDetail { get; set; }//exchange
        public List<WeatherInfoEO> WeatherInfos { get; set; }
        public FlightLoadEO FlightLoad { get; set; }
        public byte[] BackgroundImage { get; set; }
    }
}
