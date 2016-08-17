
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Web.API.Shared;
using QR.IPrism.Models;
using QR.IPrism.Models.Shared;
using System.Web;
using System.IO;
using QR.IPrism.Models.Module;
using System.Threading.Tasks;
using QR.IPrism.Web.Helper;
using System.Text;
using QR.IPrism.Web.Models;

namespace QR.IPrism.API.Controllers.Module
{
    [Authorize(Roles = "F2, F1, CS, CSD, PO, LT, Admin")]
    public class CrewProfileController : ApiBaseController
    {
        private readonly ICrewProfileAdapter _crewProfileAdapter;
        public CrewProfileController(ICrewProfileAdapter crewProfileAdapter)
        {
            _crewProfileAdapter = crewProfileAdapter;
        }

        [Route("api/crewtraininghistory/")]
        public HttpResponseMessage GetTrainingHistory()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _crewProfileAdapter.GetTrainingHistory(LoggedInStaffNo).Result);
        }

        [Route("api/crewqualvisa/")]
        public HttpResponseMessage GetQualnVisa()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _crewProfileAdapter.GetQualnVisaDetails(LoggedInStaffNo).Result);
        }

        [Route("api/crewcareerpath/")]
        public HttpResponseMessage GetCareerPath()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _crewProfileAdapter.GetCareerPathDetails(LoggedInStaffNo).Result);
        }

        [Route("api/crewidp/")]
        public HttpResponseMessage GetIDPDetails()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _crewProfileAdapter.GetIDPDetails(LoggedInStaffNo).Result);
        }

        [Route("api/crewmydoc/")]
        public HttpResponseMessage GetMyDocDetails()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _crewProfileAdapter.GetMyDocDetails(LoggedInStaffNo).Result);
        }

        [Route("api/crewdstvstd/")]
        public HttpResponseMessage GetDestVisitedDetails()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _crewProfileAdapter.GetDestinationsVisitedDetails(LoggedInStaffNo).Result);
        }
    }
}

