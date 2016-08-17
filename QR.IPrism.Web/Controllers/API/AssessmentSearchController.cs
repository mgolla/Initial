using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Models.Module;
using QR.IPrism.Web.API.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using QR.IPrism.Utility;

namespace QR.IPrism.Web.Controllers.API
{
    public class AssessmentSearchController : ApiBaseController
    {
        private readonly IAssessmentSearchAdapter _assmtSearchAdapter;
        //private readonly IPOAssessmentAdapter _poAssessmentAdapter;
        private readonly ISharedAdapter _sharedAdapter;

        public AssessmentSearchController(IAssessmentSearchAdapter assmtSearchAdapter, ISharedAdapter sharedAdapter)
        {
            _assmtSearchAdapter = assmtSearchAdapter;
            _sharedAdapter = sharedAdapter;
        }

        [Authorize(Roles = "CS,CSD,PO,LT,Admin")]
        [HttpPost]
        [Route("api/getAssmtSearchresult")]
        public HttpResponseMessage GetAssmtSearchResult(AssessmentSearchRequestFilterModel assmtSearchFilterModel)
        {
            assmtSearchFilterModel.AssessorUserID = LoggedInStaffNo;
            return Request.CreateResponse(HttpStatusCode.OK, _assmtSearchAdapter.GetAssmtSearchResultAsync(assmtSearchFilterModel).Result);
        }

        //[HttpPost]
        [Authorize(Roles = "CS,CSD,PO,LT,Admin")]
        [Route("api/getsavedUnscheduledAssmt")]
        public HttpResponseMessage SavedUnscheduledAssessment()
        {
            AssessmentSearchModel filter = new AssessmentSearchModel();
            filter.StaffNumber = LoggedInStaffDetailId;
            return Request.CreateResponse(HttpStatusCode.OK, _assmtSearchAdapter.SavedUnscheduledAssessmentAsync(filter).Result);
        }

        [Authorize(Roles = "PO,LT,Admin")]
        [Route("api/GetAllPreviousAssessment/{id}")]
        public HttpResponseMessage GetAllPreviousAssessment(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _assmtSearchAdapter.GetAllPreviousAssessment(id).Result);
        }

        [Authorize(Roles = "CS,CSD,PO,LT,Admin")]
        [Route("api/validateUnscheduledData")]
        public HttpResponseMessage PostValidateUnscheduledData(AssessmentSearchModel filter)
        {
            //AssessmentSearchModel filter = new AssessmentSearchModel();
            filter.FlightNo = FlightPrefix.Prefix + filter.FlightNo;
            filter.AssessorStaffNo = LoggedInStaffDetailId;
            return Request.CreateResponse(HttpStatusCode.OK, _assmtSearchAdapter.ValidateUnscheduledData(filter).Result);
        }

        [Authorize(Roles = "CS,CSD,PO,LT,Admin")]
        [HttpPost]
        [Route("api/getCrewExpectedAsmnt")]
        public HttpResponseMessage GetCrewExpectedAsmnt(AssessmentSearchRequestFilterModel assmtSearchFilterModel)
        {
            assmtSearchFilterModel.AssessorUserID = LoggedInStaffNo;
            return Request.CreateResponse(HttpStatusCode.OK, _assmtSearchAdapter.GetCrewExpectedAsmnt(assmtSearchFilterModel));
        }

    }
}
