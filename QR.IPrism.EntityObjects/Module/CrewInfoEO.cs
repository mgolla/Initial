using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class CrewInfoEO
    {
        public string FlightNo { get; set; }
        public string CrewID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string POS { get; set; }
        public string Type { get; set; }
        public DateTime? FlightDate { get; set; }
        public DateTime? LastRefresh { get; set; }
        public string CrewRoute { get; set; }
        public byte[] CrewPhoto { get; set; }
        public string CrewPhotoUrl { get; set; }

        public bool IsAbinitio { get; set; }
        public bool IsAbinitioEntitled { get; set; }
        public bool IsCrewEntitled { get; set; }
        public bool IsCouple { get; set; }
        public string CrewEncryptID { get; set; }

        public IEnumerable<CrewEntitlementEO> CrewEntitlements { get; set; }
    }
}
