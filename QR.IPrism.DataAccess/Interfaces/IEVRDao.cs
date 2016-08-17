using QR.IPrism.EntityObjects.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.BusinessObjects.Interfaces
{
    public interface IEVRDao
    {
        /// <summary>
        /// Gets EVR Request details for logged in user.
        /// </summary>
        /// <param name="inputs">EVR Request Filter</param>
        /// <returns>List of EVR Details</returns>
        Task<List<EVRResultEO>> GetEVRSearchResultAsyc(EVRRequestFilterEO inputs);
        Task<EVRInsertOutputEO> InsertVoyageReportAsyc(EVRReportMainEO inputs, string staffId, string staffNumber, string staffUserId);
        Task<List<EVRDraftOutputEO>> GetDraftVRForUserAsyc(string flightDetsId, string userId);
        Task<List<EVRListsEO>> GetVRLastTenFlightAsyc(string userId);
        Task<List<EVRResultEO>> GetLastTenVRs(string userId);
        Task<List<EVRPendingEO>> GetPendingVRForUserAsyc(string userId);
        void UpdateNoVRAsyc(string flightDetId, string crewDetailId, string userId);
        Task<List<EVRDraftOutputEO>> GetSubmittedEVRsAsyc(string flightDetId, string crewDetailId, string userId);
        Task<EVRCrewViewEO> GetVRDetailsCrewAsyc(VRIdEO vrId);
        Task<ViewVREnterVREO> GetVRDetailEnterVRAsyc(VRIdEO vrId);
        void DeleteVRAsyc(string VRId);
    }
}
