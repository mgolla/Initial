using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Models.Shared;

namespace QR.IPrism.Adapter.Interfaces
{
    public interface IEVRAdapter
    {
        /// <summary>
        /// Gets the EVR request search result from DAO layer.
        /// </summary>
        /// <param name="inputs">EVR request filter</param>
        /// <returns>List of EVRModels</returns>
        Task<IEnumerable<EVRResultModel>> GetEVRSearchResult(EVRRequestFilterModel inputs);
        //Task<EVRResultModel> GetDeptForReportAboutAsync(LookupModel inputs);
        Task<EVRInsertOutputModel> InsertVoyageReport(EVRReportMainModel inputs, string staffId, string staffNumber, string staffUserId);
        Task<List<EVRDraftOutputModel>> GetDraftVRForUser(string flightDetsId, string userId);
        Task<List<EVRListsModel>> GetVRLastTenFlight(string userId);
        Task<List<EVRResultModel>> GetLastTenVRs(string userId);
        Task<List<EVRPendingModel>> GetPendingVRForUser(string userId);
        void UpdateNoVR(string flightDetId, string crewDetailId, string userId);
        Task<List<EVRDraftOutputModel>> GetSubmittedEVRs(string flightDetId, string crewDetailId, string userId);
        Task<EVRCrewViewModel> GetEVRViewDetailsCrew(VRIdModel vrId);
        Task<ViewVREnterVRModel> GetVRDetailEnterVR(VRIdModel vrId);
        void DeleteVR(string VRId);
    }
}
