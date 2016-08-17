
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
using System.Data;
using System.Xml.Linq;

namespace QR.IPrism.API.Controllers.Module
{
     [Authorize(Roles = "F1, F2, CS, CSD, PO, LT, Admin, CrewLocator")]
    public class DepartmentNewsController : ApiBaseController
    {
         private readonly IDashboardAdapter _iDashboardAdapter;
         public DepartmentNewsController(IDashboardAdapter iDashboardAdapter)
        {
            _iDashboardAdapter = iDashboardAdapter;
        }

         public HttpResponseMessage Post(NewsFilterModel filter)
        {
                   
            return Request.CreateResponse(HttpStatusCode.OK, _iDashboardAdapter.GetNewsAsycByType(filter).Result);
        }


         [Route("api/AirlinesNews/GetAirlinesNews")]
         public HttpResponseMessage AirlinesNews(NewsFilterModel filter)
         {
             return Request.CreateResponse(HttpStatusCode.OK, _iDashboardAdapter.GetAirlinesNewsAsyc(filter).Result);
         }
         
    }
}

