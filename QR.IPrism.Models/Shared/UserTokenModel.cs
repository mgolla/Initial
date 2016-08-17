using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Shared
{
    public class UserTokenModel
    {
        public string UserTokenId { get; set; }
        public string Subject { get; set; }
        public string StaffNo { get; set; }
        public string ClientId { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string Token { get; set; }
    }
}
