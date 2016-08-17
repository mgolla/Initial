
using AutoMapper;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.BusinessObjects.Implementation;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Adapter.Helper;
using QR.IPrism.Models.ViewModels;
using QR.IPrism.Utility;
using QR.IPrism.Models.Shared;
using System.IO;

namespace QR.IPrism.Adapter.Implementation
{
    public class OverviewAdapter : IOverviewAdapter
    {
        private IOverviewDao _overviewDao = new OverviewDao();

        /// <summary>
        /// Get Overviews 
        /// </summary>
        /// <returns></returns>
        public async Task<OverviewViewModel> GetOverviewAsyc(OverviewFilterModel filterInput)
        {
            //Define variables 
            OverviewModel overviewList = new OverviewModel();
            OverviewViewModel vm = new OverviewViewModel();


            OverviewEO overview = await _overviewDao.GetOverviewAsyc(Mapper.Map(filterInput, new OverviewFilterEO())); //Get Overview data from stored procedure                        
            Mapper.Map<OverviewEO, OverviewModel>(overview, overviewList);


            vm.OverviewModel = overviewList;


            return vm;
        }
        public async Task<FlightLoadModel> GetFlightLoadAsyc(OverviewFilterModel filterInput)
        {
            if (filterInput.FlightNo != string.Empty && filterInput.STD != string.Empty && filterInput.Sectorfrom != string.Empty)
            {
                FlightLoadFilter filter = new FlightLoadFilter();
                filter.FltNum = filterInput.FlightNo;
                filter.FltDate = Convert.ToDateTime(filterInput.STD);
                filter.Origin = filterInput.Sectorfrom;
                var flightLoadData = new FlightLoadAdapter().GetFlightLoad(filter);
                return flightLoadData;
            }
            return new FlightLoadModel();
        }
        /// <summary>
        /// Get StationInfos 
        /// </summary>
        /// <returns></returns>
        public async Task<StationInfoViewModel> GetStationInfoAsyc(StationInfoFilterModel filterInput)
        {
            //Define variables 
            StationInfoModel stationInfoModel = new StationInfoModel();
            StationInfoViewModel vm = new StationInfoViewModel();


            StationInfoEO stationInfo = await _overviewDao.GetStationInfoAsyc(Mapper.Map(filterInput, new StationInfoFilterEO())); //Get StationInfo data from stored procedure                        
            Mapper.Map<StationInfoEO, StationInfoModel>(stationInfo, stationInfoModel);


            vm.StationInfo = stationInfoModel;

            return vm;
        }
        /// <summary>
        /// Get HotelInfos 
        /// </summary>
        /// <returns></returns>
        public async Task<HotelInfoViewModel> GetHotelInfoAsyc(HotelInfoFilterModel filterInput)
        {
            //Define variables 
            HotelInfoModel hotelInfoModel = new HotelInfoModel();
            HotelInfoViewModel vm = new HotelInfoViewModel();


            HotelInfoEO hotelInfo = await _overviewDao.GetHotelInfoAsyc(Mapper.Map(filterInput, new HotelInfoFilterEO())); //Get HotelInfo data from stored procedure                        
            Mapper.Map<HotelInfoEO, HotelInfoModel>(hotelInfo, hotelInfoModel);


            vm.HotelInfoModel = hotelInfoModel;

            return vm;
        }
        /// <summary>
        /// Get CrewInfos 
        /// </summary>
        /// <returns></returns>
        public async Task<CrewInfoViewModel> GetCrewInfoAsyc(CommonFilterModel filterInput)
        {
            //Define variables 
            CrewInfoViewModel vm = new CrewInfoViewModel();
            
            List<CrewInfoModel> crewInfoList = Mapper.Map(await _overviewDao.GetCrewInfoAsyc(Mapper.Map(filterInput, new CommonFilterEO())), new List<CrewInfoModel>());
            vm.IsDataLoaded = IsDataLoaded.Yes;

            vm.CP = crewInfoList.Where(v => v.POS == CrewGrade.CP || v.POS == CrewGrade.FO).ToList();
            vm.CSD = crewInfoList.Where(v => v.POS == CrewGrade.CSD || v.POS == CrewGrade.CD).ToList();
            vm.CS = crewInfoList.Where(v => v.POS == CrewGrade.CS).ToList();
            vm.F1 = crewInfoList.Where(v => v.POS == CrewGrade.F1).ToList();
            vm.F2 = crewInfoList.Where(v => v.POS == CrewGrade.F2).ToList();


            //List<CrewInfoEO> crewInfos = new List<CrewInfoEO>();
            //vm.IsDataLoaded = IsDataLoaded.Yes;
            //foreach (CrewInfoEO crewInfoModel in data)
            //{
            //    vm.IsDataLoaded = IsDataLoaded.Yes;
            //    if (!string.IsNullOrEmpty(filterInput.CrewImagePath) && !string.IsNullOrEmpty(filterInput.CrewImageType))
            //    {
            //        crewInfoModel.CrewID = crewInfoModel.CrewID.PadLeft(5, '0');
            //        crewInfoModel.CrewPhotoUrl = filterInput.CrewImagePath + "/" + crewInfoModel.CrewID +  filterInput.CrewImageType;

            //        //if (!string.IsNullOrEmpty(imgPath) && File.Exists(imgPath))
            //        //{
            //        //    crewInfoModel.CrewPhoto = File.ReadAllBytes(imgPath);

            //        //}
            //        //else {

            //        //    if (!string.IsNullOrEmpty(imgPath))
            //        //    {
            //        //        imgPath = imgPath.Replace(@"/", @"\");
            //        //        if (File.Exists(imgPath))
            //        //        {
            //        //            crewInfoModel.CrewPhoto = File.ReadAllBytes(imgPath);

            //        //        }
            //        //    }

            //        //}
            //    }

            //    crewInfos.Add(crewInfoModel);
            //}


            ////Get CrewInfo data from stored procedure                        
            //Mapper.Map<List<CrewInfoEO>, List<CrewInfoModel>>(crewInfos, crewInfoList);

            //vm.CP = crewInfoList.Where(v => v.POS == CrewGrade.CP || v.POS == CrewGrade.FO).ToList();
            //vm.CSD = crewInfoList.Where(v => v.POS == CrewGrade.CSD || v.POS == CrewGrade.CD).ToList();
            //vm.CS = crewInfoList.Where(v => v.POS == CrewGrade.CS).ToList();
            //vm.F1 = crewInfoList.Where(v => v.POS == CrewGrade.F1).ToList();
            //vm.F2 = crewInfoList.Where(v => v.POS == CrewGrade.F2).ToList();

            //vm.CrewInfosList = crewInfoList;

            return vm;
        }
        /// <summary>
        /// Get SummaryOfServices 
        /// </summary>
        /// <returns></returns>
        public async Task<SummaryOfServiceViewModel> GetSummaryOfServicesAsyc(SummaryOfServiceFilterModel filterInput)
        {
            //Define variables 
            SOSModel summaryOfServiceList = new SOSModel();
            SummaryOfServiceViewModel vm = new SummaryOfServiceViewModel();


            SOSEO summaryOfService = await _overviewDao.GetSummaryOfServicesAsyc(Mapper.Map(filterInput, new SummaryOfServiceFilterEO())); //Get SummaryOfService data from stored procedure                        
            Mapper.Map<SOSEO, SOSModel>(summaryOfService, summaryOfServiceList);


            vm.SOSModel = summaryOfServiceList;


            return vm;
        }
    }
}



