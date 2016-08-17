using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class AssessmentSearchModel
    {

        public string StaffNumber { get; set; }
        public string AssessorStaffNo { get; set; }

        public string CrewName { get; set; }

        public string AssessmentStatus { get; set; }
        public string AssessmentType { get; set; }

        public string CustomStatus { get; set; }

        public DateTime? FormatedDate { get; set; }

        public string ExpectedDate { get; set; }

        public string FlightNo { get; set; }
        public DateTime? FlightDate { get; set; }

        public string SectorFrom { get; set; }

        public string SectorTo { get; set; }

        public DateTime? DateofAssessment { get; set; }

        public string AssessmentID { get; set; }



        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Grade { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string AssessmentDate { get; set; }
        public string ExpDateOfCompletion { get; set; }

        public string LastAssessment { get; set; }
        public string AssessmentsScheduled { get; set; }
        public string PmName { get; set; }

        public DateTime? CutOffDate { get; set; }
    }
}
