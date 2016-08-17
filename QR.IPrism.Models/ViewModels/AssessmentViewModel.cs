using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.ViewModels
{
    public class AssessmentViewModel
    {
        public AssessmentModel Assessment;
        public List<RatingGuideLineModel> RatingGuideLines;
        public List<AssessmentModel> POAssessmentScheduleDetails { get; set; }
        public List<AssessmentModel> POAssessmentCSDList { get; set; }
        public List<AssessmentModel> POAssessmentCSList { get; set; }

    }
}
