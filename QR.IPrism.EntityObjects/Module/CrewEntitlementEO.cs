using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class CrewEntitlementEO
    {
        public string EntitlementYear { get; set; }
        public string EntitlementAvailable { get; set; }
        public DateTime? EntitlementLastUsed { get; set; }
        public DateTime? EntitlementNextDate { get; set; }

        public bool IsCrewEntitled { get; set; }
        public string Message { get; set; }

        public string Setup_slots { get; set; }
        public string Setup_days { get; set; }        
        //used slots
        public string Used_No_of_slots { get; set; }
        // used days
        public string Used_No_of_days { get; set; }
        public string Next_No_of_slots { get; set; }
        public string Next_No_of_days { get; set; }
        public bool IsEntitledCurrentYear { get; set; }
        public bool IsEntitledNextYear { get; set; }
    }
}
