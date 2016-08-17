using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class WeatherInfoEO
    {
        public DateTime? WeatherDate { get; set; }//weatherdate
        public string AirportCode { get; set; }//airport_code
        public string City { get; set; }//city
        public string TempLow { get; set; }//temp_low
        public string TempHigh { get; set; }//temp_high
        public string LocationCode { get; set; }//locationcode
        public string WeekDay { get; set; } //week_day
    }
}
