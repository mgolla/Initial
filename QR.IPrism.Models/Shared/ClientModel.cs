using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Shared
{
    public class ClientModel
    {
        public string ClientId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public int ApplicationType { get; set; }
        public int TokenLifeTime { get; set; }
        public string AllowedOrigin { get; set; }
        public string IsActive { get; set; }
    }
}
