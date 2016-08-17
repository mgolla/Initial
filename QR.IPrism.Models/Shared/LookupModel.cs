using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Shared
{
    public class LookupModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public string FilterText { get; set; }
        public string Type { get; set; }
        public string ParentLookUpId { get; set; }
        public string ParentLookUpName { get; set; }
        public string ParentChildLookUpName { get; set; }
    }
}
