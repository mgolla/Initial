using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Adapter.Interfaces
{
    public interface IFlightAddEditAdapter
    {
        Task<IEnumerable<FlightInfoModel>> GetFlightDetails(FlightDelayFilterModel filter, string staffNo, string userId);
        Task<FlightInfoModel> GetSingleFlight(string flightDetailsId, string userId);
        Task<IEnumerable<FlightInfoModel>> GetFlightDetailForPaste(string staffId, string flightDetailsId);
        Task<IEnumerable<FlightCrewsModel>> GetAutoSuggestStaffInformation(SearchCriteriaModel searchCriteria);
        Task<IEnumerable<FlightCrewsModel>> GetAutoSuggestStaffByGrade(SearchCriteriaModel searchCriteria, string grade);
        Task<ResponseModel> InsertFlightDetails(FlightInfoModel inputs, string staffId);
        Task<string> IsVrForFlight(string id);
        Task<string> IsDelayRportForFlight(string id);
        void DeleteFlightDetails(string id);
        Task<IEnumerable<FlightCrewsModel>> GetCrewsForFlight(string flightDetailsId);
    }
}
