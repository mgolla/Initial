using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.EntityObjects.Shared;

namespace QR.IPrism.EntityObjects.Module
{
    public class HousingNotificationEO
    {
        public NotificationDetailsEO Notification { get; set; }
        public HousingEO RequestDetails { get; set; }
        public HousingEO ExistingAcc { get; set; }
        public HousingEO InitiatorAcc { get; set; }
        public List<MessageEO> Messages { get; set; }
        public List<FileEO> Files { get; set; }
    }
}
