using QR.IPrism.EntityObjects.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.DataAccess.Interfaces
{
    public interface IKafouDao
    {
        Task<List<SearchRecognitionResultEO>> SearchMyRecognitionInfoAsyc(SearchRecognitionRequestEO eoSearchCrewRecognition, string staffNumber);
        Task<List<RecognisedCrewDetailsEO>> GetWallOfFameRecognitionListAsyc();
        Task<List<FlightDetailsEO>> SearchFlightsForCCRAsyc(string userID);
        Task<List<SearchRecognitionResultEO>> SearchCrewRecognitionInfoAsyc(SearchRecognitionRequestEO eoSearchCrewRecognition);
        Task<SearchCrewRecognitionEO> GetCrewStatusSourceGradeDataAsyc();
        Task<CrewRecognitionEO> GetInitialCrewRecognitionAsyc(string flightId, string crewDetails);
        Task<CrewRecognitionEO> GetCrewRecognitionAsyc(string recognitionID);
        Task<List<FileEO>> GetRecognitionAttachmentsAsyc(string recognitionID);
        Task<string> InsertUpdateRecognitionAsyc(CrewRecognitionEO model);
        //string InsertUpdateRecognitionAsyc(CrewRecognitionEO model);
        Task<CommonRecognitionEO> GetRecognitionTypeSourceAsyc();
        Task<List<CrewRecognitionOverviewEO>> GetCrewRecognitionByFlightAsyc(string flightIdList, string crewDetailsID);
        //Task<List<FlightDetailsEO>> SearchFlightsForCCR(string userID, int pageIndex, int pageSize, string sortOrderColumn, string orderBy);
    }
}
