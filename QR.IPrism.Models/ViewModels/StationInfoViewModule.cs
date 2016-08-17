
using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.ViewModels
{
    public class StationInfoViewModel
    {
        public List<StationInfoModel> StationInfosList { get; set; }
        public StationInfoModel StationInfo { get; set; }
    }
}


