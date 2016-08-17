using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.DataAccess.Interfaces
{
    public interface IFlightDelayDao
    {
        Task<IEnumerable<FlightInfoEO>> GetDelaySearchResults(FlightDelayFilterEO filter, string staffNo, string staffId);
        Task<IEnumerable<FlightInfoEO>> GetEnterDelayFlightDetails(FlightDelayFilterEO filter, string staffNo, string staffId);
        //Task<IEnumerable<LookupEO>> GetSectorDetails();
        Task<IEnumerable<FlightDelayEO>> GetDelayLookupValues();
        void SetFlightDelayDetails(FlightDelayEO inputs, string staffId, string staffNo);
        Task<IEnumerable<FlightDelayEO>> GetDelayReasons(string id);
        Task<string> IsEnterDelayForFlight(string id);
    }
}
