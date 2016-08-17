using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;
using QR.IPrism.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Adapter.Interfaces
{
    public interface IRosterAdapter
    {
        Task<RosterViewModel> GetRostersAsyc(RosterFilterModel filterInput);
        Task<List<LookupModel>> GetCodeExplanationsAsyc(string filters);
        Task<List<LookupModel>> GetUTCDiffAsyc(string filters);
        Task<List<HotelInfoModel>> GetPrintHotelInfosAsyc(HotelInfoFilterModel filterInput);
        Task<RosterViewModel> GetWeeklyRostersAsyc(RosterFilterModel filterInput);
        Task<RosterViewModel> GetRosterForAssmt(RosterFilterModel filterInput);
    }
}
