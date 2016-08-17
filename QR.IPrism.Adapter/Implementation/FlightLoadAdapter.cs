using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Models.Module;
using QR.IPrism.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Adapter.Implementation
{
    public class FlightLoadAdapter
    {
        public string _sessionId { get; set; }

        public string AppCode { get; set; }

        public string UserName { get; set; }

        private void AddCustomHeaderUserInformation(OperationContextScope scope)
        {
            string contextId = _sessionId + "~" + AppCode + "~" + UserName;
            var header = new MessageHeader<string>(contextId);
            var untyped = header.GetUntypedHeader("__Context", "urn:tempuri.org");
            OperationContext.Current.OutgoingMessageHeaders.Add(untyped);
        }


        public FlightLoadModel GetFlightLoad(FlightLoadFilter filter)
        {
            FlightLoadModel flightLoadModel = new FlightLoadModel();
            ISharedAdapter _srdAdapter = new SharedAdapter();
            var initialData = _srdAdapter.GetCommonInfoAsyc(ODSFlightLoad.CodeCommon).Result;

            AppCode = ODSFlightLoad.ODSAppCodeValue;
            UserName = ODSFlightLoad.ODSUserNameValue;
            string password = ODSFlightLoad.ODSPasswordValue;

            var app = initialData.Where(x => x.Value == ODSFlightLoad.ODSAppCode).FirstOrDefault();
            if (app != null)
            {
                AppCode = app.Text;
            }
            var us = initialData.Where(x => x.Value == ODSFlightLoad.ODSUserName).FirstOrDefault();
            if (us != null)
            {
                UserName = us.Text;
            }
            var pw = initialData.Where(x => x.Value == ODSFlightLoad.ODSPassword).FirstOrDefault();
            if (pw != null)
            {
                password = pw.Text;
            }
          
            OdsUserAuthentication.UsrMgmtSvcClient cli = new OdsUserAuthentication.UsrMgmtSvcClient();


            _sessionId = cli.LoginLite(AppCode, UserName, password, "", false, 60);

            FlightDetailService.FlightDetailClient cli1 = new FlightDetailService.FlightDetailClient();
            AddCustomHeaderUserInformation(new OperationContextScope(cli1.InnerChannel));
            CultureInfo culture = new CultureInfo("en-GB");

            var fullFlightDetails = cli1.GetFlightDetails(new QR.IPrism.Adapter.FlightDetailService.DNBFlightSearchCriteria()
            {
                FlightDate = Convert.ToDateTime(filter.FltDate, culture),
                FlightNumber = filter.FltNum.Replace("QR", string.Empty),
                Origin = filter.Origin,
            });


            if (fullFlightDetails != null && fullFlightDetails.lstFlightSummary != null)
            {
                var data = fullFlightDetails.lstFlightSummary.FirstOrDefault();
                flightLoadModel.AircraftDesc = fullFlightDetails.FlightDuration;
                flightLoadModel.AircraftRegNo = fullFlightDetails.ArcRegNo;
                flightLoadModel.SchedeptDate = fullFlightDetails.DEP;
                flightLoadModel.ActualArrDate = fullFlightDetails.ARR;
                flightLoadModel.FirstClassSeatCount = (fullFlightDetails.Capacity_F).ToString();
                flightLoadModel.FirstClassBookedLoad = (data.Booked_F).ToString();
                flightLoadModel.BusinessClassSeatCount = (fullFlightDetails.Capacity_J).ToString();
                flightLoadModel.BusinessClassBookedLoad = (data.Booked_J).ToString();
                flightLoadModel.EconomyClassSeatCount = (fullFlightDetails.Capacity_Y).ToString();
                flightLoadModel.EconomyClassBookedLoad = (data.Booked_Y).ToString();
            }

            return flightLoadModel;

        }

    }

}










