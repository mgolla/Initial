using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class SearchRecognitionResultModel
    {
        public string RecognitionId { get; set; }

        public string RecognitionType { get; set; }

        public string RecognitionTypeID { get; set; }

        public string RecognitionStatus { get; set; }

        public string RecognitionStatusID { get; set; }

        public DateTime RecognitionDate { get; set; }

        public string StaffDetails { get; set; }

        public string FlightDetails { get; set; }

        public string StaffNumber { get; set; }

        public string SubmittedByCrew { get; set; }

        public string FlightNumber { get; set; }

        public string FlightID { get; set; }
    }
}
