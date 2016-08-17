
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class DocumentModel
    {
        public string name { get; set; }
        public string rights { get; set; }
        public string size { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public int DocumentCount { get; set; }
        public string RowType { get; set; }
        public string DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string FileCode { get; set; }
        public string FileId { get; set; }
        public FileModel File { get; set; }
        public byte[] DocumentContent { get; set; }
        List<DocumentModel> childDocuments { get; set; }
    }
   
}


