using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class MealEO
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string Service { get; set; }
        public string Towel { get; set; }
        public byte[] File { get; set; }
        public string FileId { get; set; }
        public string FileCode { get; set; }
        public string MenuCardName { get; set; }
        public string MenuCardType { get; set; }
        public string Moviesnack { get; set; }
        public string MoviesnackValue { get; set; }
    }
}
