
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class FileModel
    {
        private DateTime _createdate;

        public string FileId { get; set; }
        public String FileName { get; set; }
        public String FileType { get; set; }
        public String FileSize { get; set; }
        public Byte[] FileContent { get; set; }

        public string ClassKeyId { get; set; }
        public string ClassType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate
        {
            get { return _createdate; }
            set { _createdate = DateTime.Now; }
        }
        public string FileNamePrefix { get; set; }
    }
}


