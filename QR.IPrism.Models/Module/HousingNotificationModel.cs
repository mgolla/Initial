using QR.IPrism.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class HousingNotificationModel
    {
        public NotificationDetailsModel Notification { get; set; }
        public HousingModel RequestDetails { get; set; }
        public HousingModel ExistingAcc { get; set; }
        public HousingModel InitiatorAcc { get; set; }
        public List<MessageModel> Messages { get; set; }
        public List<FileModel> Files { get; set; }
    }
}