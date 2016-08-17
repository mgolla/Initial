using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    ///// <summary>
    ///// Class CrewRecognitionDetails.
    ///// </summary>
    public class CrewRecognitionDetailsHistoryEO
    {

        public string RecognitionHistoryId { get; set; }

        public string RecognitionId { get; set; }

        public string RecognitionStatus { get; set; }

        public string IsActive { get; set; }

        public string CreatedBy { get; set; }
    }

    public class RecognitionQRValueEO
    {

        public string RecognitionDataId { get; set; }

        public string RecognitionDetailsId { get; set; }

        //public string Id { get; set; }
        public string Id { get; set; }

        //public string QRValue { get; set; }
        public string QRValue { get; set; }

        public string IsActive { get; set; }

        public string CreatedBy { get; set; }
    }

    /// <summary>
    /// Class StaffDetailsModel.
    /// </summary>
    public class StaffDetailsEO
    {
        public StaffDetailsEO()
        {
            StaffDetails = new FlightCrewsEO();
            RecognitionQRValueList = new List<RecognitionQRValueEO>();
        }


        public string RecognitionId { get; set; }

        public string RecognitionDetailsId { get; set; }

        public FlightCrewsEO StaffDetails { get; set; }

        public string IsActive { get; set; }

        public string CreatedBy { get; set; }

        public List<RecognitionQRValueEO> RecognitionQRValueList { get; set; }
    }
}
