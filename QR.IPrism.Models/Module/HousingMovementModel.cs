using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models
{
    public class HousingMovementModel
    {
        public string AccomodationType { get; set; }
        public string BedRoomNo { get; set; }
        public string BuildingName { get; set; }
        public string BuildingNo { get; set; }
        public string Comments { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CrewDetailsId { get; set; }
        public string FlatName { get; set; }
        public string FlatNo { get; set; }
        public string FormattedCreatedDate { get; set; }
        public string FormattedFromDate { get; set; }
        public string FormattedToDate { get; set; }
        public string FormattedTxnDate { get; set; }
        public string FromAccomodation { get; set; }
        public DateTime FromDate { get; set; }
        public string IsActive { get; set; }
        public bool IsChecked { get; set; }
        public bool IsCheckedAll { get; set; }
        public string MovementType { get; set; }
        public string MovmentId { get; set; }
        public string Nationality { get; set; }
        public string PrevBedRoomNo { get; set; }
        public string PrevBuildingName { get; set; }
        public string PrevBuildingNo { get; set; }
        public string PrevFlatName { get; set; }
        public string PrevFlatNo { get; set; }
        public string StaffGrade { get; set; }
        public string StaffName { get; set; }
        public string StaffNo { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime TxnDate { get; set; }
    }
}
