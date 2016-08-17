using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class FileEO
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string ClassKeyId { get; set; }
        public string EntityTypeId { get; set; }
        public string ClassType { get; set; }
        public string CreatedBy { get; set; }
        public byte[] FileContent { get; set; }
        public string FileNamePrefix { get; set; }
    }
}
