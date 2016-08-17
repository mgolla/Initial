using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class CrewRecognitionModel
    {
         public CrewRecognitionModel()
        {
            RecognitionType = new RecognitionTypeModel();
            RecognitionSource = new RecognitionSourceModel();
            StaffDetailsList = new List<StaffDetailsModel>();
            RecognitionQRValueList = new List<RecognitionQRValueModel>();
            FlightDetails = new FlightDetailsModel();
            RecognitionStatusList = new List<RecognitionStatusModel>();
            CrewRecognitionDetailsHistory = new CrewRecognitionDetailsHistoryModel();
            //AttachmentList = new List<AttachmentModel>();
            AttachmentList = new List<FileModel>();
            //ListOfKendoFileUploaded = new List<QR.PrismNeo.Entity.Common.KendoFileUploadEditModel>();
            RecognisedCrewDetailsModels = new List<RecognisedCrewDetailsModel>();
        }


        public string RecognitionId { get; set; }

        public RecognitionTypeModel RecognitionType { get; set; }

        public RecognitionSourceModel RecognitionSource { get; set; }       

        public FlightDetailsModel FlightDetails { get; set; }

        public int? EvrNo { get; set; }

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

        public List<StaffDetailsModel> StaffDetailsList { get; set; }

        public List<RecognitionQRValueModel> RecognitionQRValueList { get; set; }

        //public IList<AttachmentModel> AttachmentList { get; set; }
        public IList<FileModel> AttachmentList { get; set; }

        public List<RecognitionStatusModel> RecognitionStatusList { get; set; }

        public CrewRecognitionDetailsHistoryModel CrewRecognitionDetailsHistory { get; set; }

        //public List<QR.PrismNeo.Entity.Common.KendoFileUploadEditModel> ListOfKendoFileUploaded { get; set; }

        public List<RecognisedCrewDetailsModel> RecognisedCrewDetailsModels { get; set; }
    }
}
