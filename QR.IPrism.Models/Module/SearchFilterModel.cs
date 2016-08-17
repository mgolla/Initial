using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class SearchFilterModel
    {
        public string Query { get; set; }
        public int Start { get; set; }
        public int MaxSearchResult { get; set; }
        public string IndexDirectory { get; set; }
        public string DocType { get; set; }
        public string FileCode{ get; set; }

    }
}
