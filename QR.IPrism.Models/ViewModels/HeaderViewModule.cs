using QR.IPrism.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class HeaderViewModule
    {
        public List<MenuModel> Menus { get; set; }
        public int AlertCounter { get; set; }
        public UserContextModel StaffInfo { get; set; }
        public NotificationDetailsModel NotificationDetails { get; set; }
    }
}
