using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class EVRDraftOutputModel
    {
        public string VRId { get; set; }
        public string VRInstaceId { get; set; }
        public string FlightDetsId { get; set; }
        public string VRNo { get; set; }
        public string ReportAbtId { get; set; }
        public string ReportAbtName { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string VRMasterStatusId { get; set; }
        public string VRMasterStatusName { get; set; }
        public string OwnerDept { get; set; }
    }
}
