using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class FlightDetailsEO
    {

        /// <summary>
        /// Get or sets Flight Details ID
        /// </summary>

        public string FlightDetsID { get; set; }

        /// <summary>
        /// Get or sets Flight Number
        /// </summary>

        public string FlightNumber { get; set; }

        /// <summary>
        /// Get or sets Sector From
        /// </summary>

        public string SectorFrom { get; set; }

        /// <summary>
        /// Get or sets Sector To
        /// </summary>

        public string SectorTo { get; set; }

        /// <summary>
        /// Get or sets combination of Sector From & To
        /// </summary>

        public string Sector { get; set; }

        /// <summary>
        /// Get or sets  Scheduled Departure Time
        /// </summary>

        public DateTime ScheDeptTime { get; set; }

        /// <summary>
        ///  Get or sets Scheduled Arrival Time
        /// </summary>

        public DateTime ScheArrTime { get; set; }

        /// <summary>
        /// Get or sets  Actual Departure Time
        /// </summary>

        public DateTime ActDeptTime { get; set; }

        /// <summary>
        /// Get or sets  Actual Arrival Time
        /// </summary>

        public DateTime ActArrTime { get; set; }

        /// <summary>
        /// Get or sets Air Craft Reg No
        /// </summary>

        public string AirCraftRegNo { get; set; }

        /// <summary>
        /// Get or sets Air Craft Type
        /// </summary>

        public string AirCraftType { get; set; }

        /// <summary>
        /// Get or sets Air Craft Family
        /// </summary>

        public string AirCraftFamily { get; set; }

        /// <summary>
        /// Get or sets Passenger Load in First class
        /// </summary>

        public int PassengerLoadFC { get; set; }

        /// <summary>
        /// Get or sets Passenger Load in Business class
        /// </summary>

        public int PassengerLoadJC { get; set; }

        /// <summary>
        /// Get or sets Passenger Load in Economy class
        /// </summary>

        public int PassengerLoadYC { get; set; }

        /// <summary>
        /// Get or sets Infant Load in First class
        /// </summary>

        public int InfantLoadFC { get; set; }

        /// <summary>
        /// Get or sets Infant Load in Business class
        /// </summary>

        public int InfantLoadJC { get; set; }

        /// <summary>
        /// Get or sets Infant Load in Economy class
        /// </summary>

        public int InfantLoadYC { get; set; }

        /// <summary>
        /// Get or sets Is Grooming Check Done 
        /// </summary>

        public string IsGroomingCheck { get; set; }

        /// <summary>
        /// Get or sets Is CSD CSD Briefed Done
        /// </summary>

        public string IsCsdCsBriefed { get; set; }

        /// <summary>
        /// Get or sets Seat Capacity in First Class
        /// </summary>

        public int SeatCapacityFC { get; set; }

        /// <summary>
        /// Get or sets Seat Capacity in Business Class
        /// </summary>

        public int SeatCapacityJC { get; set; }

        /// <summary>
        /// Get or sets Seat Capacity in Economy Class
        /// </summary>

        public int SeatCapacityYC { get; set; }

        /// <summary>
        /// Get or sets Crew Complement in Flight
        /// </summary>

        public string CrewComplement { get; set; }

        /// <summary>
        ///  Gets or sets Is Active
        /// </summary>

        public string IsActive { get; set; }

        /// <summary>
        ///  Gets or sets Created By
        /// </summary>

        public string CreatedBy { get; set; }

        /// <summary>
        ///  Gets or sets IsDelay
        /// </summary>

        public string IsDelay { get; set; }

        /// <summary>
        ///  Gets or sets DepartureDelay
        /// </summary>

        public string DepartureDelay { get; set; }

        /// <summary>
        ///  Gets or sets DelayTags
        /// </summary>

        public string DelayTags { get; set; }
    }
}
