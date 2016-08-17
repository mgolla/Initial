using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Shared
{
    public class MessageEO
    {
        public string Id { get; set; }
        public string MessageCode { get; set; }
        public string Message { get; set; }
        public string Module { get; set; }
        public string Type { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
    }
}
