/*********************************************************************
 * Name         : IHousingDao.cs
 * Description  : Interface for Getting data from Database.
 * Create Date  : 25th Jan 2016
 * Last Modified: 26th Jan 2016
 * Copyright By : Qatar Airways
 *********************************************************************/

using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.BusinessObjects.Interfaces
{
    public interface IHousingDao
    {
        /// <summary>
        /// Gets Housing Request details for logged in user.
        /// Shows all of request raised by user in past specify to filter.
        /// </summary>
        /// <param name="inputs">Housing Request Filter</param>
        /// <returns>List of Housing Details</returns>
        Task<List<HousingEO>> GetHousingSearchResultAsyc(HousingRequestFilterEO inputs);

        /// <summary>
        /// Gets all the vacant building/flats/bedrooms details.
        /// </summary>
        /// <returns>List of housing details</returns>
        Task<IEnumerable<BuildingEO>> GetHousingVacantBuildingAsync(string staffId);

        /// <summary>
        /// Gets all the vacant building/flats/bedrooms details.
        /// </summary>
        /// <returns>List of housing details</returns>
        Task<IEnumerable<BuildingEO>> GetOccupBldgForSwapRoomsAsync(string staffId);
        
        /// <summary>
        /// Gets Housing Flat Nationality
        /// </summary>
        /// <param name="inputFlat">flat id</param>
        /// <returns>Nationality</returns>
        Task<BuildingEO> GetHousingFlatNationalityAsync(string flatId);

        /// <summary>
        /// Creates a new housing request for move in.
        /// </summary>
        /// <returns>Response details</returns>
        Task<ResponseEO> InsertHousingServiceRequests(HousingEO inputs, string staffId);

        /// <summary>
        /// Gets existing accommodation details of logged in user
        /// </summary>
        /// <param name="crewdetailId">logged in user id</param>
        /// <returns>Housing details</returns>
        Task<HousingEO> GetExistingAccommAsync(string crewdetailId);

        /// <summary>
        /// Get request details by request id
        /// </summary>
        /// <param name="requestNo">request number</param>
        /// <returns>Housing details</returns>
        Task<HousingNotificationEO> GetServiceRequestDetails(string requestNo);

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
        Task<CrewInfoEO> GetCrewEntitlements(string staffId);

        Task<IEnumerable<HousingGuestEO>> GetCrewRelationDetails(string staffNo);

        /// <summary>
        /// Get crew entitlements list details 
        /// </summary>
        /// <param name="staffId">crew id</param>
        /// <returns></returns>
        Task<CrewEntitlementEO> GetHousingEntitlements(string crewDetID, string requestTypeName);

        Task<IEnumerable<HousingGuestEO>> GetGuestDetails(string staffNo);

        Task<bool> IsGuestAccomDateOverlap(string crewDetId, DateTime? fromDate, DateTime? toDate);

        Task<IEnumerable<BuildingEO>> GetFlatAvailabiltyDetails(string flatNo, string buildingName);

        Task<IEnumerable<UserContextEO>> GetStaffsByFlatId(string flatId, string buildingId);

        Task<IEnumerable<UserContextEO>> GetStaffByBedroomId_Swap(string bedroomId, string buildingId);

        Task<bool> UpdateFlatMateApproval(string requestNo, string status);

        Task<HousingNotificationEO> GetHousingNotification(string id, string staffNo);
    }
}
