using AutoMapper;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.BusinessObjects.Implementation;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.DataAccess.Implementation;
using QR.IPrism.DataAccess.Interfaces;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;
using QR.IPrism.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Adapter.Implementation
{
    public class KafouAdapter : IKafouAdapter
    {

        #region Private Variables
        private readonly IKafouDao _kafouDao = new KafouDao();
        #endregion

        public async Task<List<SearchRecognitionResultModel>> SearchMyRecognitionInfo(SearchRecognitionRequestModel eoSearchCrewRecognition, string staffNumber)
        {
            var filter = Mapper.Map(eoSearchCrewRecognition, new SearchRecognitionRequestEO());
            return Mapper.Map(await _kafouDao.SearchMyRecognitionInfoAsyc(filter, staffNumber), new List<SearchRecognitionResultModel>());
        }
        public async Task<List<RecognisedCrewDetailsModel>> GetWallOfFameRecognitionList()
        {
            return Mapper.Map(await _kafouDao.GetWallOfFameRecognitionListAsyc(), new List<RecognisedCrewDetailsModel>());
        }
        
        public async Task<List<FlightDetailsModel>> SearchFlightsForCCR(string userID)
        {
            return Mapper.Map(await _kafouDao.SearchFlightsForCCRAsyc(userID), new List<FlightDetailsModel>());
        }
        
        public async Task<List<SearchRecognitionResultModel>> SearchCrewRecognitionInfo(SearchRecognitionRequestModel eoSearchCrewRecognition)
        {
            var filter = Mapper.Map(eoSearchCrewRecognition, new SearchRecognitionRequestEO());
            return Mapper.Map(await _kafouDao.SearchCrewRecognitionInfoAsyc(filter), new List<SearchRecognitionResultModel>());
        }


        public async Task<SearchCrewRecognitionModel> GetCrewStatusSourceGradeData()
        {
            return Mapper.Map(await _kafouDao.GetCrewStatusSourceGradeDataAsyc(), new SearchCrewRecognitionModel());
        }

        public async Task<CrewRecognitionModel> GetInitialCrewRecognition(string flightId, string crewDetails)
        {
            return Mapper.Map(await _kafouDao.GetInitialCrewRecognitionAsyc(flightId, crewDetails), new CrewRecognitionModel());
        }

        public async Task<CrewRecognitionModel> GetCrewRecognition(string recognitionID)
        {
            return Mapper.Map(await _kafouDao.GetCrewRecognitionAsyc(recognitionID), new CrewRecognitionModel());
        }

        public async Task<List<FileModel>> GetRecognitionAttachments(string recognitionID)
        {
            return Mapper.Map(await _kafouDao.GetRecognitionAttachmentsAsyc(recognitionID), new List<FileModel>());
        }

        public async Task<string> InsertUpdateRecognition(CrewRecognitionModel model)
        {
            var filter = Mapper.Map(model, new CrewRecognitionEO());
            return await _kafouDao.InsertUpdateRecognitionAsyc(filter);
        }

        //public string InsertUpdateRecognition(CrewRecognitionModel model)
        //{
        //    var filter = Mapper.Map(model, new CrewRecognitionEO());
        //    return _kafouDao.InsertUpdateRecognitionAsyc(filter);
        //}


        public async Task<CommonRecognitionModel> GetRecognitionTypeSource()
        {
            return Mapper.Map(await _kafouDao.GetRecognitionTypeSourceAsyc(), new CommonRecognitionModel());
        }


        //public async Task<List<FlightDetailsModel>> SearchFlightsForCCR(string userID, int pageIndex, int pageSize, string sortOrderColumn, string orderBy)
        //{
        //    return Mapper.Map(await _kafouDao.SearchFlightsForCCRAsyc(userID), new List<FlightDetailsModel>());
        //}


        public async Task<List<CrewRecognitionOverviewModel>> GetCrewRecognitionByFlight(string flightIdList, string crewDetailsID)
        {
            return Mapper.Map(await _kafouDao.GetCrewRecognitionByFlightAsyc(flightIdList, crewDetailsID), new List<CrewRecognitionOverviewModel>());
        }
    }
}
