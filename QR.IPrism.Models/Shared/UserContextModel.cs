using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace QR.IPrism.Models.Shared
{
    // The [DataContract] and [DataMember] attribute is to be added only when required for other Models.
    // as this class is to be [Serializable].
    [Serializable]
    [DataContract]
    public class UserContextModel
    {
        [DataMember]
        public string StaffNumber { get; set; }
        [DataMember]
        public string StaffName { get; set; }
        [DataMember]
        public string CrewDetailsId { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Grade { get; set; }
        [DataMember]
        public string Nationality { get; set; }
        [DataMember]
        public List<RoleModel> Role { get; set; }
        [DataMember]
        public string JoiningDate { get; set; }
        [DataMember]
        public string DateOfBirth { get; set; }
        [DataMember]
        public string NextToKIN { get; set; }
        [DataMember]
        public string PermanentAddress { get; set; }
        [DataMember]
        public string RPNumber { get; set; }
        [DataMember]
        public DateTime? RPExpiryDate { get; set; }
        [DataMember]
        public string ReportingTo { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Contact { get; set; }
        [DataMember]
        public string ExpInCurrentGradeInMonths { get; set; }
        [DataMember]
        public int FlightFlownInCurrentGrade { get; set; }
        [DataMember]
        public string CurrentAccomodation { get; set; }
        [DataMember]
        public byte[] CrewPhoto { get; set; }
        [DataMember]
        public string ExpInMonths { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public bool IsCaptchaFilled { get; set; }
        [DataMember]
        public string AdminKey { get; set; }
        [DataMember]
        public string CrewPhotoUrl { get; set; }
        [DataMember]
        public bool IsAdmin
        {
            get
            {
                return !string.IsNullOrEmpty(AdminKey) ? true : false;
            }
        }
        [DataMember]
        public string ImpersonatedBy { get; set; }
        [DataMember]
        public string ImpersonatedByDtlsId { get; set; }
        [DataMember]
        public string ImpersonatedByUsrId { get; set; }
        [DataMember]
        public string ImpersonatedByName { get; set; }
    }
}
