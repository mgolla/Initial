using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Shared
{
    public class MessageModel
    {
        public string Id { get; set; }
        public string RequestId { get; set; }
        public string RequestGuid { get; set; }
        public string MessageCode { get; set; }
        public string Message { get; set; }
        public string Module { get; set; }
        public string Type { get; set; }
        public string CreatedBy { get; set; }
        public String Status { get; set; }
        public string CreatedDate { get; set; }
    }
}
