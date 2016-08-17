using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Shared
{
    public class UserContextEO
    {
        private string currentaccomodation = string.Empty;

        public string StaffNumber { get; set; }
        public string StaffName { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public string Grade { get; set; }
        public string Nationality { get; set; }
        public string CrewDetailsId { get; set; }
        public List<RoleEO> Role { get; set; }
        public string JoiningDate { get; set; }
        public string DateOfBirth { get; set; }
        public string NextToKIN { get; set; }
        public string PermanentAddress { get; set; }
        public string RPNumber { get; set; }
        public DateTime? RPExpiryDate { get; set; }
        public string ReportingTo { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string ExpInCurrentGradeInMonths { get; set; }
        public int FlightFlownInCurrentGrade { get; set; }
        public string ExpInMonths { get; set; }
        public byte[] CrewPhoto { get; set; }
        public string CrewPhotoUrl { get; set; }
        
        public string FlatNumber { get; set; }
        public string BuildingNumber { get; set; }
        public string StreetNumber { get; set; }
        public string BuildingArea { get; set; }
        // As the Stored procedure will give current accomodation in parts as 
        // Flat Number, Building Number, Street Number and Building Area, "CurrentAccomodation" property is added below for contactination
        public string CurrentAccomodation 
        {
            get 
            { 
                return (this.currentaccomodation = String.Concat(this.FlatNumber, 
                                                    String.Concat(this.BuildingNumber), 
                                                    String.Concat(this.StreetNumber), 
                                                    String.Concat(this.BuildingArea)));
            }
            set 
            { 
                this.currentaccomodation = "#" + value ;
            } 
        }
        public string UserId { get; set; }
        public string AdminKey { get; set; }
    }
}
