using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class EmployeeModel
    {
        public int EmpSeqId { get; set; }
        public string Title { get; set; }
        public string StaffNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Country { get; set; }
        public string Gender { get; set; }
    }
}
