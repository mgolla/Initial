using Oracle.DataAccess.Client;
using QR.IPrism.DataAccess.Interfaces;
using QR.IPrism.Enterprise;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using QR.IPrism.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.DataAccess.Implementation
{
    public class FlightDelayDao : IFlightDelayDao
    {
        public async Task<IEnumerable<FlightInfoEO>> GetDelaySearchResults(FlightDelayFilterEO filter, string staffNo, string staffId)
        {
            var dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            var parameterList = new List<ODPCommandParameter>();
            var response = new List<FlightInfoEO>();
            FlightInfoEO row = null;

            try
            {
                parameterList.Add(new ODPCommandParameter("pi_flight_number", filter.FlightNumber ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_from_date", Common.ToOracleDate(filter.FromDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("pi_to_date", Common.ToOracleDate(filter.ToDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("pi_sector_from", filter.SectorFrom ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_sector_to", filter.SectorTo ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_delay_type", filter.DelayType ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_delay_comment", filter.DelayComment ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_delay_reason", filter.DelayReason ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                //parameterList.Add(new ODPCommandParameter("pi_staff_id", staffNo ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_user_id", staffId ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                //parameterList.Add(new ODPCommandParameter("pi_pageIndex", String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                //parameterList.Add(new ODPCommandParameter("pi_pageSize", String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_SortOrder", String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_orderByColumn", String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_TotalCount", String.Empty, ParameterDirection.Output, OracleDbType.Decimal, 50));
                parameterList.Add(new ODPCommandParameter("prc_flights_detail", ParameterDirection.Output, OracleDbType.RefCursor));

                //using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("ivrs_flight_delay_pkg.get_flight_information", parameterList))
                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_evr.get_flight_information_mig", parameterList))
                {
                    while (objReader.Read())
                    {
                        row = new FlightInfoEO();
                        row.FlightDelayRptId = objReader["flightdelayrptid"] != null ? objReader["flightdelayrptid"].ToString() : string.Empty;
                        row.FlightDetsID = objReader["flightdetsid"] != null ? objReader["flightdetsid"].ToString() : string.Empty;
                        row.FlightNumber = objReader["flightnumber"] != null ? objReader["flightnumber"].ToString() : string.Empty;
                        row.Sector = objReader["sector"] != null ? objReader["sector"].ToString() : string.Empty;
                        row.SectorFrom = objReader["sectorfrom"] != null ? objReader["sectorfrom"].ToString() : string.Empty;
                        row.SectorTo = objReader["sectorto"] != null ? objReader["sectorto"].ToString() : string.Empty;
                        row.AirCraftRegNo = objReader["aircraftregno"] != null ? objReader["aircraftregno"].ToString() : string.Empty;
                        row.AirCraftType = objReader["aircrafttype"] != null ? objReader["aircrafttype"].ToString() : string.Empty;

                        row.ScheduledDeptTime = objReader["schedeptdate"] != null && objReader["schedeptdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["schedeptdate"].ToString()) : (DateTime?)null;

                        row.ScheduledArrTime = objReader["schearrdate"] != null && objReader["schearrdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["schearrdate"].ToString()) : (DateTime?)null;

                        row.ActualDeptTime = objReader["actdeptdate"] != null && objReader["actdeptdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["actdeptdate"].ToString()) : (DateTime?)null;

                        row.ActualArrTime = objReader["actarrdate"] != null && objReader["actarrdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["actarrdate"].ToString()) : (DateTime?)null;

                        row.CreatedDate = objReader["receiveddate"] != null && objReader["receiveddate"].ToString() != String.Empty
                            ? Convert.ToDateTime(objReader["receiveddate"].ToString()) : (DateTime?)null;

                        row.IsFromAIMS = objReader["isfromaims"] != null ? objReader["isfromaims"].ToString() == "Y" : false;
                        row.IsManuallyEntered = objReader["ismanuallyentered"] != null ? objReader["ismanuallyentered"].ToString() == "Y" : false;
                        row.IsFerry = objReader["isferry"] != null ? objReader["isferry"].ToString() == "Y" : false;

                        row.FlightCrewsDetail = GetFlightCrewDetails(objReader["flightdetsid"].ToString()).Result;

                        row.FlightDelay = GetFlightDelayData(objReader);
                        response.Add(row);
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

            return response;
        }

        public async Task<IEnumerable<FlightInfoEO>> GetEnterDelayFlightDetails(FlightDelayFilterEO filter, string staffNo, string staffId)
        {
            var dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            var parameterList = new List<ODPCommandParameter>();
            var response = new List<FlightInfoEO>();
            FlightInfoEO row = null;

            try
            {
                parameterList.Add(new ODPCommandParameter("pi_flight_number", filter.FlightNumber ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_atd_date", Common.ToOracleDate(filter.FromDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("pi_sector_from", filter.SectorFrom ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_sector_to", filter.SectorTo ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_staffno", staffNo, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_flight_delay", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_evr.get_delay_flight_proc", parameterList))
                {
                    while (objReader.Read())
                    {
                        row = new FlightInfoEO();
                        row.FlightDelay = new FlightDelayEO();

                        row.FlightDetsID = objReader["flightdetsid"] != null ? objReader["flightdetsid"].ToString() : string.Empty;
                        row.FlightNumber = objReader["flightnumber"] != null ? objReader["flightnumber"].ToString() : string.Empty;
                        row.Sector = objReader["sector"] != null ? objReader["sector"].ToString() : string.Empty;
                        row.SectorFrom = objReader["sectorfrom"] != null ? objReader["sectorfrom"].ToString() : string.Empty;
                        row.SectorTo = objReader["sectorto"] != null ? objReader["sectorto"].ToString() : string.Empty;
                        row.AirCraftRegNo = objReader["aircraftregno"] != null ? objReader["aircraftregno"].ToString() : string.Empty;
                        row.AirCraftType = objReader["aircrafttype"] != null ? objReader["aircrafttype"].ToString() : string.Empty;

                        row.ScheduledDeptTime = objReader["schedeptdate"] != null && objReader["schedeptdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["schedeptdate"].ToString()) : (DateTime?)null;

                        row.ScheduledArrTime = objReader["schearrdate"] != null && objReader["schearrdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["schearrdate"].ToString()) : (DateTime?)null;

                        row.ActualDeptTime = objReader["actdeptdate"] != null && objReader["actdeptdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["actdeptdate"].ToString()) : (DateTime?)null;

                        row.ActualArrTime = objReader["actarrdate"] != null && objReader["actarrdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["actarrdate"].ToString()) : (DateTime?)null;

                        row.IsFromAIMS = objReader["isfromaims"] != null ? objReader["isfromaims"].ToString() == "Y" : false;
                        row.IsManuallyEntered = objReader["ismanuallyentered"] != null ? objReader["ismanuallyentered"].ToString() == "Y" : false;
                        row.IsFerry = objReader["isferry"] != null ? objReader["isferry"].ToString() == "Y" : false;
                        row.DelayReportStatus = objReader["delaystatus"] != null ? objReader["delaystatus"].ToString() : string.Empty;
                        row.FlightCrewsDetail = GetFlightCrewDetails(objReader["flightdetsid"].ToString()).Result;
                        row.FlightDelay.CrewCount = objReader["flightdetsid"] != null ? GetCrewCount(objReader["flightdetsid"].ToString()).Result : String.Empty;
                        
                        row.FlightDelay.FlightRoute = objReader["flightroute"] != null ? objReader["flightroute"].ToString() : string.Empty;
                        row.FlightDelay.PassengerLoadFC = objReader["passengerloadfc"] != null ? objReader["passengerloadfc"].ToString() : string.Empty;
                        row.FlightDelay.PassengerLoadJC = objReader["passengerloadjc"] != null ? objReader["passengerloadjc"].ToString() : string.Empty;
                        row.FlightDelay.PassengerLoadYC = objReader["passengerloadyc"] != null ? objReader["passengerloadyc"].ToString() : string.Empty;
                        row.FlightDelay.InfantLoadFC = objReader["infantloadfc"] != null ? objReader["infantloadfc"].ToString() : string.Empty;
                        row.FlightDelay.InfantLoadJC = objReader["infantloadjc"] != null ? objReader["infantloadjc"].ToString() : string.Empty;
                        row.FlightDelay.InfantLoadYC = objReader["infantloadyc"] != null ? objReader["infantloadyc"].ToString() : string.Empty;
                        row.FlightDelay.SeatCapacityFC = objReader["seatcapacityfc"] != null ? objReader["seatcapacityfc"].ToString() : string.Empty;
                        row.FlightDelay.SeatCapacityJC = objReader["seatcapacityjc"] != null ? objReader["seatcapacityjc"].ToString() : string.Empty;
                        row.FlightDelay.SeatCapacityYC = objReader["seatcapacityyc"] != null ? objReader["seatcapacityyc"].ToString() : string.Empty;
                        row.FlightDelay.IsGroomingCheck = objReader["isgroomingcheck"] != null ? objReader["isgroomingcheck"].ToString() : string.Empty;
                        row.FlightDelay.GroomingCheckComment = objReader["groomingcheckcomment"] != null ? objReader["groomingcheckcomment"].ToString() : string.Empty;
                        row.FlightDelay.IsCsdCsBriefed = objReader["iscsdcsbriefed"] != null ? objReader["iscsdcsbriefed"].ToString() : string.Empty;
                        row.FlightDelay.CsdCsBriefedComment = objReader["csdcsbriefedcomment"] != null ? objReader["csdcsbriefedcomment"].ToString() : string.Empty;
                        row.FlightDelay.ArrivalDelay = objReader["ArrDelay"] != null ? objReader["ArrDelay"].ToString() : string.Empty;
                        row.FlightDelay.DepartureDelay = objReader["DeptDelay"] != null ? objReader["DeptDelay"].ToString() : string.Empty;

                        response.Add(row);
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

            return response;
        }

        public async Task<IEnumerable<FlightDelayEO>> GetDelayLookupValues()
        {
            List<FlightDelayEO> response = new List<FlightDelayEO>();
            FlightDelayEO delay = null;
            List<ODPCommandParameter> parametersList = new List<ODPCommandParameter>();
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
           // var commandCursorParameter = new List<CommandCursorParameter>();

            try
            {
                //commandCursorParameter.Add(new CommandCursorParameter("prc_delay_type", ParameterDirection.Output, OracleType.Cursor));
                //commandCursorParameter.Add(new CommandCursorParameter("", ParameterDirection.Output, OracleType.Cursor));
                parametersList.Add(new ODPCommandParameter("prc_delay_type", ParameterDirection.Output, OracleDbType.RefCursor));
                parametersList.Add(new ODPCommandParameter("prc_delay_reason", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("ivrs_flight_delay_pkg.get_delay_lookup_values",
                    parametersList))
                {
                    while (objReader.Read())
                    {
                        delay = new FlightDelayEO();
                        delay.DelayType = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
                        delay.DelayId = objReader["lookupid"] != null ? objReader["lookupid"].ToString() : string.Empty;
                        response.Add(delay);
                    }

                    objReader.NextResult();

                    while (objReader.Read())
                    {
                        delay = new FlightDelayEO();
                        delay.FlightDelayCatId = objReader["flightdelaycatid"] != null ? objReader["flightdelaycatid"].ToString() : string.Empty;
                        delay.FlightDelayCatName = objReader["flightdelaycatname"] != null ? objReader["flightdelaycatname"].ToString() : string.Empty;
                        response.Add(delay);
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

            return response;
        }

        public async void SetFlightDelayDetails(FlightDelayEO inputs, string staffId, string staffNo)
        {
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_flight_dets_id", inputs.FlightDetsID, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_flight_delay_type", inputs.DelayType ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_flight_delay_reason", inputs.FlightDelayCatId ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_flight_delay_comment", inputs.DelayComment ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_delayflight_isadmin", "N", ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_delayflight_userid", staffId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_staff_number", staffNo, ParameterDirection.Input, OracleDbType.Varchar2));

                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("ivrs_flight_delay_pkg.insert_flight_delay_cause", parameterList);
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

        public async Task<IEnumerable<FlightDelayEO>> GetDelayReasons(string id)
        {
            List<FlightDelayEO> response = new List<FlightDelayEO>();
            FlightDelayEO delay = null;
            List<ODPCommandParameter> parametersList = new List<ODPCommandParameter>();
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            //var commandCursorParameter = new List<CommandCursorParameter>();

            try
            {
                parametersList.Add(new ODPCommandParameter("pi_flight_dets_id", id, ParameterDirection.Input, OracleDbType.Varchar2));

                //commandCursorParameter.Add(new CommandCursorParameter("po_default_reason", ParameterDirection.Output, OracleType.Cursor));
                //commandCursorParameter.Add(new CommandCursorParameter("po_actual_reason", ParameterDirection.Output, OracleType.Cursor));

                parametersList.Add(new ODPCommandParameter("po_default_reason", ParameterDirection.Output, OracleDbType.RefCursor));
                parametersList.Add(new ODPCommandParameter("po_actual_reason", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_evr.get_flight_delay_cause",parametersList))
                {
                    //Select default reason
                    while (objReader.Read())
                    {
                        delay = new FlightDelayEO();
                        delay.FlightDelayCatId = objReader["flightdelaycatid"] != null ? objReader["flightdelaycatid"].ToString() : string.Empty;
                        delay.FlightDelayCatName = objReader["flightdelaycatname"] != null ? objReader["flightdelaycatname"].ToString() : string.Empty;
                        response.Add(delay);
                    }
                    objReader.NextResult();

                    //Select actual reason of delay
                    while (objReader.Read())
                    {
                        delay = new FlightDelayEO();
                        delay.FlightDelayCatId = objReader["flightdelaycatid"] != null ? objReader["flightdelaycatid"].ToString() : string.Empty;
                        delay.FlightDelayCatName = objReader["flightdelaycatname"] != null ? objReader["flightdelaycatname"].ToString() : string.Empty;
                        delay.DelayType = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
                        delay.DelayComment = objReader["comments"] != null ? objReader["comments"].ToString() : string.Empty;
                        delay.IsSelected = true;
                        //flightDelay.StaffName = drDelayReason["firstname"].ToString();
                        //flightDelay.StaffId = drDelayReason["staffno"].ToString();
                        //flightDelay.StaffGrade = drDelayReason["grade"].ToString();
                        response.Add(delay);
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

            return response;
        }

        public async Task<string> IsEnterDelayForFlight(string id)
        {
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            string result = String.Empty;

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_flight_dets_id", id, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_is_admin", "N", ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_is_exceeded", ParameterDirection.Output, OracleDbType.Varchar2, 100));
                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("ivrs_flight_delay_pkg.IsEnterDelayForFlight", parameterList);

                result = command.Parameters["po_is_exceeded"] != null ? command.Parameters["po_is_exceeded"].Value.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }

            return result;
        }


        private FlightDelayEO GetFlightDelayData(IDataReader objReader)
        {
            var FlightDelay = new FlightDelayEO();

            FlightDelay.CrewCount = objReader["flightdetsid"] != null ? GetCrewCount(objReader["flightdetsid"].ToString()).Result : String.Empty;
            FlightDelay.FlightRoute = objReader["flightroute"] != null ? objReader["flightroute"].ToString() : string.Empty;
            FlightDelay.PassengerLoadFC = objReader["passengerloadfc"] != null ? objReader["passengerloadfc"].ToString() : string.Empty;
            FlightDelay.PassengerLoadJC = objReader["passengerloadjc"] != null ? objReader["passengerloadjc"].ToString() : string.Empty;
            FlightDelay.PassengerLoadYC = objReader["passengerloadyc"] != null ? objReader["passengerloadyc"].ToString() : string.Empty;
            FlightDelay.InfantLoadFC = objReader["infantloadfc"] != null ? objReader["infantloadfc"].ToString() : string.Empty;
            FlightDelay.InfantLoadJC = objReader["infantloadjc"] != null ? objReader["infantloadjc"].ToString() : string.Empty;
            FlightDelay.InfantLoadYC = objReader["infantloadyc"] != null ? objReader["infantloadyc"].ToString() : string.Empty;
            FlightDelay.SeatCapacityFC = objReader["seatcapacityfc"] != null ? objReader["seatcapacityfc"].ToString() : string.Empty;
            FlightDelay.SeatCapacityJC = objReader["seatcapacityjc"] != null ? objReader["seatcapacityjc"].ToString() : string.Empty;
            FlightDelay.SeatCapacityYC = objReader["seatcapacityyc"] != null ? objReader["seatcapacityyc"].ToString() : string.Empty;
            FlightDelay.IsGroomingCheck = objReader["isgroomingcheck"] != null ? objReader["isgroomingcheck"].ToString() : string.Empty;
            FlightDelay.GroomingCheckComment = objReader["groomingcheckcomment"] != null ? objReader["groomingcheckcomment"].ToString() : string.Empty;
            FlightDelay.IsCsdCsBriefed = objReader["iscsdcsbriefed"] != null ? objReader["iscsdcsbriefed"].ToString() : string.Empty;
            FlightDelay.CsdCsBriefedComment = objReader["csdcsbriefedcomment"] != null ? objReader["csdcsbriefedcomment"].ToString() : string.Empty;
            FlightDelay.StaffName = objReader["staffname"] != null ? objReader["staffname"].ToString() : string.Empty;
            FlightDelay.StaffId = objReader["staffno"] != null ? objReader["staffno"].ToString() : string.Empty;
            FlightDelay.DelayType = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
            FlightDelay.DelayComment = objReader["comments"] != null ? objReader["comments"].ToString() : string.Empty;
            FlightDelay.FlightDelayCatName = objReader["delayreasons"] != null ? objReader["delayreasons"].ToString() : string.Empty;

            return FlightDelay;
        }

        private async Task<string> GetCrewCount(string flightDetailsId)
        {
            string response;
            List<ODPCommandParameter> parameterList = new List<ODPCommandParameter>();
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList.Add(new ODPCommandParameter("pi_flightdetailsid", flightDetailsId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_crewCount", ParameterDirection.Output, OracleDbType.Varchar2, 100));

                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("ivrs_enter_vr_pkg.GetCrewCount", parameterList);
                response = command.Parameters["po_crewCount"] != null ? command.Parameters["po_crewCount"].Value.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }

            return response;
        }

        private async Task<List<FlightCrewsEO>> GetFlightCrewDetails(string flightDetailsId)
        {
            List<FlightCrewsEO> result = new List<FlightCrewsEO>();
            List<ODPCommandParameter> parameterList = new List<ODPCommandParameter>();
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            FlightCrewsEO row = null;

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_flightdetailsid", flightDetailsId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("prc_flight_crew", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("ivrs_enter_vr_pkg.GetFlightCrewDetails", parameterList))
                {
                    while (objReader.Read())
                    {
                        row = new FlightCrewsEO();
                        row.FlightCrewId = objReader["crewdetailsid"] != null ? objReader["crewdetailsid"].ToString() : string.Empty;
                        row.StaffNumber = objReader["staffnumber"] != null ? objReader["staffnumber"].ToString() : string.Empty;
                        row.StaffName = objReader["crewname"] != null ? objReader["crewname"].ToString() : string.Empty;
                        row.StaffGrade = objReader["grade"] != null ? objReader["grade"].ToString() : string.Empty;
                        row.IsActingCSD = objReader["actingcsd"] != null ? objReader["actingcsd"].ToString() : string.Empty;
                        row.CabinCrewPosition = objReader["cabincrewposition"] != null ? objReader["cabincrewposition"].ToString() : string.Empty;
                        row.AnnounceLang = objReader["Announcelang"] != null ? objReader["Announcelang"].ToString() : string.Empty;
                        result.Add(row);
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
            return result;
        }
    }
}