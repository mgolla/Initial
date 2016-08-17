/*********************************************************************
 * Name          : Housing.cs
 * Description   : POCO class for housing module.
 * Create Date   : 25th Jan 2016
 * Last Modified : 25th Jan 2016
 * Copyright By  : Qatar Airways
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class HousingEO
    {
        public string RequestId { get; set; }
        public string RequestNumber { get; set; }
        public string RequestType { get; set; }
        public string Description { get; set; }
        public DateTime? RequestDate { get; set; }
        public string RequestStatus { get; set; }
        public string RequestDetailsID { get; set; }
        public string RequestTypeID { get; set; }

        public string CrewId { get; set; }
        public string RequestReason { get; set; }
        public DateTime? RequestDateClose { get; set; }
        public string DeptId { get; set; }
        
        public string StaffNo { get; set; }
        public string StaffName { get; set; }
               
        public string RequestedItemId { get; set; }
        public int RequestedQty { get; set; }

        public string MaintainanceTypeId { get; set; }
        public string MaintainanceType { get; set; }
        public string MaintainanceSubTypeId { get; set; }
        public string MaintainanceSubType { get; set; }
               
        public string MobileNo { get; set; }
        public string LandLineNo { get; set; }
        
        //Change Accommodation and Moving Out Company accommodation
        //public string LandLineNo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public HousingGuestEO Guests { get; set; }
        public HousingStayOutEO StayOut { get; set; }
        public BuildingEO BuildingDetails { get; set; }
    }
}
