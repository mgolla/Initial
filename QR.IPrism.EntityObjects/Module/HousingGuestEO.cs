using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class HousingGuestEO
    {
        public string CrewId { get; set; }
        public string GuestName { get; set; }
        public string GuestGender { get; set; }
        public string Relationship { get; set; }
        public string RelationType { get; set; }
        public string NameWithRelation { get; set; }        
        public DateTime? CheckinDate { get; set; }
        public DateTime? CheckoutDate { get; set; }
        public int NoOfDays { get; set; }
        public DateTime? ExtendedCheckoutDate { get; set; }
        public string OverStayedStatus { get; set; }
    }
}
