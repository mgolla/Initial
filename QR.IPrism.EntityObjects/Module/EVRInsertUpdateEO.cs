using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class EVRInsertUpdateEO
    {
        public string FlightDetId { get; set; }
        public string ReportAbtId { get; set; }
        public string CategoryId { get; set; }
        public string SubCategoryId { get; set; }
        //public string Owner { get; set; }
        //public string NonOwner { get; set; }
        public string NODId { get; set; }
        public string IsCritical { get; set; }
        public string IsCabinFirstClass { get; set; }
        public string IsCabinBusinessClass { get; set; }
        public string IsCabinEconomyClass { get; set; }
        public string IsCabinClassNA { get; set; }
        //public bool IsCabibAll { get; set; }
        //public bool IsNA { get; set; }
        public string IsCSR { get; set; }
        public string IsOHS { get; set; }
        public string IsPO { get; set; }
        public string IsVrRestricted { get; set; }
        public string IsInfoVr { get; set; }
        public string IsNotIfNotReq { get; set; }
        public string vrFactComment { get; set; }
        public string vrActionComment { get; set; }
        public string vrResultComment { get; set; }
        public string IsRuleSetChanged { get; set; }
        public string IsAdmin { get; set; }
        public string IsNew { get; set; }
        public string Status { get; set; }
        public string VrCrewsId { get; set; }

        public string InsertType { get; set; }
        public string VrId { get; set; }
        public string VrNo { get; set; }
        public string VrInstanceId { get; set; }
        //public string StaffNumber { get; set; }
    }
}
