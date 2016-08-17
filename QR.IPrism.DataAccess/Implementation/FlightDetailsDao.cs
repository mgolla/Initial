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
    public class FlightDetailsDao : IFlightDetailsDao
    {
        public async Task<IEnumerable<FlightInfoEO>> GetFlightDetails(FlightDelayFilterEO filter, string staffNo, string userId)
        {
            var dbframework = new DBFramework(Constants.CONNECTION_STR);
            var parameterList = new List<CommandParameter>();
            var response = new List<FlightInfoEO>();
            FlightInfoEO row = null;

            try
            {
                parameterList.Add(new CommandParameter("pi_flight_number", filter.FlightNumber ?? string.Empty, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_atd_date", Common.OracleDateFormateddMMMyyyy(filter.FromDate) ?? string.Empty, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_sector_from", filter.SectorFrom ?? string.Empty, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_sector_to", filter.SectorTo ?? string.Empty, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_staffno", staffNo, ParameterDirection.Input, DbType.String));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_evr.Get_add_flight_proc", parameterList, "po_add_flight"))
                {
                    while (objReader.Read())
                    {
                        row = new FlightInfoEO();
                        row.FlightDetsID = objReader["flightdetsid"] != null ? objReader["flightdetsid"].ToString() : string.Empty;
                        row.FlightNumber = objReader["flightnumber"] != null ? objReader["flightnumber"].ToString() : string.Empty;
                        row.Sector = objReader["sector"] != null ? objReader["sector"].ToString() : string.Empty;
                        row.SectorFrom = objReader["sectorfrom"] != null ? objReader["sectorfrom"].ToString() : string.Empty;
                        row.SectorTo = objReader["sectorto"] != null ? objReader["sectorto"].ToString() : string.Empty;
                        row.AirCraftRegNo = objReader["aircraftregno"] != null ? objReader["aircraftregno"].ToString() : string.Empty;
                        row.AirCraftType = objReader["aircrafttype"] != null ? objReader["aircrafttype"].ToString() : string.Empty;
                        row.AirCraftTypeId = objReader["aircrafttypeid"] != null ? objReader["aircrafttypeid"].ToString() : string.Empty;

                        row.ScheduledArrTime = objReader["schearrdate"] != null && objReader["schearrdate"].ToString() != String.Empty ?
                          Convert.ToDateTime(objReader["schearrdate"].ToString()) : (DateTime?)null;

                        row.ScheduledDeptTime = objReader["schedeptdate"] != null && objReader["schedeptdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["schedeptdate"].ToString()) : (DateTime?)null;


                        row.ActualDeptTime = objReader["actdeptdate"] != null && objReader["actdeptdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["actdeptdate"].ToString()) : (DateTime?)null;

                        row.ActualArrTime = objReader["actarrdate"] != null && objReader["actarrdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["actarrdate"].ToString()) : (DateTime?)null;

                        row.ModifiedDate = objReader["MODIFIEDDATE"] != null && objReader["MODIFIEDDATE"].ToString() != String.Empty
                            ? Convert.ToDateTime(objReader["MODIFIEDDATE"].ToString()) : (DateTime?)null;

                        row.IsFromAIMS = objReader["isfromaims"] != null ? objReader["isfromaims"].ToString() == "Y" : false;
                        row.IsManuallyEntered = objReader["ismanuallyentered"] != null ? objReader["ismanuallyentered"].ToString() == "Y" : false;
                        row.IsFerry = objReader["isferry"] != null ? objReader["isferry"].ToString() == "Y" : false;

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

        public async Task<IEnumerable<FlightInfoEO>> GetFlightDetailForPaste(string staffId, string flightDetailsId)
        {
            var dbframework = new DBFramework(Constants.CONNECTION_STR);
            var parameterList = new List<CommandParameter>();
            var response = new List<FlightInfoEO>();
            FlightInfoEO row = null;

            try
            {
                parameterList.Add(new CommandParameter("pi_crewdetails_id", staffId, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_flightdetsid", flightDetailsId, ParameterDirection.Input, DbType.String));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("ivrs_enter_vr_pkg.getExistingFlightForPaste", parameterList, "prc_flightDet"))
                {
                    while (objReader.Read())
                    {
                        row = new FlightInfoEO();
                        row.FlightDetsID = objReader["flightdetsid"] != null ? objReader["flightdetsid"].ToString() : string.Empty;
                        row.FlightNumber = objReader["flightnumber"] != null ? objReader["flightnumber"].ToString() : string.Empty;
                        row.SectorFrom = objReader["sectorfrom"] != null ? objReader["sectorfrom"].ToString() : string.Empty;
                        row.SectorTo = objReader["sectorto"] != null ? objReader["sectorto"].ToString() : string.Empty;

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

        public async Task<FlightInfoEO> GetSingleFlight(string flightDetailsId, string userId)
        {
            var dbframework = new DBFramework(Constants.CONNECTION_STR);
            var parameterList = new List<CommandParameter>();
            FlightInfoEO row = new FlightInfoEO()
            {
                FlightDelay = new FlightDelayEO()
            };
            List<CommandCursorParameter> commandCursorParameter = new List<CommandCursorParameter>();
            FlightCrewsEO crew = null;

            try
            {
                parameterList.Add(new CommandParameter("pi_flightdetailsid", flightDetailsId, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_user_id", userId, ParameterDirection.Input, DbType.String));

                commandCursorParameter.Add(new CommandCursorParameter("po_flights_detail", ParameterDirection.Output, OracleType.Cursor));
                commandCursorParameter.Add(new CommandCursorParameter("po_flight_crews", ParameterDirection.Output, OracleType.Cursor));
                commandCursorParameter.Add(new CommandCursorParameter("po_crew_complement", ParameterDirection.Output, OracleType.Cursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_evr.get_singleflightdetail_proc", parameterList, commandCursorParameter))
                {
                    while (objReader.Read())
                    {
                        row.FlightDetsID = objReader["flightdetsid"] != null ? objReader["flightdetsid"].ToString() : string.Empty;
                        row.FlightNumber = objReader["flightnumber"] != null ? objReader["flightnumber"].ToString() : string.Empty;
                        row.Sector = objReader["sector"] != null ? objReader["sector"].ToString() : string.Empty;
                        row.SectorFrom = objReader["sectorfrom"] != null ? objReader["sectorfrom"].ToString() : string.Empty;
                        row.SectorTo = objReader["sectorto"] != null ? objReader["sectorto"].ToString() : string.Empty;
                        row.AirCraftRegNo = objReader["aircraftregno"] != null ? objReader["aircraftregno"].ToString() : string.Empty;
                        row.AirCraftType = objReader["aircrafttype"] != null ? objReader["aircrafttype"].ToString() : string.Empty;
                        row.AirCraftTypeId = objReader["aircrafttypeval"] != null ? objReader["aircrafttypeval"].ToString() : string.Empty;

                        row.ScheduledArrTime = objReader["schearrdate"] != null && objReader["schearrdate"].ToString() != String.Empty ?
                          Convert.ToDateTime(objReader["schearrdate"].ToString()) : (DateTime?)null;

                        row.ScheduledDeptTime = objReader["schedeptdate"] != null && objReader["schedeptdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["schedeptdate"].ToString()) : (DateTime?)null;


                        row.ActualDeptTime = objReader["actdeptdate"] != null && objReader["actdeptdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["actdeptdate"].ToString()) : (DateTime?)null;

                        row.ActualArrTime = objReader["actarrdate"] != null && objReader["actarrdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["actarrdate"].ToString()) : (DateTime?)null;

                        row.ModifiedDate = objReader["MODIFIEDDATE"] != null && objReader["MODIFIEDDATE"].ToString() != String.Empty
                            ? Convert.ToDateTime(objReader["MODIFIEDDATE"].ToString()) : (DateTime?)null;

                        row.IsFromAIMS = objReader["isfromaims"] != null ? objReader["isfromaims"].ToString() == "Y" : false;
                        row.IsManuallyEntered = objReader["ismanuallyentered"] != null ? objReader["ismanuallyentered"].ToString() == "Y" : false;
                        row.IsFerry = objReader["isferry"] != null ? objReader["isferry"].ToString() == "Y" : false;

                        row.FlightDelay.ArrivalDelay = objReader["ArrDelay"] != null ? objReader["ArrDelay"].ToString() : string.Empty;
                        row.FlightDelay.DepartureDelay = objReader["DeptDelay"] != null ? objReader["DeptDelay"].ToString() : string.Empty;
                        row.FlightDelay.SubmittedVRs = objReader["SubmittedVRs"] != null ? objReader["SubmittedVRs"].ToString() : string.Empty;
                        row.FlightDelay.DraftVRs = objReader["DraftVRs"] != null ? objReader["DraftVRs"].ToString() : string.Empty;
                        row.FlightDelay.NoVR = objReader["NoVRs"] != null ? objReader["NoVRs"].ToString() : string.Empty;
                        row.FlightDelay.VrNotSubmitted = objReader["VrNotSubmitteds"] != null ? objReader["VrNotSubmitteds"].ToString() : string.Empty;
                        row.FlightDelay.FlightRoute = objReader["flightroute"] != null ? objReader["flightroute"].ToString() : string.Empty;

                        row.FlightDelay.PassengerLoadFC = objReader["passengerloadfc"] != null ?
                            objReader["passengerloadfc"].ToString() == "-1" ? "NA" : objReader["passengerloadfc"].ToString() : "NA";
                        row.FlightDelay.PassengerLoadYC = objReader["passengerloadyc"] != null ?
                            objReader["passengerloadyc"].ToString() == "-1" ? "NA" : objReader["passengerloadyc"].ToString() : "NA";
                        row.FlightDelay.PassengerLoadJC = objReader["passengerloadjc"] != null ?
                            objReader["passengerloadjc"].ToString() == "-1" ? "NA" : objReader["passengerloadjc"].ToString() : "NA";

                        row.FlightDelay.InfantLoadFC = objReader["infantloadfc"] != null ?
                            objReader["infantloadfc"].ToString() == "-1" ? "NA" : objReader["infantloadfc"].ToString() : "NA";
                        row.FlightDelay.InfantLoadJC = objReader["infantloadjc"] != null ?
                          objReader["infantloadjc"].ToString() == "-1" ? "NA" : objReader["infantloadjc"].ToString() : "NA";
                        row.FlightDelay.InfantLoadYC = objReader["infantloadyc"] != null ?
                          objReader["infantloadyc"].ToString() == "-1" ? "NA" : objReader["infantloadyc"].ToString() : "NA";

                        row.FlightDelay.SeatCapacityFC = objReader["seatcapacityfc"] != null ?
                            objReader["seatcapacityfc"].ToString() == "-1" ? "NA" : objReader["seatcapacityfc"].ToString() : "NA";
                        row.FlightDelay.SeatCapacityJC = objReader["seatcapacityjc"] != null ?
                          objReader["seatcapacityjc"].ToString() == "-1" ? "NA" : objReader["seatcapacityjc"].ToString() : "NA";
                        row.FlightDelay.SeatCapacityYC = objReader["seatcapacityyc"] != null ?
                          objReader["seatcapacityyc"].ToString() == "-1" ? "NA" : objReader["seatcapacityyc"].ToString() : "NA";

                        row.FlightDelay.IsGroomingCheck = objReader["isgroomingcheck"] != null ? objReader["isgroomingcheck"].ToString() : string.Empty;
                        row.FlightDelay.GroomingCheckComment = objReader["groomingcheckcomment"] != null ? objReader["groomingcheckcomment"].ToString() : string.Empty;
                        row.FlightDelay.IsCsdCsBriefed = objReader["iscsdcsbriefed"] != null ? objReader["iscsdcsbriefed"].ToString() : string.Empty;
                        row.FlightDelay.CsdCsBriefedComment = objReader["csdcsbriefedcomment"] != null ? objReader["csdcsbriefedcomment"].ToString() : string.Empty;
                        row.FlightDelay.NoVrSubmittedStatus = objReader["NoVrsByUser"] != null ? objReader["NoVrsByUser"].ToString() : string.Empty;
                        // row.FlightDelay.SubmittedVRsbyUser = objReader["submittedvrsbyuser"] != null ? objReader["submittedvrsbyuser"].ToString() : string.Empty;
                    }

                    objReader.NextResult();

                    row.FlightCrewsDetail = new List<FlightCrewsEO>();
                    while (objReader.Read())
                    {
                        crew = new FlightCrewsEO();
                        crew.FlightCrewId = objReader["crewdetailsid"] != null ? objReader["crewdetailsid"].ToString() : string.Empty;
                        crew.StaffNumber = objReader["staffnumber"] != null ? objReader["staffnumber"].ToString() : string.Empty;
                        crew.StaffName = objReader["crewname"] != null ? objReader["crewname"].ToString() : string.Empty;
                        crew.StaffGrade = objReader["grade"] != null ? objReader["grade"].ToString() : string.Empty;
                        crew.IsActingCSD = objReader["actingcsd"] != null ? objReader["actingcsd"].ToString() : string.Empty;
                        crew.CabinCrewPosition = objReader["cabincrewposition"] != null && objReader["cabincrewposition"].ToString() != string.Empty ?
                            objReader["cabincrewposition"].ToString() : "NA";
                        crew.AnnounceLang = objReader["Announcelang"] != null && objReader["Announcelang"].ToString() != string.Empty ?
                            objReader["Announcelang"].ToString() : "NA";
                        row.FlightCrewsDetail.Add(crew);
                    }

                    objReader.NextResult();

                    while (objReader.Read())
                    {
                        row.FlightDelay.CrewCount = objReader["crew_complement"] != null ? objReader["crew_complement"].ToString() : string.Empty;
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

            return row;
        }

        public async Task<FlightInfoEO> GetSingleFlightOld(string flightDetailsId, string userId)
        {
            var dbframework = new DBFramework(Constants.CONNECTION_STR);
            var parameterList = new List<CommandParameter>();
            FlightInfoEO row = new FlightInfoEO()
            {
                FlightDelay = new FlightDelayEO()
            };
            List<CommandCursorParameter> commandCursorParameter = new List<CommandCursorParameter>();
            FlightCrewsEO crew = null;

            try
            {
                parameterList.Add(new CommandParameter("pi_flightdetailsid", flightDetailsId, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_userid", userId, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("po_crewcount", ParameterDirection.Output, DbType.String,50));

                commandCursorParameter.Add(new CommandCursorParameter("prc_flights_detail", ParameterDirection.Output, OracleType.Cursor));
                commandCursorParameter.Add(new CommandCursorParameter("prc_flight_crew", ParameterDirection.Output, OracleType.Cursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("ivrs_enter_vr_pkg.GetFlight", parameterList, commandCursorParameter))
                {
                    while (objReader.Read())
                    {

                        row.FlightDetsID = objReader["flightdetsid"] != null ? objReader["flightdetsid"].ToString() : string.Empty;
                        row.FlightNumber = objReader["flightnumber"] != null ? objReader["flightnumber"].ToString() : string.Empty;
                        row.IsFromAIMS = objReader["isfromaims"] != null ? objReader["isfromaims"].ToString() == "Y" : false;
                        row.IsManuallyEntered = objReader["ismanuallyentered"] != null ? objReader["ismanuallyentered"].ToString() == "Y" : false;
                        row.Sector = objReader["sector"] != null ? objReader["sector"].ToString() : string.Empty;
                        row.SectorFrom = objReader["sectorfrom"] != null ? objReader["sectorfrom"].ToString() : string.Empty;
                        row.SectorTo = objReader["sectorto"] != null ? objReader["sectorto"].ToString() : string.Empty;
                        row.ModifiedDate = objReader["MODIFIEDDATE"] != null && objReader["MODIFIEDDATE"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["MODIFIEDDATE"].ToString()) : (DateTime?)null;

                        row.AirCraftRegNo = objReader["aircraftregno"] != null ? objReader["aircraftregno"].ToString() : string.Empty;
                        row.AirCraftType = objReader["aircrafttype"] != null ? objReader["aircrafttype"].ToString() : string.Empty;
                        row.AirCraftTypeId = objReader["aircrafttypeval"] != null ? objReader["aircrafttypeval"].ToString() : string.Empty;

                        row.ScheduledArrTime = objReader["schearrdate"] != null && objReader["schearrdate"].ToString() != String.Empty ?
                          Convert.ToDateTime(objReader["schearrdate"].ToString()) : (DateTime?)null;

                        row.ScheduledDeptTime = objReader["schedeptdate"] != null && objReader["schedeptdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["schedeptdate"].ToString()) : (DateTime?)null;


                        row.ActualDeptTime = objReader["actdeptdate"] != null && objReader["actdeptdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["actdeptdate"].ToString()) : (DateTime?)null;

                        row.ActualArrTime = objReader["actarrdate"] != null && objReader["actarrdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["actarrdate"].ToString()) : (DateTime?)null;

                        row.CreatedDate = objReader["MODIFIEDDATE"] != null && objReader["MODIFIEDDATE"].ToString() != String.Empty
                            ? Convert.ToDateTime(objReader["MODIFIEDDATE"].ToString()) : (DateTime?)null;

                        row.IsFerry = objReader["isferry"] != null ? objReader["isferry"].ToString() == "Y" : false;

                        row.FlightDelay.ArrivalDelay = objReader["ArrDelay"] != null ? objReader["ArrDelay"].ToString() : string.Empty;
                        row.FlightDelay.DepartureDelay = objReader["DeptDelay"] != null ? objReader["DeptDelay"].ToString() : string.Empty;
                        row.FlightDelay.SubmittedVRs = objReader["SubmittedVRs"] != null ? objReader["SubmittedVRs"].ToString() : string.Empty;
                        row.FlightDelay.DraftVRs = objReader["DraftVRs"] != null ? objReader["DraftVRs"].ToString() : string.Empty;
                        row.FlightDelay.NoVR = objReader["NoVRs"] != null ? objReader["NoVRs"].ToString() : string.Empty;
                        row.FlightDelay.VrNotSubmitted = objReader["VrNotSubmitteds"] != null ? objReader["VrNotSubmitteds"].ToString() : string.Empty;
                        row.FlightDelay.FlightRoute = objReader["flightroute"] != null ? objReader["flightroute"].ToString() : string.Empty;

                        row.FlightDelay.PassengerLoadFC = objReader["passengerloadfc"] != null ?
                            objReader["passengerloadfc"].ToString() == "-1" ? "NA" : objReader["passengerloadfc"].ToString() : "NA";
                        row.FlightDelay.PassengerLoadYC = objReader["passengerloadyc"] != null ?
                            objReader["passengerloadyc"].ToString() == "-1" ? "NA" : objReader["passengerloadyc"].ToString() : "NA";
                        row.FlightDelay.PassengerLoadJC = objReader["passengerloadjc"] != null ?
                            objReader["passengerloadjc"].ToString() == "-1" ? "NA" : objReader["passengerloadjc"].ToString() : "NA";

                        row.FlightDelay.InfantLoadFC = objReader["infantloadfc"] != null ?
                            objReader["infantloadfc"].ToString() == "-1" ? "NA" : objReader["infantloadfc"].ToString() : "NA";
                        row.FlightDelay.InfantLoadJC = objReader["infantloadjc"] != null ?
                          objReader["infantloadjc"].ToString() == "-1" ? "NA" : objReader["infantloadjc"].ToString() : "NA";
                        row.FlightDelay.InfantLoadYC = objReader["infantloadyc"] != null ?
                          objReader["infantloadyc"].ToString() == "-1" ? "NA" : objReader["infantloadyc"].ToString() : "NA";

                        row.FlightDelay.SeatCapacityFC = objReader["seatcapacityfc"] != null ?
                            objReader["seatcapacityfc"].ToString() == "-1" ? "NA" : objReader["seatcapacityfc"].ToString() : "NA";
                        row.FlightDelay.SeatCapacityJC = objReader["seatcapacityjc"] != null ?
                          objReader["seatcapacityjc"].ToString() == "-1" ? "NA" : objReader["seatcapacityjc"].ToString() : "NA";
                        row.FlightDelay.SeatCapacityYC = objReader["seatcapacityyc"] != null ?
                          objReader["seatcapacityyc"].ToString() == "-1" ? "NA" : objReader["seatcapacityyc"].ToString() : "NA";

                        row.FlightDelay.IsGroomingCheck = objReader["isgroomingcheck"] != null ? objReader["isgroomingcheck"].ToString() : string.Empty;
                        row.FlightDelay.GroomingCheckComment = objReader["groomingcheckcomment"] != null ? objReader["groomingcheckcomment"].ToString() : string.Empty;
                        row.FlightDelay.IsCsdCsBriefed = objReader["iscsdcsbriefed"] != null ? objReader["iscsdcsbriefed"].ToString() : string.Empty;
                        row.FlightDelay.CsdCsBriefedComment = objReader["csdcsbriefedcomment"] != null ? objReader["csdcsbriefedcomment"].ToString() : string.Empty;
                        row.FlightDelay.NoVrSubmittedStatus = objReader["NoVrsByUser"] != null ? objReader["NoVrsByUser"].ToString() : string.Empty;
                    }

                    objReader.NextResult();

                    row.FlightCrewsDetail = new List<FlightCrewsEO>();
                    while (objReader.Read())
                    {
                        crew = new FlightCrewsEO();
                        crew.FlightCrewId = objReader["crewdetailsid"] != null ? objReader["crewdetailsid"].ToString() : string.Empty;
                        crew.StaffNumber = objReader["staffnumber"] != null ? objReader["staffnumber"].ToString() : string.Empty;
                        crew.StaffName = objReader["crewname"] != null ? objReader["crewname"].ToString() : string.Empty;
                        crew.StaffGrade = objReader["grade"] != null ? objReader["grade"].ToString() : string.Empty;
                        crew.IsActingCSD = objReader["actingcsd"] != null ? objReader["actingcsd"].ToString() : string.Empty;
                        crew.CabinCrewPosition = objReader["cabincrewposition"] != null && objReader["cabincrewposition"].ToString() != string.Empty ?
                            objReader["cabincrewposition"].ToString() : "NA";
                        crew.AnnounceLang = objReader["Announcelang"] != null && objReader["Announcelang"].ToString() != string.Empty ?
                            objReader["Announcelang"].ToString() : "NA";
                        row.FlightCrewsDetail.Add(crew);
                    }

                    objReader.NextResult();

                    while (objReader.Read())
                    {
                        row.FlightDelay.CrewCount = objReader["crew_complement"] != null ? objReader["crew_complement"].ToString() : string.Empty;
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

            return row;
        }

        public async Task<IEnumerable<FlightCrewsEO>> GetAutoSuggestStaffInformation(SearchCriteriaEO searchCriteria)
        {
            FlightCrewsEO row = null;
            List<CommandParameter> parametersList = null;
            List<FlightCrewsEO> lstUser = new List<FlightCrewsEO>();

            DBFramework dbframework = new DBFramework(Constants.CONNECTION_STR);
            {
                try
                {
                    parametersList = new List<CommandParameter>();
                    parametersList.Add(new CommandParameter("pi_search_criteria", searchCriteria.CrewSearchCriteria ?? string.Empty, ParameterDirection.Input, DbType.String));
                    parametersList.Add(new CommandParameter("pi_data_source_id", searchCriteria.Id ?? string.Empty, ParameterDirection.Input, DbType.String));
                    parametersList.Add(new CommandParameter("pi_data_source_name", searchCriteria.Name ?? string.Empty, ParameterDirection.Input, DbType.String));
                    parametersList.Add(new CommandParameter("pi_grade_cat", searchCriteria.Grade ?? string.Empty, ParameterDirection.Input, DbType.String));
                    parametersList.Add(new CommandParameter("p_key", Constants.DECREPT_KEY, ParameterDirection.Input, DbType.String));

                    using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("COMN_AUTOSEARCH_PKG.search_staff", parametersList, "prc_cur_staff"))
                    {
                        while (objReader.Read())
                        {
                            row = new FlightCrewsEO();
                            row.StaffNumber = objReader["staffno"] != null ? objReader["staffno"].ToString() : string.Empty;
                            row.StaffName = objReader["staffnamewithno"] != null ? objReader["staffnamewithno"].ToString() : string.Empty;
                            row.FlightCrewId = objReader["CREWDETAILSID"] != null ? objReader["CREWDETAILSID"].ToString() : string.Empty;
                            row.StaffGrade = objReader["grade"] != null ? objReader["grade"].ToString() : string.Empty;
                            lstUser.Add(row);
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

            //                commonEOAS = new EOCommonAS();
            //                commonEOAS.CrewDetailsID = drCommon["crewdetailsid"].ToString();
            //                commonEOAS.UserID = drCommon["userid"].ToString();
            //                commonEOAS.StaffNumber = drCommon["staffno"].ToString();
            //                commonEOAS.FirstName = drCommon["firstname"].ToString();
            //                commonEOAS.MiddleName = drCommon["middlename"].ToString();
            //                commonEOAS.LastName = drCommon["lastname"].ToString();
            //                commonEOAS.Grade = drCommon["grade"].ToString();
            //                commonEOAS.GradeCat = drCommon["gradecat"].ToString();
            //                commonEOAS.GradeCatDet = drCommon["gradecatdet"].ToString();
            //                commonEOAS.EmailId = drCommon["officialmail"].ToString();

            //                //Added for iPrism Change Accommodation Changes - To retieve stay with friend staff id
            //                commonEOAS.StaffNameWithNo = drCommon["staffnamewithno"].ToString();

            //                if ((Convert.ToString(drCommon["gradecat"])).ToUpper().Equals("DC"))
            //                {
            //                    if (drCommon["gender"].ToString().ToUpper().Equals("M"))
            //                    {
            //                        commonEOAS.IconPath = deckCrewMaleImage;
            //                    }

            //                    else
            //                    {
            //                        // commonEOAS.IconPath = deckCrewFemaleImage;
            //                        commonEOAS.IconPath = deckCrewMaleImage;
            //                    }
            //                }
            //                else
            //                {
            //                    if (drCommon["gender"].ToString().ToUpper().Equals("M"))
            //                    {
            //                        if (drCommon["isactive"].ToString().ToUpper().Equals("Y"))
            //                        {
            //                            commonEOAS.IconPath = maleImage;
            //                        }
            //                        else
            //                        {
            //                            commonEOAS.IconPath = maleInactiveImage;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (drCommon["isactive"].ToString().ToUpper().Equals("Y"))
            //                        {
            //                            commonEOAS.IconPath = femaleImage;
            //                        }
            //                        else
            //                        {
            //                            commonEOAS.IconPath = femaleInactiveImage;
            //                        }
            //                    }
            //                }

            //                DateTime.TryParse(drCommon["doj"].ToString(), out dojDate);
            //                commonEOAS.DOJ = dojDate;
            //                //commonEOAS.DOJ = Convert.ToDateTime(drCommon["doj"].ToString());
            //                commonEOAS.Nationality = drCommon["nationality"].ToString();
            //                commonEOAS.FleetType = drCommon["fleettype"].ToString();
            //                commonEOAS.FlyingHours = drCommon["flyinghours"].ToString();
            //                commonEOAS.IsActive = drCommon["isactive"].ToString();
            //                commonEOAS.Position = drCommon["POSITION"].ToString();


            return lstUser;
        }

        public async Task<ResponseEO> UpdateFlightDetails(FlightInfoEO inputs, string staffId)
        {
            ResponseEO response = new ResponseEO();
            List<CommandParameter> parameterList = null;
            DBFramework dbframework = new DBFramework(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<CommandParameter>();
                //parameterList.Add(new CommandParameter("PI_DATE", DateTime.Now, ParameterDirection.Input, DbType.DateTime));
                //DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("PROC_TEST_DATE", parameterList);


                parameterList.Add(new CommandParameter("pi_flightdetsid", inputs.FlightDetsID ?? string.Empty, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_flightnumber", inputs.FlightNumber ?? string.Empty, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_sectorfrom", inputs.SectorFrom ?? string.Empty, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_sectorto", inputs.SectorTo ?? string.Empty, ParameterDirection.Input, DbType.String));

                parameterList.Add(new CommandParameter("pi_schedeptdate", inputs.ScheduledDeptTime, ParameterDirection.Input, DbType.DateTime));
                parameterList.Add(new CommandParameter("pi_schearrdate", inputs.ScheduledArrTime, ParameterDirection.Input, DbType.DateTime));
                parameterList.Add(new CommandParameter("pi_actdeptdate", inputs.ActualDeptTime, ParameterDirection.Input, DbType.DateTime));
                parameterList.Add(new CommandParameter("pi_actarrdate", inputs.ActualArrTime, ParameterDirection.Input, DbType.DateTime));
                parameterList.Add(new CommandParameter("pi_aircraftregno", inputs.AirCraftRegNo, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_aircrafttype", inputs.AirCraftTypeId ?? string.Empty, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_last_modifieddate", inputs.ModifiedDate, ParameterDirection.Input, DbType.DateTime));
                parameterList.Add(new CommandParameter("pi_userid", staffId ?? string.Empty, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_winwebnumber", inputs.WinWebNumber, ParameterDirection.Input, DbType.String));
               
                parameterList.Add(new CommandParameter("pi_flightroute", inputs.FlightDelay.FlightRoute, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_passengerloadfc", inputs.FlightDelay.PassengerLoadFC == "NA"? "-1" :  inputs.FlightDelay.PassengerLoadFC,
                    ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_passengerloadjc", inputs.FlightDelay.PassengerLoadJC == "NA"? "-1" : inputs.FlightDelay.PassengerLoadJC,
                    ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_passengerloadyc", inputs.FlightDelay.PassengerLoadYC == "NA"? "-1" : inputs.FlightDelay.PassengerLoadYC,
                    ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_infantloadfc", inputs.FlightDelay.InfantLoadFC == "NA"? "-1" :inputs.FlightDelay.InfantLoadFC,
                    ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_infantloadjc", inputs.FlightDelay.InfantLoadJC == "NA"? "-1" :inputs.FlightDelay.InfantLoadJC,
                    ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_infantloadyc", inputs.FlightDelay.InfantLoadYC == "NA"? "-1" :inputs.FlightDelay.InfantLoadYC,
                    ParameterDirection.Input, DbType.String));

                parameterList.Add(new CommandParameter("pi_isgroomingcheck", inputs.FlightDelay.IsGroomingCheck ?? string.Empty, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_groomingcheckcomment", inputs.FlightDelay.GroomingCheckComment ?? string.Empty, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_iscsdcsbriefed", inputs.FlightDelay.IsCsdCsBriefed ?? string.Empty, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_csdcsbriefedcomment", inputs.FlightDelay.CsdCsBriefedComment ?? string.Empty, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_seatcapacityfc", inputs.FlightDelay.SeatCapacityFC == "NA"? "-1" :inputs.FlightDelay.SeatCapacityFC ,
                    ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_seatcapacityjc", inputs.FlightDelay.SeatCapacityJC == "NA"? "-1" :inputs.FlightDelay.SeatCapacityJC ,
                    ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_seatcapacityyc", inputs.FlightDelay.SeatCapacityYC == "NA" ? "-1" : inputs.FlightDelay.SeatCapacityYC,
                    ParameterDirection.Input, DbType.String));

                parameterList.Add(new CommandParameter("pi_crew_details", inputs.CrewDetail ?? string.Empty, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_ismanuallyentered", inputs.IsManuallyEntered ? "Y" : "N", ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_ferry", inputs.IsFerry ? "Y" : "N", ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("po_modifieddate", ParameterDirection.Output, DbType.String, 100));

                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("ivrs_master_data_pkg.UpdateFlightDetails", parameterList);
                response.ResponseId = command.Parameters["po_modifieddate"] != null ? command.Parameters["po_modifieddate"].Value.ToString() : string.Empty;

                UpdateCrewDetails(inputs, staffId);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Duplicate"))
                {
                    response.Message = "Duplicate Flight Details can not be saved.";
                }
                else if (ex.Message.Contains("DataConcurrency"))
                {
                    response.Message = "Flight Details already modified by other User.";
                }
                else if (ex.Message.Contains("ATADate"))
                {
                    response.Message = "Actual Time of Arrival should be within last 24 hrs of UTC.";
                }
                else if (ex.Message.Contains("FlightNotExits"))
                {
                    response.Message = "Flight Number not found.";
                }
                else if (ex.Message.Contains("CsdCSNotExits"))
                {
                    response.Message = "At least one staff should be PO/LT/CSD/CS.";
                }
                else
                {
                    throw ex;
                }
            }
            finally
            {
                dbframework.CloseConnection();
            }

            return response;
        }

        public async Task<IEnumerable<FlightCrewsEO>> GetCrewsForFlight(string flightDetailsId)
        {
            var dbframework = new DBFramework(Constants.CONNECTION_STR);
            var parameterList = new List<CommandParameter>();
            List<FlightCrewsEO> result = new List<FlightCrewsEO>();
            FlightCrewsEO crew = null;

            try
            {
                parameterList.Add(new CommandParameter("pi_flight_dets_id", flightDetailsId, ParameterDirection.Input, DbType.String));
                //using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("ivrs_master_data_pkg.GetUserForFlight", parameterList, "po_ref_cur_user"))
                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_kafou.getuserforflight_mig", parameterList, "po_ref_cur_user"))
                {
                    while (objReader.Read())
                    {
                        crew = new FlightCrewsEO();
                        crew.FlightCrewId = objReader["userid"] != null ? objReader["userid"].ToString() : string.Empty;
                        crew.StaffName = objReader["crewname"] != null ? objReader["crewname"].ToString() : string.Empty;
                        crew.StaffNumber = objReader["staffnumber"] != null ? objReader["staffnumber"].ToString() : string.Empty;
                        crew.StaffGrade = objReader["grade"] != null ? objReader["grade"].ToString() : string.Empty;
                        crew.IsActingCSD = objReader["actingcsd"] != null ? objReader["actingcsd"].ToString() : string.Empty;
                        crew.CabinCrewPosition = objReader["cabincrewposition"] != null ? objReader["cabincrewposition"].ToString() : string.Empty;
                        crew.CabinCrewPositionVal = objReader["cabincrewpositionval"] != null ? objReader["cabincrewpositionval"].ToString() : string.Empty;
                        crew.AnnounceLang = objReader["Announcelang"] != null ? objReader["Announcelang"].ToString() : string.Empty;
                        crew.AnnounceLangVal = objReader["announcelangval"] != null ? objReader["announcelangval"].ToString() : string.Empty;
                        crew.StaffGradeId = objReader["gradeid"] != null ? objReader["gradeid"].ToString() : string.Empty;
                        result.Add(crew);
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

        private async void UpdateCrewDetails(FlightInfoEO inputs, string staffId)
        {
            DeleteFlightCrewDetails(inputs.FlightDetsID);
            foreach (var item in inputs.FlightCrewsDetail)
            {
                InsertCrewDetails(item, inputs.FlightDetsID, staffId);
            }
        }

        private async void DeleteFlightCrewDetails(string id)
        {
            DBFramework dbframework = new DBFramework(Constants.CONNECTION_STR);
            try
            {
                List<CommandParameter> parameterList = new List<CommandParameter>();
                parameterList.Add(new CommandParameter("pi_flightdetsid", id, ParameterDirection.Input, DbType.String));
                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("ivrs_master_data_pkg.DeleteFlightCrewsDetails", parameterList);
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

        private async void InsertCrewDetails(FlightCrewsEO inputs, string flightDetsId, string staffId)
        {
            List<CommandParameter> parameterList = null;
            DBFramework dbframework = new DBFramework(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<CommandParameter>();
                parameterList.Add(new CommandParameter("pi_flightdetsid", flightDetsId, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_crewid", inputs.FlightCrewId, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_actingcsd", ((inputs.IsActingCSD != null && inputs.IsActingCSD != string.Empty) ? inputs.IsActingCSD : "N"), ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_userid", staffId, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_cabincrewposition", inputs.CabinCrewPosition, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("pi_Announcelang", inputs.AnnounceLang, ParameterDirection.Input, DbType.String));

                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("ivrs_master_data_pkg.InsertCrewDetails", parameterList);
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

        public async void DeleteFlightDetails(string id)
        {
            DBFramework dbframework = new DBFramework(Constants.CONNECTION_STR);
            try
            {
                List<CommandParameter> parameterList = new List<CommandParameter>();
                parameterList.Add(new CommandParameter("pi_flightnumber", id, ParameterDirection.Input, DbType.String));
                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("ivrs_master_data_pkg.DeleteFlightDetails", parameterList);
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

        public async Task<string> IsDelayRportForFlight(string id)
        {
            DBFramework dbframework = new DBFramework(Constants.CONNECTION_STR);
            string result = string.Empty;

            try
            {
                List<CommandParameter> parameterList = new List<CommandParameter>();
                parameterList.Add(new CommandParameter("pi_flight_dets_id", id, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("po_is_DelayForFlight", ParameterDirection.Output, DbType.String,5));
                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("ivrs_master_data_pkg.IsDelayForFlight", parameterList);
                result = command.Parameters["po_is_DelayForFlight"] != null ? command.Parameters["po_is_DelayForFlight"].Value.ToString() : string.Empty;
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

        public async Task<string> IsVrForFlight(string id)
        {
            DBFramework dbframework = new DBFramework(Constants.CONNECTION_STR);
            string result = string.Empty;

            try
            {
                List<CommandParameter> parameterList = new List<CommandParameter>();
                parameterList.Add(new CommandParameter("pi_flight_dets_id", id, ParameterDirection.Input, DbType.String));
                parameterList.Add(new CommandParameter("po_is_VrForFlight", ParameterDirection.Output, DbType.String, 5));
                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("ivrs_master_data_pkg.IsVrForFlight", parameterList);
                result = command.Parameters["po_is_VrForFlight"] != null ? command.Parameters["po_is_VrForFlight"].Value.ToString() : string.Empty;
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