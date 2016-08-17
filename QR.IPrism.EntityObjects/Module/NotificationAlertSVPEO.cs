using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class NotificationAlertSVPEO
    {
         public string Doc { get; set; }
         public string DocDate { get; set; }
         public string DocId { get; set; }
         public string DocDesc { get; set; }
         public string DocType { get; set; }
         public string AlertFolder { get; set; }
         public string AlertFolderName { get; set; }
         public string FileCode { get; set; }
         public string FileId { get; set; }
         public string FileName { get; set; }
         public string Link { get; set; }

         public FileEO File { get; set; }
         public FileEO THUMBIMAGE { get; set; }
         public bool IsViewed { get; set; }
    }
}
