using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class RosterViewModelEO
    {
        public List<RosterEO> Rosters { get; set; }
        public string CurrentDateUTC { get; set; }
        public int RosterTypeID { get; set; }// 1- Weekly , 2- Monthly
        public int TimeFormat { get; set; }// 1- UTC , 2- Doha , 3- Local
        public int IsDataLoaded { get; set; }

        public DateTime StartDate { get; set; }//"23-OCT-2015"
        public DateTime EndDate { get; set; }//"29-OCT-2015"
        public string Range { get; set; }//23 OCT - 29 OCT or if it is monthly OCT 2015
        public int IsNext { get; set; }
        public int SelectedRosterTempID { get; set; }// 1- Weekly , 2- Monthly
        public bool IsWorking { get; set; }
        public string ErrorMgs { get; set; }
    }
}
