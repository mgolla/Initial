using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Web.API.Shared;

namespace QR.IPrism.Web.API.Controllers.Shared
{
     [Authorize(Roles = "F1, F2, CS, CSD, PO, LT, Admin, CrewLocator")]
    public class MenuManagerController : ApiBaseController
    {
        private readonly ISharedAdapter _srdAdapter;
        public MenuManagerController(ISharedAdapter srdAdapter)
        {
            _srdAdapter = srdAdapter;
        }
        [Route("api/MenuManager")]
        public HttpResponseMessage Get()
        {
            string staffNo = LoggedInStaffNo;
            return Request.CreateResponse(HttpStatusCode.OK, _srdAdapter.GetUserMenuListAsyc(LoggedInStaffNo).Result);
        }
    }
}
