using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class CrewRecognitionEO
    {
        public CrewRecognitionEO()
        {
            RecognitionType = new RecognitionTypeEO();
            RecognitionSource = new RecognitionSourceEO();
            StaffDetailsList = new List<StaffDetailsEO>();
            RecognitionQRValueList = new List<RecognitionQRValueEO>();
            FlightDetails = new FlightDetailsEO();
            RecognitionStatusList = new List<RecognitionStatusEO>();
            CrewRecognitionDetailsHistory = new CrewRecognitionDetailsHistoryEO();
            //AttachmentList = new List<AttachmentModel>();
            AttachmentList = new List<FileEO>();
            //ListOfKendoFileUploaded = new List<QR.PrismNeo.Entity.Common.KendoFileUploadEditModel>();
            RecognisedCrewDetailsModels = new List<RecognisedCrewDetailsEO>();
        }


        public string RecognitionId { get; set; }

        public RecognitionTypeEO RecognitionType { get; set; }

        public RecognitionSourceEO RecognitionSource { get; set; }       

        public FlightDetailsEO FlightDetails { get; set; }

        //public int? EvrNo { get; set; }
        public string EvrNo { get; set; }

        public string RaisedByStaffNo { get; set; }

        public string PassengerDetails { get; set; }

        //public DataTable GridData { get; set; }

        public string RecognitionComments { get; set; }

        public string AdditionalComments { get; set; }

        public string ApvRejCrewId { get; set; }

        public string ApvRejCrewGradeId { get; set; }

        public string ApvRejComments { get; set; }

        public string IsPostToWall { get; set; }

        public string RecognitionStatusId { get; set; }

        public string IsActive { get; set; }

        public string SubmittedBy { get; set; }

        public string RecognitionStatusName { get; set; }

        public List<StaffDetailsEO> StaffDetailsList { get; set; }

        public List<RecognitionQRValueEO> RecognitionQRValueList { get; set; }

        //public IList<AttachmentModel> AttachmentList { get; set; }
        public IList<FileEO> AttachmentList { get; set; }

        public List<RecognitionStatusEO> RecognitionStatusList { get; set; }

        public CrewRecognitionDetailsHistoryEO CrewRecognitionDetailsHistory { get; set; }

        //public List<QR.PrismNeo.Entity.Common.KendoFileUploadEditModel> ListOfKendoFileUploaded { get; set; }

        public List<RecognisedCrewDetailsEO> RecognisedCrewDetailsModels { get; set; }
    }
}
