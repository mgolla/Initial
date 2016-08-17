using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class RecognisedCrewDetailsEO
    {
        public string RecognitionId { get; set; }

        public string RecognitionType { get; set; }

        public DateTime RecognitionDate { get; set; }

        public string RecognisedCrew { get; set; }

        public string FlightDetails { get; set; }

        public string CrewRecogStatus { get; set; }

        public string CrewRecogSrc { get; set; }

        public string SubmittedByCrew { get; set; }

        public string FlightNumber { get; set; }

        public string OverallComments { get; set; }

        public string ApproverComments { get; set; }

        public string ApproverCrewID { get; set; }

        public string ApproverStaffNumber { get; set; }

        public string ApproverStaffName { get; set; }

        public string RecognisedStaffNumber { get; set; }

        public string RecognisedStaffName { get; set; }
    }
}
