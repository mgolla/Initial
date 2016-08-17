
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.EntityObjects.Module;

namespace QR.IPrism.BusinessObjects.Interfaces
{
    public interface ISearchDao
    {
        Task<List<FlightInfoEO>> GetFlightInfosAsyc(CommonFilterEO filterInput);
        Task<List<CurrencyDetailEO>> GetCurrencyDetailsAsyc(CommonFilterEO filterInput);
        Task<List<TrainingTransportEO>> GetTrainingTransportsAsyc(CommonFilterEO filterInput);
        Task<List<WeatherInfoEO>> GetWeatherInfosAsyc(CommonFilterEO filterInput);
        Task<List<CrewLocatorEO>> GetCrewLocatorsAsyc();
        Task<List<AssessmentSearchEO>> GetAutoSuggestStaffInfo(string filterInput);
        Task<List<LocationFlightEO>> GetLocationFlightsAsyc(CommonFilterEO filterInput);
        Task<List<LocationCrewDetailEO>> GetLocationCrewDetailsAsyc(CommonFilterEO filterInput);
        Task<List<DutySummaryEO>> GetDutySummarysAsyc(CommonFilterEO filterInput);
        Task<String> RunCrewLocatorProcess();
    }
}


