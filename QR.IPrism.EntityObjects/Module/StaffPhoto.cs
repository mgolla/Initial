using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class StaffPhoto
    {
        public string StaffNo { get; set; }
        public byte[] photo { get; set; }
        public string AIMSStaffNo { get; set; }
    }
}
