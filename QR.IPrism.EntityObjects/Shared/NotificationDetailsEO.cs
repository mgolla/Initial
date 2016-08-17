using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Shared
{
    public class NotificationDetailsEO
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? ActionByDate { get; set; }
        public string Severity { get; set; }
        public string ToCrewId { get; set; }
        public string FromCrewId { get; set; }
        public string Status { get; set; }
        public string RequestId { get; set; }
        public string RequestGuid { get; set; }
    }
}
