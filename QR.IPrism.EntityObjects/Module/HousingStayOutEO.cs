using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class HousingStayOutEO
    {
        //Start - Added for iPrism Housing Requests - ChangeAccommodation, Swap Rooms, Stay Out Request
        public string FriendStaffId { get; set; }
        public string FriendStaffName { get; set; }
        public string SwapRoomCategoryId { get; set; }
        public string SwapRoomCategory { get; set; }
        public string SwapRoomIsPaintedCleaned { get; set; }
        public string StayOutRequestTypeId { get; set; }
        public string StayOutRequestType { get; set; }
        public DateTime? StayOutRequestFromDate { get; set; }
        public DateTime? StayOutRequestToDate { get; set; }
        public string StayOutCrewRelationId { get; set; }
        public string StayOutCrewRelation { get; set; }
        public string StayOutCrewRelationName { get; set; }
        //public bool GenerateLetter { get; set; }
        //public string NoOfSlots { get; set; }
    }
}
