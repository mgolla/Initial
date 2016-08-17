using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class TrainingHistoryEO
    {
        public String Name { get; set; }
        public Decimal NoOfDays { get; set; }
        // Keeping Date as String as the format is yet to be checked form Database 
        // as in few cases its seen that Date is of format 14-Feb-2013 (5 Years 1 month).
        public String Date { get; set; }
    }
}
