using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class CurrencyDetailModel
    {
        public string AirportCode { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public System.Decimal ConversionRate { get; set; }
        public System.Decimal InvConversionRate { get; set; }
    }
}


