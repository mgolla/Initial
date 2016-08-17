
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class WeatherInfoModel
    {
        public System.DateTime WeatherDate { get; set; }
        public string AirportCode { get; set; }
        public string City { get; set; }
        public string TempLow { get; set; }
        public string TempHigh { get; set; }
        public string LocationCode { get; set; }
        public string WeekDay { get; set; } //week_day
    }
}


