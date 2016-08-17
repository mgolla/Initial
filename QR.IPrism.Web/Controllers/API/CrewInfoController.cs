
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
using System.Threading.Tasks;

namespace QR.IPrism.API.Controllers.Module
{
     [Authorize(Roles = "F1, F2, CS, CSD, PO, LT, Admin, CrewLocator")]
    public class CrewInfoController : ApiBaseController
    {
         private readonly IOverviewAdapter _overviewAdapter;
         public CrewInfoController(IOverviewAdapter overviewAdapter)
        {
            _overviewAdapter = overviewAdapter;
        }
         public async Task<HttpResponseMessage> Post(CommonFilterModel filter)
        {
            filter.FlightNo = filter.FlightNo.Replace(FlightPrefix.Prefix, "");
            return Request.CreateResponse(HttpStatusCode.OK, await _overviewAdapter.GetCrewInfoAsyc(filter));
        }

         public async Task<HttpResponseMessage> Get(CommonFilterModel filter)
        {
            return Request.CreateResponse(HttpStatusCode.OK, await _overviewAdapter.GetCrewInfoAsyc(filter));
        }
    }
}

