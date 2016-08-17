using System;
using System.Collections.Generic;
using System.Linq;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Models.Module;
using QR.IPrism.Web.API.Shared;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;

namespace QR.IPrism.Web.Controllers.API
{
    public class AssessmentListController : ApiBaseController
    {
        #region Private variables
        private readonly IAssessmentListAdapter _assessmentListAdapter;
        #endregion

        #region Constructor
        public AssessmentListController(IAssessmentListAdapter assessmentListAdapter)
        {
            _assessmentListAdapter = assessmentListAdapter;
        }
        #endregion

        #region GET Methods

        [Authorize(Roles = "CS,CSD,PO,LT,Admin")]
        public async Task<HttpResponseMessage> Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, await _assessmentListAdapter.GetAssessmentListResultAsync(LoggedInStaffDetailId));
        }

        //[HttpGet]
        //[Route("api/getPoAssessmentDetails")]
        //public HttpResponseMessage GetPoAssessmentDetails(AssessmentSearchRequestFilterModel filterInput)
        //{
        //    filterInput.AssessorUserID = LoggedInStaffNo;
        //    return Request.CreateResponse(HttpStatusCode.OK, _assessmentListAdapter.GetPOAssessmentDataAsync(filterInput));
        //}

        [Authorize(Roles = "PO,LT,Admin")]
        [Route("api/getPoAssessmentDetails")]
        public async Task<HttpResponseMessage> PostGetPoDetails(AssessmentSearchRequestFilterModel filterinput)
        {
            filterinput.AssessorUserID = LoggedInStaffDetailId;
            return Request.CreateResponse(HttpStatusCode.OK, await _assessmentListAdapter.GetPOAssessmentDataAsync(filterinput));
        }

        //[HttpPost]
        //[Route("api/searchPoAssessmentDetails")]
        //public HttpResponseMessage POPoDetails(AssessmentSearchRequestFilterModel filterinput)
        //{

        //    //AssessmentSearchRequestFilterModel filterinput = new AssessmentSearchRequestFilterModel();
        //    filterinput.AssessorUserID = LoggedInStaffDetailId;
        //    return Request.CreateResponse(HttpStatusCode.OK, _assessmentListAdapter.GetSearchCSDCSResultAsync(filterinput).Result);
        //}

        //[HttpPost]
        //[Route("api/getPoSearchResult")]
        //public HttpResponseMessage SearchPoDetails(AssessmentSearchRequestFilterModel filterInput)
        //{

        //   // AssessmentSearchRequestFilterModel filterinput = new AssessmentSearchRequestFilterModel();
        //    filterInput.AssessorUserID = LoggedInStaffDetailId;
        //    return Request.CreateResponse(HttpStatusCode.OK, _assessmentListAdapter.GetPOAssessmentDataAsync(filterInput).Result);
        //}

        [Authorize(Roles = "CS,CSD,PO,LT,Admin")]
        [Route("api/cancelScheduledasmnt/{id}")]
        public HttpResponseMessage GetCancelRequest(string id)
        {
            _assessmentListAdapter.CancelSchedluedAsmnt(id);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [Authorize(Roles = "PO,LT,Admin")]
        [Route("api/SchedulAssessment")]
        public async Task<HttpResponseMessage> ScheduleAssessment(AssessmentModel input)
        {
            input.AssessorStaffNo = LoggedInStaffNo;
            return Request.CreateResponse(HttpStatusCode.OK, await _assessmentListAdapter.ScheduleAssessment(input));
        }

        [Authorize(Roles = "F1,F2,CS,CSD,PO,LT,Admin")]
        [Route("api/getViewAssmtAsync/{id}")]
        public async Task<HttpResponseMessage> GetMyAssessmentList(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, await _assessmentListAdapter.ViewAssmtAsync(id));
        }

        [Authorize(Roles = "F1,F2,CS,CSD,PO,LT,Admin")]
        [Route("api/getPreviousAssessments")]
        public async Task<HttpResponseMessage> PostPreviousAssessmentsAsync(AssessmentSearchModel model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, await _assessmentListAdapter.GetPreviousAssessmentsAsync(model, LoggedInStaffDetailId));
        }

        #endregion

    }
}