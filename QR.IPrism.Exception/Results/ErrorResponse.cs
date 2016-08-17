using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Exception.Results
{
    public class ErrorResponse
    {
        /// <summary>
        /// Pre-defined error code
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// Id to track the exception
        /// </summary>
        public string CorelationId { get; set; }
        /// <summary>
        /// User Friendly Error Message
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
