using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QR.IPrism.EntityObjects.Module
{
    public class ObjectiveEO
    {
        public string ObjectiveID { get; set; }
        public string AssessmentObjectiveID { get; set; }
        public string AssessmentID { get; set; }
        public string ObjectiveName { get; set; }
        public byte[] ObjectiveImage { get; set; }

        public string Description { get; set; }
        public int Rank { get; set; }
        public int Weightage { get; set; }
        public decimal ObjScore { get; set; }
        public string FinancialYear { get; set; }
        public string Caption { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Grade { get; set; }
        public string GradeType { get; set; }
        public string GradeId { get; set; }
        public string IsActive { get; set; }
        public string ApprovedBy { get; set; }
        public string Rating { get; set; }
        public string RatingDescription { get; set; }
        public string RatingID { get; set; }
        public string TotalScore { get; set; }
        public string DisplayName { get; set; }
        public string Comments { get; set; }
        public string CreatedBy { get; set; }
        public List<CompetencyEO> ExceedsExpectations { get; set; }
        public List<CompetencyEO> Competencies { get; set; }
        public List<AssessmentIDPEO> AssmIDPs { get; set; }
        public string ObjectiveDetailedID { get; set; }
    }
}
 