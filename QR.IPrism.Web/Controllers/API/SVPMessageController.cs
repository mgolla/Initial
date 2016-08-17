
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

namespace QR.IPrism.API.Controllers.Module
{
     [Authorize(Roles = "F1, F2, CS, CSD, PO, LT, Admin, CrewLocator")]
    public class SVPMessageController : ApiBaseController
    {
        private readonly IDashboardAdapter _iDashboardAdapter;
        public SVPMessageController(IDashboardAdapter iDashboardAdapter)
        {
            _iDashboardAdapter = iDashboardAdapter;
        }

        public HttpResponseMessage Post(NotificationAlertSVPFilterModel filter)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _iDashboardAdapter.GetSVPMessagesAsyc().Result);
        }
    }
}

