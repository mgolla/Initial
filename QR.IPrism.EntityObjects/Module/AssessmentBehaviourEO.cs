using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class AssessmentBehaviourEO
    {
        public DateTime? ReportingDate { get; set; }
        public string StaffName { get; set; }
        public string ObjectiveName { get; set; }
        public string CategoryName { get; set; }
        public string RootCauseName { get; set; }
        public string FinalValidity { get; set; }
        public string FinalAction { get; set; }
        public DateTime? DateOfInvestigation { get; set; }
        public DateTime? DateOfEffectiveLetter { get; set; }
        public string Situation { get; set; }
        public string Observation { get; set; }
        public string ExpectedResult { get; set; }

        public string IdpObjectiveName { get; set; }
        public string IdpObservation { get; set; }
        public string IdpSituation { get; set; }
        public string IdpExpectedResult { get; set; }
        public DateTime? IdpStartDate { get; set; }
        public DateTime? IdpEndDate { get; set; }
        public string IdpIsReviewApplicable { get; set; }
        public string IsCrewCommentsRequired { get; set; }
        public string IdpComment { get; set; }
    }
}
