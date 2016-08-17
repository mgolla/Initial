using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QR.IPrism.EntityObjects.Module
{
    public class CompetencyEO
    {
        public string CompetencyId { get; set; }
        public string CompetencyConfigId { get; set; }
        public string AssessmentCompID { get; set; }
        public string ObjectiveID { get; set; }
        public string GradeId { get; set; }
        public string strListCompetency { get; set; }
        public string AssessmentObjectiveID { get; set; }
        public string CompetencyName { get; set; }
        public string CompetencyDetail { get; set; }
        public string Description { get; set; }
        public string CompetencyTypeName { get; set; }
        public decimal CompetencyWeight { get; set; }
        public decimal CompetencyScore { get; set; }
        public string Comments { get; set; }
        public string Rating { get; set; }
        public string RatingID { get; set; }
        public string Type { get; set; }
        public string IsActive { get; set; }
        public string IsEEChecked { get; set; }
        public string LookUpID { get; set; }
        public string LookUpName { get; set; }
        public string LookUpDescription { get; set; }
        public string ObjectiveDetID { get; set; }
        public string CreatedBy { get; set; }
        public int Rank { get; set; }
    }
}
