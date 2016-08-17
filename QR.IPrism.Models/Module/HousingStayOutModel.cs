using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class HousingStayOutModel
    {
        public string FriendStaffId { get; set; }
        public string FriendStaffName { get; set; }
        public string FriendStaffNo { get; set; }
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
    }
}
