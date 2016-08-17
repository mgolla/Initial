using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QR.IPrism.Models.Module
{
    public class RatingGuideLineModel
    {
        public string RatingGuidelineID { get; set; }
        public string Compliance { get; set; }
        public string ComplianceDesc { get; set; }
        public string GuidelineDesc { get; set; }
        public string Rating { get; set; }
        public string RatingDescription { get; set; }
    }
}
