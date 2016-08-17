using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class AssessmentSearchRequestFilterModel
    {
        public string AssessorUserID { get; set; }
        public string StaffNumber { get; set; }
        public string StaffName { get; set; }
        public string Grade { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string AssessmentStatus { get; set; }
        public string PendingAssessment { get; set; }
        public string FlightNumber { get; set; }
        public string SectorTo { get; set; }
        
    }
}
