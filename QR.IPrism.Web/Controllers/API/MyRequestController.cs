
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
    public class MyRequestController : ApiBaseController
    {
      private readonly IDashboardAdapter _iDashboardAdapter;
      public MyRequestController(IDashboardAdapter iDashboardAdapter)
        {
            _iDashboardAdapter = iDashboardAdapter;
        }
              
        public HttpResponseMessage Post(MyRequestFilterModel filter)
        {
            filter.StaffID = LoggedInStaffNo;
            //Test Data 
            //filter.StaffID = "03566";
           return Request.CreateResponse(HttpStatusCode.OK, _iDashboardAdapter.GetMyRequestsAsyc(filter).Result);
        }
    }
}

