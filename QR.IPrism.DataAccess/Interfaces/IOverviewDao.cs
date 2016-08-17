using QR.IPrism.EntityObjects.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.BusinessObjects.Interfaces
{
    public interface IOverviewDao
    {
        Task<List<OverviewEO>> GetOverviewsAsyc(OverviewFilterEO filterInput);
        Task<OverviewEO> GetOverviewAsyc(OverviewFilterEO filterInput);
        Task<StationInfoEO> GetStationInfoAsyc(StationInfoFilterEO filterInput);
        Task<HotelInfoEO> GetHotelInfoAsyc(HotelInfoFilterEO filterInput);
        Task<List<CrewInfoEO>> GetCrewInfoAsyc(CommonFilterEO filterInput);
        Task<List<StaffPhoto>> GetStaffPhotoAsyc(String ids);
        Task<SOSEO> GetSummaryOfServicesAsyc(SummaryOfServiceFilterEO filterInput);
    }
}


