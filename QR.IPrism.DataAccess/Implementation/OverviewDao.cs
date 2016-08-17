using QR.IPrism.EntityObjects.Module;
using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Enterprise;
using QR.IPrism.Utility;
using QR.IPrism.BusinessObjects.Interfaces;
using System.IO;
using Oracle.DataAccess.Client;
using QR.IPrism.EntityObjects.Shared;
using QR.IPrism.BusinessObjects.Data_Layer.Shared;

namespace QR.IPrism.BusinessObjects.Implementation
{
    public class OverviewDao : IOverviewDao
    {
        /*public async Task<OverviewEO> GetOverviewAsyc(OverviewFilterEO filterInput)
        {

            OverviewEO response = null;
            DBFramework dbframework = new DBFramework(Constants.CONNECTION_STR);
            {
                List<CommandParameter> perametersList = null;
                List<CommandCursorParameter> commandCursorParameter = new List<CommandCursorParameter>();
                commandCursorParameter.Add(new CommandCursorParameter("po_flightload", ParameterDirection.Output, OracleType.Cursor));
                commandCursorParameter.Add(new CommandCursorParameter("po_weather", ParameterDirection.Output, OracleType.Cursor));
                commandCursorParameter.Add(new CommandCursorParameter("po_exchange", ParameterDirection.Output, OracleType.Cursor));
                commandCursorParameter.Add(new CommandCursorParameter("po_transport", ParameterDirection.Output, OracleType.Cursor));
                //commandCursorParameter.Add(new CommandCursorParameter("po_station_image", ParameterDirection.Output, OracleType.Cursor));
                try
                {
                    perametersList = new List<CommandParameter>();
                    perametersList.Add(new CommandParameter("pi_staffno", filterInput.StaffNo.Trim(), ParameterDirection.Input, DbType.String));
                    perametersList.Add(new CommandParameter("pi_sectorfrom", filterInput.Sectorfrom.Trim(), ParameterDirection.Input, DbType.String));
                    perametersList.Add(new CommandParameter("pi_sectorto", filterInput.SectorTo.Trim(), ParameterDirection.Input, DbType.String));
                    perametersList.Add(new CommandParameter("pi_std", filterInput.STD.Trim(), ParameterDirection.Input, DbType.Date));
                    perametersList.Add(new CommandParameter("pi_flightno", filterInput.FlightNo.Trim(), ParameterDirection.Input, DbType.String));


                    using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PRISM_IPA_DASHBOARD.get_fduty_overview", perametersList, commandCursorParameter))
                    {
                        response = new OverviewEO();
                        List<WeatherInfoEO> weatherInfoResponses = new List<WeatherInfoEO>();
                        while (objReader.Read())
                        {
                            response.FlightLoad = new FlightLoadEO();
                            response.FlightLoad.AircraftDesc = objReader["aircraft_desc"] != null ? objReader["aircraft_desc"].ToString() : string.Empty;
                            response.FlightLoad.AircraftRegNo = objReader["aircraftregno"] != null ? objReader["aircraftregno"].ToString() : string.Empty;
                            response.FlightLoad.SchedeptDate = objReader["schedeptdate"] != null ? (DateTime?)(objReader["schedeptdate"]) : null;
                            response.FlightLoad.ActualArrDate = objReader["actarrdate"] != null ? (DateTime?)(objReader["actarrdate"]) : null;
                            response.FlightLoad.FirstClassSeatCount = objReader["f_seat_count"] != null && objReader["f_seat_count"].ToString() != string.Empty ? (objReader["f_seat_count"]).ToString() : "NA";
                            response.FlightLoad.FirstClassBookedLoad = objReader["f_booked_load"] != null && objReader["f_booked_load"].ToString() != string.Empty ? (objReader["f_booked_load"]).ToString() : "NA";
                            response.FlightLoad.BusinessClassSeatCount = objReader["j_seat_count"] != null && objReader["j_seat_count"].ToString() != string.Empty ? (objReader["j_seat_count"]).ToString() : "NA";
                            response.FlightLoad.BusinessClassBookedLoad = objReader["j_booked_load"] != null && objReader["j_booked_load"].ToString() != string.Empty ? (objReader["j_booked_load"]).ToString() : "NA";
                            response.FlightLoad.EconomyClassSeatCount = objReader["y_seat_count"] != null && objReader["y_seat_count"].ToString() != string.Empty ? (objReader["y_seat_count"]).ToString() : "NA";
                            response.FlightLoad.EconomyClassBookedLoad = objReader["y_booked_load"] != null && objReader["y_booked_load"].ToString() != string.Empty ? (objReader["y_booked_load"]).ToString() : "NA";

                            response.FlightLoad.DepartureName = objReader["sectorfrom_city"] != null ? objReader["sectorfrom_city"].ToString() : string.Empty;
                            response.FlightLoad.ArrivalName = objReader["sectorto_city"] != null ? objReader["sectorto_city"].ToString() : string.Empty;

                            response.FlightLoad.RouteFlight = objReader["aroute"] != null && objReader["aroute"].ToString() != string.Empty ? objReader["aroute"].ToString() : "N/A";
                            response.FlightLoad.LastBatchTime = objReader["lastbatchtime"] != null && objReader["lastbatchtime"].ToString() != string.Empty ? Convert.ToDateTime(objReader["lastbatchtime"].ToString()) : default(DateTime);

                        }
                        objReader.NextResult();
                        while (objReader.Read())
                        {
                            WeatherInfoEO weatherInfoResponse = new WeatherInfoEO();
                            weatherInfoResponse.WeatherDate = objReader["weatherdate"] != null ? (DateTime?)objReader["weatherdate"] : null;
                            weatherInfoResponse.AirportCode = objReader["airport_code"] != null ? objReader["airport_code"].ToString() : string.Empty;
                            weatherInfoResponse.City = objReader["city"] != null ? objReader["city"].ToString() : string.Empty;
                            weatherInfoResponse.TempLow = objReader["temp_low"] != null ? objReader["temp_low"].ToString() : string.Empty;
                            weatherInfoResponse.TempHigh = objReader["temp_high"] != null ? objReader["temp_high"].ToString() : string.Empty;
                            weatherInfoResponse.LocationCode = objReader["locationcode"] != null ? objReader["locationcode"].ToString() : string.Empty;
                            weatherInfoResponse.WeekDay = objReader["week_day"] != null ? objReader["week_day"].ToString() : string.Empty;
                            weatherInfoResponses.Add(weatherInfoResponse);
                        }
                        response.WeatherInfos = weatherInfoResponses;
                        objReader.NextResult();
                        while (objReader.Read())
                        {
                            response.CurrencyDetail = new CurrencyDetailEO();

                            response.CurrencyDetail.AirportCode = objReader["airport_code"] != null ? objReader["airport_code"].ToString() : string.Empty;
                            response.CurrencyDetail.FromCurrency = objReader["from_curr"] != null ? objReader["from_curr"].ToString() : string.Empty;
                            response.CurrencyDetail.ToCurrency = objReader["to_curr"] != null ? objReader["to_curr"].ToString() : string.Empty;
                            response.CurrencyDetail.ConversionRate = objReader["conversion_rate"] != null ? Convert.ToDecimal(objReader["conversion_rate"]) : 0;
                            response.CurrencyDetail.InvConversionRate = objReader["inv_conversion_rate"] != null ? Convert.ToDecimal(objReader["inv_conversion_rate"]) : 0;
                        }
                        objReader.NextResult();
                        while (objReader.Read())
                        {
                            response.TransportTripDetail = new TransportTripDetailEO();

                            response.TransportTripDetail.StaffNo = objReader["staffno"] != null ? objReader["staffno"].ToString() : string.Empty;
                            response.TransportTripDetail.TripId = objReader["trip_id"] != null ? Convert.ToInt32(objReader["trip_id"]) : 0;
                            response.TransportTripDetail.PickupLocation = objReader["pickup_location"] != null ? objReader["pickup_location"].ToString() : string.Empty;
                            response.TransportTripDetail.PickupTime = objReader["pickup_time"] != null ? (DateTime?)objReader["pickup_time"] : null;
                            response.TransportTripDetail.DutyCode = objReader["duty_code"] != null ? objReader["duty_code"].ToString() : string.Empty;
                            response.TransportTripDetail.DutyType = objReader["duty_type"] != null ? objReader["duty_type"].ToString() : string.Empty;
                            response.TransportTripDetail.ReportingTime = objReader["reporting_time"] != null ? (DateTime?)objReader["reporting_time"] : null;
                        }


                    }
                }
                catch (Exception ex)
                {

                    throw ex;

                }
                finally
                {
                    dbframework.CloseConnection();
                }
            }
            return response;
        }*/

        public async Task<OverviewEO> GetOverviewAsyc(OverviewFilterEO filterInput)
        {

            OverviewEO response = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {

                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_staffno", filterInput.StaffNo.Trim(), ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_flightno", filterInput.FlightNo.Trim(), ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_sectorfrom", filterInput.Sectorfrom.Trim(), ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_sectorto", filterInput.SectorTo.Trim(), ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_std", Common.ToOracleDate(filterInput.STD.Trim()), ParameterDirection.Input, OracleDbType.Date));

                    perametersList.Add(new ODPCommandParameter("po_flightload", ParameterDirection.Output, OracleDbType.RefCursor));
                    perametersList.Add(new ODPCommandParameter("po_weather", ParameterDirection.Output, OracleDbType.RefCursor));
                    perametersList.Add(new ODPCommandParameter("po_exchange", ParameterDirection.Output, OracleDbType.RefCursor));
                    perametersList.Add(new ODPCommandParameter("po_transport", ParameterDirection.Output, OracleDbType.RefCursor));


                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("PRISM_IPA_DASHBOARD.get_fduty_overview", perametersList))
                    {
                        response = new OverviewEO();
                        List<WeatherInfoEO> weatherInfoResponses = new List<WeatherInfoEO>();
                        while (objReader.Read())
                        {
                            response.FlightLoad = new FlightLoadEO();
                            response.FlightLoad.AircraftDesc = objReader["aircraft_desc"] != null ? objReader["aircraft_desc"].ToString() : string.Empty;
                            response.FlightLoad.AircraftRegNo = objReader["aircraftregno"] != null ? objReader["aircraftregno"].ToString() : string.Empty;
                            response.FlightLoad.SchedeptDate = objReader["schedeptdate"] != null ? (DateTime?)(objReader["schedeptdate"]) : null;
                            response.FlightLoad.ActualArrDate = objReader["actarrdate"] != null ? (DateTime?)(objReader["actarrdate"]) : null;
                            response.FlightLoad.FirstClassSeatCount = objReader["f_seat_count"] != null && objReader["f_seat_count"].ToString() != string.Empty ? (objReader["f_seat_count"]).ToString() : "NA";
                            response.FlightLoad.FirstClassBookedLoad = objReader["f_booked_load"] != null && objReader["f_booked_load"].ToString() != string.Empty ? (objReader["f_booked_load"]).ToString() : "NA";
                            response.FlightLoad.BusinessClassSeatCount = objReader["j_seat_count"] != null && objReader["j_seat_count"].ToString() != string.Empty ? (objReader["j_seat_count"]).ToString() : "NA";
                            response.FlightLoad.BusinessClassBookedLoad = objReader["j_booked_load"] != null && objReader["j_booked_load"].ToString() != string.Empty ? (objReader["j_booked_load"]).ToString() : "NA";
                            response.FlightLoad.EconomyClassSeatCount = objReader["y_seat_count"] != null && objReader["y_seat_count"].ToString() != string.Empty ? (objReader["y_seat_count"]).ToString() : "NA";
                            response.FlightLoad.EconomyClassBookedLoad = objReader["y_booked_load"] != null && objReader["y_booked_load"].ToString() != string.Empty ? (objReader["y_booked_load"]).ToString() : "NA";

                            response.FlightLoad.DepartureName = objReader["sectorfrom_city"] != null ? objReader["sectorfrom_city"].ToString() : string.Empty;
                            response.FlightLoad.ArrivalName = objReader["sectorto_city"] != null ? objReader["sectorto_city"].ToString() : string.Empty;

                            response.FlightLoad.RouteFlight = objReader["aroute"] != null && objReader["aroute"].ToString() != string.Empty ? objReader["aroute"].ToString() : "N/A";
                            response.FlightLoad.LastBatchTime = objReader["lastbatchtime"] != null && objReader["lastbatchtime"].ToString() != string.Empty ? (DateTime?)(objReader["lastbatchtime"]) : null;

                        }
                        objReader.NextResult();
                        while (objReader.Read())
                        {
                            WeatherInfoEO weatherInfoResponse = new WeatherInfoEO();
                            weatherInfoResponse.WeatherDate = objReader["weatherdate"] != null ? (DateTime?)objReader["weatherdate"] : null;
                            weatherInfoResponse.AirportCode = objReader["airport_code"] != null ? objReader["airport_code"].ToString() : string.Empty;
                            weatherInfoResponse.City = objReader["city"] != null ? objReader["city"].ToString() : string.Empty;
                            weatherInfoResponse.TempLow = objReader["temp_low"] != null ? objReader["temp_low"].ToString() : string.Empty;
                            weatherInfoResponse.TempHigh = objReader["temp_high"] != null ? objReader["temp_high"].ToString() : string.Empty;
                            weatherInfoResponse.LocationCode = objReader["locationcode"] != null ? objReader["locationcode"].ToString() : string.Empty;
                            weatherInfoResponse.WeekDay = objReader["week_day"] != null ? objReader["week_day"].ToString() : string.Empty;
                            weatherInfoResponses.Add(weatherInfoResponse);
                        }
                        response.WeatherInfos = weatherInfoResponses;
                        objReader.NextResult();
                        while (objReader.Read())
                        {
                            response.CurrencyDetail = new CurrencyDetailEO();

                            response.CurrencyDetail.AirportCode = objReader["airport_code"] != null ? objReader["airport_code"].ToString() : string.Empty;
                            response.CurrencyDetail.FromCurrency = objReader["from_curr"] != null ? objReader["from_curr"].ToString() : string.Empty;
                            response.CurrencyDetail.ToCurrency = objReader["to_curr"] != null ? objReader["to_curr"].ToString() : string.Empty;
                            response.CurrencyDetail.ConversionRate = objReader["conversion_rate"] != null ? Convert.ToDecimal(objReader["conversion_rate"]) : 0;
                            response.CurrencyDetail.InvConversionRate = objReader["inv_conversion_rate"] != null ? Convert.ToDecimal(objReader["inv_conversion_rate"]) : 0;
                        }
                        objReader.NextResult();
                        while (objReader.Read())
                        {
                            response.TransportTripDetail = new TransportTripDetailEO();

                            response.TransportTripDetail.StaffNo = objReader["staffno"] != null ? objReader["staffno"].ToString() : string.Empty;
                            response.TransportTripDetail.TripId = objReader["trip_id"] != null ? Convert.ToInt32(objReader["trip_id"]) : 0;
                            response.TransportTripDetail.PickupLocation = objReader["pickup_location"] != null ? objReader["pickup_location"].ToString() : string.Empty;
                            response.TransportTripDetail.PickupTime = objReader["pickup_time"] != null ? (DateTime?)objReader["pickup_time"] : null;
                            response.TransportTripDetail.DutyCode = objReader["duty_code"] != null ? objReader["duty_code"].ToString() : string.Empty;
                            response.TransportTripDetail.DutyType = objReader["duty_type"] != null ? objReader["duty_type"].ToString() : string.Empty;
                            response.TransportTripDetail.ReportingTime = objReader["reporting_time"] != null ? (DateTime?)objReader["reporting_time"] : null;
                        }


                    }
                }
                catch (Exception ex)
                {

                    throw ex;

                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }

            return response;
        }

        public async Task<List<OverviewEO>> GetOverviewsAsyc(OverviewFilterEO filterInput)
        {
            List<OverviewEO> response = new List<OverviewEO>();
            return response;
        }

        public async Task<StationInfoEO> GetStationInfoAsyc(StationInfoFilterEO filterInput)
        {
            StationInfoEO response = new StationInfoEO();
            response.IsDataLoaded = IsDataLoaded.No;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_station_code", filterInput.StationCode.Trim(), ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_station", ParameterDirection.Output, OracleDbType.RefCursor));

                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("PRISM_IPA_DASHBOARD.get_fduty_stationinfo_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new StationInfoEO();

                            response.IsDataLoaded = IsDataLoaded.Yes;
                            response.StationCode = objReader["station_code"] != null ? objReader["station_code"].ToString() : string.Empty;
                            response.CountryName = objReader["country_name"] != null ? objReader["country_name"].ToString() : string.Empty;
                            response.AirportName = objReader["airport_name"] != null ? objReader["airport_name"].ToString() : string.Empty;
                            response.Language = objReader["language"] != null ? objReader["language"].ToString() : string.Empty;
                            response.HandlingAgent = objReader["handling_agents"] != null ? objReader["handling_agents"].ToString() : string.Empty;
                            response.Catering = objReader["catering"] != null ? objReader["catering"].ToString() : string.Empty;
                            response.StationContact = objReader["station_contact"] != null ? objReader["station_contact"].ToString() : string.Empty;
                            response.CountryCode = objReader["country_code"] != null ? objReader["country_code"].ToString() : string.Empty;
                            response.Currency = objReader["currency"] != null ? objReader["currency"].ToString() : string.Empty;
                            response.CrewInformation = objReader["crew_information"] != null ? objReader["crew_information"].ToString() : string.Empty;
                            response.CustomerInformation = objReader["customer_information"] != null ? objReader["customer_information"].ToString() : string.Empty;
                            response.CityName = objReader["city_name"] != null ? objReader["city_name"].ToString() : string.Empty;
                            response.LocalTime = objReader["local_time"] != null ? objReader["local_time"].ToString() : string.Empty;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }
            return response;
        }

        public async Task<HotelInfoEO> GetHotelInfoAsyc(HotelInfoFilterEO filterInput)
        {
            HotelInfoEO response = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_airportcode", filterInput.AirportCode.Trim(), ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_hotel", ParameterDirection.Output, OracleDbType.RefCursor));

                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("PRISM_IPA_DASHBOARD.get_fduty_hotelinfo_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new HotelInfoEO();

                            response.CityCode = objReader["city_code"] != null ? objReader["city_code"].ToString() : string.Empty;
                            response.HotelName = objReader["hotel_name"] != null ? objReader["hotel_name"].ToString() : string.Empty;
                            response.Address = objReader["address"] != null ? objReader["address"].ToString() : string.Empty;
                            response.Fax = objReader["fax"] != null ? objReader["fax"].ToString() : string.Empty;
                            response.HotelContactPersonName = objReader["hotel_contact_personname"] != null ? objReader["hotel_contact_personname"].ToString() : string.Empty;
                            response.MealsAllowanceQarBreakfast = objReader["meals_allowance_qar_breakfast"] != null ? objReader["meals_allowance_qar_breakfast"].ToString() : string.Empty;
                            response.MealsAllowanceQarLunch = objReader["meals_allowance_qar_lunch"] != null ? objReader["meals_allowance_qar_lunch"].ToString() : string.Empty;
                            response.MealsAllowanceQarDinner = objReader["meals_allowance_qar_dinner"] != null ? objReader["meals_allowance_qar_dinner"].ToString() : string.Empty;
                            response.HotelInformation = objReader["hotel_information"] != null ? objReader["hotel_information"].ToString() : string.Empty;
                            response.OtherInformation = objReader["other_information"] != null ? objReader["other_information"].ToString() : string.Empty;
                            response.Telephone = objReader["telephone"] != null ? objReader["telephone"].ToString() : string.Empty;
                            response.Lattitude = objReader["latitude"] != null ? objReader["latitude"].ToString() : string.Empty;
                            response.Longitude = objReader["longitude"] != null ? objReader["longitude"].ToString() : string.Empty;


                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }
            return response;
        }

        public async Task<List<CrewInfoEO>> GetCrewInfoAsyc(CommonFilterEO filterInput)
        {

            List<CrewInfoEO> responseList = new List<CrewInfoEO>();
            List<CrewInfoEO> finalResponseList = new List<CrewInfoEO>();

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.AIMS_CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("flight_no", filterInput.FlightNo.Trim(), ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("v_arr", filterInput.Arrival.Trim(), ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("flight_date", Common.ToOracleDate(filterInput.FlightDate.Trim()), ParameterDirection.Input, OracleDbType.Date));
                    perametersList.Add(new ODPCommandParameter("fpis_crew", ParameterDirection.Output, OracleDbType.RefCursor));

                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("GETCREWONBOARD", perametersList))
                    {
                        while (objReader.Read())
                        {
                            CrewInfoEO response = new CrewInfoEO();

                            response.FlightNo = objReader["FLTNO"] != null ? Convert.ToString(objReader["FLTNO"]) : string.Empty;
                            response.CrewID = objReader["CrewID"] != null ? Convert.ToString(objReader["CrewID"].ToString()) : string.Empty;
                            response.Name = objReader["Name"] != null ? objReader["Name"].ToString() : string.Empty;
                            response.Description = objReader["Description"] != null ? objReader["Description"].ToString() : string.Empty;
                            response.Type = objReader["Typ"] != null ? objReader["Typ"].ToString() : string.Empty;
                            response.FlightDate = objReader["FlightDate"] != null ? (DateTime?)objReader["FlightDate"] : null;
                            response.LastRefresh = objReader["Last_Refresh"] != null ? (DateTime?)objReader["Last_Refresh"] : null;
                            response.CrewRoute = objReader["Crew_Route"] != null ? objReader["Crew_Route"].ToString() : string.Empty;
                            response.POS = objReader["POS"] != null ? objReader["POS"].ToString() : string.Empty;
                            response.CrewPhotoUrl = filterInput.CrewImagePath + "/" + response.CrewID + filterInput.CrewImageType;
                            responseList.Add(response);
                        }
                    }
                    finalResponseList = responseList;

                    //string crewIds = string.Join(",", responseList.Select(crw => crw.CrewID));
                    //List<StaffPhoto> staffPhotos = await GetStaffPhotoAsyc(crewIds);

                    //finalResponseList = (from crew in responseList
                    //                     join photo in staffPhotos on crew.CrewID equals photo.AIMSStaffNo into list
                    //                     from crewPhoto in list.DefaultIfEmpty()
                    //                     select new CrewInfoEO()
                    //                     {
                    //                         CrewPhoto = crewPhoto != null && crewPhoto.photo != null ? crewPhoto.photo : null,
                    //                         CrewID = crew.CrewID,
                    //                         CrewRoute = crew.CrewRoute,
                    //                         Description = crew.Description,
                    //                         FlightDate = crew.FlightDate,
                    //                         FlightNo = crew.FlightNo,
                    //                         LastRefresh = crew.LastRefresh,
                    //                         Name = crew.Name,
                    //                         POS = crew.POS,
                    //                         Type = crew.Type
                    //                     }).ToList();


                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }
            return finalResponseList;
        }

        public async Task<List<StaffPhoto>> GetStaffPhotoAsyc(String ids)
        {
            List<StaffPhoto> responseList = new List<StaffPhoto>();

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_stafflist", ids, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_key", Constants.DECREPT_KEY, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_staffphoto", ParameterDirection.Output, OracleDbType.RefCursor));

                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_dashboard.get_photo_multiplecrew_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            StaffPhoto response = new StaffPhoto();

                            response.StaffNo = objReader["staffno"] != null ? Convert.ToString(objReader["staffno"]) : string.Empty;
                            response.photo = objReader["photo"] != null ? (byte[])(objReader["photo"]) : null;
                            response.AIMSStaffNo = objReader["AIMS_StaffNo"] != null ? Convert.ToString(objReader["AIMS_StaffNo"]) : string.Empty;

                            responseList.Add(response);
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }
            return responseList;
        }

        public async Task<SOSEO> GetSummaryOfServicesAsyc(SummaryOfServiceFilterEO filterInput)
        {

            SOSEO sos = new SOSEO();
            sos.FlightNo = filterInput.FlightNo;
            sos.SectorFrom = filterInput.SectorFrom;
            sos.SectorTo = filterInput.SectorTo;
            sos.IsDataLoaded = IsDataLoaded.No;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;

                IDataReader objReader = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_flightno", filterInput.FlightNo, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_sectorfrom", filterInput.SectorFrom, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_sectorto", filterInput.SectorTo, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_flag_sos", ParameterDirection.Output, OracleDbType.RefCursor));
                    perametersList.Add(new ODPCommandParameter("po_sos_economy", ParameterDirection.Output, OracleDbType.RefCursor));
                    perametersList.Add(new ODPCommandParameter("po_sos_nf_economy", ParameterDirection.Output, OracleDbType.RefCursor));
                    perametersList.Add(new ODPCommandParameter("po_sos_pi", ParameterDirection.Output, OracleDbType.RefCursor));
                    perametersList.Add(new ODPCommandParameter("po_sos_ng", ParameterDirection.Output, OracleDbType.RefCursor));
                    perametersList.Add(new ODPCommandParameter("po_sos_nf_premium", ParameterDirection.Output, OracleDbType.RefCursor));

                    objReader = await ODPDataAccess.ExecuteSPReaderAsync("PRISM_IPA_DASHBOARD.get_fduty_sos_all_proc", perametersList);
                    while (objReader.Read())
                    {

                        filterInput.economyFlag = objReader["economy"] != null ? Convert.ToInt32(objReader["economy"]) : 0;
                        filterInput.nfEconomyFlag = objReader["nf_economy"] != null ? Convert.ToInt32(objReader["nf_economy"]) : 0;
                        filterInput.nfPremium = objReader["nf_premium"] != null ? Convert.ToInt32(objReader["nf_premium"]) : 0;
                        filterInput.piFlag = objReader["premium_interim"] != null ? Convert.ToInt32(objReader["premium_interim"]) : 0;
                        filterInput.ngFlag = objReader["ng_premium"] != null ? Convert.ToInt32(objReader["ng_premium"]) : 0;
                    }
                    objReader.NextResult();
                    if (filterInput.economyFlag > 0)
                    {
                        sos.IsDataLoaded = IsDataLoaded.Yes;
                        sos.EconomySOS = GetEconomyFlagSOSByReader(ref objReader, SOSCursor.economyFlag);
                    }
                    objReader.NextResult();
                    if (filterInput.nfEconomyFlag > 0)
                    {
                        sos.IsDataLoaded = IsDataLoaded.Yes;
                        sos.EconomySOS = GetnfEconomyFlagSOSByReader(ref objReader, SOSCursor.nfEconomyFlag);
                    }
                    objReader.NextResult();
                    if (filterInput.piFlag > 0)
                    {
                        sos.IsDataLoaded = IsDataLoaded.Yes;
                        sos.PremiumSOS = GetpiFlagSOSByReader(ref objReader, SOSCursor.piFlag);
                    }
                    objReader.NextResult();
                    if (filterInput.ngFlag > 0)
                    {
                        sos.IsDataLoaded = IsDataLoaded.Yes;
                        sos.PremiumSOS = GetngFlagSOSByReader(ref objReader, SOSCursor.ngFlag);
                    }
                    objReader.NextResult();
                    if (filterInput.nfPremium > 0)
                    {
                        sos.IsDataLoaded = IsDataLoaded.Yes;
                        sos.PremiumSOS = GetnfPremiumSOSByReader(ref objReader, SOSCursor.nfPremium);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    objReader.Close();
                    ODPDataAccess.CloseConnection();
                }
            }
            return sos;
        }

        public List<SummaryOfServiceEO> GetEconomyFlagSOSByReader(ref IDataReader objReader, int cursorType)
        {
            List<SummaryOfServiceEO> responseListData = new List<SummaryOfServiceEO>();
            while (objReader.Read())
            {
                List<int> list = new List<int>();
                SummaryOfServiceEO response = new SummaryOfServiceEO();
                response.SOSType = SOSType.Economy;

                response.FlightNo = GetData(objReader["FLIGHT_NO"]);
                response.Sectorfrom = GetData(objReader["SECTORFROM"]);
                response.Sectorto = GetData(objReader["SECTORTO"]);
                response.Utc = GetData(objReader["UTC"]);
                response.Dep = GetData(objReader["DEP"]);
                response.Arr = GetData(objReader["ARR"]);
                response.Block = GetData(objReader["BLOCK"]);
                response.Earset = GetData(objReader["EARSET"]);
                response.Amenitykit = GetData(objReader["AMENITYKIT"]);
                response.Menucard = GetData(objReader["MENUCARD"]);
                response.BarSvc = GetData(objReader["BAR_SVC"]);



                response.NgServiceProcess = GetData(objReader["NG_SERVICE_PROCESS"]);
                response.Otherinformation = GetData(objReader["OTHERINFORMATION"]);
                response.Documentpath = GetData(objReader["DOCUMENTPATH"]);
                response.Linkdescription = GetData(objReader["LINKDESCRIPTION"]);
                response.Otherinformation1 = GetData(objReader["OTHERINFORMATION1"]);

                List<MealEO> meals = new List<MealEO>();
                MealEO meal1 = new MealEO();
                meal1.Type = MealType.Meal1;
                meal1.Code = GetData(objReader["MEAL_1"]);
                meal1.MenuCardName = GetData(objReader["MEAL_1_MENUCARDNAME"]);
                meal1.MenuCardType = GetData(objReader["MEAL_1_MENUCARDTYPE"]);
                meal1.Service = GetData(objReader["MEAL_1_MENUCARDTYPE"]);
                meal1.FileId = GetData(objReader["MEAL_1_MENUCARDID"]);
                meal1.FileCode = "DOCUMENTS";
                meal1.Towel = GetData(objReader["MEAL_1_TOWEL"]);
                meal1.Service = GetData(objReader["MEAL_1_SEQ"]);

                meals.Add(meal1);

                MealEO meal2 = new MealEO();
                meal2.Type = MealType.Meal2;
                meal2.Code = GetData(objReader["MEAL_2"]);
                meal2.MenuCardName = GetData(objReader["MEAL_2_MENUCARDNAME"]);
                meal2.MenuCardType = GetData(objReader["MEAL_2_MENUCARDTYPE"]);
                meal2.FileId = GetData(objReader["MEAL_2_MENUCARDID"]);
                meal2.FileCode = "DOCUMENTS";
                meal2.Towel = GetData(objReader["MEAL_2_TOWEL"]);
                meal2.Service = GetData(objReader["MEAL_2_SEQ"]);

                meals.Add(meal2);
                response.Meals = meals;

                responseListData.Add(response);
            }
            return responseListData;
        }
        public List<SummaryOfServiceEO> GetnfEconomyFlagSOSByReader(ref IDataReader objReader, int cursorType)
        {
            List<SummaryOfServiceEO> responseListData = new List<SummaryOfServiceEO>();
          
                while (objReader.Read())
                {
                    SummaryOfServiceEO response = new SummaryOfServiceEO();
                    response.SOSType = SOSType.Economy;
                    response.FlightNo = GetData(objReader["FLIGHT_NO"]);
                    response.Sectorfrom = GetData(objReader["SECTORFROM"]);
                    response.Sectorto = GetData(objReader["SECTORTO"]);
                    response.Utc = GetData(objReader["UTC"]);
                    response.Dep = GetData(objReader["DEP"]);
                    response.Arr = GetData(objReader["ARR"]);
                    response.Block = GetData(objReader["BLOCK"]);
                    response.Earset = GetData(objReader["EARSET"]);
                    response.Amenitykit = GetData(objReader["AMENITYKIT"]);
                    response.Menucard = GetData(objReader["MENUCARD"]);
                    response.Documenturl = GetData(objReader["DOCUMENTURL"]);
                    response.Linkdescription = GetData(objReader["LINKDESCRIPTION"]);
                    response.Uniquecol = GetData(objReader["UNIQUECOL"]);
                    if (objReader["COMFORTPOUCH"] != System.DBNull.Value)
                    {
                        response.Comfortpouch = GetData(objReader["COMFORTPOUCH"]);
                    }
                    else
                    {
                        response.Comfortpouch = "N";
                    }
                    if (objReader["COMFORTBAG"] != System.DBNull.Value)
                    {
                        response.Comfortbag = GetData(objReader["COMFORTBAG"]);
                    }
                    else
                    {
                        response.Comfortbag = "N";
                    }
                    response.AtoDrinks = GetData(objReader["ATO_DRINKS"]);

                    response.Otherinformation = GetData(objReader["OTHERINFORMATION"]);
                    response.Otherinformation1 = GetData(objReader["OTHERINFORMATION1"]);

                    List<MealEO> meals = new List<MealEO>();
                    MealEO meal1 = new MealEO();
                    meal1.Type = MealType.Meal1;
                    meal1.Code = GetData(objReader["MEAL_1"]);
                    meal1.MenuCardName = GetData(objReader["MEAL_1_MENUCARDNAME"]);
                    meal1.MenuCardType = GetData(objReader["MEAL_1_MENUCARDTYPE"]);
                    meal1.Service = GetData(objReader["MEAL_1_MENUCARDTYPE"]);
                    meal1.FileId = GetData(objReader["MEAL_1_MENUCARDID"]);
                    meal1.FileCode = "DOCUMENTS";
                    meal1.Service = GetData(objReader["MEAL_1_SEQ"]);
                    meal1.Moviesnack = MovieSnackType.MSnack1;
                    meal1.MoviesnackValue = GetData(objReader["MOVIESNACKS_1"]);
                    meals.Add(meal1);

                    MealEO meal2 = new MealEO();
                    meal2.Type = MealType.Meal2;
                    meal2.Code = GetData(objReader["MEAL_2"]);
                    meal2.MenuCardName = GetData(objReader["MEAL_2_MENUCARDNAME"]);
                    meal2.MenuCardType = GetData(objReader["MEAL_2_MENUCARDTYPE"]);
                    meal2.FileId = GetData(objReader["MEAL_2_MENUCARDID"]);
                    meal2.FileCode = "DOCUMENTS";
                    meal2.Service = GetData(objReader["MEAL_2_SEQ"]);
                    meal2.Moviesnack = MovieSnackType.MSnack2;
                    meal2.MoviesnackValue = GetData(objReader["MOVIESNACKS_2"]);
                    meals.Add(meal2);

                    MealEO meal3 = new MealEO();
                    meal3.Type = MealType.Meal3;
                    meal3.Code = GetData(objReader["MEAL_3"]);
                    meal3.MenuCardName = GetData(objReader["MEAL_3_MENUCARDNAME"]);
                    meal3.MenuCardType = GetData(objReader["MEAL_3_MENUCARDTYPE"]);
                    meal3.FileId = GetData(objReader["MEAL_3_MENUCARDID"]);
                    meal3.FileCode = "DOCUMENTS";
                    meal3.Service = GetData(objReader["MEAL_3_SEQ"]);
                    meals.Add(meal3);

                    response.Meals = meals;

                    response.Menuinfo = GetData(objReader["MENUINFO"]);
                    response.NfServiceProcess = GetData(objReader["NF_SERVICE_PROCESS"]);

                    responseListData.Add(response);
                }
            
            return responseListData;
        }
        public List<SummaryOfServiceEO> GetnfPremiumSOSByReader(ref IDataReader objReader, int cursorType)
        {
            List<SummaryOfServiceEO> responseListData = new List<SummaryOfServiceEO>();
            while (objReader.Read())
            {
                List<int> list = new List<int>();
                SummaryOfServiceEO response = new SummaryOfServiceEO();
                response.SOSType = SOSType.Premium;
                response.FlightNo = GetData(objReader["FLIGHT_NO"]);
                response.Sectorfrom = GetData(objReader["SECTORFROM"]);
                response.Sectorto = GetData(objReader["SECTORTO"]);
                response.Dep = GetData(objReader["DEP"]);
                response.Arr = GetData(objReader["ARR"]);
                response.Block = GetData(objReader["BLOCK"]);
                response.Earset = GetData(objReader["EARSET"]);

                response.Amenitykit = GetData(objReader["AMENITY"]);
                response.Menucard = GetData(objReader["MENUCARD"]);
                response.Comfortpouch = GetData(objReader["COMFORTPOUCH"]);
                response.Comfortbag = GetData(objReader["COMFORTBAG"]);
                List<BlanketEO> blankets = new List<BlanketEO>();
                BlanketEO blanketjc = new BlanketEO();
                blanketjc.Code = BlanketType.BlanketJC;
                blanketjc.Description = GetData(objReader["BLANKET_JC"]);
                blankets.Add(blanketjc);
                BlanketEO blanketfc = new BlanketEO();
                blanketfc.Code = BlanketType.BlanketFC;
                blanketfc.Description = GetData(objReader["BLANKET_FC"]);
                blankets.Add(blanketfc);
                response.Blankets = blankets;
                response.Documentpath = GetData(objReader["DOCUMENTPATH"]);
                response.Linkdescription = GetData(objReader["LINKDESCRIPTION"]);
                response.Menuinfo = GetData(objReader["MENUINFO"]);
                response.Utc = GetData(objReader["UTC"]);
                response.Uniquecol = GetData(objReader["UNIQUECOL"]);
                response.NgServiceProcess = GetData(objReader["NG_SERVICE_PROCESS"]);
                response.Otherinformation = GetData(objReader["OTHERINFORMATION"]);
                response.Otherinformation1 = GetData(objReader["OTHERINFORMATION1"]);

                List<MealEO> meals = new List<MealEO>();
                MealEO mealFc = new MealEO();
                mealFc.Type = MealType.MealFC;
                mealFc.Code = GetData(objReader["FC_MEAL"]);
                mealFc.MenuCardName = GetData(objReader["FC_MEAL_MENUCARDNAME"]);
                mealFc.MenuCardType = GetData(objReader["FC_MEAL_MENUCARDTYPE"]);
                mealFc.Service = GetData(objReader["FC_MEAL_SEQ"]);
                mealFc.FileId = GetData(objReader["FC_MEAL_MENUCARDID"]);
                mealFc.FileCode = "DOCUMENTS";
                meals.Add(mealFc);

                MealEO mealJC = new MealEO();
                mealJC.Type = MealType.MealJC;
                mealJC.Code = GetData(objReader["JC_MEAL"]);
                mealJC.MenuCardName = GetData(objReader["JC_MEAL_MENUCARDNAME"]);
                mealJC.MenuCardType = GetData(objReader["JC_MEAL_MENUCARDTYPE"]);
                mealJC.Service = GetData(objReader["JC_MEAL_SEQ"]);
                mealJC.FileId = GetData(objReader["JC_MEAL_MENUCARDID"]);
                mealJC.FileCode = "DOCUMENTS";

                meals.Add(mealJC);

                response.Meals = meals;
                response.AtoDrinks = GetData(objReader["ATO_DRINKS"]);
                responseListData.Add(response);
            }
            return responseListData;
        }
        public List<SummaryOfServiceEO> GetpiFlagSOSByReader(ref IDataReader objReader, int cursorType)
        {
            List<SummaryOfServiceEO> responseListData = new List<SummaryOfServiceEO>();
            while (objReader.Read())
            {
                List<int> list = new List<int>();
                SummaryOfServiceEO response = new SummaryOfServiceEO();
                response.SOSType = SOSType.Premium;
                response.FlightNo = GetData(objReader["FLIGHT_NO"]);
                response.Sectorfrom = GetData(objReader["SECTORFROM"]);
                response.Sectorto = GetData(objReader["SECTORTO"]);
                response.Utc = GetData(objReader["UTC"]);
                response.Dep = GetData(objReader["DEP"]);
                response.Arr = GetData(objReader["ARR"]);
                response.Block = GetData(objReader["BLOCK"]);

                response.Amenitykit = GetData(objReader["AMENITYKIT"]);
                response.Comfortpouch = GetData(objReader["COMFORTPOUCH"]);
                response.Comfortbag = GetData(objReader["COMFORTBAG"]);
                List<MealEO> meals = new List<MealEO>();
                MealEO Fc1 = new MealEO();
                Fc1.Type = MealType.MealFC;
                Fc1.Code = GetData(objReader["MEAL_FC_1MEAL"]);
                Fc1.MenuCardName = GetData(objReader["MEAL_1_MENUCARDNAME"]);
                Fc1.MenuCardType = GetData(objReader["MEAL_1_MENUCARDTYPE"]);
                Fc1.Service = GetData(objReader["FC_1MEAL_SEQ"]);
                Fc1.FileId = GetData(objReader["MEAL_1_MENUCARDID"]);
                Fc1.FileCode = "DOCUMENTS";


                meals.Add(Fc1);

                MealEO Fc2 = new MealEO();
                Fc2.Type = MealType.MealFC;
                Fc2.Code = GetData(objReader["MEAL_FC_2MEAL"]);
                Fc2.MenuCardName = GetData(objReader["MEAL_2_MENUCARDNAME"]);
                Fc2.MenuCardType = GetData(objReader["MEAL_2_MENUCARDTYPE"]);
                Fc2.FileId = GetData(objReader["MEAL_2_MENUCARDID"]);
                Fc2.FileCode = "DOCUMENTS";
                Fc2.Service = GetData(objReader["FC_2MEAL_SEQ"]);
                meals.Add(Fc2);

                MealEO jc1 = new MealEO();
                jc1.Type = MealType.MealJC;
                jc1.Code = GetData(objReader["MEAL_JC_1MEAL"]);
                jc1.MenuCardName = GetData(objReader["MEAL_3_MENUCARDNAME"]);
                jc1.MenuCardType = GetData(objReader["MEAL_3_MENUCARDTYPE"]);

                jc1.FileId = GetData(objReader["MEAL_3_MENUCARDID"]);
                jc1.FileCode = "DOCUMENTS";


                meals.Add(jc1);

                MealEO jc2 = new MealEO();
                jc2.Type = MealType.MealJC;
                jc2.Code = GetData(objReader["MEAL_JC_2MEAL"]);
                jc2.MenuCardName = GetData(objReader["MEAL_4_MENUCARDNAME"]);
                jc2.MenuCardType = GetData(objReader["MEAL_4_MENUCARDTYPE"]);
                jc2.FileId = GetData(objReader["MEAL_4_MENUCARDID"]);
                jc2.FileCode = "DOCUMENTS";

                meals.Add(jc2);

                response.Meals = meals;

                response.AtoDrinks = GetData(objReader["ATO_DRINKS"]);
                response.Otherinformation = GetData(objReader["OTHERINFORMATION"]);
                response.Documentpath = GetData(objReader["DOCUMENTPATH"]);
                response.Linkdescription = GetData(objReader["LINKDESCRIPTION"]);
                response.Otherinformation1 = GetData(objReader["OTHERINFORMATION1"]);
                response.Earset = GetData(objReader["EARSET"]);
                response.Menucard = GetData(objReader["MENUCARD"]);

                List<BlanketEO> blankets = new List<BlanketEO>();
                BlanketEO blanketjc = new BlanketEO();
                blanketjc.Code = BlanketType.BlanketFC;
                blanketjc.Description = GetData(objReader["BLANKET_JC"]);
                blankets.Add(blanketjc);
                BlanketEO blanketfc = new BlanketEO();
                blanketfc.Code = BlanketType.BlanketJC;
                blanketfc.Description = GetData(objReader["BLANKET_FC"]);
                blankets.Add(blanketfc);
                response.Blankets = blankets;

                responseListData.Add(response);


            }
            return responseListData;
        }
        public List<SummaryOfServiceEO> GetngFlagSOSByReader(ref IDataReader objReader, int cursorType)
        {
            List<SummaryOfServiceEO> responseListData = new List<SummaryOfServiceEO>();
            while (objReader.Read())
            {
                List<int> list = new List<int>();
                SummaryOfServiceEO response = new SummaryOfServiceEO();
                response.FlightNo = GetData(objReader["FLIGHT_NO"]);
                response.Sectorfrom = GetData(objReader["SECTORFROM"]);
                response.Sectorto = GetData(objReader["SECTORTO"]);
                response.Dep = GetData(objReader["DEP"]);
                response.Arr = GetData(objReader["ARR"]);
                response.Block = GetData(objReader["BLOCK"]);
                response.Amenity = GetData(objReader["AMENITY"]);
                response.Earset = GetData(objReader["EARSET"]);

                response.Menucard = GetData(objReader["MENUCARD"]);
                response.Comfortpouch = GetData(objReader["COMFORTPOUCH"]);
                response.Comfortbag = GetData(objReader["COMFORTBAG"]);

                List<BlanketEO> blankets = new List<BlanketEO>();
                BlanketEO blanketjc = new BlanketEO();
                blanketjc.Code = BlanketType.BlanketFC;
                blanketjc.Description = GetData(objReader["BLANKET_JC"]);
                blankets.Add(blanketjc);
                BlanketEO blanketfc = new BlanketEO();
                blanketfc.Code = BlanketType.BlanketJC;
                blanketfc.Description = GetData(objReader["BLANKET_FC"]);
                blankets.Add(blanketfc);
                response.Blankets = blankets;


                List<MealEO> meals = new List<MealEO>();
                MealEO Fc1 = new MealEO();
                Fc1.Type = MealType.MealFC;
                Fc1.Code = GetData(objReader["FC1_MEAL"]);
                Fc1.MenuCardName = GetData(objReader["FC1_MENUCARDNAME"]);
                Fc1.MenuCardType = GetData(objReader["FC1_MENUCARDTYPE"]);

                Fc1.Service = GetData(objReader["FC1_MEAL_SEQ"]);
                Fc1.FileId = GetData(objReader["FC1_MENUCARDID"]);
                Fc1.FileCode = "DOCUMENTS";

                meals.Add(Fc1);

                MealEO Fc2 = new MealEO();
                Fc2.Type = MealType.MealFC;
                Fc2.Code = GetData(objReader["FC2_MEAL"]);
                Fc2.MenuCardName = GetData(objReader["FC2_MENUCARDNAME"]);
                Fc2.MenuCardType = GetData(objReader["FC2_MENUCARDTYPE"]);

                Fc2.Service = GetData(objReader["FC2_MEAL_SEQ"]);
                Fc2.FileId = GetData(objReader["FC2_MENUCARDID"]);
                Fc2.FileCode = "DOCUMENTS";

                meals.Add(Fc2);

                MealEO jc1 = new MealEO();
                jc1.Type = MealType.MealJC;
                jc1.Code = GetData(objReader["JC1_MEAL"]);
                jc1.MenuCardName = GetData(objReader["JC1_MENUCARDNAME"]);
                jc1.MenuCardType = GetData(objReader["JC1_MENUCARDTYPE"]);

                jc1.Service = GetData(objReader["JC1_MEAL_SEQ"]);
                jc1.FileId = GetData(objReader["JC1_MENUCARDID"]);
                jc1.FileCode = "DOCUMENTS";

                meals.Add(jc1);

                MealEO jc2 = new MealEO();
                Fc2.Type = MealType.MealJC;
                jc2.Code = GetData(objReader["JC2_MEAL"]);
                jc2.MenuCardName = GetData(objReader["JC2_MENUCARDNAME"]);
                jc2.MenuCardType = GetData(objReader["JC2_MENUCARDTYPE"]);

                jc2.Service = GetData(objReader["JC2_MEAL_SEQ"]);
                jc2.FileId = GetData(objReader["JC2_MENUCARDID"]);
                jc2.FileCode = "DOCUMENTS";

                meals.Add(jc2);

                response.Meals = meals;

                response.AtoDrinks = GetData(objReader["ATO_DRINKS"]);
                response.Otherinformation = GetData(objReader["OTHERINFORMATION"]);
                response.Documentpath = GetData(objReader["DOCUMENTPATH"]);
                response.Linkdescription = GetData(objReader["LINKDESCRIPTION"]);
                response.Utc = GetData(objReader["UTC"]);
                response.Otherinformation1 = GetData(objReader["OTHERINFORMATION1"]);

                response.FlightNo = GetData(objReader["FLIGHT_NO"]);

                response.SOSType = SOSType.Premium;

                responseListData.Add(response);


            }
            return responseListData;
        }
        public String GetData(object column)
        {
            string data = string.Empty;
            data = column != null && !string.IsNullOrEmpty(column.ToString()) ? column.ToString() : string.Empty;
            return data;
        }

    }
}
