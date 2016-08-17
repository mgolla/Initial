using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{


    ///// <summary>
    ///// Class CrewRecognitionDetails.
    ///// </summary>
    public class CrewRecognitionDetailsHistoryModel
    {

        public string RecognitionHistoryId { get; set; }

        public string RecognitionId { get; set; }

        public string RecognitionStatus { get; set; }

        public string IsActive { get; set; }

        public string CreatedBy { get; set; }
    }

    public class RecognitionQRValueModel
    {

        public string RecognitionDataId { get; set; }

        public string RecognitionDetailsId { get; set; }

        public string Id { get; set; }

        public string QRValue { get; set; }

        public string IsActive { get; set; }

        public string CreatedBy { get; set; }
    }

    /// <summary>
    /// Class StaffDetailsModel.
    /// </summary>

    public class StaffDetailsModel
    {
        public StaffDetailsModel()
        {
            StaffDetails = new FlightCrewsModel();
            RecognitionQRValueList = new List<RecognitionQRValueModel>();
        }


        public string RecognitionId { get; set; }

        public string RecognitionDetailsId { get; set; }

        public FlightCrewsModel StaffDetails { get; set; }

        public string IsActive { get; set; }

        public string CreatedBy { get; set; }

        public List<RecognitionQRValueModel> RecognitionQRValueList { get; set; }
    }

}
