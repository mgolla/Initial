
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
using QR.IPrism.Utility;

namespace QR.IPrism.API.Controllers.Module
{
     [Authorize(Roles = "F1, F2, CS, CSD, PO, LT, Admin, CrewLocator")]
    public class SummaryOfServiceController : ApiBaseController
    {
        private readonly IOverviewAdapter _overviewAdapter;
        public SummaryOfServiceController(IOverviewAdapter overviewAdapter)
        {
            _overviewAdapter = overviewAdapter;
        }

        public HttpResponseMessage Post(SummaryOfServiceFilterModel filter)
        {
            filter.FlightNo = FlightPrefix.Prefix + filter.FlightNo.Replace(FlightPrefix.Prefix, "");
            //Test Data 
            //filter.FlightNo = "QR874";
            //filter.SectorFrom = "DOH";
            //filter.SectorTo = "CAN";

            //filter.FlightNo = "QR1";
            //filter.SectorFrom = "DOH";
            //filter.SectorTo = "LHR";
            return Request.CreateResponse(HttpStatusCode.OK, _overviewAdapter.GetSummaryOfServicesAsyc(filter).Result);
        }
        public HttpResponseMessage Get(SummaryOfServiceFilterModel filter)
        {
            //Test Data 
            //SummaryOfServiceFilterModel filter = new SummaryOfServiceFilterModel();
            ////filter.FlightNo = "QR1";
            ////filter.SectorFrom = "DOH";
            ////filter.SectorTo = "LHR";

            //filter.FlightNo = "QR874";
            //filter.SectorFrom = "DOH";
            //filter.SectorTo = "CAN";

            return Request.CreateResponse(HttpStatusCode.OK, _overviewAdapter.GetSummaryOfServicesAsyc(filter).Result);
        }
    }
}

