using AutoMapper;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.DataAccess.Implementation;
using QR.IPrism.DataAccess.Interfaces;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Adapter.Implementation
{
    public class FlightDelayAdapter : IFlightDelayAdapter
    {
        #region Private Variables
        private readonly IFlightDelayDao _flightDelayDao = new FlightDelayDao();
        #endregion

        public async Task<IEnumerable<FlightInfoModel>> GetDelaySearchResults(FlightDelayFilterModel inputs, string staffNo, string staffId)
        {
            var filter = Mapper.Map(inputs, new FlightDelayFilterEO());
            return Mapper.Map(await _flightDelayDao.GetDelaySearchResults(filter, staffNo, staffId), new List<FlightInfoModel>());
        }

        public async Task<IEnumerable<FlightInfoModel>> GetEnterDelayFlightDetails(FlightDelayFilterModel inputs, string staffNo, string staffId)
        {
            var filter = Mapper.Map(inputs, new FlightDelayFilterEO());
            return Mapper.Map(await _flightDelayDao.GetEnterDelayFlightDetails(filter, staffNo, staffId), new List<FlightInfoModel>());
        }

        //public async Task<IEnumerable<LookupModel>> GetSectorDetails()
        //{
        //    return Mapper.Map(await _flightDelayDao.GetSectorDetails(), new List<LookupModel>());
        //}

        public async Task<IEnumerable<FlightDelayModel>> GetDelayLookupValues()
        {
            return Mapper.Map(await _flightDelayDao.GetDelayLookupValues(), new List<FlightDelayModel>());
        }
        
        public void SetFlightDelayDetails(FlightDelayModel inputs, string staffId, string staffNo)
        {
            _flightDelayDao.SetFlightDelayDetails(Mapper.Map(inputs, new FlightDelayEO()), staffId, staffNo);
        }

        public async Task<IEnumerable<FlightDelayModel>> GetDelayReasons(string id)
        {
            return Mapper.Map(await _flightDelayDao.GetDelayReasons(id), new List<FlightDelayModel>());
        }

        public string IsEnterDelayForFlight(string id)
        {
            return _flightDelayDao.IsEnterDelayForFlight(id).Result;
        }
    }
}