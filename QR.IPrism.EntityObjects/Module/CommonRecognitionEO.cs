using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.EntityObjects.Shared;

namespace QR.IPrism.EntityObjects.Module
{
    #region Using Directives
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    #endregion

    /// <summary>
    /// Class RecognitionType.
    /// </summary>
    public class RecognitionTypeEO
    {

        public string Id { get; set; }

        public string Type { get; set; }
    }

    /// <summary>
    /// Class Recognition Source.
    /// </summary>
    public class RecognitionSourceEO
    {
        public string Id { get; set; }

        public string Source { get; set; }
    }

    /// <summary>
    /// Class RecognitionStatus.
    /// </summary>
    public class RecognitionStatusEO
    {

        public string Id { get; set; }

        public string Status { get; set; }
    }

    /// <summary>
    /// Class RecognitionStatus.
    /// </summary>
    public class GradeEO
    {

        public string GradeId { get; set; }

        public string GradeName { get; set; }
    }

    /// <summary>
    /// Common Recognition Model.
    /// </summary>
    public class CommonRecognitionEO
    {

        public List<LookupEO> RecognitionTypeList { get; set; }

        public List<LookupEO> RecognitionSourceList { get; set; }

        public IList<RecognitionStatusEO> RecognitionStatusList { get; set; }

        public IList<GradeEO> RecognisedCrewGradeList { get; set; }

        public IList<GradeEO> SubmittedCrewGradeList { get; set; }

    }
}
