
using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.DataAccess.Implementation;
using QR.IPrism.Enterprise;
using QR.IPrism.Utility;
using Oracle.DataAccess.Client;

namespace QR.IPrism.BusinessObjects.Implementation
{
    public class SearchDao : ISearchDao
    {
        public async Task<List<FlightInfoEO>> GetFlightInfosAsyc(CommonFilterEO filterInput)
        {

            List<FlightInfoEO> responseList = new List<FlightInfoEO>();
            FlightInfoEO response = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    //perametersList.Add(new ODPCommandParameter("pi_schedeptdate_to", filterInput.FromDate, ParameterDirection.Input, OracleDbType.Varchar2));

                    //New Change: As current Stored procedure was taking data from today to last 6 monhts and next 2 months , so the Dates were commented, 
                    // but now we are uncommenting it as the business has decided to send date time from UI.
                    perametersList.Add(new ODPCommandParameter("pi_schedeptdate", Common.ToOracleDate(filterInput.FromDate), ParameterDirection.Input, OracleDbType.Date));


                    perametersList.Add(new ODPCommandParameter("pi_sector_from", filterInput.FromSector, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_sector_to", filterInput.ToSector, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_flight_list", ParameterDirection.Output, OracleDbType.RefCursor));

                    //prism_mig_dashboard.get_flight_list_1_proc
                    using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_dashboard.get_sector_flights_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new FlightInfoEO();

                            response.FlightNumber = objReader["FLIGHTNUMBER"] != null && objReader["FLIGHTNUMBER"].ToString() != string.Empty ? (System.String)(objReader["FLIGHTNUMBER"]) : default(System.String);
                            response.Sector = objReader["SECTOR"] != null && objReader["SECTOR"].ToString() != string.Empty ? (System.String)(objReader["SECTOR"]) : default(System.String);

                            response.SectorFrom = objReader["SECTORFROM"] != null && objReader["SECTORFROM"].ToString() != string.Empty ? (System.String)(objReader["SECTORFROM"]) : default(System.String);
                            response.SectorTo = objReader["SECTORTO"] != null && objReader["SECTORTO"].ToString() != string.Empty ? (System.String)(objReader["SECTORTO"]) : default(System.String);
                            //response.AirCraftRegNo = objReader["AIRCRAFTREGNO"] != null && objReader["AIRCRAFTREGNO"].ToString() != string.Empty ? (System.String)(objReader["AIRCRAFTREGNO"]) : default(System.String);
                            //response.AirCraftType = objReader["AIRCRAFTTYPE"] != null && objReader["AIRCRAFTTYPE"].ToString() != string.Empty ? (System.String)(objReader["AIRCRAFTTYPE"]) : default(System.String);
                            //response.ScheduledDeptTime = objReader["SCHEDEPTDATE"] != null && objReader["SCHEDEPTDATE"].ToString() != string.Empty ? (System.DateTime)(objReader["SCHEDEPTDATE"]) : default(System.DateTime);
                            //response.StandardTimeDepartureUtc = objReader["STD_UTC"] != null && objReader["STD_UTC"].ToString() != string.Empty ? (System.DateTime)(objReader["STD_UTC"]) : default(System.DateTime);
                            //response.StandardTimeArrivalUtc = objReader["STA_UTC"] != null && objReader["STA_UTC"].ToString() != string.Empty ? (System.DateTime)(objReader["STA_UTC"]) : default(System.DateTime);
                            ////response.ReportingTime = objReader["air_craft_reg_no"] != null && objReader["air_craft_reg_no"].ToString() != string.Empty ? (System.DateTime)(objReader["air_craft_reg_no"]) : default(System.DateTime);
                            //response.Route = objReader["ROUTE"] != null && objReader["ROUTE"].ToString() != string.Empty ? (System.String)(objReader["ROUTE"]) : default(System.String);

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
                    dbframework.CloseConnection();
                }
            }
            return responseList;
        }
        public async Task<List<CurrencyDetailEO>> GetCurrencyDetailsAsyc(CommonFilterEO filterInput)
        {

            List<CurrencyDetailEO> responseList = new List<CurrencyDetailEO>();
            CurrencyDetailEO response = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_curr_code", filterInput.CurrencyCode, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_airport_code", filterInput.City, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_city", "", ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_country", filterInput.Country, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_currency", ParameterDirection.Output, OracleDbType.RefCursor));


                    using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_search.get_currency_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new CurrencyDetailEO();

                            //response.City = objReader["city"] != null && objReader["city"].ToString() != string.Empty ? (System.String)(objReader["city"]) : default(System.String);
                            //response.Country = objReader["country"] != null && objReader["country"].ToString() != string.Empty ? (System.String)(objReader["country"]) : default(System.String);
                            response.AirportCode = objReader["AIRPORT_CODE"] != null && objReader["AIRPORT_CODE"].ToString() != string.Empty ? (System.String)(objReader["AIRPORT_CODE"]) : default(System.String);
                            response.FromCurrency = objReader["FROM_CURR"] != null && objReader["FROM_CURR"].ToString() != string.Empty ? (System.String)(objReader["FROM_CURR"]) : default(System.String);
                            response.ToCurrency = objReader["TO_CURR"] != null && objReader["TO_CURR"].ToString() != string.Empty ? (System.String)(objReader["TO_CURR"]) : default(System.String);
                            response.ConversionRate = objReader["conversion_rate"] != null && objReader["conversion_rate"].ToString() != string.Empty ? (System.Decimal)(objReader["conversion_rate"]) : default(System.Decimal);
                            response.InvConversionRate = objReader["inv_conversion_rate"] != null && objReader["inv_conversion_rate"].ToString() != string.Empty ? (System.Decimal)(objReader["inv_conversion_rate"]) : default(System.Decimal);

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
                    dbframework.CloseConnection();
                }
            }
            return responseList;
        }
        public async Task<List<TrainingTransportEO>> GetTrainingTransportsAsyc(CommonFilterEO filterInput)
        {

            List<TrainingTransportEO> responseList = new List<TrainingTransportEO>();
            TrainingTransportEO response = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    if (filterInput.IsTraining)
                    {
                        perametersList = new List<ODPCommandParameter>();
                        perametersList.Add(new ODPCommandParameter("PI_TRAINCODE", filterInput.TrainCode, ParameterDirection.Input, OracleDbType.Varchar2));
                        perametersList.Add(new ODPCommandParameter("PI_TRAINDATE", filterInput.TrainDate, ParameterDirection.Input, OracleDbType.Varchar2));
                        perametersList.Add(new ODPCommandParameter("pi_staffno", filterInput.StaffID, ParameterDirection.Input, OracleDbType.Varchar2));
                        perametersList.Add(new ODPCommandParameter("po_transport", ParameterDirection.Output, OracleDbType.RefCursor));

                        using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PRISM_IPA_DASHBOARD.GET_TRAINING_TRANSPORT_PROC", perametersList))
                        {
                            while (objReader.Read())
                            {
                                response = new TrainingTransportEO();

                                response.StaffNo = objReader["STAFFNO"] != null && objReader["STAFFNO"].ToString() != string.Empty ? (System.String)(objReader["STAFFNO"]) : default(System.String);
                                response.TripId = objReader["trip_id"] != null && objReader["trip_id"].ToString() != string.Empty ? Convert.ToString(objReader["trip_id"]) : default(System.String);
                                response.PickupLocation = objReader["pickup_location"] != null && objReader["pickup_location"].ToString() != string.Empty ? (System.String)(objReader["pickup_location"]) : default(System.String);
                                response.PickupTime = objReader["pickup_time"] != null && objReader["pickup_time"].ToString() != string.Empty ? (System.DateTime)(objReader["pickup_time"]) : default(System.DateTime);
                                response.DutyCode = objReader["duty_code"] != null && objReader["duty_code"].ToString() != string.Empty ? (System.String)(objReader["duty_code"]) : default(System.String);
                                response.DutyType = objReader["duty_type"] != null && objReader["duty_type"].ToString() != string.Empty ? (System.String)(objReader["duty_type"]) : default(System.String);
                                response.ReportingTime = objReader["reporting_time"] != null && objReader["reporting_time"].ToString() != string.Empty ? (System.DateTime)(objReader["reporting_time"]) : default(System.DateTime);

                                responseList.Add(response);
                            }
                        }
                    }
                    else
                    {
                        perametersList = new List<ODPCommandParameter>();
                        perametersList.Add(new ODPCommandParameter("pi_staffno", filterInput.StaffID, ParameterDirection.Input, OracleDbType.Varchar2));


                        perametersList.Add(new ODPCommandParameter("pi_date_from", Common.ToOracleDate(Convert.ToDateTime(filterInput.FromDate)), ParameterDirection.Input, OracleDbType.Date));
                        perametersList.Add(new ODPCommandParameter("pi_date_to", Common.ToOracleDate(Convert.ToDateTime(filterInput.ToDate)), ParameterDirection.Input, OracleDbType.Date));
                        perametersList.Add(new ODPCommandParameter("po_transport", ParameterDirection.Output, OracleDbType.RefCursor));

                        using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_search.get_transport_proc", perametersList))
                        {
                            while (objReader.Read())
                            {
                                response = new TrainingTransportEO();

                                response.FlightNumber = objReader["Flight_Number"] != null && objReader["Flight_Number"].ToString() != string.Empty ? (System.String)(objReader["Flight_Number"]) : String.Empty;
                               response.StaffNo = objReader["STAFFNO"] != null && objReader["STAFFNO"].ToString() != string.Empty ? (System.String)(objReader["STAFFNO"]) : default(System.String);
                                response.TripId = objReader["trip_id"] != null && objReader["trip_id"].ToString() != string.Empty ? Convert.ToString(objReader["trip_id"]) : default(System.String);
                                response.PickupLocation = objReader["pickup_location"] != null && objReader["pickup_location"].ToString() != string.Empty ? (System.String)(objReader["pickup_location"]) : default(System.String);
                                response.PickupTime = objReader["pickup_time"] != null && objReader["pickup_time"].ToString() != string.Empty ? (System.DateTime)(objReader["pickup_time"]) : default(System.DateTime);
                                //response.DutyCode = objReader["duty_code"] != null && objReader["duty_code"].ToString() != string.Empty ? (System.String)(objReader["duty_code"]) : default(System.String);
                                response.DutyType = objReader["duty_type"] != null && objReader["duty_type"].ToString() != string.Empty ? (System.String)(objReader["duty_type"]) : default(System.String);
                                response.ReportingTime = objReader["reporting_time"] != null && objReader["reporting_time"].ToString() != string.Empty ? (System.DateTime)(objReader["reporting_time"]) : default(System.DateTime);

                                responseList.Add(response);
                            }
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
            return responseList;
        }
        public async Task<List<WeatherInfoEO>> GetWeatherInfosAsyc(CommonFilterEO filterInput)
        {

            List<WeatherInfoEO> responseList = new List<WeatherInfoEO>();
            WeatherInfoEO response = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_airport_code", filterInput.City, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_city", "", ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_country", filterInput.Country, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_date_from", Common.ToOracleDate(filterInput.FromDate), ParameterDirection.Input, OracleDbType.Date));
                    perametersList.Add(new ODPCommandParameter("pi_date_to", Common.ToOracleDate(filterInput.ToDate), ParameterDirection.Input, OracleDbType.Date));
                    perametersList.Add(new ODPCommandParameter("po_weather", ParameterDirection.Output, OracleDbType.RefCursor));

                    using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_search.get_weather_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new WeatherInfoEO();

                            response.WeatherDate = objReader["WEATHERDATE"] != null && objReader["WEATHERDATE"].ToString() != string.Empty ? (System.DateTime)(objReader["WEATHERDATE"]) : default(System.DateTime);
                            response.AirportCode = objReader["airport_code"] != null && objReader["airport_code"].ToString() != string.Empty ? (System.String)(objReader["airport_code"]) : default(System.String);
                            response.City = objReader["city"] != null && objReader["city"].ToString() != string.Empty ? (System.String)(objReader["city"]) : default(System.String);
                            response.TempLow = objReader["temp_low"] != null && objReader["temp_low"].ToString() != string.Empty ? (System.String)(objReader["temp_low"]) : default(System.String);
                            response.TempHigh = objReader["temp_high"] != null && objReader["temp_high"].ToString() != string.Empty ? (System.String)(objReader["temp_high"]) : default(System.String);
                            //response.LocationCode = objReader["location_code"] != null && objReader["location_code"].ToString() != string.Empty ?(System.String)(objReader["location_code"]) : default(System.String) ;								
                            response.WeekDay = objReader["week_day"] != null && objReader["week_day"].ToString() != string.Empty ? (System.String)(objReader["week_day"]) : default(System.String);

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
                    dbframework.CloseConnection();
                }
            }
            return responseList;
        }

        public async Task<List<CrewLocatorEO>> GetCrewLocatorsAsyc()
        {

            List<CrewLocatorEO> responseList = new List<CrewLocatorEO>();
            CrewLocatorEO response = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("po_loc_crew", ParameterDirection.Output, OracleDbType.RefCursor));

                    using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PRISM_CREWLOCATION.GET_LOCATION_PROC", perametersList))
                    {
                        while (objReader.Read())
                        {
                            if ((objReader["LATITUDE"] != null && objReader["LATITUDE"].ToString() != string.Empty) && (objReader["LONGITUDE"] != null && objReader["LONGITUDE"].ToString() != string.Empty))
                            {
                                response = new CrewLocatorEO();

                                response.LocationCode = objReader["LOC"] != null && objReader["LOC"].ToString() != string.Empty ? (System.String)(objReader["LOC"]) : default(System.String); objReader["LOC"].ToString();
                                response.CityName = objReader["CITY_NAME"] != null && objReader["CITY_NAME"].ToString() != string.Empty ? (System.String)(objReader["CITY_NAME"]) : default(System.String); objReader["CITY_NAME"].ToString();
                                response.AirportName = objReader["AIRPORT_NAME"] != null && objReader["AIRPORT_NAME"].ToString() != string.Empty ? (System.String)(objReader["AIRPORT_NAME"]) : default(System.String); objReader["AIRPORT_NAME"].ToString();
                                response.CountryName = objReader["COUNTRY_NAME"] != null && objReader["COUNTRY_NAME"].ToString() != string.Empty ? (System.String)(objReader["COUNTRY_NAME"]) : default(System.String); objReader["COUNTRY_NAME"].ToString();
                                response.Latitude = objReader["LATITUDE"] != null && objReader["LATITUDE"].ToString() != string.Empty ? Convert.ToString(objReader["LATITUDE"]) : default(System.String); objReader["LATITUDE"].ToString();
                                response.Longitude = objReader["LONGITUDE"] != null && objReader["LONGITUDE"].ToString() != string.Empty ? Convert.ToString(objReader["LONGITUDE"]) : default(System.String); objReader["LONGITUDE"].ToString();
                                response.StationCount = objReader["STATION_CREW_COUNT"] != null && objReader["STATION_CREW_COUNT"].ToString() != string.Empty ? Convert.ToInt32(objReader["STATION_CREW_COUNT"].ToString()) : default(System.Int32);
                                response.FlightCount = objReader["FLIGHT_CREW_COUNT"] != null && objReader["FLIGHT_CREW_COUNT"].ToString() != string.Empty ? Convert.ToInt32(objReader["FLIGHT_CREW_COUNT"].ToString()) : default(System.Int32);
                                response.TotalCount = objReader["LOC_CREW_COUNT"] != null && objReader["LOC_CREW_COUNT"].ToString() != string.Empty ? Convert.ToInt32(objReader["LOC_CREW_COUNT"].ToString()) : default(System.Int32);

                                if (response.FlightCount > 0 && response.StationCount > 0)
                                {
                                    response.CssClass = "purple";
                                }
                                else
                                {
                                    if (response.StationCount > 0)
                                    {
                                        response.CssClass = "red";
                                    }
                                    else
                                    {
                                        response.CssClass = "green";
                                    }
                                }

                                responseList.Add(response);
                            }
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
            return responseList;
        }

        public async Task<String> RunCrewLocatorProcess()
        {

            String responseList = "";

            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("po_loc_crew", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_crewlocation_dl.add_crewlocation_data_proc", perametersList))
                    {
                        while (objReader.Read())
                        {

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
            return responseList;
        }

        public async Task<List<DutySummaryEO>> GetDutySummarysAsyc(CommonFilterEO filterInput)
        {

            List<DutySummaryEO> responseList = new List<DutySummaryEO>();
            DutySummaryEO response = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("po_duty_summary", ParameterDirection.Output, OracleDbType.RefCursor));
                    
                    using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_crewlocation.get_duty_summary_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new DutySummaryEO();
                            response.DutyGroup = objReader["duty_group"] != null && objReader["duty_group"].ToString() != string.Empty ? (System.String)(objReader["duty_group"]) : default(System.String);
                            response.CrewCount = objReader["duty_crew_count"] != null && objReader["duty_crew_count"].ToString() != string.Empty ? Convert.ToInt32(objReader["duty_crew_count"]) : default(System.Int32);


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
                    dbframework.CloseConnection();
                }
            }
            return responseList;
        }

        public async Task<List<LocationCrewDetailEO>> GetLocationCrewDetailsAsyc(CommonFilterEO filterInput)
        {

            List<LocationCrewDetailEO> responseList = new List<LocationCrewDetailEO>();
            LocationCrewDetailEO response = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_airport_code", filterInput.AirportCode, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_city_name", filterInput.City, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_country_name", filterInput.Country, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_flight_no", filterInput.FlightNo, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_staffno", filterInput.StaffID, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_grade", filterInput.Grade, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_loc_indicator", filterInput.Location, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_crew_details", ParameterDirection.Output, OracleDbType.RefCursor));
                   
                    using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_crewlocation.get_location_crew_details_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new LocationCrewDetailEO();

                            response.Location = objReader["LOC"] != null && objReader["LOC"].ToString() != string.Empty ? (System.String)(objReader["LOC"]) : default(System.String);
                            response.Indicator = objReader["LOC_INDICATOR"] != null && objReader["LOC_INDICATOR"].ToString() != string.Empty ? (System.String)(objReader["LOC_INDICATOR"]) : default(System.String);
                            response.StaffNo = objReader["STAFFNO"] != null && objReader["STAFFNO"].ToString() != string.Empty ? (System.String)(objReader["STAFFNO"]) : default(System.String);
                            response.DutyCode = objReader["DUTY_CODE"] != null && objReader["DUTY_CODE"].ToString() != string.Empty ? (System.String)(objReader["DUTY_CODE"]) : default(System.String);
                            response.DutyGroup = objReader["DUTY_GROUP"] != null && objReader["DUTY_GROUP"].ToString() != string.Empty ? (System.String)(objReader["DUTY_GROUP"]) : default(System.String);
                            response.FlightNo = objReader["FLIGHT_NO"] != null && objReader["FLIGHT_NO"].ToString() != string.Empty ? (System.String)(objReader["FLIGHT_NO"]) : default(System.String);
                            response.FromSector = objReader["FLIGHT_SECTORFROM"] != null && objReader["FLIGHT_SECTORFROM"].ToString() != string.Empty ? (System.String)(objReader["FLIGHT_SECTORFROM"]) : default(System.String);
                            response.ToSector = objReader["FLIGHT_SECTORTO"] != null && objReader["FLIGHT_SECTORTO"].ToString() != string.Empty ? (System.String)(objReader["FLIGHT_SECTORTO"]) : default(System.String);
                            response.DepartureDate = objReader["FLIGHT_DEP_DATE"] != null && objReader["FLIGHT_DEP_DATE"].ToString() != string.Empty ? (DateTime?)(objReader["FLIGHT_DEP_DATE"]) : null;
                            response.ArrivalDate = objReader["FLIGHT_ARR_DATE"] != null && objReader["FLIGHT_ARR_DATE"].ToString() != string.Empty ? (DateTime?)(objReader["FLIGHT_ARR_DATE"]) : null;
                            response.RosterInfo = objReader["ROSTER_IND"] != null && objReader["ROSTER_IND"].ToString() != string.Empty ? (System.String)(objReader["ROSTER_IND"]) : default(System.String);


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
                    dbframework.CloseConnection();
                }
            }
            return responseList;
        }

        public async Task<List<LocationFlightEO>> GetLocationFlightsAsyc(CommonFilterEO filterInput)
        {

            List<LocationFlightEO> responseList = new List<LocationFlightEO>();
            LocationFlightEO response = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_loc", filterInput.AirportCode, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_loc_flights", ParameterDirection.Output, OracleDbType.RefCursor));
                   
                    using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_crewlocation.get_location_flight_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new LocationFlightEO();

                            response.Location = objReader["LOC"] != null && objReader["LOC"].ToString() != string.Empty ? (System.String)(objReader["LOC"]) : default(System.String);
                            response.FlightNo = objReader["FLIGHT_NO"] != null && objReader["FLIGHT_NO"].ToString() != string.Empty ? (System.String)(objReader["FLIGHT_NO"]) : default(System.String);
                            response.Sectors = objReader["SECTOR"] != null && objReader["sectors"].ToString() != string.Empty ? (System.String)(objReader["SECTOR"]) : default(System.String);
                            response.DepartureDate = objReader["FLIGHT_DEP_DATE"] != null && objReader["FLIGHT_DEP_DATE"].ToString() != string.Empty ? (System.DateTime)(objReader["FLIGHT_DEP_DATE"]) : default(System.DateTime);
                            response.ArrivalDate = objReader["FLIGHT_ARR_DATE"] != null && objReader["FLIGHT_ARR_DATE"].ToString() != string.Empty ? (System.DateTime)(objReader["FLIGHT_ARR_DATE"]) : default(System.DateTime);
                            response.FlightCrewCount = objReader["FLIGHT_CREW_COUNT"] != null && objReader["FLIGHT_CREW_COUNT"].ToString() != string.Empty ? (System.Int32)(objReader["FLIGHT_CREW_COUNT"]) : default(System.Int32);

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
                    dbframework.CloseConnection();
                }
            }
            return responseList;
        }

        public async Task<List<AssessmentSearchEO>> GetAutoSuggestStaffInfo(string filterInput)
        {

            List<AssessmentSearchEO> responseList = new List<AssessmentSearchEO>();
            AssessmentSearchEO response = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("p_searchcriteria", filterInput, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("cur_crew", ParameterDirection.Output, OracleDbType.RefCursor));

                    using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("comn_crewsearch_pkg.search_crew", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new AssessmentSearchEO();

                            response.StaffNumber = objReader["staffno"] != null && objReader["staffno"].ToString() != string.Empty ? (System.String)(objReader["staffno"]) : default(System.String);
                            response.FirstName = objReader["firstname"] != null && objReader["firstname"].ToString() != string.Empty ? (System.String)(objReader["firstname"]) : default(System.String);
                            response.LastName = objReader["lastname"] != null && objReader["lastname"].ToString() != string.Empty ? (System.String)(objReader["lastname"]) : default(System.String);
                            response.Grade = objReader["grade"] != null && objReader["grade"].ToString() != string.Empty ? (System.String)(objReader["grade"]) : default(System.String);

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
                    dbframework.CloseConnection();
                }
            }
            return responseList;
        }
    }
}

