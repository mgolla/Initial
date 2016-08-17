
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Adapter.Interfaces
{
    public interface ISearchAdapter
    {
        Task<List<FlightInfoModel>> GetFlightInfosAsyc(CommonFilterModel filterInput);
        Task<SearchViewModel> GetSearchAsyc(SearchFilterModel filterInput);
        Task<List<WeatherInfoModel>> GetWeatherInfosAsyc(CommonFilterModel filterInput);
        Task<List<TrainingTransportModel>> GetTrainingTransportsAsyc(CommonFilterModel filterInput);
        Task<List<CurrencyDetailModel>> GetCurrencyDetailsAsyc(CommonFilterModel filterInput);
        Task<List<CrewLocatorModel>> GetCrewLocatorsAsyc();
        Task<List<AssessmentSearchModel>> GetAutoSuggestStaffInfo(string filter);
        Task<List<LocationCrewDetailModel>> GetLocationCrewDetailsAsyc(CommonFilterModel filterInput);
        Task<List<DutySummaryModel>> GetDutySummarysAsyc(CommonFilterModel filterInput);
        Task<List<LocationFlightModel>> GetLocationFlightsAsyc(CommonFilterModel filterInput);
        Task<String> RunCrewLocatorProcess();
    }
}


