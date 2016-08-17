
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
using QR.IPrism.Models.Shared;

namespace QR.IPrism.API.Controllers.Module
{
    [Authorize(Roles = "F1,F2,CS,CSD,PO,LT,Admin")]
    public class NotificationAlertController : ApiBaseController
    {
        private readonly IDashboardAdapter _iDashboardAdapter;
        private readonly ISharedAdapter _srdAdapter;
        public NotificationAlertController(IDashboardAdapter iDashboardAdapter, ISharedAdapter srdAdapter)
        {
            _srdAdapter = srdAdapter;
            _iDashboardAdapter = iDashboardAdapter;
        }

        [Route("api/NotificationAlert/post")]
        [HttpPost]
        public HttpResponseMessage Post(NotificationAlertSVPFilterModel filter)
        {
            filter.StaffID = LoggedInStaffNo;
            return Request.CreateResponse(HttpStatusCode.OK, _iDashboardAdapter.GetNotificationAlertsAsyc(filter).Result);
        }
        [Route("api/AlterNotificationHeader/post")]
        [HttpPost]
        public HttpResponseMessage PostAlterNotification(NotificationAlertSVPFilterModel filter)
        {
            filter.StaffID = LoggedInStaffNo;
            return Request.CreateResponse(HttpStatusCode.OK, _iDashboardAdapter.GetAlertNotificationHeaderAsyc(filter).Result);
        }

        [Route("api/AlterNotificationHeaderUpdate/post")]
        [HttpPost]
        public HttpResponseMessage UpdateAlterNotification(NotificationAlertSVPFilterModel filter)
        {
            filter.StaffID = LoggedInStaffNo;
            return Request.CreateResponse(HttpStatusCode.OK, _iDashboardAdapter.UpdateAlertNotificationOnHeader(filter).Result);
        }


        [Route("api/notification/{id}")]
        public HttpResponseMessage GetNotification(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _srdAdapter.SearchNotifications(id, LoggedInStaffNo).Result);
        }

        [Route("api/allNotification")]
        public HttpResponseMessage PostAllNotification(NotificationDetailsModel input)
        {
            input.ToCrewId = LoggedInStaffNo;
            return Request.CreateResponse(HttpStatusCode.OK, _srdAdapter.GetAllNotifications(input).Result);
        }

        [Route("api/updatenotification/")]
        public HttpResponseMessage PostNotification(NotificationDetailsModel model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _srdAdapter.UpdateCrewNotificationDetails(model).Result);
        }

    }
}

