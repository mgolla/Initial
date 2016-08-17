using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Web.API.Shared;
using QR.IPrism.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QR.IPrism.API.Controllers.Module
{
    [Authorize(Roles = "CS, CSD, PO, LT, Admin")]
    public class EVRSearchController : ApiBaseController
    {
        #region Private variables
        private readonly IEVRAdapter _evrAdapter;
        #endregion


        #region Constructor
        public EVRSearchController(IEVRAdapter evrAdapter)
        {
            _evrAdapter = evrAdapter;
        }
        #endregion

        #region API Methods

        /// <summary>
        /// Gets the eVR request search details based on filter.
        /// </summary>
        /// <param name="filter">EVR Filter</param>
        /// <returns>List of EVR request</returns>
        public HttpResponseMessage Post(EVRRequestFilterModel filter)
        {
            filter.StaffId = LoggedInStaffNo; ;
            return Request.CreateResponse(HttpStatusCode.OK, _evrAdapter.GetEVRSearchResult(filter).Result);
        }

        #endregion
    }
}