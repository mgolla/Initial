
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
    public class HotelInfoController : ApiBaseController
    {
        private readonly IOverviewAdapter _overviewAdapter;
        public HotelInfoController(IOverviewAdapter overviewAdapter)
        {
            _overviewAdapter = overviewAdapter;
        }
                      
        public HttpResponseMessage Post(HotelInfoFilterModel filter)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _overviewAdapter.GetHotelInfoAsyc(filter).Result);
        }

        public HttpResponseMessage Get(HotelInfoFilterModel filter)
        {
            //Test Data 
            //HotelInfoFilterModel filter = new HotelInfoFilterModel();
            //filter.AirportCode = "DOH";
            return Request.CreateResponse(HttpStatusCode.OK, _overviewAdapter.GetHotelInfoAsyc(filter).Result);
        }
    }
}

