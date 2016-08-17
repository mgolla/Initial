using AutoMapper;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.BusinessObjects.Implementation;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Adapter.Implementation
{
    public class CrewProfileAdapter: ICrewProfileAdapter
    {
        private ICrewProfileDao _iCrewProfileDao = new CrewProfileDao();

        public async Task<List<TrainingHistoryModel>> GetTrainingHistory(string StaffNumber)
        {
            //Define variables 
            List<TrainingHistoryModel> vm = new List<TrainingHistoryModel>();

            List<TrainingHistoryEO> trainingHistoryEOList = await _iCrewProfileDao.GetCrewTrainingHistoryAsync(StaffNumber);
            Mapper.Map<List<TrainingHistoryEO>, List<TrainingHistoryModel>>(trainingHistoryEOList, vm);

            return vm;
        }

        public async Task<List<IDPModel>> GetIDPDetails(string StaffNumber)
        {
            //Define variables 
            List<IDPModel> vm = new List<IDPModel>();

            List<IDPEO> idpEOList = await _iCrewProfileDao.GetCrewIDPAsync(StaffNumber);
            Mapper.Map<List<IDPEO>, List<IDPModel>>(idpEOList, vm);

            return vm;
        }


        public async Task<List<FileModel>> GetMyDocDetails(string StaffNumber)
        {
            //Define variables 
            List<FileModel> vm = new List<FileModel>();

            List<FileEO> myDocEOList = await _iCrewProfileDao.GetCrewMyDocAsync(StaffNumber);
            Mapper.Map<List<FileEO>, List<FileModel>>(myDocEOList, vm);

            return vm;
        }


        public async Task<List<DestinationsVisitedModel>> GetDestinationsVisitedDetails(string StaffNumber)
        {
            //Define variables 
            List<DestinationsVisitedModel> vm = new List<DestinationsVisitedModel>();

            List<DestinationsVisitedEO> dstVstdEOList = await _iCrewProfileDao.GetCrewDstVstdAsync(StaffNumber);
            Mapper.Map<List<DestinationsVisitedEO>, List<DestinationsVisitedModel>>(dstVstdEOList, vm);

            return vm;
        }


        public async Task<List<QualnVisaModel>> GetQualnVisaDetails(string StaffNumber)
        {
            //Define variables 
            List<QualnVisaModel> vm = new List<QualnVisaModel>();

            List<QualnVisaEO> qualVisaEOList = await _iCrewProfileDao.GetCrewQualnVisaAsync(StaffNumber);
            Mapper.Map<List<QualnVisaEO>, List<QualnVisaModel>>(qualVisaEOList, vm);

            return vm;
        }


        public async Task<List<CareerPathModel>> GetCareerPathDetails(string StaffNumber)
        {
            //Define variables 
            List<CareerPathModel> vm = new List<CareerPathModel>();

            List<CareerPathEO> careerPathEOList = await _iCrewProfileDao.GetCrewCareerPathAsync(StaffNumber);
            Mapper.Map<List<CareerPathEO>, List<CareerPathModel>>(careerPathEOList, vm);

            return vm;
        }
    }
}
