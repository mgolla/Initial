using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Shared
{
    [Serializable]
    [DataContract]
    public class RoleModel
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int RoleId { get; set; }
    }
}
