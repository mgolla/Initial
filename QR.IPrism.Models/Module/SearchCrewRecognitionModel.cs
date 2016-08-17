using QR.IPrism.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class SearchCrewRecognitionModel
    {
        public SearchCrewRecognitionModel()
        {
            RecognitionSourceList = new List<LookupModel>();
            RecognitionStatusList = new List<LookupModel>();
            RecognisedCrewGradeList = new List<LookupModel>();
            SubmittedCrewGradeList = new List<LookupModel>();
            RecognitionTypeList = new List<LookupModel>();
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

        public List<LookupModel> RecognitionSourceList { get; set; }

        public List<LookupModel> RecognitionStatusList { get; set; }

        public List<LookupModel> RecognisedCrewGradeList { get; set; }

        public List<LookupModel> SubmittedCrewGradeList { get; set; }

        public List<LookupModel> RecognitionTypeList { get; set; }
    }
}
