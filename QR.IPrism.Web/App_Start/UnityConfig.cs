using Microsoft.Practices.Unity;
using System.Web.Http;
using System;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Adapter.Implementation;
using QR.IPrism.Security.Authentication;

namespace QR.IPrism.Web
{
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });
        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        public static void RegisterTypes(IUnityContainer container)
        {
            container
                .RegisterType<IEmployeeAdapter, EmployeeAdapter>()
                .RegisterType<ISharedAdapter, SharedAdapter>()
                .RegisterType<ILookupAdapter, LookupAdapter>()
                .RegisterType<IRosterAdapter, RosterAdapter>()
                .RegisterType<IOverviewAdapter, OverviewAdapter>()
                .RegisterType<IDashboardAdapter, DashboardAdapter>()
                .RegisterType<IHousingAdapter, HousingAdapter>()
                .RegisterType<ICrewProfileAdapter, CrewProfileAdapter>()
                .RegisterType<IFlightDelayAdapter, FlightDelayAdapter>()
                .RegisterType<IEVRAdapter, EVRAdapter>()
                .RegisterType<ICrewProfileAdapter, CrewProfileAdapter>()
                .RegisterType<IAssessmentListAdapter, AssessmentListAdapter>()
                .RegisterType<IAssessmentAdapter, AssessmentAdapter>()
                .RegisterType<IAssessmentSearchAdapter, AssessmentSearchAdapter>()
                .RegisterType<ISearchAdapter, SearchAdapter>()
                //.RegisterType<IPOAssessmentAdapter, POAssessmentAdapter>();
                .RegisterType<IFlightAddEditAdapter, FlightAddEditAdapter>()
             .RegisterType<ISecurityManager, SecurityManager>()
             .RegisterType<ITokenManager, TokenManager>()
            .RegisterType<IKafouAdapter, KafouAdapter>();
        }
        #endregion
    }
}