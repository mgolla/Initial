using AutoMapper;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.DataAccess.Implementation;
using QR.IPrism.DataAccess.Interfaces;
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
    public class FlightAddEditAdapter : IFlightAddEditAdapter
    {
        #region Private Variables
        private readonly IFlightDetailsDao _flightDelayDao;
        #endregion

        public FlightAddEditAdapter(FlightDetailsDao flightDetailsDao)
        {
            _flightDelayDao = new FlightDetailsDao();
        }

        public async Task<IEnumerable<FlightInfoModel>> GetFlightDetails(FlightDelayFilterModel filter, string staffNo, string userId)
        {
            var input = Mapper.Map(filter, new FlightDelayFilterEO());
            return Mapper.Map(await _flightDelayDao.GetFlightDetails(input, staffNo, userId), new List<FlightInfoModel>());
        }

        public async Task<FlightInfoModel> GetSingleFlight(string flightDetailsId, string userId)
        {
            return Mapper.Map(await _flightDelayDao.GetSingleFlight(flightDetailsId, userId), new FlightInfoModel());
        }

        public async Task<IEnumerable<FlightInfoModel>> GetFlightDetailForPaste(string staffId, string flightDetailsId)
        {
            return Mapper.Map(await _flightDelayDao.GetFlightDetailForPaste(staffId, flightDetailsId), new List<FlightInfoModel>());
        }

        public async Task<IEnumerable<FlightCrewsModel>> GetAutoSuggestStaffInformation(SearchCriteriaModel searchCriteria)
        {
            var input = Mapper.Map(searchCriteria, new SearchCriteriaEO());
            return Mapper.Map(await _flightDelayDao.GetAutoSuggestStaffInformation(input), new List<FlightCrewsModel>());
        }

        public async Task<IEnumerable<FlightCrewsModel>> GetAutoSuggestStaffByGrade(SearchCriteriaModel searchCriteria, string grade)
        {
            var input = Mapper.Map(searchCriteria, new SearchCriteriaEO());
            if (grade == CrewGrade.PO)
            {
                return Mapper.Map(_flightDelayDao.GetAutoSuggestStaffInformation(input).Result
                .Where(i => (i.StaffGrade == CrewGrade.CS || i.StaffGrade == CrewGrade.CSD || i.StaffGrade == CrewGrade.F1 || i.StaffGrade == CrewGrade.F2)).ToList(), new List<FlightCrewsModel>());
            }
            else
            {
                return Mapper.Map(_flightDelayDao.GetAutoSuggestStaffInformation(input).Result
                           .Where(i => (i.StaffGrade == CrewGrade.F1 || i.StaffGrade == CrewGrade.F2)).ToList(), new List<FlightCrewsModel>());
            }
        }

        public async Task<ResponseModel> InsertFlightDetails(FlightInfoModel inputs, string staffId)
        {
            var filter = Mapper.Map(inputs, new FlightInfoEO());
            return Mapper.Map(await _flightDelayDao.UpdateFlightDetails(filter, staffId), new ResponseModel());
        }

        public Task<string> IsVrForFlight(string id)
        {
            return _flightDelayDao.IsVrForFlight(id);
        }

        public Task<string> IsDelayRportForFlight(string id)
        {
            return _flightDelayDao.IsDelayRportForFlight(id);
        }

        public void DeleteFlightDetails(string id)
        {
            _flightDelayDao.DeleteFlightDetails(id);
        }

        public async Task<IEnumerable<FlightCrewsModel>> GetCrewsForFlight(string flightDetailsId)
        {
            return Mapper.Map(await _flightDelayDao.GetCrewsForFlight(flightDetailsId), new List<FlightCrewsModel>());
        }
    }
}
