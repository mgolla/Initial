using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Shared
{
    public class ResponseModel
    {
        public int RequestId { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public string ResponseId { get; set; }
        public string Details { get; set; }
        public string RequestNumber { get; set; }
        public string IsFormValid { get; set; }
    }
}
