/*********************************************************************
 * Name          : Housing.cs
 * Description   : POCO class for vacant buildings.
 * Create Date   : 28th Jan 2016
 * Last Modified : 28th Jan 2016
 * Copyright By  : Qatar Airways
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class BuildingModel
    {
        [Required]
        public string BuildingDetailSid { get; set; }
        public string BuildingName { get; set; }
        public string BuildingNumber { get; set; }

        [Required]
        public string FlatId { get; set; }
        public string FlatNumber { get; set; }
        public string FlatType { get; set; }
        public string BedroomNo { get; set; }

        [Required]
        public string BedroomDetailsId { get; set; }
        public string TelephoneNo { get; set; }
        public string Nationality { get; set; }
        public string Grade { get; set; }
        public string HouseIncharge { get; set; }
        public string BuildingFacilities { get; set; }
        public string FloorNumber { get; set; }
        public string RoomCount { get; set; }

        public string Area { get; set; }
        public string StreetNo { get; set; }
        public string PostBoxNo { get; set; }
    }
}
