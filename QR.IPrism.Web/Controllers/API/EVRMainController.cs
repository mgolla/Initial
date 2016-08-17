using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Web.API.Shared;
using QR.IPrism.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QR.IPrism.Models.Shared;

namespace QR.IPrism.API.Controllers.Module
{    
    public class EVRMainController : ApiBaseController
    {

        #region Private variables
        private readonly IEVRAdapter _evrAdapter;
        private readonly ISharedAdapter _srdAdapter;
        #endregion


        #region Constructor
        public EVRMainController(ISharedAdapter srdAdapter, IEVRAdapter evrAdapter)
        {
            _srdAdapter = srdAdapter;
            _evrAdapter = evrAdapter;
        }
        #endregion

        #region GET API Methods

        [Authorize(Roles = "CS, CSD, PO, LT, Admin")]
        [Route("api/evrdraft/{flightDetailsId}")]
        public HttpResponseMessage GetDraftVRForUser(string flightDetailsId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _evrAdapter.GetDraftVRForUser(flightDetailsId, LoggedInUserId).Result);
        }

        [Authorize(Roles = "CS, CSD, PO, LT, Admin")]
        [Route("api/evrlastten/")]
        public HttpResponseMessage GetLastTenVRForUser()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _evrAdapter.GetVRLastTenFlight(LoggedInStaffDetailId).Result);
        }
        
        [Authorize(Roles = "PO, Admin")]
        [Route("api/evrforpoassmt/{id}")]
        public HttpResponseMessage GetEvrForPoAssmt(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _evrAdapter.GetLastTenVRs(id).Result);
        }

        [Authorize(Roles = "CS, CSD, PO, LT, Admin")]
        [Route("api/evrpending/")]
        public HttpResponseMessage GetPendingEVRs()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _evrAdapter.GetPendingVRForUser(LoggedInStaffNo).Result);
        }

        [Authorize(Roles = "CS, CSD, PO, LT, Admin")]
        [Route("api/updatenovr/{id}")]
        public HttpResponseMessage GetUpdateNOVR(string id)
        {
            _evrAdapter.UpdateNoVR(id, LoggedInStaffDetailId, LoggedInUserId);
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        [Authorize(Roles = "CS, CSD, PO, LT, Admin")]
        [Route("api/evrsubmitted/{flightDetailsId}")]
        public HttpResponseMessage GetSubmittedVRForUser(string flightDetailsId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _evrAdapter.GetSubmittedEVRs(flightDetailsId, LoggedInStaffDetailId , LoggedInUserId).Result);
        }

        [Authorize(Roles = "CS, CSD, PO, LT, Admin")]
        [Route("api/evrdrftupdate/{evrdrfid}")]
        public HttpResponseMessage GetEVRUpdate(string evrdrfid)
        {
            VRIdModel input = new VRIdModel
            {
                VrId = evrdrfid,
                VrInstanceId = null
            };
            return Request.CreateResponse(HttpStatusCode.OK, _evrAdapter.GetVRDetailEnterVR(input).Result);
        }

        [Authorize(Roles = "CS, CSD, PO, LT, Admin")]
        [Route("api/evrdelete/{evrdrfid}")]
        public HttpResponseMessage GetDeleteVR(string evrdrfid)
        {
            _evrAdapter.DeleteVR(evrdrfid);
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        [Authorize(Roles = "CS, CSD, PO, LT, Admin")]
        [Route("api/evrdetails/{evrsubmtId}")]
        public HttpResponseMessage GetEVRSubmittedView(string evrsubmtId)
        {
            VRIdModel input = new VRIdModel
            {
                VrId = evrsubmtId,
                VrInstanceId = null
            };
            return Request.CreateResponse(HttpStatusCode.OK, _evrAdapter.GetEVRViewDetailsCrew(input).Result);
        }

        #endregion

        #region POST API Methods

        [Authorize(Roles = "CS, CSD, PO, LT, Admin")]
        [Route("api/evrsavepost")]
        public HttpResponseMessage PostEVRSave(EVRReportMainModel input)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _evrAdapter.InsertVoyageReport(input, LoggedInStaffDetailId, LoggedInStaffNo, LoggedInUserId).Result);
        }

        [Authorize(Roles = "CS, CSD, PO, LT, Admin")]
        [Route("api/evrdownload/")]
        [HttpPost]
        public HttpResponseMessage PostEVRFileDownload(VRDocumentDetailModel filter)
        {
            HttpResponseMessage result = null;
            result = Request.CreateResponse(HttpStatusCode.OK);

            result.Content = new ByteArrayContent(filter.VrDocContent);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = filter.VrDocName;
            return result;
        }

        [Authorize(Roles = "CS, CSD, PO, LT, Admin")]
        [Route("api/evrDeleteComnDoc")]
        public HttpResponseMessage PostDeleteDocument(List<String> docs)
        {
            _srdAdapter.EVR_SetDeletedDocInActive(docs);
            _srdAdapter.DeleteAttachment_comn(docs);
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        #endregion

    }
}