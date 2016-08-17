/*********************************************************************
 * Name          : Housing.cs
 * Description   : POCO class for housing request filter.
 * Create Date   : 25th Jan 2016
 * Last Modified : 25th Jan 2016
 * Copyright By  : Qatar Airways
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class HousingRequestFilterModel
    {
        public string StaffId { get; set; }
        public int RequestId { get; set; }
        public string RequestType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Status { get; set; }
    }
}
