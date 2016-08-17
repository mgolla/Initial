using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Web.API.Shared;
using QR.IPrism.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QR.IPrism.Models.Shared;
using System.Globalization;

namespace QR.IPrism.API.Controllers.Module
{
    //[Authorize(Roles = "CS, CSD, PO, LT, Admin")]
    public class KafouController : ApiBaseController
    {

        #region Private variables
        private readonly IEVRAdapter _evrAdapter;
        private readonly ISharedAdapter _srdAdapter;
        private readonly IKafouAdapter _kfAdapter;
        private readonly IFlightAddEditAdapter _flightAddEditAdapter;
        #endregion


        #region Constructor
        public KafouController(IEVRAdapter evrAdapter, ISharedAdapter srdAdapter, IKafouAdapter kfAdapter, IFlightAddEditAdapter flightAddEditAdapter)
        {
            _evrAdapter = evrAdapter;
            _srdAdapter = srdAdapter;
            _kfAdapter = kfAdapter;
            _flightAddEditAdapter = flightAddEditAdapter;
        }
        #endregion

        #region GET API Methods

        [Route("api/kafouflights/")]
        public HttpResponseMessage GetKafouFlights()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _kfAdapter.SearchFlightsForCCR(LoggedInStaffNo).Result);
        }

        [Route("api/kfbyflightIdList/{flightIdlst}")]
        public HttpResponseMessage GetKFByFlights(string flightIdlst)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _kfAdapter.GetCrewRecognitionByFlight(flightIdlst, LoggedInStaffDetailId).Result);
        }

        [Route("api/GetKFCrewsForFlight/{flightDetailsId}")]
        public HttpResponseMessage GetCrewsForFlight(string flightDetailsId)
        {
            IEnumerable<FlightCrewsModel> crewsOnFlight = _flightAddEditAdapter.GetCrewsForFlight(flightDetailsId).Result;
            return Request.CreateResponse(HttpStatusCode.OK, crewsOnFlight);
        }

        [Route("api/getIntialKFAddEdit/{flightDetailsId}")]
        public HttpResponseMessage GetCrewsInitialRecog(string flightDetailsId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _kfAdapter.GetInitialCrewRecognition(flightDetailsId, string.Empty).Result);
        }

        [Route("api/getKFSearchParams/")]
        public HttpResponseMessage GetSearchParams()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _kfAdapter.GetCrewStatusSourceGradeData().Result);
        }

        [Route("api/kafoustatus/")]
        public HttpResponseMessage GetKafouStatus()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _kfAdapter.GetCrewStatusSourceGradeData().Result);
        }

        [Route("api/kafouRcgTypeStatus/")]
        public HttpResponseMessage GetKafouRecTypeStatus()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _kfAdapter.GetRecognitionTypeSource().Result);
        }

        //[Route("api/kafouflightdetails/")]
        //public HttpResponseMessage GetKafouFlightDetails()
        //{
        //    //return Request.CreateResponse(HttpStatusCode.OK, _kfAdapter.SearchFlightsForCCR(LoggedInUserId, 1, 30, "", "").Result);
        //    return Request.CreateResponse(HttpStatusCode.OK, _kfAdapter.SearchFlightsForCCR(LoggedInUserId).Result);
        //}

        [Route("api/kafouflighthourselapsed/{flightId}/{flightHrs}")]
        public HttpResponseMessage GetKFFlightHoursElapsed(string flightId, string flightHrs)
        {
            List<FlightDetailsModel> flightDetails = _kfAdapter.SearchFlightsForCCR(LoggedInStaffNo).Result;

            if (flightDetails == null || flightDetails.Count == 0)
                return Request.CreateResponse(HttpStatusCode.OK, "true");

            CultureInfo culture = new CultureInfo("en-US");
            FlightDetailsModel flight = flightDetails.FirstOrDefault(f => f.FlightDetsID == flightId);
            if (flight == null)
                return Request.CreateResponse(HttpStatusCode.OK, "true");

            DateTime ArrivalTime = flight.ActArrTime;
            DateTime schArrivalTime = flight.ScheArrTime;
            DateTime currentDateTime = DateTime.UtcNow;
            TimeSpan elapsedTime;
            if (flight.ActArrTime.ToString("dd-MMM-yyyy HH:mm") == DateTime.MinValue.ToString("dd-MMM-yyyy HH:mm"))
                elapsedTime = currentDateTime - schArrivalTime;
            else
                elapsedTime = currentDateTime - ArrivalTime;

            //int flilghtHrs = 0;
            //int.TryParse(_srdAdapter.GetConfigFromDB("PASTHRSDPYFLIGHTSFORENTERCCR"), out flilghtHrs);

            if (elapsedTime.TotalHours > double.Parse(flightHrs.ToString()))
                return Request.CreateResponse(HttpStatusCode.OK, "true");
            else
                return Request.CreateResponse(HttpStatusCode.OK, "false");

            
        }

        [Route("api/getKFRecogDetails/{recogId}")]
        public HttpResponseMessage GetKFRecogDetails(string recogId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _kfAdapter.GetCrewRecognition(recogId).Result);
        }

        [Route("api/getkfwalloffame")]
        public HttpResponseMessage GetWallOfFameRecognitionList()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _kfAdapter.GetWallOfFameRecognitionList().Result);
        }

        #endregion

        #region POST API Methods

        [Route("api/kfsearch/")]
        public HttpResponseMessage PostKFSearch(SearchRecognitionRequestModel filter)
        {
            filter.RecognisedCrewID = LoggedInStaffDetailId;
            return Request.CreateResponse(HttpStatusCode.OK, _kfAdapter.SearchCrewRecognitionInfo(filter).Result);
        }

        [Route("api/kafoumyrecog/")]
        public HttpResponseMessage PostKafouMyRecog(SearchRecognitionRequestModel _searchFilter)
        {
            _searchFilter.SubmittedByCrew = LoggedInStaffDetailId; ;
            return Request.CreateResponse(HttpStatusCode.OK, _kfAdapter.SearchMyRecognitionInfo(_searchFilter, LoggedInStaffDetailId).Result);
        }

        [Route("api/kafouDeleteComnDoc")]
        public HttpResponseMessage PostDeleteDocument(List<String> docs)
        {
            _srdAdapter.EVR_SetDeletedDocInActive(docs);
            _srdAdapter.DeleteAttachment_comn(docs);
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        [Route("api/kafousave/")]
        public HttpResponseMessage PostKafouSave(CrewRecognitionModel _filter)
        {
            _filter.SubmittedBy = LoggedInStaffDetailId;
            return Request.CreateResponse(HttpStatusCode.OK, _kfAdapter.InsertUpdateRecognition(_filter).Result);
            //return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        #endregion

    }
}