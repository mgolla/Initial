/*********************************************************************
 * Name          : IHousingAdapter.cs
 * Description   : Interface for Getting Housing details for DAO layer.
 * Create Date   : 25th Jan 2016
 * Last Modified : 26th Jan 2016
 * Copyright By  : Qatar Airways
 *********************************************************************/

using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;
using QR.IPrism.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Adapter.Interfaces
{
    public interface IHousingAdapter
    {
        /// <summary>
        /// Gets the Housing request search result from DAO layer.
        /// </summary>
        /// <param name="inputs">Housing request filter</param>
        /// <returns>List of HousingModel</returns>
        Task<IEnumerable<HousingModel>> GetHousingSearchResultAsyc(HousingRequestFilterModel inputs);

        /// <summary>
        /// Gets all the vacant building/flats/bedrooms details.
        /// </summary>
        /// <returns>List of housing details</returns>
        Task<IEnumerable<BuildingModel>> GetHousingVacantBuildingAsync(string staffId);

         /// <summary>
        /// Gets all the vacant building/flats/bedrooms details.
        /// </summary>
        /// <returns>List of housing details</returns>
        Task<IEnumerable<BuildingModel>> GetOccupBldgForSwapRoomsAsync(string staffId);
               

        /// <summary>
        /// Gets Housing Flat Nationality
        /// </summary>
        /// <param name="inputFlat"></param>
        /// <returns></returns>
        Task<BuildingModel> GetHousingFlatNationalityAsync(string flatId);

        /// <summary>
        /// Creates a new housing request for move in.
        /// </summary>
        /// <returns>Response details</returns>
        Task<ResponseModel> CreateChangeAcc_MoveInAsync(HousingModel inputs, string staffId, string staffNo);

        /// <summary>
        /// Creates a new housing request for move in.
        /// </summary>
        /// <returns>Response details</returns>
        Task<ResponseModel> CreateGuestAccAsync(HousingModel housing, string staffId, string staffNo);

        /// <summary>
        /// Creates a new housing request for move in.
        /// </summary>
        /// <returns>Response details</returns>
        Task<ResponseModel> CreateMovingOutAsync(HousingModel housing, string staffId, string staffNo);

        Task<ResponseModel> CreateSwapRoomsAsync(HousingModel inputs, string staffId, string staffNo);

        /// <summary>
        /// Gets existing accommodation details of logged in user
        /// </summary>
        /// <param name="crewdetailId">logged in user id</param>
        /// <returns>Housing details</returns>
        Task<HousingModel> GetExistingAccommAsync(string crewdetailId);

        /// <summary>
        /// Get request details by request id
        /// </summary>
        /// <param name="requestNo">request number</param>
        /// <returns>Housing details</returns>
        Task<HousingNotificationModel> GetServiceRequestDetails(string requestNo);

        /// <summary>
        /// Cancel housing request
        /// </summary>
        /// <param name="requestId">by request id</param>
        void CancelHousingRequest(string requestId);

        /// <summary>
        /// Get crew entitlements list details 
        /// </summary>
        /// <param name="staffId">crew id</param>
        /// <returns></returns>
        Task<CrewInfoModel> GetCrewEntitlements(string staffId);

        //Task<IEnumerable<HousingModel>> GetLastRequest(string staffId, string requestTypeId);

        //Task<bool> ValidateSwapRequestByFriend(string staffNumber);

        //Task<CrewEntitlementModel> GetGuestEntitlements(string crewDetID, string requestTypeID);

        /// <summary>
        /// Get crew entitlements list details 
        /// </summary>
        /// <param name="staffId">crew id</param>
        /// <returns></returns>
        Task<CrewEntitlementModel> GetHousingEntitlements(string crewDetID, string requestTypeName);

        Task<IEnumerable<HousingGuestModel>> GetGuestDetails(string staffNo);

        Task<IEnumerable<HousingGuestModel>> GetCrewRelationDetails(string staffNo);

        Task<IEnumerable<UserContextModel>> GetStaffsByFlatId(string flatId, string buildingId);

        Task<IEnumerable<UserContextModel>> GetStaffByBedroomId_Swap(string bedroomId, string buildingId);
        
        void UpdateFlatMateApproval(MessageModel model, string staffNo, string crewId);

        Task<HousingNotificationModel> GetHousingNotification(string id, string staffNo);

        bool IsModelValid(HousingModel inputs);
    }
}
