using QR.IPrism.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.ViewModels
{
    public class AssessmentPOViewModel
    {
        public List<AssessmentSearchModel> POAssessmentScheduled { get; set; }
        public List<AssessmentSearchModel> POAssessmentCSDList { get; set; }
        public List<AssessmentSearchModel> POAssessmentCSList { get; set; }
    }
}
