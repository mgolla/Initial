using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class FlightLoadFilter
    {

        public string Carrier { get; set; }
        public DateTime FltDate { get; set; }
        public string FltNum { get; set; }
        public string Origin { get; set; }

    }
}
