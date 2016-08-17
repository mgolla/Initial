using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Models;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;

namespace QR.IPrism.Adapter.Interfaces
{
    public interface IEmployeeAdapter
    {
        Task<List<EmployeeModel>> GetAsync();
        Task<EmployeeModel> GetAsync(string id);
        Task<ResponseModel> PostAsync(EmployeeModel emp);
        Task<ResponseModel> PutAsync(EmployeeModel emp);
        Task<ResponseModel> DeleteAsync(string id);
    }
}
