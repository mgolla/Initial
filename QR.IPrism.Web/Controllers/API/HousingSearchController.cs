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
     [Authorize(Roles = "F1,F2,CS,CSD,LT,Admin")]
    public class HousingSearchController : ApiBaseController
    {
        #region Private variables
        private readonly IHousingAdapter _housingAdapter;
        #endregion

        #region Constructor
        public HousingSearchController(IHousingAdapter housingAdapter)
        {
            _housingAdapter = housingAdapter;
        }
        #endregion

        #region API Methods

        /// <summary>
        /// Gets the Housing request search details based on filter.
        /// </summary>
        /// <param name="filter">Housing Filter</param>
        /// <returns>List of Housing request</returns>
        public HttpResponseMessage Post(HousingRequestFilterModel filter)
        {
            filter.StaffId = LoggedInStaffNo;;
            return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.GetHousingSearchResultAsyc(filter).Result);
        }

        #endregion
    }
}
