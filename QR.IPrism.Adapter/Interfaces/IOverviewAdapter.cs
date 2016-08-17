
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Adapter.Interfaces
{
    public interface IOverviewAdapter
    {
        Task<OverviewViewModel> GetOverviewAsyc(OverviewFilterModel filterInput);
       // Task<List<OverviewViewModel>> GetOverviewsAsyc(OverviewFilterModel filterInput);
        Task<FlightLoadModel> GetFlightLoadAsyc(OverviewFilterModel filterInput);
        Task<StationInfoViewModel> GetStationInfoAsyc(StationInfoFilterModel filterInput);
        Task<HotelInfoViewModel> GetHotelInfoAsyc(HotelInfoFilterModel filterInput);
        Task<CrewInfoViewModel> GetCrewInfoAsyc(CommonFilterModel filterInput);
        Task<SummaryOfServiceViewModel> GetSummaryOfServicesAsyc(SummaryOfServiceFilterModel filterInput);
    }
}


