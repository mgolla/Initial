
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class DepartmentNewsIFEGuideModel
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
    public class NewsFilterModel
    {
        public int NewsType { get; set; }
        public string RSSUrl { get; set; }
        public string ProxyURL { get; set; }        
        public string ProxyAccount { get; set; }
        public string ProxyPassword { get; set; }
        public string ProxyDomain { get; set; }

    }
}


