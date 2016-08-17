using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Models.Module;
using QR.IPrism.Web.API.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QR.IPrism.Web.Controllers.API
{
    public class FlightDelayController : ApiBaseController
    {
        #region Private variables
        private readonly IFlightDelayAdapter _flightDelayAdapter;
        private readonly ISharedAdapter _sharedAdapter;
        #endregion

        #region Constructor
        public FlightDelayController(IFlightDelayAdapter flightDelayAdapter, ISharedAdapter sharedAdapter)
        {
            _flightDelayAdapter = flightDelayAdapter;
            _sharedAdapter = sharedAdapter;
        }
        #endregion

        [Route("api/GetDelaysearchresults")]
        public HttpResponseMessage PostDelaySearchResults(FlightDelayFilterModel inputs)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _flightDelayAdapter.GetDelaySearchResults(inputs, LoggedInStaffNo, LoggedInUserId).Result);
        }

        [Route("api/GetEnterflightdelayresults")]
        public HttpResponseMessage PostEnterDelayFlightDetails(FlightDelayFilterModel inputs)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _flightDelayAdapter.GetEnterDelayFlightDetails(inputs, LoggedInStaffNo, LoggedInStaffDetailId).Result);
        }

        //[Route("api/GetSectordetails")]
        //public HttpResponseMessage GetSectorDetails()
        //{
        //    return Request.CreateResponse(HttpStatusCode.OK, _flightDelayAdapter.GetSectorDetails().Result);
        //}

        [Route("api/SetFlightdelaydetails")]
        public HttpResponseMessage PostFlightDelayDetails(List<FlightDelayModel> input)
        {
            foreach (var item in input)
            {
                _flightDelayAdapter.SetFlightDelayDetails(item, LoggedInStaffDetailId, LoggedInStaffNo);
            }
           
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        [Route("api/GetDelaylookupvalues")]
        public HttpResponseMessage GetDelayLookupValues()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _flightDelayAdapter.GetDelayLookupValues().Result);
        }

        [Route("api/GetDelayReason/{id}")]
        public HttpResponseMessage GetDelayReason(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _flightDelayAdapter.GetDelayReasons(id).Result);
        }


        [Route("api/IsEnterDelayForFlight/{id}")]
        public HttpResponseMessage GetIsEnterDelayForFlight(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _flightDelayAdapter.IsEnterDelayForFlight(id));
        }
    }
}
