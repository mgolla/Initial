using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class EVRPendingEO
    {
        public string FLIGHTDETSID { get; set; }
        public string FLIGHTNUMBER { get; set; }
        public DateTime? ACTDEPTDATE { get; set; }
        public string ISFROMAIMS { get; set; }
        public string ISMANUALLYENTERED { get; set; }
        public string SECTOR { get; set; }
        public string SECTORFROM { get; set; }
        public string SECTORTo { get; set; }
        public DateTime? MODIFIEDDATE { get; set; }
        public DateTime? ACTARRDATE { get; set; }
        public string AIRCRAFTREGNO { get; set; }
        public string AIRCRAFTTYPEID { get; set; }
        public DateTime? SCHEARRDATE { get; set; }
        public DateTime? SCHEDEPTDATE { get; set; }
        public string ISFERRY { get; set; }
        public string AIRCRAFTTYPE { get; set; }
        public int SUBMITTEDVRS { get; set; }
        public int DRAFTVRS { get; set; }
        public int DRAFTVRSBYUSER { get; set; }
        public string NOVRS { get; set; }
        public string NOVRSBYUSER { get; set; }
        public string VRNOTSUBMITTEDS { get; set; }
        public string SUBMITTEDVRSBYUSER { get; set; }
        public string DELAYTAGS { get; set; }
        public string POSCREWCOUNT { get; set; } 
        
    }
}
