
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class OverviewModel
    {
        public TransportTripDetailModel TransportTripDetail { get; set; }//Bus Pickup
        public CurrencyDetailModel CurrencyDetail { get; set; }//exchange
        public List<WeatherInfoModel> WeatherInfos { get; set; }
        public FlightLoadModel FlightLoad { get; set; }
        public byte[] BackgroundImage { get; set; }
    }
    public class Background
    {
        public byte[] Image { get; set; }
    }
}


