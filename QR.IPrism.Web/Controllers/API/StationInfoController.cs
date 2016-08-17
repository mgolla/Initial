using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QR.IPrism.Web.API.Shared;

namespace QR.IPrism.API.Controllers.Module
{
     [Authorize(Roles = "F1, F2, CS, CSD, PO, LT, Admin, CrewLocator")]
    public class StationInfoController : ApiBaseController
    {
        private readonly IOverviewAdapter _overviewAdapter;
        public StationInfoController(IOverviewAdapter overviewAdapter)
        {
            _overviewAdapter = overviewAdapter;
        }

        public HttpResponseMessage Post(StationInfoFilterModel filter)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _overviewAdapter.GetStationInfoAsyc(filter).Result);
        }
        public HttpResponseMessage Get(StationInfoFilterModel filter)
        {
            //StationInfoFilterModel filter = new StationInfoFilterModel();
            //filter.StationCode = "DOH";

            return Request.CreateResponse(HttpStatusCode.OK, _overviewAdapter.GetStationInfoAsyc(filter).Result);
        }
    }
}

