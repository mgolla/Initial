using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class EVRReportMainModel
    {
        //public EVRReportAboutModel EVRRprtAbt { get; set; }
        //public EVRPassengerModel EVRPassenger { get; set; }
        //public DocumentModel DocumentVR { get; set; }

        public EVRInsertUpdateModel vrInsertUpdate { get; set; }
        public List<EVRPassengerModel> VRPassengers { get; set; }
        public List<DocumentModel> VrDocument { get; set; }

        // The below one can be removed if its already been in put in Flight Details Model
        //public UserContextEO UserContect { get; set; }
    }
}
