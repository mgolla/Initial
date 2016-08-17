using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Shared
{
    public class ResponseEO
    {
        public string RequestId { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public string ResponseId { get; set; }
        public string ResponseDetailsId { get; set; }
        public string RequestNumber { get; set; }
    }
}
