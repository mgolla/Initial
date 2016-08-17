using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.BusinessObjects.Interfaces
{
    public interface IRosterDao
    {
        Task<RosterViewModelEO> GetRostersAsyc(RosterFilterEO filterInput);
        Task<List<LookupEO>> GetCodeExplanationsAsyc(string filters);
        Task<List<LookupEO>> GetUTCDiffAsyc(string filters);
        Task<List<HotelInfoEO>> GetPrintHotelInfosAsyc(HotelInfoFilterEO filterInput);
    }
}
