using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class DocumentEO
    {
         public string RowType { get; set; }
         public string DocumentId { get; set; }
         public string DocumentName { get; set; }
         public string FileCode { get; set; }
         public string FileId { get; set; }
         public FileEO File { get; set; }
         public int DocumentCount { get; set; }

         public byte[] DocumentContent { get; set; }

         public string ContentType { get; set; }

         public string ContentID { get; set; }
    }
}
