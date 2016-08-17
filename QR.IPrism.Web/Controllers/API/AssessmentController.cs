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
using QR.IPrism.Utility;
using QR.IPrism.Models.Shared;

namespace QR.IPrism.Web.Controllers.API
{
    public class AssessmentController : ApiBaseController
    {
        #region Private variables
        private readonly IAssessmentAdapter _assessmentAdapter;
        private readonly ISharedAdapter _sharedAdapter;
        #endregion

        #region Constructor
        public AssessmentController(IAssessmentAdapter assessmentAdapter, ISharedAdapter sharedAdapter)
        {
            _assessmentAdapter = assessmentAdapter;
            _sharedAdapter = sharedAdapter;
        }
        #endregion

        #region GET Methods

        [Authorize(Roles = "F1,F2,CS,CSD,PO,LT,Admin")]
        [Route("api/getassessmentdetails/{id}")]
        public HttpResponseMessage GetAssessmentDetails(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _assessmentAdapter.GetAssessmentDetailsAsync(id).Result);
        }

        [Authorize(Roles = "F1,F2,CS,CSD,PO,LT,Admin")]
        [Route("api/getassessmnetbygrade/{id}")]
        public HttpResponseMessage GetAssessmnetByGrade(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _assessmentAdapter.GetAssessmnetByGradeAsync(id).Result);
        }

        [Authorize(Roles = "F1,F2,CS,CSD,PO,LT,Admin")]
        [Route("api/getassessorassesseeflightdetails/{id}")]
        public HttpResponseMessage GetAssessorAssesseeFlightDetails(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _assessmentAdapter.GetAssessorAssesseeFlightDetailsAsync(id).Result);
        }

        [Authorize(Roles = "F1,F2,CS,CSD,PO,LT,Admin")]
        [Route("api/getratingguidelines")]
        public HttpResponseMessage GetRatingGuidelines()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _assessmentAdapter.GetRatingGuidelinesAsync());
        }

        //[Route("api/getassessmentobjectivepercentages")]
        //public HttpResponseMessage GetAssessmentObjectivePercentages()
        //{
        //    return Request.CreateResponse(HttpStatusCode.OK, _assessmentAdapter.GetAssessmentObjectivePercentagesAsync());
        //}

        [Authorize(Roles = "F1,F2,CS,CSD,PO,LT,Admin")]
        [Route("api/getBehaviourNotification/{id}")]
        public HttpResponseMessage GetBehaviourActionIssue(string id)
        {
            var notification = _sharedAdapter.SearchNotifications(id, LoggedInStaffNo).Result.ToList();
            if (notification != null)
            {
                var behidp = _assessmentAdapter.GetBehaviourActionIssue(notification[0].RequestGuid).Result;
                behidp.RequestGuid = notification[0].RequestGuid;
                return Request.CreateResponse(HttpStatusCode.OK, behidp);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
            }
        }

        [Authorize(Roles = "F1,F2,CS,CSD,PO,LT,Admin")]
        [Route("api/postBehaviourIdp")]
        public HttpResponseMessage PostBehaviourIdp(MessageModel response)
        {
            var notfy = new NotificationDetailsModel()
            {
                Id = response.Id,
                Status = "D"
            };

            if (_sharedAdapter.UpdateCrewNotificationDetails(notfy).Result)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _assessmentAdapter.InsertIDPCrewComment(response.RequestGuid, LoggedInStaffNo, response.Status, response.Message).Result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
            }
        }

        [Authorize(Roles = "CS,CSD,PO,LT,Admin")]
        [Route("api/DeleteAssessment/{id}")]
        public HttpResponseMessage GetDeleteAssessment(String id)
        {
            _assessmentAdapter.DeleteAssessment(id);
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        #endregion

        #region Post Methods

        [Authorize(Roles = "CS,CSD,PO,LT,Admin")]
        [Route("api/insertupdateassessment")]
        public HttpResponseMessage Insert_UpdateAssessObjectCompDetails(AssessmentModel assessmentdet)
        {
            assessmentdet.AssessorGrade = UserContext.Grade;
            assessmentdet.AssessorCrewDetID = LoggedInStaffDetailId;
            assessmentdet.CreatedBy = LoggedInUserId;
            if (assessmentdet.AssessmentStatus == Assessment.COMPLETED)
            {
                if (ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, _assessmentAdapter.Insert_UpdateAssessObjectCompDetailsAsync(assessmentdet, LoggedInStaffNo).Result);

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, assessmentdet);
                }
            }


            return Request.CreateResponse(HttpStatusCode.OK, _assessmentAdapter.Insert_UpdateAssessObjectCompDetailsAsync(assessmentdet, LoggedInStaffNo).Result);

        }
        #endregion
    }
}