using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class EVRListsModel
    {
        public string FLIGHTDETSID { get; set; }
        public string FLIGHTNUMBER { get; set; }
        public string SECTOR { get; set; }
        public DateTime? ATD_UTC { get; set; }
        public DateTime? ATA_UTC { get; set; }
        public string AircraftRegNo { get; set; }
        public string AircraftType { get; set; }
        public int VRCount { get; set; }
    }
}
