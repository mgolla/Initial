using QR.IPrism.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Adapter.Interfaces
{
    public interface IKafouAdapter
    {
        Task<List<SearchRecognitionResultModel>> SearchMyRecognitionInfo(SearchRecognitionRequestModel eoSearchCrewRecognition, string staffNumber);
        Task<List<RecognisedCrewDetailsModel>> GetWallOfFameRecognitionList();
        Task<List<FlightDetailsModel>> SearchFlightsForCCR(string userID);
        Task<List<SearchRecognitionResultModel>> SearchCrewRecognitionInfo(SearchRecognitionRequestModel eoSearchCrewRecognition);
        Task<SearchCrewRecognitionModel> GetCrewStatusSourceGradeData();
        Task<CrewRecognitionModel> GetInitialCrewRecognition(string flightId, string crewDetails);
        Task<CrewRecognitionModel> GetCrewRecognition(string recognitionID);
        Task<List<FileModel>> GetRecognitionAttachments(string recognitionID);
        Task<string> InsertUpdateRecognition(CrewRecognitionModel model);
        //string InsertUpdateRecognition(CrewRecognitionModel model);
        Task<CommonRecognitionModel> GetRecognitionTypeSource();
        Task<List<CrewRecognitionOverviewModel>> GetCrewRecognitionByFlight(string flightIdList, string crewDetailsID);
        //Task<List<FlightDetailsModel>> SearchFlightsForCCR(string userID, int pageIndex, int pageSize, string sortOrderColumn, string orderBy);
    }
}
