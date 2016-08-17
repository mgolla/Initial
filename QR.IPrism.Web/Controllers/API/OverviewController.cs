
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
using QR.IPrism.Utility;
using System.IO;
using QR.IPrism.Models.ViewModels;
using System.Threading.Tasks;

namespace QR.IPrism.API.Controllers.Module
{
     [Authorize(Roles = "F1, F2, CS, CSD, PO, LT, Admin, CrewLocator")]
    public class OverviewController : ApiBaseController
    {
        private readonly IOverviewAdapter _overviewAdapter;
        public OverviewController(IOverviewAdapter overviewAdapter)
        {
            _overviewAdapter = overviewAdapter;
        }

        public async Task<HttpResponseMessage> Post(OverviewFilterModel filter)
        {
            filter.FlightNo = FlightPrefix.Prefix + filter.FlightNo;
            filter.StaffNo = LoggedInStaffNo;
            return Request.CreateResponse(HttpStatusCode.OK, await _overviewAdapter.GetOverviewAsyc(filter));
        }

        public async Task<HttpResponseMessage> Get(OverviewFilterModel filter)
        {
            filter.FlightNo = FlightPrefix.Prefix + filter.FlightNo;
            return Request.CreateResponse(HttpStatusCode.OK, await _overviewAdapter.GetOverviewAsyc(filter));
        }
    }
}

