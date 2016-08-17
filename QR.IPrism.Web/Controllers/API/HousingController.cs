using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;
using QR.IPrism.Utility;
using QR.IPrism.Web.API.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QR.IPrism.API.Controllers.Module
{
    [Authorize(Roles = "F1,F2,CS,CSD,LT,Admin")]
    public class HousingController : ApiBaseController
    {
        #region Private variables
        private readonly IHousingAdapter _housingAdapter;
        private readonly ISharedAdapter _sharedAdapter;
        #endregion

        #region Constructor
        public HousingController(IHousingAdapter housingAdapter, ISharedAdapter sharedAdapter)
        {
            _housingAdapter = housingAdapter;
            _sharedAdapter = sharedAdapter;
        }
        #endregion

        #region GET Methods

        [Route("api/getnationalitybyflat/{id}")]
        public HttpResponseMessage GetNationalityByFlat(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.GetHousingFlatNationalityAsync(id).Result);
        }

        [Route("api/getvacantbuilding")]
        public HttpResponseMessage GetVacantBuilding()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.GetHousingVacantBuildingAsync(LoggedInStaffNo).Result);
        }

        [Route("api/getoccupiedbuilding")]
        public HttpResponseMessage GetOccupBldgForSwapRooms()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.GetOccupBldgForSwapRoomsAsync(LoggedInStaffNo).Result);
        }

        [Route("api/getexistingaccom")]
        public HttpResponseMessage GetExistingAccomm()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.GetExistingAccommAsync(LoggedInStaffDetailId).Result);
        }

        [Route("api/housingnotification/{id}")]
        public HttpResponseMessage GetHousingNotification(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.GetHousingNotification(id, LoggedInStaffNo).Result);
        }

        [Route("api/getrequestdetails/{id}")]
        public HttpResponseMessage GetRequestDetails(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.GetServiceRequestDetails(id).Result);
        }

        [Route("api/getcomments/{id}")]
        public HttpResponseMessage GetComments(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _sharedAdapter.GetAllComments(id).Result);
        }

        [Route("api/cancelrequest/{id}")]
        public HttpResponseMessage GetCancelRequest(string id)
        {
            _housingAdapter.CancelHousingRequest(id);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [Route("api/crewentitlement")]
        public HttpResponseMessage GetCrewEntitlements()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.GetCrewEntitlements(LoggedInStaffDetailId).Result);
        }

        [Route("api/housingentitlements/{id}")]
        public HttpResponseMessage GetHousingEntitlements(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.GetHousingEntitlements(LoggedInStaffNo, id).Result);
        }

        [Route("api/entitlementforcrew")]
        public HttpResponseMessage PostHousingEntitlements(HousingModel model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.GetHousingEntitlements(model.CrewId, model.RequestType).Result);
        }

        [Route("api/guestdetails")]
        public HttpResponseMessage GetGuestDetails()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.GetGuestDetails(LoggedInStaffNo).Result);
        }

        [Route("api/getcrewrelations")]
        public HttpResponseMessage GetCrewRelationDetails()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.GetCrewRelationDetails(LoggedInStaffDetailId).Result);
        }

        [Route("api/staffsbyflatid")]
        public HttpResponseMessage PostStaffsByFlatId(BuildingModel building)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.GetStaffsByFlatId(building.FlatId, building.BuildingDetailSid).Result);
        }

        [Route("api/staffsforswap")]
        public HttpResponseMessage PostStaffByBedroomId_Swap(BuildingModel building)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.GetStaffByBedroomId_Swap(building.BedroomDetailsId, building.BuildingDetailSid).Result);
        }

        [Route("api/updateswapnotification")]
        public HttpResponseMessage PostUpdateFlatMateApproval(MessageModel message)
        {
            _housingAdapter.UpdateFlatMateApproval(message, LoggedInStaffNo, LoggedInStaffDetailId);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [Route("api/getstayoutrequestype")]
        public HttpResponseMessage GetStayOutRequestType()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _sharedAdapter.GetStayOutRequestType().Result);
        }

        [Route("api/getswaproomsrequesttype")]
        public HttpResponseMessage GetSwapRoomsRequestType()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _sharedAdapter.GetSwapRoomsRequestType().Result);
        }

        [Route("api/gethousingdocument/{id}")]
        public HttpResponseMessage GetHousingRequestDocument(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _sharedAdapter.GetAttachments(id).Result);
        }

        [Route("api/deleteHousingDoc")]
        public HttpResponseMessage PostDeleteDocument(List<String> docs)
        {
            _sharedAdapter.DeleteAttachment_entity(docs);
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        #endregion

        #region Post Methods

        [Route("api/housingpost")]
        public HttpResponseMessage PostChangeAcc_MoveIn(HousingModel input)
        {
            if (_housingAdapter.IsModelValid(input))
            {
                input.CrewId = LoggedInStaffNo;
                return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.CreateChangeAcc_MoveInAsync(input, LoggedInStaffDetailId, LoggedInStaffNo).Result);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseModel()
                {
                    IsSuccess = false,
                    Message = "INVALIDFIELDS"
                });
            }
        }

        [Route("api/submitguestacc")]
        public HttpResponseMessage PostGuestAcc(HousingModel input)
        {
            if (_housingAdapter.IsModelValid(input))
            {
                input.CrewId = LoggedInStaffNo;
                return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.CreateGuestAccAsync(input, LoggedInStaffDetailId, LoggedInStaffNo).Result);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseModel()
                {
                    IsSuccess = false,
                    Message = "INVALIDFIELDS"
                });
            }
        }

        [Route("api/submitmovingout")]
        public HttpResponseMessage PostMovingOut(HousingModel input)
        {
            if (_housingAdapter.IsModelValid(input))
            {

                input.CrewId = LoggedInStaffNo;
                return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.CreateMovingOutAsync(input, LoggedInStaffDetailId, LoggedInStaffNo).Result);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseModel()
                {
                    IsSuccess = false,
                    Message = "INVALIDFIELDS"
                });
            }
        }

        [Route("api/submitstayout")]
        public HttpResponseMessage PostStayOut(HousingModel input)
        {
            if (_housingAdapter.IsModelValid(input))
            {
                input.CrewId = LoggedInStaffNo;
                return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.CreateMovingOutAsync(input, LoggedInStaffDetailId, LoggedInStaffNo).Result);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseModel()
                {
                    IsSuccess = false,
                    Message = "INVALIDFIELDS"
                });
            }
        }

        [Route("api/submitswaproom")]
        public HttpResponseMessage PostSwapRoom(HousingModel input)
        {
            if (_housingAdapter.IsModelValid(input))
            {
                input.CrewId = LoggedInStaffNo;
                return Request.CreateResponse(HttpStatusCode.OK, _housingAdapter.CreateSwapRoomsAsync(input, LoggedInStaffDetailId, LoggedInStaffNo).Result);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseModel()
                {
                    IsSuccess = false,
                    Message = "INVALIDFIELDS"
                });
            }
        }
        #endregion
    }
}