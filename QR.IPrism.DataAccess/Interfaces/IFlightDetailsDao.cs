using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.DataAccess.Interfaces
{
    public interface IFlightDetailsDao
    {
        Task<IEnumerable<FlightInfoEO>> GetFlightDetails(FlightDelayFilterEO filter, string staffNo, string userId);
        Task<FlightInfoEO> GetSingleFlight(string flightDetailsId, string userId);
        Task<IEnumerable<FlightInfoEO>> GetFlightDetailForPaste(string staffId, string flightDetailsId);
        Task<IEnumerable<FlightCrewsEO>> GetAutoSuggestStaffInformation(SearchCriteriaEO searchCriteria);
        Task<ResponseEO> UpdateFlightDetails(FlightInfoEO inputs, string staffId);
        Task<string> IsVrForFlight(string id);
        Task<string> IsDelayRportForFlight(string id);
        void DeleteFlightDetails(string id);
        Task<IEnumerable<FlightCrewsEO>> GetCrewsForFlight(string flightDetailsId);
    }
}
