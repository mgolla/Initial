using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;
using QR.IPrism.Web.API.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QR.IPrism.Web.Controllers.API
{
    public class FlightAddEditController : ApiBaseController
    {
        #region Private variables
        private readonly IFlightAddEditAdapter _flightAddEditAdapter;
        private readonly ISharedAdapter _sharedAdapter;
        #endregion

        #region Constructor
        public FlightAddEditController(IFlightAddEditAdapter flightAddEditAdapter, ISharedAdapter sharedAdapter)
        {
            _flightAddEditAdapter = flightAddEditAdapter;
            _sharedAdapter = sharedAdapter;
        }
        #endregion

        [Route("api/GetFlightDetails")]
        public HttpResponseMessage PostFlightDetails(FlightDelayFilterModel filter)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _flightAddEditAdapter.GetFlightDetails(filter, LoggedInStaffNo, LoggedInStaffDetailId).Result);
        }

        [Route("api/GetAllLookUpDetails/{id}")]
        public HttpResponseMessage GetAllLookUpDetails(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _sharedAdapter.GetAllLookUpDetails(id).Result);
        }

        [Route("api/GetAllLookUpWithParentDetails/{id}")]
        public HttpResponseMessage GetAllLookUpWithParentDetails(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _sharedAdapter.GetAllLookUpWithParentDetails("Crew Position", "Aircraft Type", id).Result);
        }

        [Route("api/GetSingleFlight/{id}")]
        public HttpResponseMessage GetSingleFlight(string id)
        {
            //return Request.CreateResponse(HttpStatusCode.OK, _flightAddEditAdapter.GetSingleFlight(id, LoggedInStaffDetailId).Result);
            return Request.CreateResponse(HttpStatusCode.OK, _flightAddEditAdapter.GetSingleFlight(id, LoggedInUserId).Result);
        }

        [Route("api/GetFlightDetailForPaste/{id}")]
        public HttpResponseMessage GetFlightDetailForPaste(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _flightAddEditAdapter.GetFlightDetailForPaste(LoggedInStaffDetailId, id).Result);
        }

        [Route("api/GetAutoSuggestStaffByGrade/")]
        public HttpResponseMessage PostAutoSuggestStaffByGrade(SearchCriteriaModel model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _flightAddEditAdapter.GetAutoSuggestStaffByGrade(model, UserContext.Grade).Result);
        }

        [Route("api/GetAutoSuggestStaffInformation/")]
        public HttpResponseMessage PostAutoSuggestStaffInformation(SearchCriteriaModel model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _flightAddEditAdapter.GetAutoSuggestStaffInformation(model).Result);
        }

        [Route("api/InsertFlightDetails/")]
        public HttpResponseMessage PostInsertFlightDetails(FlightInfoModel model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _flightAddEditAdapter.InsertFlightDetails(model, LoggedInStaffDetailId).Result);
        }

        [Route("api/IsVrForFlight/{id}")]
        public HttpResponseMessage GetIsVrForFlight(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _flightAddEditAdapter.IsVrForFlight(id).Result);
        }

        [Route("api/IsDelayRportForFlight/{id}")]
        public HttpResponseMessage GetIsDelayRportForFlight(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _flightAddEditAdapter.IsDelayRportForFlight(id).Result);
        }

        [Route("api/DeleteFlightDetails/{id}")]
        public HttpResponseMessage GetDeleteFlightDetails(string id)
        {
            _flightAddEditAdapter.DeleteFlightDetails(id);
            return Request.CreateResponse(HttpStatusCode.OK, "success");
        }

        [Route("api/GetCrewsForFlight/{id}")]
        public HttpResponseMessage GetCrewsForFlight(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _flightAddEditAdapter.GetCrewsForFlight(id));
        }
    }
}