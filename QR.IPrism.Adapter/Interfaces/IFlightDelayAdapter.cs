using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Adapter.Interfaces
{
    public interface IFlightDelayAdapter
    {
        Task<IEnumerable<FlightInfoModel>> GetDelaySearchResults(FlightDelayFilterModel filter, string staffNo, string staffId);
        Task<IEnumerable<FlightInfoModel>> GetEnterDelayFlightDetails(FlightDelayFilterModel filter, string staffNo, string staffId);
        //Task<IEnumerable<LookupModel>> GetSectorDetails();
        Task<IEnumerable<FlightDelayModel>> GetDelayLookupValues();
        void SetFlightDelayDetails(FlightDelayModel inputs, string staffId, string staffNo);
        Task<IEnumerable<FlightDelayModel>> GetDelayReasons(string id);
        string IsEnterDelayForFlight(string id);
    }
}