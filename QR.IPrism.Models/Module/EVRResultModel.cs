using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class EVRResultModel
    {
        public string VRId { get; set; }
        public int VRNo { get; set; }
        public string FlightNumber { get; set; }
        public string FlightDetailId { get; set; }
        public DateTime? ATD_UTC { get; set; }
        public string Sector { get; set; }
        public string Status { get; set; }
        public string Department { get; set; }
        public string ReportAbout { get; set; }

        //For Draft Enter EVR.
        public string Category { get; set; }
        public string SubCategory { get; set; }
    }
}
