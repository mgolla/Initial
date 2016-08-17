using QR.IPrism.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    /// <summary>
    /// Class RecognitionType.
    /// </summary>
    public class RecognitionTypeModel
    {

        public string Id { get; set; }

        public string Type { get; set; }
    }

    /// <summary>
    /// Class Recognition Source.
    /// </summary>
    public class RecognitionSourceModel
    {
        public string Id { get; set; }

        public string Source { get; set; }
    }

    /// <summary>
    /// Class RecognitionStatus.
    /// </summary>
    public class RecognitionStatusModel
    {

        public string Id { get; set; }

        public string Status { get; set; }
    }

    /// <summary>
    /// Class RecognitionStatus.
    /// </summary>
    public class GradeModel
    {

        public string GradeId { get; set; }

        public string GradeName { get; set; }
    }

    /// <summary>
    /// Common Recognition Model.
    /// </summary>
    public class CommonRecognitionModel
    {

        public List<LookupModel> RecognitionTypeList { get; set; }

        public List<LookupModel> RecognitionSourceList { get; set; }

        public IList<RecognitionStatusModel> RecognitionStatusList { get; set; }

        public IList<GradeModel> RecognisedCrewGradeList { get; set; }

        public IList<GradeModel> SubmittedCrewGradeList { get; set; }

    }
}
