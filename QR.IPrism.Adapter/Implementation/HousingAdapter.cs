/*********************************************************************
 * Name          : HousingAdapter.cs
 * Description   : Implements IHousingAdapter.
 * Create Date   : 25th Jan 2016
 * Last Modified : 26th Jan 2016
 * Copyright By  : Qatar Airways
 *********************************************************************/

using AutoMapper;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.BusinessObjects.Data_Layer.Shared;
using QR.IPrism.BusinessObjects.Implementation;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;
using QR.IPrism.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Adapter.Implementation
{
    public class HousingAdapter : IHousingAdapter
    {
        #region Private Variables
        private readonly IHousingDao _housingDao = new HousingDao();
        private readonly ISharedDao _sharedDao = new SharedDao();
        #endregion

        #region IHousingAdapter Implementation

        /// <summary>
        /// Gets the Housing request search result from DAO layer.
        /// </summary>
        /// <param name="inputs">Housing request filter</param>
        /// <returns>List of HousingModel</returns>
        public async Task<IEnumerable<HousingModel>> GetHousingSearchResultAsyc(HousingRequestFilterModel inputs)
        {
            var filter = Mapper.Map(inputs, new HousingRequestFilterEO());
            return Mapper.Map(await _housingDao.GetHousingSearchResultAsyc(filter), new List<HousingModel>());
        }

        /// <summary>
        /// Gets all the vacant building/flats/bedrooms details.
        /// </summary>
        /// <returns>List of housing details</returns>
        public async Task<IEnumerable<BuildingModel>> GetHousingVacantBuildingAsync(string staffId)
        {
            return Mapper.Map(await _housingDao.GetHousingVacantBuildingAsync(staffId), new List<BuildingModel>());
        }

        /// <summary>
        /// Gets all the vacant building/flats/bedrooms details.
        /// </summary>
        /// <returns>List of housing details</returns>
        public async Task<IEnumerable<BuildingModel>> GetOccupBldgForSwapRoomsAsync(string staffId)
        {
            return Mapper.Map(await _housingDao.GetOccupBldgForSwapRoomsAsync(staffId), new List<BuildingModel>());
        }

        /// <summary>
        /// Gets Housing Flat Nationality
        /// </summary>
        /// <param name="inputFlat">flat number</param>
        /// <returns>Nationality</returns>
        public async Task<BuildingModel> GetHousingFlatNationalityAsync(string flatId)
        {
            return Mapper.Map(await _housingDao.GetHousingFlatNationalityAsync(flatId), new BuildingModel());
        }

        public async Task<HousingModel> GetExistingAccommAsync(string crewdetailId)
        {
            return Mapper.Map(await _housingDao.GetExistingAccommAsync(crewdetailId), new HousingModel());
        }

        public async Task<HousingNotificationModel> GetServiceRequestDetails(string requestNo)
        {
            return Mapper.Map(await _housingDao.GetServiceRequestDetails(requestNo), new HousingNotificationModel());
        }

        /// <summary>
        /// Cancel housing request
        /// </summary>
        /// <param name="requestId">by request id</param>
        public void CancelHousingRequest(string requestId)
        {
            _housingDao.CancelHousingRequest(requestId);
        }

        public async Task<CrewInfoModel> GetCrewEntitlements(string staffId)
        {
            return Mapper.Map(await _housingDao.GetCrewEntitlements(staffId), new CrewInfoModel());
        }

        public async Task<CrewEntitlementModel> GetHousingEntitlements(string crewDetId, string requestTypeName)
        {
            return Mapper.Map(await _housingDao.GetHousingEntitlements(crewDetId, requestTypeName), new CrewEntitlementModel());
        }

        public async Task<IEnumerable<HousingGuestModel>> GetGuestDetails(string staffNo)
        {
            return Mapper.Map(await _housingDao.GetGuestDetails(staffNo), new List<HousingGuestModel>());
        }

        public async Task<IEnumerable<HousingGuestModel>> GetCrewRelationDetails(string staffNo)
        {
            return Mapper.Map(await _housingDao.GetCrewRelationDetails(staffNo), new List<HousingGuestModel>());
        }

        /// <summary>
        /// Creates a new housing request for move in.
        /// </summary>
        /// <returns>Response details</returns>
        public async Task<ResponseModel> CreateChangeAcc_MoveInAsync(HousingModel inputs, string staffId, string staffNo)
        {
            inputs.RequestDate = DateTime.Now;
            inputs.RequestDateClose = inputs.RequestDate.AddDays(10);
            var filter = Mapper.Map(inputs, new HousingEO());
            var result = Mapper.Map(await _housingDao.InsertHousingServiceRequests(filter, staffNo), new ResponseModel());

            if (result != null && !string.IsNullOrWhiteSpace(result.RequestNumber))
            {
                var message = new MessageEO();

                message.Type = _sharedDao.GetConfigLookUpDetails(HousingConstants.EntityType).Result
                    .Where(i => i.Text == HousingConstants.Housing).FirstOrDefault().Value;
                message.Message = inputs.AdditionalInfo;
                message.CreatedBy = staffId;
                message.Id = result.ResponseId;

                _sharedDao.InsertComment(message);
            }

            return result;
        }

        /// <summary>
        /// Creates a new housing request for Swap Rooms
        /// </summary>
        /// <returns>Response details</returns>
        public async Task<ResponseModel> CreateSwapRoomsAsync(HousingModel inputs, string staffId, string staffNo)
        {
            inputs.RequestDate = DateTime.Now;
            inputs.RequestDateClose = inputs.RequestDate.AddDays(10);
            inputs.StayOut.SwapRoomIsPaintedCleaned = Constants.ACTIVE;

            var filter = Mapper.Map(inputs, new HousingEO());
            var response = await _housingDao.InsertHousingServiceRequests(filter, staffNo);

            if (response != null && !string.IsNullOrWhiteSpace(response.RequestNumber) && !string.IsNullOrWhiteSpace(response.ResponseId))
            {
                var message = new MessageEO();
                message.Type = _sharedDao.GetConfigLookUpDetails(HousingConstants.EntityType).Result
                    .Where(i => i.Text == HousingConstants.Housing).FirstOrDefault().Value;
                message.Message = inputs.AdditionalInfo;
                message.CreatedBy = staffId;
                message.Id = response.ResponseId;
                _sharedDao.InsertComment(message);

                var notification = CreateSwapRoomNotfication(inputs, response, staffId);
                notification.ToCrewId = inputs.StayOut.FriendStaffNo;
                await _sharedDao.InsertCrewNotificationDetails(notification, staffNo);
            }
            else if (response != null && response.Message == "NOT ALLOWED")
            {
                response.Message = "HOUGUESTACC03";
            }
            else
            {
                response = new ResponseEO()
                {
                    Message = "HOUERROR01"
                };
            }
            return Mapper.Map(response, new ResponseModel());
        }

        public async Task<ResponseModel> CreateGuestAccAsync(HousingModel housing, string staffId, string staffNo)
        {
            IEnumerable<BuildingEO> flatMates = null;
            ResponseEO response = null;
            housing.RequestDate = DateTime.Now;
            housing.RequestDateClose = housing.RequestDate.AddDays(10);
            var housingEO = Mapper.Map(housing, new HousingEO());

            bool overlap = await _housingDao.IsGuestAccomDateOverlap(staffId, housing.Guests.CheckinDate, housing.Guests.CheckoutDate);

            if (overlap == false)
            {
                flatMates = await _housingDao.GetFlatAvailabiltyDetails(housing.BuildingDetails.FlatNumber, housing.BuildingDetails.BuildingName);
                response = await _housingDao.InsertHousingServiceRequests(housingEO, staffNo);

                if (!string.IsNullOrWhiteSpace(response.ResponseId) && !string.IsNullOrWhiteSpace(response.RequestNumber))
                {
                    var notification = GuestAccFlatMatesNotfication(housing, response, staffId);
                    foreach (var item in flatMates.Where(i => i.StaffId != staffNo))
                    {
                        notification.ToCrewId = item.StaffId;
                        await _sharedDao.InsertCrewNotificationDetails(notification, staffNo);
                    }

                    //Flatmate notification not generated if the request is split 
                    if (housing.Guests.CheckinDate.Value.Year != housing.Guests.CheckoutDate.Value.Year)
                    {
                        foreach (var item in flatMates.Where(i => i.StaffId != housing.CrewId))
                        {
                            notification.ToCrewId = item.StaffId;
                            await _sharedDao.InsertCrewNotificationDetails(notification, staffNo);
                        }
                    }
                }
                else if (response != null && response.Message == "NOT ALLOWED")
                {
                    response.Message = "HOUGUESTACC03";
                }
                else
                {
                    response = new ResponseEO()
                    {
                        Message = "HOUERROR01"
                    };
                }
            }
            else
            {
                response = new ResponseEO()
                {
                    Message = "HOUGUESTACC01"
                };
            }
            return Mapper.Map(response, new ResponseModel());
        }

        public async Task<ResponseModel> CreateMovingOutAsync(HousingModel housing, string staffId, string staffNo)
        {
            housing.RequestDate = DateTime.Now;
            housing.RequestDateClose = housing.RequestDate.AddDays(10);
            var filter = Mapper.Map(housing, new HousingEO());
            var result = Mapper.Map(await _housingDao.InsertHousingServiceRequests(filter, staffNo), new ResponseModel());

            if (result != null && !string.IsNullOrWhiteSpace(result.RequestNumber))
            {
                var message = new MessageEO();

                message.Type = _sharedDao.GetConfigLookUpDetails(HousingConstants.EntityType).Result
                    .Where(i => i.Text == HousingConstants.Housing).FirstOrDefault().Value;
                message.Message = housing.AdditionalInfo;
                message.CreatedBy = staffId;
                message.Id = result.ResponseId;

                _sharedDao.InsertComment(message);
            }

            return result;
        }

        public void UpdateFlatMateApproval(MessageModel model, string staffNo, string staffId)
        {
            if (_housingDao.UpdateFlatMateApproval(model.RequestId, model.Status).Result)
            {
                #region Add Comments

                var message = new MessageEO();
                message.Type = _sharedDao.GetConfigLookUpDetails(HousingConstants.EntityType).Result
                    .Where(i => i.Text == HousingConstants.Housing).FirstOrDefault().Value;
                message.Message = model.Message;
                message.CreatedBy = staffId;
                message.Id = model.RequestGuid;
                _sharedDao.InsertComment(message);

                #endregion

                #region Insert Notification for initiator

                var notfy = new NotificationDetailsEO()
                {
                    Id = model.Id,
                    ToCrewId = staffNo,
                    Date = DateTime.MinValue,
                    ActionByDate = DateTime.MinValue
                };

                List<NotificationDetailsEO> lstNotification = _sharedDao.SearchNotifications(notfy).Result.ToList();

                var newNotfy = new NotificationDetailsEO();
                newNotfy.Type = HousingConstants.Swap_Req_Approval;
                newNotfy.Date = DateTime.Now;
                newNotfy.ActionByDate = newNotfy.Date.Value.AddDays(5);
                newNotfy.Severity = HousingConstants.Severity_High;
                newNotfy.ToCrewId = lstNotification[0].FromCrewId;
                newNotfy.FromCrewId = lstNotification[0].ToCrewId;
                newNotfy.Status = NotificationStatus.Sent_For_Approval;
                newNotfy.Description = lstNotification[0].RequestId + " - " + HousingConstants.Swap_Flatmate_Approval;
                newNotfy.RequestId = lstNotification[0].RequestId;
                newNotfy.RequestGuid = lstNotification[0].RequestGuid;
                _sharedDao.InsertCrewNotificationDetails(newNotfy, staffNo);

                #endregion

                #region update notification status
                notfy = new NotificationDetailsEO();
                notfy.Id = model.Id;
                notfy.Status = NotificationStatus.Done;
                _sharedDao.UpdateCrewNotificationDetails(notfy);
                #endregion
            }
        }

        public async Task<IEnumerable<UserContextModel>> GetStaffsByFlatId(string flatId, string buildingId)
        {
            return Mapper.Map(await _housingDao.GetStaffsByFlatId(flatId, buildingId), new List<UserContextModel>());
        }

        public async Task<IEnumerable<UserContextModel>> GetStaffByBedroomId_Swap(string bedroomId, string buildingId)
        {
            return Mapper.Map(await _housingDao.GetStaffByBedroomId_Swap(bedroomId, buildingId), new List<UserContextModel>());
        }

        public async Task<HousingNotificationModel> GetHousingNotification(string id, string staffNo)
        {
            return Mapper.Map(await _housingDao.GetHousingNotification(id, staffNo), new HousingNotificationModel());
        }

        #endregion

        private NotificationDetailsEO GuestAccFlatMatesNotfication(HousingModel housing, ResponseEO response, string crewId)
        {
            var notification = new NotificationDetailsEO();
            notification.ActionByDate = housing.RequestDateClose;
            notification.Description = response.RequestNumber + "-" + housing.RequestType + " request";
            notification.Date = housing.RequestDate;
            notification.FromCrewId = crewId;
            notification.Type = HousingConstants.Guest_Accommodation;
            notification.Severity = HousingConstants.Severity_Medium;
            notification.RequestId = response.RequestNumber;
            notification.RequestGuid = response.ResponseDetailsId;
            notification.Status = NotificationStatus.Submitted;

            return notification;
        }

        private NotificationDetailsEO CreateSwapRoomNotfication(HousingModel housing, ResponseEO response, string crewId)
        {
            var notification = new NotificationDetailsEO();
            notification.ActionByDate = housing.RequestDateClose;
            notification.Description = response.RequestNumber + "-" + housing.RequestType + HousingConstants.Swap_Req;
            notification.Date = housing.RequestDate;
            notification.FromCrewId = crewId;
            notification.Type = HousingConstants.Swap_Rooms;
            notification.Severity = HousingConstants.Severity_Medium;
            notification.RequestId = response.RequestNumber;
            notification.RequestGuid = response.ResponseId;
            notification.Status = NotificationStatus.Submitted;

            return notification;
        }

        public bool IsModelValid(HousingModel inputs)
        {
            if (string.IsNullOrWhiteSpace(inputs.RequestId) || string.IsNullOrWhiteSpace(inputs.RequestType))
            {
                return false;
            }

            switch (inputs.RequestType)
            {
                case HousingConstants.CA:
                case HousingConstants.MI:
                    return ValidateChangeAccommodation(inputs);
                case HousingConstants.GA:
                    return ValidateGuestAccommodation(inputs);
                case HousingConstants.SR:
                    return ValidateSwapAccommodation(inputs);
                case HousingConstants.MO:
                    return ValidateMoveOut(inputs);
                case HousingConstants.SO:
                    return ValidateStayOut(inputs);
            }

            return false;
        }

        private bool ValidateChangeAccommodation(HousingModel inputs)
        {
            if (string.IsNullOrWhiteSpace(inputs.BuildingDetails.BuildingDetailSid) ||
                string.IsNullOrWhiteSpace(inputs.BuildingDetails.BuildingName) ||
                string.IsNullOrWhiteSpace(inputs.BuildingDetails.FlatId) ||
                string.IsNullOrWhiteSpace(inputs.BuildingDetails.BedroomDetailsId) ||
                string.IsNullOrWhiteSpace(inputs.RequestReason) ||
                string.IsNullOrWhiteSpace(inputs.AdditionalInfo))
            {
                return false;
            }

            return true;
        }

        private bool ValidateGuestAccommodation(HousingModel inputs)
        {
            if (string.IsNullOrWhiteSpace(inputs.Guests.Relationship) || string.IsNullOrWhiteSpace(inputs.Guests.GuestName) ||
               inputs.Guests.CheckinDate == null || inputs.Guests.CheckoutDate == null)
            {
                return false;
            }

            return true;
        }

        private bool ValidateSwapAccommodation(HousingModel inputs)
        {
            if (string.IsNullOrWhiteSpace(inputs.BuildingDetails.BuildingDetailSid) ||
                string.IsNullOrWhiteSpace(inputs.BuildingDetails.BuildingName) ||
                string.IsNullOrWhiteSpace(inputs.BuildingDetails.FlatId) ||
                string.IsNullOrWhiteSpace(inputs.BuildingDetails.BedroomDetailsId) ||
                string.IsNullOrWhiteSpace(inputs.RequestReason) ||
                string.IsNullOrWhiteSpace(inputs.StayOut.FriendStaffId) ||
                string.IsNullOrWhiteSpace(inputs.StayOut.FriendStaffNo) ||
                string.IsNullOrWhiteSpace(inputs.StayOut.SwapRoomCategoryId) ||
                string.IsNullOrWhiteSpace(inputs.AdditionalInfo))
            {
                return false;
            }

            return true;
        }

        private bool ValidateMoveOut(HousingModel inputs)
        {
            if (string.IsNullOrWhiteSpace(inputs.RequestReason) ||
                inputs.Guests.CheckoutDate == null ||
                string.IsNullOrWhiteSpace(inputs.BuildingDetails.BuildingName) ||
                string.IsNullOrWhiteSpace(inputs.BuildingDetails.Area) ||
                string.IsNullOrWhiteSpace(inputs.BuildingDetails.StreetNo) ||
                string.IsNullOrWhiteSpace(inputs.BuildingDetails.PostBoxNo) ||
                string.IsNullOrWhiteSpace(inputs.MobileNo) ||
                string.IsNullOrWhiteSpace(inputs.LandLineNo) ||
                string.IsNullOrWhiteSpace(inputs.AdditionalInfo))
            {
                return false;
            }

            return true;
        }

        private bool ValidateStayOut(HousingModel inputs)
        {
            if (string.IsNullOrWhiteSpace(inputs.StayOut.StayOutRequestTypeId) ||
                string.IsNullOrWhiteSpace(inputs.RequestReason) ||
                inputs.StayOut.StayOutRequestFromDate == null || inputs.StayOut.StayOutRequestToDate == null ||
                string.IsNullOrWhiteSpace(inputs.BuildingDetails.StreetNo) ||
                string.IsNullOrWhiteSpace(inputs.BuildingDetails.BuildingName) ||
                string.IsNullOrWhiteSpace(inputs.BuildingDetails.Area) ||
                string.IsNullOrWhiteSpace(inputs.BuildingDetails.PostBoxNo) ||
                string.IsNullOrWhiteSpace(inputs.MobileNo) ||
                string.IsNullOrWhiteSpace(inputs.LandLineNo) ||
                string.IsNullOrWhiteSpace(inputs.StayOut.StayOutCrewRelationId) ||
                string.IsNullOrWhiteSpace(inputs.StayOut.StayOutCrewRelationName))
            {
                return false;
            }

            return true;
        }
    }
}