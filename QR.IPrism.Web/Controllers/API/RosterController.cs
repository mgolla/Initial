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
using System.IO;
using System.Configuration;
using System.Web;
using System.Threading.Tasks;


namespace QR.IPrism.API.Controllers.Module
{
     [Authorize(Roles = "F1, F2, CS, CSD, PO, LT, Admin, CrewLocator")]
    public class RosterController : ApiBaseController
    {
        private readonly IRosterAdapter _rosterAdapter;
        public RosterController(IRosterAdapter rosterAdapter)
        {
            _rosterAdapter = rosterAdapter;
        }
        [Route("api/Roster/GetRosters")]
        public async Task<HttpResponseMessage> PostRosters(RosterFilterModel filter)
        {
            if (string.IsNullOrEmpty(filter.StaffID )) { 
             filter.StaffID = LoggedInStaffNo;
            }

            return Request.CreateResponse(HttpStatusCode.OK, await _rosterAdapter.GetRostersAsyc(filter));
        }

        [Route("api/AssessmentRoster")]
        public HttpResponseMessage PostAssessmentRosters(RosterFilterModel filter)
        {
            if (string.IsNullOrEmpty(filter.StaffID))
            {
                filter.StaffID = LoggedInStaffNo;
            }

            return Request.CreateResponse(HttpStatusCode.OK, _rosterAdapter.GetRosterForAssmt(filter).Result);
        }

        [Route("api/Roster/GetWeeklyRosters")]
        public async Task<HttpResponseMessage> PostWeeklyRosters(RosterFilterModel filter)
        {
            filter.StaffID = LoggedInStaffNo;
            return Request.CreateResponse(HttpStatusCode.OK, await _rosterAdapter.GetWeeklyRostersAsyc(filter));
        }

        [Route("api/Roster/GetBackgroundImage")]
        public HttpResponseMessage PostBackgroundImage(BackgroundFilterModel filter)
        {
            Background background = new Background();

            if (!string.IsNullOrEmpty(filter.ImagePath) && File.Exists(filter.ImagePath))
            {
                background.Image = File.ReadAllBytes(filter.ImagePath);

            }
            return Request.CreateResponse(HttpStatusCode.OK, background);
        }

        [Route("api/Roster/GetDefaultBackgroundImage")]
        public HttpResponseMessage PostDefaultBackgroundImage(BackgroundFilterModel filter)
        {
            Background background = new Background();

            var content = HttpContext.Current.Server.MapPath("~/Content");
            if (content != null)
            {
                var path = content + @"\css\styles\images\bg\1.jpg";
                if (File.Exists(path))
                {
                    background.Image = File.ReadAllBytes(path);
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, background);
        }


        [Route("api/dutycodes/{id}")]
        public async Task<HttpResponseMessage> GetCodeExplanations(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, await _rosterAdapter.GetCodeExplanationsAsyc(id));
        }
        [Route("api/utcdiff/post")]
        [HttpPost]
        public async Task<HttpResponseMessage> LoadUTCDiffs(FliterModel filter)
        {
            return Request.CreateResponse(HttpStatusCode.OK, await _rosterAdapter.GetUTCDiffAsyc(filter.Type));
        }

        [Route("api/printhotelinfo/post")]
        [HttpPost]
        public async Task<HttpResponseMessage> PrintHotelInfo(HotelInfoFilterModel filter)
        {
            return Request.CreateResponse(HttpStatusCode.OK, await _rosterAdapter.GetPrintHotelInfosAsyc(filter));
        }
    }
}
