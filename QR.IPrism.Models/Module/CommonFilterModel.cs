using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class CommonFilterModel
    {
        public string FlightNo { get; set; } //FLIGHT_NO
        public string Arrival { get; set; }//V_ARR
        public string FlightDate { get; set; } //FLIGHT_DATE

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string FromSector { get; set; }
        public string ToSector { get; set; }

        public string CurrencyCode { get; set; }
        public string AirportCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public string StaffID { get; set; }

        public bool IsTraining { get; set; }
        public string TrainCode { get; set; }
        public string TrainDate { get; set; }

        public string Grade { get; set; }
        public string Location { get; set; }

        public string CrewImagePath { get; set; }
        public string CrewImageType { get; set; }
    }
}


