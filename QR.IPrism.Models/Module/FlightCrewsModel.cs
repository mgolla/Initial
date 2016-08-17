using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class FlightCrewsModel
    {
        public string FlightCrewId { get; set; }
        public string StaffNumber { get; set; }
        public string StaffName { get; set; }
        public string StaffGrade { get; set; }
        public string FlightDetsId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string IsActingCSD { get; set; }
        public bool IsCheckedActingCSD { get; set; }
        public bool IsEnabled { get; set; }
        public string CabinCrewPosition { get; set; }
        public string CabinCrewPositionVal { get; set; }
        public string AnnounceLang { get; set; }
        public string AnnounceLangVal { get; set; }
        public string CrewDHPosition { get; set; }

        public string CrewDetailsId { get; set; }
        public string StaffGradeId { get; set; }
    }
}
