
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Web.API.Shared;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QR.IPrism.Adapter.Interfaces;

namespace QR.IPrism.API.Controllers.Module
{
     [Authorize(Roles = "F1, F2, CS, CSD, PO, LT, Admin, CrewLocator")]
    public class SearchController : ApiBaseController
    {
        private readonly ISearchAdapter _searchAdapter;
        public SearchController(ISearchAdapter searchAdapter)
        {
            _searchAdapter = searchAdapter;
        }
        

         [Route("api/Search/GetFlights")]
        public HttpResponseMessage PostFlights(CommonFilterModel filter)
        {
           return Request.CreateResponse(HttpStatusCode.OK, _searchAdapter.GetFlightInfosAsyc(filter).Result);
        }

         [Route("api/Search/GetDocumentSearchResult")]
         public HttpResponseMessage PostDocumentSearchResult(SearchFilterModel filter)
         {
           
             return Request.CreateResponse(HttpStatusCode.OK, _searchAdapter.GetSearchAsyc(filter).Result);
         }
         [Route("api/Search/GetCurrencyDetails")]
         public HttpResponseMessage PostCurrencyDetails(CommonFilterModel filter)
         {
             return Request.CreateResponse(HttpStatusCode.OK, _searchAdapter.GetCurrencyDetailsAsyc(filter).Result);
         }
         [Route("api/Search/GetTransports")]
         public HttpResponseMessage PostTransports(CommonFilterModel filter)
         {
             filter.StaffID = LoggedInStaffNo;
             return Request.CreateResponse(HttpStatusCode.OK, _searchAdapter.GetTrainingTransportsAsyc(filter).Result);
         }
         [Route("api/Search/GetWeatherInfos")]
         public HttpResponseMessage WeatherInfos(CommonFilterModel filter)
         {
             return Request.CreateResponse(HttpStatusCode.OK, _searchAdapter.GetWeatherInfosAsyc(filter).Result);
         }

          [Authorize(Roles = " Admin, CrewLocator")]
         [Route("api/Search/GetCrewLocators")]
         public HttpResponseMessage CrewLocators(CommonFilterModel filter)
         {
             return Request.CreateResponse(HttpStatusCode.OK, _searchAdapter.GetCrewLocatorsAsyc().Result);
         }
         // [HttpPost]
         [Route("api/Search/GetAutoSuggestStaffInfo")]
         public HttpResponseMessage GetAutoSuggestStaffInfo(string filter)
         {
             return Request.CreateResponse(HttpStatusCode.OK, _searchAdapter.GetAutoSuggestStaffInfo(filter).Result);
         }

         [Route("api/Search/GetLocationCrewDetails")]
         public HttpResponseMessage LocationCrewDetails(CommonFilterModel filter)
         {
             return Request.CreateResponse(HttpStatusCode.OK, _searchAdapter.GetLocationCrewDetailsAsyc(filter).Result);
         }
         [Route("api/Search/GetDutySummarys")]
         public HttpResponseMessage DutySummarys(CommonFilterModel filter)
         {
             return Request.CreateResponse(HttpStatusCode.OK, _searchAdapter.GetDutySummarysAsyc(filter).Result);
         }
         [Route("api/Search/GetLocationFlights")]
         public HttpResponseMessage LocationFlights(CommonFilterModel filter)
         {
             return Request.CreateResponse(HttpStatusCode.OK, _searchAdapter.GetLocationFlightsAsyc(filter).Result);
         }
         [Route("api/Search/GeCrewLocatorProcess")]
         public HttpResponseMessage RunCrewLocatorProcess(CommonFilterModel filter)
         {
             return Request.CreateResponse(HttpStatusCode.OK, _searchAdapter.RunCrewLocatorProcess().Result);
         }

    }
}

