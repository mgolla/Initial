
using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.ViewModels
{
    public class CrewInfoViewModel
    {
        public List<CrewInfoModel> CrewInfosList { get; set; }
        public List<CrewInfoModel> CP { get; set; }
        public List<CrewInfoModel> CSD { get; set; }
        public List<CrewInfoModel> CS { get; set; }
        public List<CrewInfoModel> F1 { get; set; }
        public List<CrewInfoModel> F2 { get; set; }
        public CrewInfoModel CrewInfoModel { get; set; }

        public int IsDataLoaded { get; set; }
        
    }
}


