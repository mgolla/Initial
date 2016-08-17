using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.EntityObjects.Shared;

namespace QR.IPrism.EntityObjects.Module
{
    public class EVRReportMainEO
    {
        public EVRInsertUpdateEO vrInsertUpdate { get; set; }
        public List<EVRPassengerEO> VRPassengers { get; set; }
        public List<DocumentEO> VrDocument { get; set; }

        // The below one can be removed if its already been in put in Flight Details Model
        //public UserContextEO UserContect { get; set; }
    }
}
