using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using QR.IPrism.Web.API.Shared;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Models;
using QR.IPrism.Exception.Results;
using QR.IPrism.Models.Shared;

namespace QR.IPrism.Web.API.Controllers.Shared
{
    [Authorize(Roles = "F1, F2, CS, CSD, PO, LT, Admin, CrewLocator")]
    public class LookupController : ApiBaseController
    {
        private readonly ILookupAdapter _lookUpAdapter;
        public LookupController(ILookupAdapter lookUpAdapter)
        {
            _lookUpAdapter = lookUpAdapter;
        }
        [Route("api/lookup/{id}")]
        public HttpResponseMessage Get(string id)
        {
            var lookupdata = _lookUpAdapter.GetLookupListAsync(id).Result;
            return Request.CreateResponse(HttpStatusCode.OK, lookupdata);
        }

        [Route("api/lookup/filter/{id}/{filterText}")]
        public HttpResponseMessage GetFilteredData(string id, string filterText)
        {
            if (filterText != null)
            {
                var lookupData = _lookUpAdapter.GetFilteredDataAsync(id, filterText).Result;
                return Request.CreateResponse(HttpStatusCode.OK, lookupData);
            }
            else
            {
                var lookupData = _lookUpAdapter.GetLookupListAsync(id).Result;
                return Request.CreateResponse(HttpStatusCode.OK, lookupData);
            }
        }

        [Route("api/lookup/autocomplete/{autoCompleteCategory}/{searchText}")]
        public HttpResponseMessage GetAutoComplete(string autoCompleteCategory, string searchText)
        {
            var lookupData = _lookUpAdapter.GetAutoCompleteAsync(autoCompleteCategory, searchText).Result;
            return Request.CreateResponse(HttpStatusCode.OK, lookupData);
        }

        [Route("api/lookup/autocomplete/{autoCompleteCategory}/{searchText}/{filterText}")]
        public HttpResponseMessage GetAutoComplete(string autoCompleteCategory, string searchText, string filterText)
        {
            var lookupData = _lookUpAdapter.GetAutoCompleteAsync(autoCompleteCategory, searchText, filterText).Result;
            return Request.CreateResponse(HttpStatusCode.OK, lookupData);
        }
        [Route("api/lookup/getGrade")]
        public HttpResponseMessage GetGrade()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _lookUpAdapter.GetGradeAsync().Result);
        }

        [Route("api/lookup/getlookupbyfilter")]
        [HttpPost]
        public HttpResponseMessage GetLookUpData(LookupSearchModel search)
        {
            search.StaffNo = LoggedInStaffNo;
            return Request.CreateResponse(HttpStatusCode.OK, _lookUpAdapter.GetLookupListAsync(search).Result);
        }

        [Route("api/getGradeByLoggedPerson")]
        public HttpResponseMessage PostGradeByLoggedPerson ()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _lookUpAdapter.GetGradeByLoggedPersonAsync(UserContext.Grade).Result);
        }
        [Route("api/lookup/getAssmtStatus")]
        public HttpResponseMessage GetAssmtStatus()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _lookUpAdapter.GetAssmtStatusAsync().Result);
        }
        [Route("api/lookup/getPendingAssessment")]
        public HttpResponseMessage GetPendingAssessment()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _lookUpAdapter.GetPendingAssessmentAsync().Result);
        }
        [Route("api/lookup/getGradeCsCsd")]
        public HttpResponseMessage GetGradeCSCSD()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _lookUpAdapter.GetGradeCSDCS().Result);
        }

        //[Route("api/lookup/getSectorFrom")]
        //public HttpResponseMessage GetSectorFrom()
        //{
        //    return Request.CreateResponse(HttpStatusCode.OK, _lookUpAdapter.GetSectorFrom().Result);
        //}

        //[Route("api/lookup/getSectorTo")]
        //public HttpResponseMessage GetSectorTo()
        //{
        //    return Request.CreateResponse(HttpStatusCode.OK, _lookUpAdapter.GetSectorTo().Result);
        //}
       
    }
}