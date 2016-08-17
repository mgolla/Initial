using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class CrewRecognitionOverviewModel
    {
        public string FlightID { get; set; }

        public string RecognitionId { get; set; }

        public string RecognitionTypeID { get; set; }

        public string RecognitionType { get; set; }

        public string RecognitionStatusID { get; set; }

        public string RecognitionStatus { get; set; }

        public string IndividualCrewCount { get; set; }

        public string RecognizedCrewID { get; set; }

        public string RecognizedStaffNumber { get; set; }
    }
}
