using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class CurrencyDetailEO //exchange
    {
        public string City { get; set; }//city
        public string Country { get; set; }//city
        public string AirportCode { get; set; }//airport_code
        public string FromCurrency { get; set; }//from_curr
        public string ToCurrency { get; set; }//to_curr
        public decimal ConversionRate { get; set; }//conversion_rate
        public decimal InvConversionRate { get; set; }//inv_conversion_rate
       
    }
}
