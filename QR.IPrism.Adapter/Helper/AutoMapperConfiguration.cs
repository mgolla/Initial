using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using QR.IPrism.EntityObjects;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using QR.IPrism.Models;
using QR.IPrism.Models.Shared;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.ViewModels;

namespace QR.IPrism.Adapter.Helper
{
    public static class AutoMapperConfiguration
    {
        public static void configureClientMapping()
        {
            configureIVRMappings();
        }
        private static void configureIVRMappings()
        {
            Mapper.CreateMap<AnalyticEO, AnalyticModel>().ReverseMap();
            Mapper.CreateMap<MessageEO, MessageModel>().ReverseMap();
            Mapper.CreateMap<RoleEO, RoleModel>().ReverseMap();
            Mapper.CreateMap<UserContextEO, UserContextModel>().ReverseMap();
            Mapper.CreateMap<UserTokenEO, UserTokenModel>().ReverseMap();
            Mapper.CreateMap<ClientEO, ClientModel>().ReverseMap();
            Mapper.CreateMap<RosterEO, RosterModel>().ReverseMap();
            Mapper.CreateMap<RosterFilterEO, RosterFilterModel>().ReverseMap();
            Mapper.CreateMap<LookupEO, LookupModel>().ReverseMap();
            Mapper.CreateMap<OverviewEO, OverviewModel>().ReverseMap();
            Mapper.CreateMap<OverviewFilterEO, OverviewFilterModel>().ReverseMap();
            Mapper.CreateMap<TransportTripDetailEO, TransportTripDetailModel>().ReverseMap();
            Mapper.CreateMap<TrainingTransportEO, TrainingTransportModel>().ReverseMap();
            Mapper.CreateMap<CurrencyDetailEO, CurrencyDetailModel>().ReverseMap();
            Mapper.CreateMap<FlightLoadEO, FlightLoadModel>().ReverseMap();
            Mapper.CreateMap<WeatherInfoEO, WeatherInfoModel>().ReverseMap();
            Mapper.CreateMap<StationInfoEO, StationInfoModel>().ReverseMap();
            Mapper.CreateMap<StationInfoFilterEO, StationInfoFilterModel>().ReverseMap();
            Mapper.CreateMap<HotelInfoEO, HotelInfoModel>().ReverseMap();
            Mapper.CreateMap<HotelInfoFilterEO, HotelInfoFilterModel>().ReverseMap();
            Mapper.CreateMap<CrewInfoEO, CrewInfoModel>().ReverseMap();
            Mapper.CreateMap<CommonFilterEO, CommonFilterModel>().ReverseMap();
            Mapper.CreateMap<SummaryOfServiceEO, SummaryOfServiceModel>().ReverseMap();
            Mapper.CreateMap<SummaryOfServiceFilterEO, SummaryOfServiceFilterModel>().ReverseMap();
            Mapper.CreateMap<SOSEO, SOSModel>().ReverseMap();
            Mapper.CreateMap<BlanketEO, BlanketModel>().ReverseMap();
            Mapper.CreateMap<MealEO, MealModel>().ReverseMap();
            Mapper.CreateMap<NotificationAlertSVPEO, NotificationAlertSVPModel>().ReverseMap();
            Mapper.CreateMap<NotificationAlertSVPFilterEO, NotificationAlertSVPFilterModel>().ReverseMap();
            Mapper.CreateMap<DocumentEO, DocumentModel>().ReverseMap();
            Mapper.CreateMap<DocumentFilterEO, DocumentFilterModel>().ReverseMap();
            Mapper.CreateMap<DepartmentNewsIFEGuideEO, DepartmentNewsIFEGuideModel>().ReverseMap();
            Mapper.CreateMap<FileEO, FileModel>().ReverseMap();
            Mapper.CreateMap<FileFilterEO, FileFilterModel>().ReverseMap();
            Mapper.CreateMap<LinkEO, LinkModel>().ReverseMap();
            Mapper.CreateMap<VisionMissionEO, VisionMissionModel>().ReverseMap();
            Mapper.CreateMap<MyRequestEO, MyRequestModel>().ReverseMap();
            Mapper.CreateMap<MyRequestFilterEO, MyRequestFilterModel>().ReverseMap();
            Mapper.CreateMap<HousingEO, HousingModel>().ReverseMap();
            Mapper.CreateMap<BuildingEO, BuildingModel>().ReverseMap();
            Mapper.CreateMap<HousingRequestFilterEO, HousingRequestFilterModel>().ReverseMap();
            Mapper.CreateMap<NewsFilterEO, NewsFilterModel>().ReverseMap();
            Mapper.CreateMap<ResponseEO, ResponseModel>().ReverseMap();
            Mapper.CreateMap<CrewEntitlementEO, CrewEntitlementModel>().ReverseMap();
            Mapper.CreateMap<HousingStayOutEO, HousingStayOutModel>().ReverseMap();
            Mapper.CreateMap<HousingGuestEO, HousingGuestModel>().ReverseMap();
            Mapper.CreateMap<TrainingHistoryEO, TrainingHistoryModel>().ReverseMap();
            Mapper.CreateMap<QualnVisaEO, QualnVisaModel>().ReverseMap();
            Mapper.CreateMap<CareerPathEO, CareerPathModel>().ReverseMap();
            Mapper.CreateMap<IDPEO, IDPModel>().ReverseMap();
            Mapper.CreateMap<DestinationsVisitedEO, DestinationsVisitedModel>().ReverseMap();
            Mapper.CreateMap<AssessmentEO, AssessmentModel>().ReverseMap();
            Mapper.CreateMap<CompetencyEO, CompetencyModel>().ReverseMap();
            Mapper.CreateMap<AssessmentIDPEO, AssessmentIDPModel>().ReverseMap();
            Mapper.CreateMap<ObjectiveEO, ObjectiveModel>().ReverseMap();
            Mapper.CreateMap<RatingGuideLineEO, RatingGuideLineModel>().ReverseMap();
            Mapper.CreateMap<AssessmentSearchEO, AssessmentSearchModel>().ReverseMap();
            Mapper.CreateMap<AssessmentSearchRequestFilterEO, AssessmentSearchRequestFilterModel>().ReverseMap();
            Mapper.CreateMap<NotificationDetailsEO, NotificationDetailsModel>().ReverseMap();
            Mapper.CreateMap<HousingNotificationEO, HousingNotificationModel>().ReverseMap();
            Mapper.CreateMap<EVRRequestFilterEO, EVRRequestFilterModel>().ReverseMap();
            Mapper.CreateMap<EVRResultEO, EVRResultModel>().ReverseMap();
            Mapper.CreateMap<EVRPassengerEO, EVRPassengerModel>().ReverseMap();
            Mapper.CreateMap<EVRInsertUpdateEO, EVRInsertUpdateModel>().ReverseMap();
            Mapper.CreateMap<EVRReportMainEO, EVRReportMainModel>().ReverseMap();
            Mapper.CreateMap<FlightDelayFilterEO, FlightDelayFilterModel>().ReverseMap();
            Mapper.CreateMap<FlightDelayEO, FlightDelayModel>().ReverseMap();
            Mapper.CreateMap<FlightInfoEO, FlightInfoModel>().ReverseMap();
            Mapper.CreateMap<AssessmentSearchEO, AssessmentSearchModel>().ReverseMap();
            Mapper.CreateMap<AssessmentSearchRequestFilterEO, AssessmentSearchRequestFilterModel>().ReverseMap();
            Mapper.CreateMap<FlightDelayFilterEO, FlightDelayFilterModel>().ReverseMap();
            Mapper.CreateMap<FlightCrewsEO, FlightCrewsModel>().ReverseMap();           
            Mapper.CreateMap<EVRInsertOutputEO, EVRInsertOutputModel>().ReverseMap();
            Mapper.CreateMap<EVRDraftOutputEO, EVRDraftOutputModel>().ReverseMap();
            Mapper.CreateMap<SearchCriteriaEO, SearchCriteriaModel>().ReverseMap();
            Mapper.CreateMap<EVRListsEO, EVRListsModel>().ReverseMap();
            Mapper.CreateMap<EVRPendingEO, EVRPendingModel>().ReverseMap();
            Mapper.CreateMap<EVRCrewViewEO, EVRCrewViewModel>().ReverseMap();
            Mapper.CreateMap<VRIdEO, VRIdModel>().ReverseMap();
            Mapper.CreateMap<VRCrewDetailEO, VRCrewDetailModel>().ReverseMap();
            Mapper.CreateMap<VRPassengerDetailEO, VRPassengerDetailModel>().ReverseMap();
            Mapper.CreateMap<VRDocumentDetailEO, VRDocumentDetailModel>().ReverseMap();
            Mapper.CreateMap<VRDeptDetailEO, VRDeptDetailModel>().ReverseMap();
            Mapper.CreateMap<VRActionResolutionEO, VRActionResolutionModel>().ReverseMap();
            Mapper.CreateMap<VRDeptEnterVREO, VRDeptEnterVRModel>().ReverseMap();
            Mapper.CreateMap<VRCrewEnterVREO, VRCrewEnterVRModel>().ReverseMap();
            Mapper.CreateMap<ViewVREnterVREO, ViewVREnterVRModel>().ReverseMap();
            Mapper.CreateMap<CrewLocatorEO, CrewLocatorModel>().ReverseMap();
            Mapper.CreateMap<DutySummaryEO, DutySummaryModel>().ReverseMap();
            Mapper.CreateMap<LocationCrewDetailEO, LocationCrewDetailModel>().ReverseMap();
            Mapper.CreateMap<LocationFlightEO, LocationFlightModel>().ReverseMap();
            Mapper.CreateMap<RosterViewModelEO, RosterViewModel>().ReverseMap();
            Mapper.CreateMap<AssessmentBehaviourEO, AssessmentBehaviourModel>().ReverseMap();
            Mapper.CreateMap<KeyContactsEO, KeyContactsModel>().ReverseMap();
            Mapper.CreateMap<SearchRecognitionRequestEO, SearchRecognitionRequestModel>().ReverseMap();
            Mapper.CreateMap<SearchRecognitionResultEO, SearchRecognitionResultModel>().ReverseMap();
            Mapper.CreateMap<FlightDetailsEO, FlightDetailsModel>().ReverseMap();
            Mapper.CreateMap<SearchCrewRecognitionEO, SearchCrewRecognitionModel>().ReverseMap();
            Mapper.CreateMap<RecognitionStatusEO, RecognitionStatusModel>().ReverseMap();
            Mapper.CreateMap<RecognitionTypeEO, RecognitionTypeModel>().ReverseMap();
            Mapper.CreateMap<RecognitionSourceEO, RecognitionSourceModel>().ReverseMap();
            Mapper.CreateMap<GradeEO, GradeModel>().ReverseMap();
            Mapper.CreateMap<CommonRecognitionEO, CommonRecognitionModel>().ReverseMap();
            Mapper.CreateMap<RecognisedCrewDetailsEO, RecognisedCrewDetailsModel>().ReverseMap();
            Mapper.CreateMap<RecognitionQRValueEO, RecognitionQRValueModel>().ReverseMap();
            Mapper.CreateMap<CrewRecognitionDetailsHistoryEO, CrewRecognitionDetailsHistoryModel>().ReverseMap();
            Mapper.CreateMap<CrewRecognitionEO, CrewRecognitionModel>().ReverseMap();
            Mapper.CreateMap<StaffDetailsEO, StaffDetailsModel>().ReverseMap();
            Mapper.CreateMap<CrewRecognitionOverviewEO, CrewRecognitionOverviewModel>().ReverseMap();
        }
    }
}
