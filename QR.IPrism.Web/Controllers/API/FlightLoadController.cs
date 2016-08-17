using System.Net;
using System.Net.Http;
using System.Web.Http;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Utility;
using QR.IPrism.Web.API.Shared;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;

namespace QR.IPrism.API.Controllers.Module
{
    public class FlightLoadController : ApiBaseController
    {
        private readonly IOverviewAdapter _overviewAdapter;
        public FlightLoadController(IOverviewAdapter overviewAdapter)
        {
            _overviewAdapter = overviewAdapter;
        }
              
        public HttpResponseMessage Post(OverviewFilterModel filter)
        {
            filter.FlightNo = filter.FlightNo;
            filter.StaffNo = LoggedInStaffNo;
            //Test Data 
            //filter.Sectorfrom = "DOH";
            //filter.SectorTo = "YUL";
            //filter.FlightNo = "420";
            //filter.STD = "22-Jan-2016";

            //filter.Sectorfrom = "DOH";
            //filter.FlightNo = "1076";
            //filter.STD = "22-Jan-2016";

            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new OverviewFilterModel());// _overviewAdapter.GetFlightLoadAsyc(filter).Result);
            }
            catch (System.Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Flight load service not working!!!!");
            }
           
        }

        public HttpResponseMessage Get(OverviewFilterModel filter)
        {
            //Test Data 
            //OverviewFilterModel filter = new OverviewFilterModel();
            //filter.Sectorfrom = "DOH";
            //filter.FlightNo = "420";
            //filter.STD = "22-Jan-2016";

            //filter.Sectorfrom = "DOH";
            //filter.FlightNo = "1076";
            //filter.STD = "22-Jan-2016";
            return Request.CreateResponse(HttpStatusCode.OK, new OverviewFilterModel());
            //return Request.CreateResponse(HttpStatusCode.OK, _overviewAdapter.GetFlightLoadAsyc(filter).Result);
        }
    }
}