using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class LocationCrewDetailModel
    {
        public string Location { get; set; }
        public string Indicator { get; set; }
        public string StaffNo { get; set; }
        public string DutyCode { get; set; }
        public string DutyGroup { get; set; }
        public string FlightNo { get; set; }
        public string FromSector { get; set; }
        public string ToSector { get; set; }
        public string RosterInfo { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }       

    }
}
