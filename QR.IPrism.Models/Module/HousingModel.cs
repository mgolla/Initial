/*********************************************************************
 * Name          : Housing.cs
 * Description   : POCO class for housing module.
 * Create Date   : 25th Jan 2016
 * Last Modified : 25th Jan 2016
 * Copyright By  : Qatar Airways
 *********************************************************************/

using QR.IPrism.Models.Module;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class HousingModel
    {
        #region "Private Variables"
        private string _description;
        #endregion

        [Required]
        public string RequestId { get; set; }
        public string RequestNumber { get; set; }
        public string RequestType { get; set; }
        public string Description
        {
            get { return this._description; }
            set { this._description = RequestNumber + " - " + RequestType + " request"; }
        }

        [Required]
        public string RequestReason { get; set; }
        public DateTime RequestDate { get; set; }
        public string RequestStatus { get; set; }
        public DateTime? RequestDateClose { get; set; }
        public string AdditionalInfo { get; set; }
        public string CrewId { get; set; }
        public string DeptId { get; set; }
        public string StaffNo { get; set; }
        public string StaffName { get; set; }

        public string RequestedItemId { get; set; }
        public int RequestedQty { get; set; }

        public string MobileNo { get; set; }
        public string LandLineNo { get; set; }

        public HousingStayOutModel StayOut { get; set; }
        public BuildingModel BuildingDetails { get; set; }
        public HousingGuestModel Guests{ get; set; }
    }
}
