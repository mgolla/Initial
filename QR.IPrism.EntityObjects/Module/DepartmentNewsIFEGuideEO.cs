using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class DepartmentNewsIFEGuideEO
    {
        public string NewstTitle { get; set; }
        public string DocFileName { get; set; }
        public string FileType { get; set; }
        public byte[] ThumbImage { get; set; }
        public string ThumbFileName { get; set; }
        public string ImageType { get; set; }
        public string FileCode { get; set; }
        public string FileId { get; set; }
        public string ValidFrom { get; set; }
        public string DocType { get; set; }
        
    }
    public class NewsFilterEO
    {
        public int NewsType { get; set; }
    }
}

