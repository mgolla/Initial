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
using QR.IPrism.EntityObjects.Shared;
using Oracle.DataAccess.Client;

namespace QR.IPrism.BusinessObjects.Implementation
{
    public class RosterDao : IRosterDao
    {
        int tempID = 1;
        public async Task<RosterViewModelEO> GetRostersAsyc(RosterFilterEO filterInput)
        {
            //return SampleDataDao.GetSampleRostersAsyc(filterInput);
            RosterViewModelEO rosterViewModelEO = new RosterViewModelEO();
            rosterViewModelEO.IsWorking = false;
            rosterViewModelEO.IsDataLoaded = IsDataLoaded.No;
            rosterViewModelEO.Rosters = new List<RosterEO>();
            RosterEO response = null;
            try
            {

                ODPDataAccess dbframework = new ODPDataAccess(Constants.AIMS_CONNECTION_STR);
                {

                    List<ODPCommandParameter> perametersList = null;
                    try
                    {
                        perametersList = new List<ODPCommandParameter>();
                        perametersList.Add(new ODPCommandParameter("CREWID", filterInput.StaffID, ParameterDirection.Input, OracleDbType.Int32));
                        perametersList.Add(new ODPCommandParameter("START_DATE", Common.ToOracleDate(filterInput.StartDate), ParameterDirection.Input, OracleDbType.Date));
                        perametersList.Add(new ODPCommandParameter("END_DATE", Common.ToOracleDate(filterInput.EndDate), ParameterDirection.Input, OracleDbType.Date));
                        perametersList.Add(new ODPCommandParameter("P_CREW", ParameterDirection.Output, OracleDbType.RefCursor));


                        using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("p_get_roster_detail_cc_iprism", perametersList))
                        {
                            while (objReader.Read())
                            {
                                response = new RosterEO();
                                response.TempID = tempID++;
                                response.StaffID = objReader["ID"] != null && objReader["ID"].ToString() != string.Empty ? objReader["ID"].ToString() : string.Empty;
                                response.StaffName = objReader["NAME"] != null && objReader["NAME"].ToString() != string.Empty ? objReader["NAME"].ToString() : string.Empty;
                                response.Cc = objReader["CC"] != null && objReader["CC"].ToString() != string.Empty ? objReader["CC"].ToString() : string.Empty;
                                response.Flight = objReader["FLT"] != null && objReader["FLT"].ToString() != string.Empty ? objReader["FLT"].ToString() : string.Empty;
                                if (filterInput.FlightDigits > 0)
                                {
                                    response.FlightDisplay = response.Flight.PadLeft(filterInput.FlightDigits, '0');

                                }
                                else
                                {
                                    response.FlightDisplay = response.Flight.PadLeft(3, '0');
                                }



                                if (filterInput.TimeFormat.Equals(TimeFormat.UTCTime) && objReader["STD_UTC"] != null)
                                {
                                    response.DutyDate = objReader["STD_UTC"] != null && objReader["STD_UTC"].ToString() != string.Empty ? Common.DateFormateddMMMyyyyWithSpace(Convert.ToDateTime(objReader["STD_UTC"])) : string.Empty;
                                    response.DutyDateActualDate = objReader["STD_UTC"] != null && objReader["STD_UTC"].ToString() != string.Empty ? Common.OracleDateFormate(Convert.ToDateTime(objReader["STD_UTC"])) : string.Empty;

                                }
                                else if (filterInput.TimeFormat.Equals(TimeFormat.DOHATime) && objReader["STD_UTC"] != null)
                                {
                                    response.DutyDate = objReader["STD_UTC"] != null && objReader["STD_UTC"].ToString() != string.Empty ? Common.DateFormateddMMMyyyyWithSpace(Convert.ToDateTime(objReader["STD_UTC"]).AddHours(Constants.DohaTimeOffset)) : string.Empty;
                                    response.DutyDateActualDate = objReader["STD_UTC"] != null && objReader["STD_UTC"].ToString() != string.Empty ? Common.OracleDateFormate(Convert.ToDateTime(objReader["STD_UTC"]).AddHours(Constants.DohaTimeOffset)) : string.Empty;
                                }

                                else if (filterInput.TimeFormat.Equals(TimeFormat.LocalTime) && objReader["STD_LOCAL"] != null)
                                {
                                    response.DutyDate = objReader["STD_LOCAL"] != null && objReader["STD_LOCAL"].ToString() != string.Empty ? Common.DateFormateddMMMyyyyWithSpace(Convert.ToDateTime(objReader["STD_LOCAL"])) : string.Empty;
                                    //response.DutyDateActualDate = objReader["STD_LOCAL"] != null && objReader["STD_LOCAL"].ToString() != string.Empty ? Convert.ToDateTime(objReader["STD_LOCAL"]) : default(DateTime);
                                    response.DutyDateActualDate = objReader["STD_LOCAL"] != null && objReader["STD_LOCAL"].ToString() != string.Empty ? Common.OracleDateFormate(Convert.ToDateTime(objReader["STD_LOCAL"])) : string.Empty;

                                }
                                else
                                {
                                    response.DutyDate = objReader["DUTYDATE"] != null && objReader["DUTYDATE"].ToString() != string.Empty ? Common.DateFormateddMMMyyyyWithSpace(Convert.ToDateTime(objReader["DUTYDATE"])) : string.Empty;
                                    response.DutyDateActualDate = objReader["DUTYDATE"] != null && objReader["DUTYDATE"].ToString() != string.Empty ? Common.OracleDateFormate(Convert.ToDateTime(objReader["DUTYDATE"])) : string.Empty;

                                }
                                response.DutyDateActual = objReader["DUTYDATE"] != null && objReader["DUTYDATE"].ToString() != string.Empty ? Common.OracleDateFormateddMMMyyyy(Convert.ToDateTime(objReader["DUTYDATE"])) : string.Empty;

                                response.DutyDateActualDateName = Convert.ToDateTime(response.DutyDateActual).ToString("ddd");

                                if (response.DutyDate.ToString().Trim() == string.Empty)
                                {
                                    if (response.DutyDateActual.ToString().Trim() != string.Empty)
                                    {
                                        response.DutyDate = Common.OracleDateFormateddMMMyyyy(Convert.ToDateTime(objReader["DUTYDATE"]));
                                    }
                                }

                                if (objReader["STD_UTC"] != null)
                                {
                                    response.DohaDate = objReader["STD_UTC"] != null && objReader["STD_UTC"].ToString() != string.Empty ? Common.DateFormateddMMMyyyyWithSpace(Convert.ToDateTime(objReader["STD_UTC"]).AddHours(Constants.DohaTimeOffset)) : string.Empty;
                                }
                                if (objReader["STD_LOCAL"] != null)
                                {
                                    response.LocalDate = objReader["STD_LOCAL"] != null && objReader["STD_LOCAL"].ToString() != string.Empty ? Common.DateFormateddMMMyyyyWithSpace(Convert.ToDateTime(objReader["STD_LOCAL"])) : string.Empty;

                                }

                                response.Departure = objReader["DEP"] != null && objReader["DEP"].ToString().Trim() != string.Empty ? objReader["DEP"].ToString().Trim() : string.Empty;
                                response.StandardTimeDeparture = objReader["STD"] != null && objReader["STD"].ToString() != string.Empty ? (DateTime?)Convert.ToDateTime(objReader["STD"]) : null;
                                response.Arrival = objReader["ARR"] != null && objReader["ARR"].ToString().Trim() != string.Empty ? objReader["ARR"].ToString().Trim() : string.Empty;



                                response.StandardTimeArrival = objReader["STA"] != null && objReader["STA"].ToString() != string.Empty ? (DateTime?)Convert.ToDateTime(objReader["STA"]) : null;
                                response.IataCoCode = objReader["IATACOCDE"] != null && objReader["IATACOCDE"].ToString() != string.Empty ? objReader["IATACOCDE"].ToString() : string.Empty;
                                response.Idicator = objReader["IDICATOR"] != null && objReader["IDICATOR"].ToString() != string.Empty ? objReader["IDICATOR"].ToString() : string.Empty;
                                response.AircraftType = objReader["AIRCRAFT_TYPE"] != null && objReader["AIRCRAFT_TYPE"].ToString() != string.Empty ? objReader["AIRCRAFT_TYPE"].ToString() : string.Empty;
                                response.Registration = objReader["REG"] != null && objReader["REG"].ToString() != string.Empty ? objReader["REG"].ToString() : string.Empty;

                                response.DutyType = objReader["DUTY_TYPE"] != null && objReader["DUTY_TYPE"].ToString() != string.Empty ? objReader["DUTY_TYPE"].ToString() : string.Empty;
                                response.NotFlyTogether = objReader["NOT_FLY_TOGETHER"] != null && objReader["NOT_FLY_TOGETHER"].ToString() != string.Empty ? objReader["NOT_FLY_TOGETHER"].ToString() : string.Empty;
                                response.Memo = objReader["MEMO"] != null && objReader["MEMO"].ToString() != string.Empty ? objReader["MEMO"].ToString() : string.Empty;

                                response.StandardTimeDepartureUtc = objReader["STD_UTC"] != null && objReader["STD_UTC"].ToString() != string.Empty ? Common.OracleDateFormateddMMMyyyyHHmm(Convert.ToDateTime(objReader["STD_UTC"])) : default(string);

                                response.StandardTimeDepartureLocal = objReader["STD_LOCAL"] != null && objReader["STD_LOCAL"].ToString() != string.Empty ? Common.OracleDateFormateddMMMyyyyHHmm(Convert.ToDateTime(objReader["STD_LOCAL"])) : default(string);
                                response.StandardTimeDepartureDoha = objReader["STD_UTC"] != null && objReader["STD_UTC"].ToString() != string.Empty ? Common.OracleDateFormateddMMMyyyyHHmm(Convert.ToDateTime(objReader["STD_UTC"]).AddHours(Constants.DohaTimeOffset)) : default(string);


                                response.StandardTimeArrivalUtc = objReader["STA_UTC"] != null && objReader["STA_UTC"].ToString() != string.Empty ? Common.OracleDateFormateddMMMyyyyHHmm(Convert.ToDateTime(objReader["STA_UTC"])) : default(string);

                                response.StandardTimeArrivalLocal = objReader["STA_LOCAL"] != null && objReader["STA_LOCAL"].ToString() != string.Empty ? Common.OracleDateFormateddMMMyyyyHHmm(Convert.ToDateTime(objReader["STA_LOCAL"])) : default(string);

                                response.StandardTimeArrivalDoha = objReader["STA_UTC"] != null && objReader["STA_UTC"].ToString() != string.Empty ? Common.OracleDateFormateddMMMyyyyHHmm(Convert.ToDateTime(objReader["STA_UTC"]).AddHours(Constants.DohaTimeOffset)) : default(string);



                                try
                                {
                                    response.Address1Training = objReader["TRN_ADDR1"] != null && objReader["TRN_ADDR1"].ToString() != string.Empty ? objReader["TRN_ADDR1"].ToString() : default(string);
                                    response.Address2Training = objReader["TRN_ADDR2"] != null && objReader["TRN_ADDR2"].ToString() != string.Empty ? objReader["TRN_ADDR2"].ToString() : default(string);
                                    response.Address3Training = objReader["TRN_ADDR3"] != null && objReader["TRN_ADDR3"].ToString() != string.Empty ? objReader["TRN_ADDR3"].ToString() : default(string);

                                }
                                catch (Exception ex)
                                {

                                }

                                response.BTime = objReader["BTIME"] != null && objReader["BTIME"].ToString() != string.Empty ? objReader["BTIME"].ToString() : string.Empty;


                                var val = await GetDateDiffInDays(Convert.ToDateTime(response.StandardTimeDepartureUtc), Convert.ToDateTime(response.StandardTimeArrivalUtc));
                                var diff = Convert.ToInt32(val);
                                if (diff > 0)
                                {

                                    var minutes = diff % 60;
                                    var hours = (diff - minutes) / 60;
                                    var final = "";
                                    if (diff > 60)
                                    {
                                        if (hours < 10)
                                        {
                                            final = "0";
                                        }

                                        final = final + hours.ToString();


                                        if (minutes > 0)
                                        {
                                            if (minutes < 10)
                                            {
                                                final = final + ":0" + minutes;
                                            }
                                            else
                                            {
                                                final = final + ":" + minutes;
                                            }


                                        }
                                        final = final + " hrs ";
                                    }
                                    else
                                    {
                                        if (hours < 10)
                                        {
                                            final = "0";
                                        }

                                        final = final + hours.ToString();
                                        if (minutes > 0)
                                        {
                                            if (minutes < 10)
                                            {
                                                final = final + ":0" + minutes;
                                            }
                                            else
                                            {
                                                final = final + ":" + minutes;
                                            }
                                        }
                                        final = final + " hr";
                                    }

                                    response.FlightTravellingTime = final;
                                }
                                rosterViewModelEO.IsDataLoaded = IsDataLoaded.Yes;
                                rosterViewModelEO.Rosters.Add(response);
                            }
                        }
                        rosterViewModelEO.IsWorking = true;
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                        rosterViewModelEO.IsWorking = false;
                        rosterViewModelEO.ErrorMgs = "Roster interface  not working !";

                    }
                    finally
                    {
                        dbframework.CloseConnection();
                    }
                }
            }
            catch (Exception exM)
            {
                //throw ex;
                rosterViewModelEO.IsWorking = false;
                rosterViewModelEO.ErrorMgs = "Roster interface  not working (DB)!";

            }

            return rosterViewModelEO;
        }

        public async Task<double> GetDateDiffInDays(DateTime from, DateTime to)
        {

            if (from != null && to != null && from.ToString().Length > 0 && to.ToString().Length > 0)
            {

                var min = 1000 * 60;

                var diff = (to - from).TotalMinutes;
                return diff;

            }
            return 0;
        }
        public async Task<List<LookupEO>> GetCodeExplanationsAsyc(string filters)
        {
            List<LookupEO> responseList = new List<LookupEO>();
            LookupEO response = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_dutycodelist", filters, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_duty", ParameterDirection.Output, OracleDbType.RefCursor));

                    using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_dashboard.get_dutynames", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new LookupEO();
                            response.Text = objReader["duty_code"] != null && objReader["duty_code"].ToString() != string.Empty ? objReader["duty_code"].ToString() : string.Empty;
                            response.Value = objReader["duty_code_desc"] != null && objReader["duty_code_desc"].ToString() != string.Empty ? objReader["duty_code_desc"].ToString() : string.Empty;
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

        public async Task<List<HotelInfoEO>> GetPrintHotelInfosAsyc(HotelInfoFilterEO filterInput)
        {
            List<HotelInfoEO> responseList = new List<HotelInfoEO>();
            HotelInfoEO response = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_airportcode", filterInput.AirportCode.Trim(), ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_hotel", ParameterDirection.Output, OracleDbType.RefCursor));

                    using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_ipa_dashboard.get_fduty_multi_hotelinfo_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new HotelInfoEO();

                            response.HotelName = objReader["hotel_name"] != null ? objReader["hotel_name"].ToString() : string.Empty;
                            response.Address = objReader["address"] != null ? objReader["address"].ToString() : string.Empty;
                            response.Telephone = objReader["telephone"] != null ? objReader["telephone"].ToString() : string.Empty;
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
        public async Task<List<LookupEO>> GetUTCDiffAsyc(string filters)
        {
            string startDate = Common.OracleDateFormateddMMMyyyy(DateTime.Today);
            filters = filters.Replace("SYSDATE", startDate);
            List<LookupEO> responseList = new List<LookupEO>();
            LookupEO response = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.AIMS_CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("LOC_DATE", filters, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("loc_date_cur", ParameterDirection.Output, OracleDbType.RefCursor));

                    using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("GET_LOCAL_DATE", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new LookupEO();
                            response.Text = objReader["STATION"] != null && objReader["STATION"].ToString() != string.Empty ? objReader["STATION"].ToString() : string.Empty;
                            response.Value = objReader["GET_TIMEVARIATION(STATION,LOC_DATE)"] != null && objReader["GET_TIMEVARIATION(STATION,LOC_DATE)"].ToString() != string.Empty ? objReader["GET_TIMEVARIATION(STATION,LOC_DATE)"].ToString().Contains("-") ? objReader["GET_TIMEVARIATION(STATION,LOC_DATE)"].ToString() : "+" + objReader["GET_TIMEVARIATION(STATION,LOC_DATE)"].ToString() : string.Empty;
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


