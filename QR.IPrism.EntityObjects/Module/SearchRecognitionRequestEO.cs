using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class SearchRecognitionRequestEO
    {
        public string GradeList { get; set; }

        public string RecognitionStatusList { get; set; }

        public string FlightNumber { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string SectorFrom { get; set; }

        public string SectorTo { get; set; }

        public string SubmittedByCrew { get; set; }

        public string RecognisedCrewID { get; set; }
    }
}
