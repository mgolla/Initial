using QR.IPrism.EntityObjects.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class SearchCrewRecognitionEO
    {
        public SearchCrewRecognitionEO()
        {
            RecognitionSourceList = new List<LookupEO>();
            RecognitionStatusList = new List<LookupEO>();
            RecognisedCrewGradeList = new List<LookupEO>();
            SubmittedCrewGradeList = new List<LookupEO>();
            RecognitionTypeList = new List<LookupEO>();
        }

        public string RecognisedCrew { get; set; }

        public string RecognisedCrewGrade { get; set; }

        public string CrewRecogStatus { get; set; }

        public string CrewRecogSource { get; set; }

        public string FlightNumber { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string SectorFrom { get; set; }

        public string SectorTo { get; set; }

        public string SubmittedByCrew { get; set; }

        public string SubByCrewGrade { get; set; }

        public string MgrOfSubByCrewId { get; set; }

        public string MgrOfSubByCrewName { get; set; }

        public DateTime RejectedFromDate { get; set; }

        public DateTime RejectedToDate { get; set; }

        public string RecognitionType { get; set; }

        public List<LookupEO> RecognitionSourceList { get; set; }

        public List<LookupEO> RecognitionStatusList { get; set; }

        public List<LookupEO> RecognisedCrewGradeList { get; set; }

        public List<LookupEO> SubmittedCrewGradeList { get; set; }

        public List<LookupEO> RecognitionTypeList { get; set; }
    }
}
