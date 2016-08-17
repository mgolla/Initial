using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;

namespace QR.IPrism.Adapter.Implementation
{
    public class EmployeeAdapter:IEmployeeAdapter
    {
        List<EmployeeModel> _empList = new List<EmployeeModel>();
        public EmployeeAdapter()
        {
            _empList.Add(new EmployeeModel()
            {
                Country = "UK",
                DOB = "04-Apr-2015",
                EmpSeqId = 1,
                FirstName = "Caroline",
                MiddleName = "Christian",
                LastName = "Brown",
                StaffNumber = "48478",
                Gender = "F"

            });
            _empList.Add(new EmployeeModel()
            {
                Country = "US",
                DOB = "24-May-2015",
                EmpSeqId = 2,
                FirstName = "Deirdre",
                MiddleName = "Dan",
                LastName = "Cameron",
                StaffNumber = "23432",
                Gender = "M"
            });
            _empList.Add(new EmployeeModel()
            {
                Country = "IND",
                DOB = "30-Jun-2015",
                EmpSeqId = 3,
                FirstName = "Emily",
                MiddleName = "David",
                LastName = "Campbell",
                StaffNumber = "67676",
                Gender = "F"
            });
            _empList.Add(new EmployeeModel()
            {
                Country = "FRN",
                DOB = "23-Jul-2015",
                EmpSeqId = 4,
                FirstName = "Michelle",
                MiddleName = "Melanie",
                LastName = "Coleman",
                StaffNumber = "45656",
                Gender = "F"
            });
        }
        public async Task<List<EmployeeModel>> GetAsync()
        {
            return _empList;
        }

        public async Task<EmployeeModel> GetAsync(string id)
        {
            return _empList.FirstOrDefault(ep => ep.StaffNumber.Equals(id));
        }

        public async Task<ResponseModel> PostAsync(EmployeeModel emp)
        {
            _empList.Add(emp);
            ResponseModel response = new ResponseModel();
            response.IsSuccess = true;
            response.Message = "Employee details addedd successfully";
            return response;
        }
        public async Task<ResponseModel> PutAsync(EmployeeModel emp)
        {
            _empList.Add(emp);
            ResponseModel response = new ResponseModel();
            response.IsSuccess = true;
            response.Message = "Employee details addedd successfully";
            return response;
        }
        public async Task<ResponseModel> DeleteAsync(string id)
        {
            ResponseModel response = new ResponseModel();
            response.IsSuccess = true;
            return response;
        }
    }
}
