using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Enterprise;
using QR.IPrism.Utility;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.EntityObjects.Module;
using System.Data;
using System.Data.OracleClient;
using System.Data.Common;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using QR.IPrism.DataAccess.Interfaces;
using QR.IPrism.EntityObjects.Shared;

namespace QR.IPrism.DataAccess.Implementation
{
    public class KafouDao : IKafouDao
    {

        public async Task<List<RecognisedCrewDetailsEO>> GetWallOfFameRecognitionListAsyc()
        {
            List<RecognisedCrewDetailsEO> response = new List<RecognisedCrewDetailsEO>();
            RecognisedCrewDetailsEO recognisedCrewDetails = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("cur_my_recog", ParameterDirection.Output, OracleDbType.RefCursor));

                //using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PN_CREW_RECOGNITION_PKG.sp_search_my_recognition", parameterList))
                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PN_CREW_RECOGNITION_PKG.sp_get_walloffame_recgn", parameterList))
                {
                    while (objReader.Read())
                    {
                        recognisedCrewDetails = new RecognisedCrewDetailsEO();
                        recognisedCrewDetails.RecognitionId = objReader["recognition_id"] != null ? objReader["recognition_id"].ToString() : string.Empty;

                        DateTime RecognisedDate;
                        DateTime.TryParse(objReader["recognisedDate"].ToString(), out RecognisedDate);
                        recognisedCrewDetails.RecognitionDate = RecognisedDate;

                        recognisedCrewDetails.RecognitionType = objReader["recognitionType"] != null ? objReader["recognitionType"].ToString() : string.Empty;
                        recognisedCrewDetails.FlightDetails = objReader["flightDetails"] != null ? objReader["flightDetails"].ToString() : string.Empty;
                        recognisedCrewDetails.FlightNumber = objReader["flightNumber"] != null ? objReader["flightNumber"].ToString() : string.Empty;
                        recognisedCrewDetails.ApproverComments = objReader["apv_rej_comments"] != null ? objReader["apv_rej_comments"].ToString() : string.Empty;
                        recognisedCrewDetails.ApproverCrewID = objReader["apv_rej_crewdetails_id"] != null ? objReader["apv_rej_crewdetails_id"].ToString() : string.Empty;
                        recognisedCrewDetails.OverallComments = objReader["overallcomments"] != null ? objReader["overallcomments"].ToString() : string.Empty;
                        recognisedCrewDetails.RecognisedStaffNumber = objReader["RecognisedCrewStaffNo"] != null ? objReader["RecognisedCrewStaffNo"].ToString() : string.Empty;
                        recognisedCrewDetails.RecognisedStaffName = objReader["RecognisedCrewStaffName"] != null ? objReader["RecognisedCrewStaffName"].ToString() : string.Empty;
                        recognisedCrewDetails.ApproverStaffNumber = objReader["ApproverStaffNo"] != null ? objReader["ApproverStaffNo"].ToString() : string.Empty;
                        recognisedCrewDetails.ApproverStaffName = objReader["ApproverStaffName"] != null ? objReader["ApproverStaffName"].ToString() : string.Empty;

                        response.Add(recognisedCrewDetails);
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

        /// <summary>
        /// Search My Recognition Information approved by Manager for iPrism
        /// </summary>
        /// <param name="eoSearchCrewRecognition"></param>
        /// <returns></returns>
        public async Task<List<SearchRecognitionResultEO>> SearchMyRecognitionInfoAsyc(SearchRecognitionRequestEO eoSearchCrewRecognition, string staffNumber)
        {
            List<SearchRecognitionResultEO> response = new List<SearchRecognitionResultEO>();
            SearchRecognitionResultEO recognisedCrewDetails = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("p_crew_det_id", staffNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("p_from_date", Common.ToOracleDate(eoSearchCrewRecognition.FromDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("p_to_date", Common.ToOracleDate(eoSearchCrewRecognition.ToDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("cur_my_recog", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PN_CREW_RECOGNITION_PKG.sp_search_my_recognition", parameterList))
                {
                    while (objReader.Read())
                    {
                        recognisedCrewDetails = new SearchRecognitionResultEO();
                        recognisedCrewDetails.RecognitionId = objReader["recognition_id"] != null ? objReader["recognition_id"].ToString() : string.Empty;

                        DateTime RecognisedDate;
                        DateTime.TryParse(objReader["recognisedDate"].ToString(), out RecognisedDate);
                        recognisedCrewDetails.RecognitionDate = RecognisedDate;

                        recognisedCrewDetails.RecognitionStatus = objReader["recognitionStatus"] != null ? objReader["recognitionStatus"].ToString() : string.Empty;
                        recognisedCrewDetails.RecognitionStatusID = objReader["recognitionStatusID"] != null ? objReader["recognitionStatusID"].ToString() : string.Empty;
                        recognisedCrewDetails.RecognitionType = objReader["recognitionType"] != null ? objReader["recognitionType"].ToString() : string.Empty;
                        recognisedCrewDetails.RecognitionTypeID = objReader["recognitionTypeID"] != null ? objReader["recognitionTypeID"].ToString() : string.Empty;
                        recognisedCrewDetails.FlightDetails = objReader["flightDetails"] != null ? objReader["flightDetails"].ToString() : string.Empty;
                        recognisedCrewDetails.FlightNumber = objReader["flightNumber"] != null ? objReader["flightNumber"].ToString() : string.Empty;
                        recognisedCrewDetails.FlightID = objReader["flightdetails_id"] != null ? objReader["flightdetails_id"].ToString() : string.Empty;

                        response.Add(recognisedCrewDetails);
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


        public async Task<List<FlightDetailsEO>> SearchFlightsForCCRAsyc(string userID)
        {
            List<FlightDetailsEO> response = new List<FlightDetailsEO>();
            FlightDetailsEO singleRes = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("pi_user_id", userID, ParameterDirection.Input, OracleDbType.Varchar2));
                //parameterList.Add(new ODPCommandParameter("pi_pageindex", string.Empty, ParameterDirection.Input, OracleDbType.Date));
                //parameterList.Add(new ODPCommandParameter("pi_pagesize", string.Empty, ParameterDirection.Input, OracleDbType.Date));
                //parameterList.Add(new ODPCommandParameter("pi_orderbycolumn", string.Empty, ParameterDirection.Input, OracleDbType.Date));
                //parameterList.Add(new ODPCommandParameter("pi_sortorder", string.Empty, ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("cur_flights_detail", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PN_CREW_RECOGNITION_PKG.get_flight_details_for_ccr_mig", parameterList))
                {
                    while (objReader.Read())
                    {
                        singleRes = new FlightDetailsEO();

                        singleRes.FlightDetsID = objReader["flightdetsid"].ToString();
                        singleRes.FlightNumber = objReader["flightNumber"].ToString();
                        singleRes.Sector = objReader["sector"].ToString();
                        singleRes.SectorFrom = objReader["sectorfrom"].ToString();
                        singleRes.SectorTo = objReader["sectorto"].ToString();

                        DateTime actualArrTime;
                        DateTime.TryParse(objReader["actarrdate"].ToString(), out actualArrTime);
                        singleRes.ActArrTime = actualArrTime;
                        DateTime actualDeptTime;
                        DateTime.TryParse(objReader["actdeptdate"].ToString(), out actualDeptTime);
                        singleRes.ActDeptTime = actualDeptTime;

                        singleRes.AirCraftRegNo = objReader["aircraftregno"].ToString().ToUpper();
                        singleRes.AirCraftType = objReader["aircrafttype"].ToString();
                        singleRes.ScheArrTime = DateTime.Parse(objReader["schearrdate"].ToString());
                        singleRes.ScheDeptTime = DateTime.Parse(objReader["schedeptdate"].ToString());

                        singleRes.DelayTags = objReader["delay_tags"] != null ? objReader["delay_tags"].ToString() : string.Empty;

                        response.Add(singleRes);
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


        public async Task<List<SearchRecognitionResultEO>> SearchCrewRecognitionInfoAsyc(SearchRecognitionRequestEO eoSearchCrewRecognition)
        {
            List<SearchRecognitionResultEO> response = new List<SearchRecognitionResultEO>();
            SearchRecognitionResultEO singleRes = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("in_recognisedby", eoSearchCrewRecognition.RecognisedCrewID != null ? 
                    eoSearchCrewRecognition.RecognisedCrewID : string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("in_grade", eoSearchCrewRecognition.GradeList != null ?
                    eoSearchCrewRecognition.GradeList : string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("in_recog_status", eoSearchCrewRecognition.RecognitionStatusList != null ?
                    eoSearchCrewRecognition.RecognitionStatusList : string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("in_flight_no", eoSearchCrewRecognition.FlightNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_from_date", Common.ToOracleDate(eoSearchCrewRecognition.FromDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("in_to_date", Common.ToOracleDate(eoSearchCrewRecognition.ToDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("in_sector_from", eoSearchCrewRecognition.SectorFrom, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_sector_to", eoSearchCrewRecognition.SectorTo, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_crew_id", eoSearchCrewRecognition.SubmittedByCrew, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("cur_flights_detail", ParameterDirection.Output, OracleDbType.RefCursor));

                //using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PN_CREW_RECOGNITION_PKG.SP_SEARCH_RECOGNITION_CREW", parameterList))
                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_kafou.sp_search_recognition_crew_mig", parameterList))
                {
                    while (objReader.Read())
                    {
                        singleRes = new SearchRecognitionResultEO();

                        singleRes.RecognitionId = objReader["recognition_id"] != null ? objReader["recognition_id"].ToString() : string.Empty;

                        DateTime RecognisedDate;
                        DateTime.TryParse(objReader["recognisedDate"].ToString(), out RecognisedDate);
                        singleRes.RecognitionDate = RecognisedDate;

                        singleRes.RecognitionStatus = objReader["recognitionStatus"] != null ? objReader["recognitionStatus"].ToString() : string.Empty;
                        singleRes.RecognitionStatusID = objReader["recognitionStatusID"] != null ? objReader["recognitionStatusID"].ToString() : string.Empty;
                        singleRes.RecognitionType = objReader["recognitionType"] != null ? objReader["recognitionType"].ToString() : string.Empty;
                        singleRes.RecognitionTypeID = objReader["recognitionTypeID"] != null ? objReader["recognitionTypeID"].ToString() : string.Empty;
                        singleRes.FlightDetails = objReader["flightDetails"] != null ? objReader["flightDetails"].ToString() : string.Empty;
                        singleRes.StaffDetails = objReader["staffDetails"] != null ? objReader["staffDetails"].ToString() : string.Empty;
                        singleRes.StaffNumber = objReader["staffNumber"] != null ? objReader["staffNumber"].ToString() : string.Empty;
                        singleRes.FlightNumber = objReader["flightNumber"] != null ? objReader["flightNumber"].ToString() : string.Empty;
                        singleRes.FlightID = objReader["flightdetails_id"] != null ? objReader["flightdetails_id"].ToString() : string.Empty;

                        response.Add(singleRes);
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


        public async Task<SearchCrewRecognitionEO> GetCrewStatusSourceGradeDataAsyc()
        {
            SearchCrewRecognitionEO searchCrewRecognitionModel = new SearchCrewRecognitionEO();
            searchCrewRecognitionModel.RecognitionStatusList = new List<LookupEO>();
            searchCrewRecognitionModel.RecognitionSourceList = new List<LookupEO>();
            searchCrewRecognitionModel.RecognisedCrewGradeList = new List<LookupEO>();
            searchCrewRecognitionModel.SubmittedCrewGradeList = new List<LookupEO>();
            searchCrewRecognitionModel.RecognitionTypeList = new List<LookupEO>();

            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("cur_recog_status", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_recog_src", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_grade", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_submcrew_grade", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_recog_type", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PN_CREW_RECOGNITION_PKG.SP_GET_RECOG_STATUS_SRC_GRADE", parameterList))
                {
                        while (objReader.Read())
                        {
                            LookupEO crewStatus = new LookupEO();
                            crewStatus.Value = objReader["LOOKUPID"] != null ? objReader["LOOKUPID"].ToString() : string.Empty;
                            crewStatus.Text = objReader["LOOKUPNAME"] != null ? objReader["LOOKUPNAME"].ToString() : string.Empty;
                            searchCrewRecognitionModel.RecognitionStatusList.Add(crewStatus);
                        }

                        objReader.NextResult();
                        while (objReader.Read())
                        {
                            LookupEO crewSrc = new LookupEO();
                            crewSrc.Value = objReader["LOOKUPID"] != null ? objReader["LOOKUPID"].ToString() : string.Empty;
                            crewSrc.Text = objReader["LOOKUPNAME"] != null ? objReader["LOOKUPNAME"].ToString() : string.Empty;
                            searchCrewRecognitionModel.RecognitionSourceList.Add(crewSrc);
                        }

                        objReader.NextResult();
                        while (objReader.Read())
                        {
                            LookupEO recogCrewGrade = new LookupEO();
                            recogCrewGrade.Value = objReader["GRADEID"] != null ? objReader["GRADEID"].ToString() : string.Empty;
                            recogCrewGrade.Text = objReader["GRADE"] != null ? objReader["GRADE"].ToString() : string.Empty;
                            searchCrewRecognitionModel.RecognisedCrewGradeList.Add(recogCrewGrade);
                        }

                        objReader.NextResult();
                        while (objReader.Read())
                        {
                            LookupEO subCrewGrade = new LookupEO();
                            subCrewGrade.Value = objReader["GRADEID"] != null ? objReader["GRADEID"].ToString() : string.Empty;
                            subCrewGrade.Text = objReader["GRADE"] != null ? objReader["GRADE"].ToString() : string.Empty;
                            searchCrewRecognitionModel.SubmittedCrewGradeList.Add(subCrewGrade);
                        }
                        objReader.NextResult();
                        while (objReader.Read())
                        {
                            LookupEO crewType = new LookupEO();
                            crewType.Value = objReader["LOOKUPID"] != null ? objReader["LOOKUPID"].ToString() : string.Empty;
                            crewType.Text = objReader["LOOKUPNAME"] != null ? objReader["LOOKUPNAME"].ToString() : string.Empty;
                            searchCrewRecognitionModel.RecognitionTypeList.Add(crewType);
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

            return searchCrewRecognitionModel;
        }


        public async Task<CrewRecognitionEO> GetInitialCrewRecognitionAsyc(string flightId, string crewDetails)
        {
            var getInitialCrewRecognition = new CrewRecognitionEO();
            var flightDetails = new FlightDetailsEO();

            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("in_FLIGHTDETSID", flightId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_CREWDETSID", crewDetails, ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("cur_flight_detls", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_crew_complement", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_crew_detls", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_recog_qrval", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PN_CREW_RECOGNITION_PKG.SP_GET_INTL_RECOGNITION", parameterList))
                {
                    while (objReader.Read())
                    {
                        //Populate Flight Details.
                        flightDetails.FlightDetsID = (objReader["FLIGHTDETSID"]) != null ? Convert.ToString(objReader["FLIGHTDETSID"]) : string.Empty;
                        flightDetails.FlightNumber = (objReader["FLIGHTNUMBER"]) != null ? Convert.ToString(objReader["FLIGHTNUMBER"]) : string.Empty;
                        flightDetails.AirCraftFamily = (objReader["AIRCRAFT_FAMILY"]) != null ? Convert.ToString(objReader["AIRCRAFT_FAMILY"]) : string.Empty;
                        flightDetails.ActDeptTime = Convert.ToDateTime(objReader["FLIGHTDATE"]);
                        flightDetails.AirCraftType = (objReader["ACTYPE"]) != null ? Convert.ToString(objReader["ACTYPE"]) : string.Empty;
                        flightDetails.AirCraftRegNo = (objReader["AIRCRAFTREGNO"]) != null ? Convert.ToString(objReader["AIRCRAFTREGNO"]) : string.Empty;
                        flightDetails.SectorFrom = (objReader["SECTORFROM"]) != null ? Convert.ToString(objReader["SECTORFROM"]) : string.Empty;
                        flightDetails.SectorTo = (objReader["SECTORTO"]) != null ? Convert.ToString(objReader["SECTORTO"]) : string.Empty;

                        flightDetails.SeatCapacityFC = int.Parse(Convert.ToString(objReader["SEATCAPACITYFC"]));
                        flightDetails.SeatCapacityJC = int.Parse(Convert.ToString(objReader["SEATCAPACITYJC"]));
                        flightDetails.SeatCapacityYC = int.Parse(Convert.ToString(objReader["SEATCAPACITYYC"]));

                        flightDetails.PassengerLoadFC = int.Parse(Convert.ToString(objReader["PASSENGERLOADFC"]));
                        flightDetails.PassengerLoadJC = int.Parse(Convert.ToString(objReader["PASSENGERLOADJC"]));
                        flightDetails.PassengerLoadYC = int.Parse(Convert.ToString(objReader["PASSENGERLOADYC"]));

                        flightDetails.InfantLoadFC = int.Parse(Convert.ToString(objReader["INFANTLOADFC"]));
                        flightDetails.InfantLoadJC = int.Parse(Convert.ToString(objReader["INFANTLOADJC"]));
                        flightDetails.InfantLoadYC = int.Parse(Convert.ToString(objReader["INFANTLOADYC"]));

                        //Check this later

                        //switch (objReader["ISGROOMINGCHECK"].ToString())
                        //{
                        //    case UtilityConstants.Constants.INVALIDSTAFFID: flightDetails.IsGroomingCheck = UtilityConstants.Constants.NA;
                        //        break;

                        //    case UtilityConstants.Constants.YES: flightDetails.IsGroomingCheck = UtilityConstants.Constants.Yes;
                        //        break;

                        //    case UtilityConstants.Constants.NO: flightDetails.IsGroomingCheck = UtilityConstants.Constants.No;
                        //        break;

                        //}
                        //switch (objReader["ISCSDCSBRIEFED"].ToString())
                        //{
                        //    case UtilityConstants.Constants.INVALIDSTAFFID: flightDetails.IsCsdCsBriefed = UtilityConstants.Constants.NA;
                        //        break;

                        //    case UtilityConstants.Constants.YES: flightDetails.IsCsdCsBriefed = UtilityConstants.Constants.Yes;
                        //        break;

                        //    case UtilityConstants.Constants.NO: flightDetails.IsCsdCsBriefed = UtilityConstants.Constants.No;
                        //        break;

                        //}

                        getInitialCrewRecognition.FlightDetails = flightDetails;
                    }
                    objReader.NextResult();


                    while (objReader.Read())
                    {
                        flightDetails.CrewComplement = objReader["crewcomplement"] != null ? objReader["crewcomplement"].ToString() : string.Empty;
                    }
                    objReader.NextResult();

                    while (objReader.Read())
                    {
                        //Populate Staff Details.
                        var crStaffDetails = new StaffDetailsEO();
                        crStaffDetails.StaffDetails.CrewDetailsId = objReader["CREWDETAILSID"] != null ? objReader["CREWDETAILSID"].ToString() : string.Empty;
                        crStaffDetails.StaffDetails.StaffNumber = objReader["STAFFNO"] != null ? objReader["STAFFNO"].ToString() : string.Empty;
                        crStaffDetails.StaffDetails.StaffName = objReader["STAFFNAME"] != null ? objReader["STAFFNAME"].ToString() : string.Empty;
                        crStaffDetails.StaffDetails.StaffGradeId = objReader["GRADEID"] != null ? objReader["GRADEID"].ToString() : string.Empty;
                        crStaffDetails.StaffDetails.StaffGrade = objReader["GRADE"] != null ? objReader["GRADE"].ToString() : string.Empty;
                        crStaffDetails.StaffDetails.CabinCrewPositionVal = objReader["WORKPOSITION"] != null ? objReader["WORKPOSITION"].ToString() : string.Empty;
                        getInitialCrewRecognition.StaffDetailsList.Add(crStaffDetails);
                    }
                    objReader.NextResult();

                    while (objReader.Read())
                    {
                        //Populate QR Value Details.
                        var crQRValue = new RecognitionQRValueEO();
                        crQRValue.Id = objReader["LOOKUPID"] != null ? objReader["LOOKUPID"].ToString() : string.Empty;
                        crQRValue.QRValue = objReader["LOOKUPNAME"] != null ? objReader["LOOKUPNAME"].ToString() : string.Empty;
                        getInitialCrewRecognition.RecognitionQRValueList.Add(crQRValue);
                    }
                    objReader.NextResult();
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

            return getInitialCrewRecognition;

            
        }


        public async Task<CrewRecognitionEO> GetCrewRecognitionAsyc(string recognitionID)
        {
            CrewRecognitionEO crewRecognition = new CrewRecognitionEO();
            var flightDetails = new FlightDetailsEO();
            IList<StaffDetailsEO> staffDetailsList = new List<StaffDetailsEO>();
            IList<RecognitionQRValueEO> recognisedQrValuesList = new List<RecognitionQRValueEO>();

            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("in_Recognition_ID", recognitionID, ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("cur_flight_detls", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_crew_detls", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_attach_detls", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_recog_qrvals", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_crew_complement", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_recog_qrval_lookup", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PN_CREW_RECOGNITION_PKG.SP_GET_CREW_RECOGNITION", parameterList))
                {
                    while (objReader.Read())
                    {
                        crewRecognition.RecognitionId = (objReader["RECOGNITION_ID"]) != null ? Convert.ToString(objReader["RECOGNITION_ID"]) : string.Empty;
                        crewRecognition.RecognitionType.Id = (objReader["RECOGNITIONTYPEID"]) != null ? Convert.ToString(objReader["RECOGNITIONTYPEID"]) : string.Empty;
                        crewRecognition.RecognitionType.Type = (objReader["RECOGNITIONTYPE"]) != null ? Convert.ToString(objReader["RECOGNITIONTYPE"]) : string.Empty;
                        crewRecognition.RecognitionSource.Id = (objReader["RECOGNITIONSOURCEID"]) != null ? Convert.ToString(objReader["RECOGNITIONSOURCEID"]) : string.Empty;
                        crewRecognition.RecognitionSource.Source = (objReader["RECOGNITIONSOURCE"]) != null ? Convert.ToString(objReader["RECOGNITIONSOURCE"]) : string.Empty;
                        crewRecognition.RecognitionStatusName = (objReader["RECOGNITION_STATUS"]) != null ? Convert.ToString(objReader["RECOGNITION_STATUS"]) : string.Empty;
                        flightDetails.FlightDetsID = (objReader["FLIGHTDETSID"]) != null ? Convert.ToString(objReader["FLIGHTDETSID"]) : string.Empty;
                        flightDetails.FlightNumber = (objReader["FLIGHTNUMBER"]) != null ? Convert.ToString(objReader["FLIGHTNUMBER"]) : string.Empty;
                        flightDetails.ActDeptTime = Convert.ToDateTime(objReader["FLIGHTDATE"]);
                        flightDetails.AirCraftType = (objReader["ACTYPE"]) != null ? Convert.ToString(objReader["ACTYPE"]) : string.Empty;
                        flightDetails.AirCraftRegNo = (objReader["AIRCRAFTREGNO"]) != null ? Convert.ToString(objReader["AIRCRAFTREGNO"]) : string.Empty;
                        flightDetails.AirCraftFamily = (objReader["AIRCRAFT_FAMILY"]) != null ? Convert.ToString(objReader["AIRCRAFT_FAMILY"]) : string.Empty;

                        flightDetails.SeatCapacityFC = int.Parse(Convert.ToString(objReader["SEATCAPACITYFC"]));
                        flightDetails.SeatCapacityJC = int.Parse(Convert.ToString(objReader["SEATCAPACITYJC"]));
                        flightDetails.SeatCapacityYC = int.Parse(Convert.ToString(objReader["SEATCAPACITYYC"]));

                        flightDetails.SectorFrom = (objReader["SECTORFROM"]) != null ? Convert.ToString(objReader["SECTORFROM"]) : string.Empty;
                        flightDetails.SectorTo = (objReader["SECTORTO"]) != null ? Convert.ToString(objReader["SECTORTO"]) : string.Empty;

                        flightDetails.PassengerLoadFC = int.Parse(Convert.ToString(objReader["PASSENGERLOADFC"]));
                        flightDetails.PassengerLoadJC = int.Parse(Convert.ToString(objReader["PASSENGERLOADJC"]));
                        flightDetails.PassengerLoadYC = int.Parse(Convert.ToString(objReader["PASSENGERLOADYC"]));

                        flightDetails.InfantLoadFC = int.Parse(Convert.ToString(objReader["INFANTLOADFC"]));
                        flightDetails.InfantLoadJC = int.Parse(Convert.ToString(objReader["INFANTLOADJC"]));
                        flightDetails.InfantLoadYC = int.Parse(Convert.ToString(objReader["INFANTLOADYC"]));
                        //switch (objReader["ISGROOMINGCHECK"].ToString())
                        //{
                        //    case UtilityConstants.Constants.INVALIDSTAFFID: flightDetails.IsGroomingCheck = UtilityConstants.Constants.NA;
                        //        break;

                        //    case UtilityConstants.Constants.YES: flightDetails.IsGroomingCheck = UtilityConstants.Constants.Yes;
                        //        break;

                        //    case UtilityConstants.Constants.NO: flightDetails.IsGroomingCheck = UtilityConstants.Constants.No;
                        //        break;

                        //}
                        //switch (objReader["ISCSDCSBRIEFED"].ToString())
                        //{
                        //    case UtilityConstants.Constants.INVALIDSTAFFID: flightDetails.IsCsdCsBriefed = UtilityConstants.Constants.NA;
                        //        break;

                        //    case UtilityConstants.Constants.YES: flightDetails.IsCsdCsBriefed = UtilityConstants.Constants.Yes;
                        //        break;

                        //    case UtilityConstants.Constants.NO: flightDetails.IsCsdCsBriefed = UtilityConstants.Constants.No;
                        //        break;

                        //}

                        crewRecognition.RaisedByStaffNo = (objReader["RAISED_BY_STAFF_NO"]) != null ? Convert.ToString(objReader["RAISED_BY_STAFF_NO"]) : string.Empty;
                        crewRecognition.PassengerDetails = (objReader["PASSENGER_DETAILS"]) != null ? Convert.ToString(objReader["PASSENGER_DETAILS"]) : string.Empty;
                        //crewRecognition.EvrNo = (objReader["EVRNO"]) != null ? Convert.ToInt32(objReader["EVRNO"]) : 0;
                        crewRecognition.EvrNo = (objReader["EVRNO"]) != null ? (objReader["EVRNO"]).ToString() : "0";
                        crewRecognition.RecognitionComments = (objReader["RECOGNITION_COMMENTS"]) != null ?
                        Convert.ToString(objReader["RECOGNITION_COMMENTS"]) : string.Empty;
                        crewRecognition.AdditionalComments = (objReader["ADDITIONAL_COMMENTS"]) != null ? Convert.ToString(objReader["ADDITIONAL_COMMENTS"]) : string.Empty;
                        crewRecognition.ApvRejComments = (objReader["APV_REJ_COMMENTS"]) != null ?
                        Convert.ToString(objReader["APV_REJ_COMMENTS"]) : string.Empty;
                        crewRecognition.IsPostToWall = (objReader["IS_POST_TO_WALL"]) != null ?
                        Convert.ToString(objReader["IS_POST_TO_WALL"]) : string.Empty;
                        crewRecognition.SubmittedBy = (objReader["RECOGNISEDBY"]) != null ? Convert.ToString(objReader["RECOGNISEDBY"]) : string.Empty;
                        crewRecognition.FlightDetails = flightDetails;

                    }
                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        StaffDetailsEO staffDetails = new StaffDetailsEO();
                        staffDetails.RecognitionId = (objReader["RECOGNITION_ID"]) != null ?
                        Convert.ToString(objReader["RECOGNITION_ID"]) : string.Empty;
                        staffDetails.RecognitionDetailsId = (objReader["RECOGNITION_DTLS_ID"]) != null ?
                        Convert.ToString(objReader["RECOGNITION_DTLS_ID"]) : string.Empty;
                        staffDetails.StaffDetails.CrewDetailsId = (objReader["CREWDETAILSID"]) != null ?
                        Convert.ToString(objReader["CREWDETAILSID"]) : string.Empty;
                        staffDetails.StaffDetails.StaffNumber = (objReader["STAFFNO"]) != null ?
                        Convert.ToString(objReader["STAFFNO"]) : string.Empty;
                        staffDetails.StaffDetails.StaffName = (objReader["STAFFNAME"]) != null ?
                        Convert.ToString(objReader["STAFFNAME"]) : string.Empty;
                        staffDetails.StaffDetails.StaffGradeId = (objReader["GRADEID"]) != null ?
                        Convert.ToString(objReader["GRADEID"]) : string.Empty;
                        staffDetails.StaffDetails.StaffGrade = (objReader["GRADE"]) != null ?
                        Convert.ToString(objReader["GRADE"]) : string.Empty;
                        staffDetails.StaffDetails.CabinCrewPositionVal = (objReader["WORKPOSITION"]) != null ?
                        Convert.ToString(objReader["WORKPOSITION"]) : string.Empty;
                        staffDetails.StaffDetails.FlightDetsId = (objReader["FLIGHTDETSID"]) != null ?
                        Convert.ToString(objReader["FLIGHTDETSID"]) : string.Empty;
                        staffDetailsList.Add(staffDetails);
                    }
                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        FileEO attachments = new FileEO();
                        attachments.FileId = (objReader["ATTACHMENTID"]) != null ?
                            Convert.ToString(objReader["ATTACHMENTID"]) : string.Empty;
                        attachments.FileName = (objReader["ATTACHMENTNAME"]) != null ?
                            Convert.ToString(objReader["ATTACHMENTNAME"]) : string.Empty;

                        attachments.FileContent = objReader["ATTACHMENT"] as byte[];

                        //crewRecognition.ListOfKendoFileUploaded.Add(attachments);
                        crewRecognition.AttachmentList.Add(attachments);

                    }
                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        RecognitionQRValueEO QRValues = new RecognitionQRValueEO();
                        QRValues.RecognitionDetailsId = (objReader["RECOGNITION_DTLS_ID"]) != null ?
                        Convert.ToString(objReader["RECOGNITION_DTLS_ID"]) : string.Empty;
                        QRValues.Id = (objReader["QRVALUESID"]) != null ?
                        Convert.ToString(objReader["QRVALUESID"]) : string.Empty;
                        QRValues.QRValue = (objReader["QRVALUESNAME"]) != null ?
                        Convert.ToString(objReader["QRVALUESNAME"]) : string.Empty;
                        recognisedQrValuesList.Add(QRValues);

                    }
                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        flightDetails.CrewComplement = objReader["crewcomplement"] != null ? objReader["crewcomplement"].ToString() : string.Empty;
                    }
                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        var crQRValue = new RecognitionQRValueEO();
                        crQRValue.Id = objReader["LOOKUPID"] != null ? objReader["LOOKUPID"].ToString() : string.Empty;
                        crQRValue.QRValue = objReader["LOOKUPNAME"] != null ? objReader["LOOKUPNAME"].ToString() : string.Empty;
                        crewRecognition.RecognitionQRValueList.Add(crQRValue);
                    }
                }

                foreach (var staffDetails in staffDetailsList)
                {
                    var filterQrValuesList = recognisedQrValuesList.Where(x => x.RecognitionDetailsId == staffDetails.RecognitionDetailsId).ToList();
                    foreach (var qrValues in filterQrValuesList)
                    {
                        staffDetails.RecognitionQRValueList.Add(qrValues);
                    }
                    crewRecognition.StaffDetailsList.Add(staffDetails);
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

            return crewRecognition;

        }


        public async Task<List<FileEO>> GetRecognitionAttachmentsAsyc(string recognitionID)
        {
            List<FileEO> attachmentList = new List<FileEO>();
            FileEO attachment = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("in_recognition_id", recognitionID, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("cur_attachment", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PN_CREW_RECOGNITION_PKG.SP_GET_RECOGN_ATTACHMENT", parameterList))
                {
                    while (objReader.Read())
                    {
                        attachment = new FileEO();
                        attachment.FileId = objReader["attachmentid"].ToString();
                        attachment.FileContent = objReader["attachment"] as byte[];
                        attachment.FileName = objReader["attachmentname"].ToString();
                        attachment.FileType = objReader["contenttype"].ToString();
                        attachment.ClassKeyId = objReader["contentid"].ToString();

                        attachmentList.Add(attachment);
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

            return attachmentList;

        }

        /// <summary>
        /// Get Recognition Type and Source
        /// </summary>
        /// <returns></returns>
        public async Task<CommonRecognitionEO> GetRecognitionTypeSourceAsyc()
        {
            CommonRecognitionEO recognitionTypeSource = new CommonRecognitionEO();
            List<LookupEO> recognitionSourceList = new List<LookupEO>();
            List<LookupEO> recognitionTypeList = new List<LookupEO>();

            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("cur_recog_type", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_recog_src", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PN_CREW_RECOGNITION_PKG.SP_GET_RECOGNITION_TYPE_SOURCE", parameterList))
                {
                    while (objReader.Read())
                    {
                        LookupEO selectItem = new LookupEO();
                        selectItem.Value = objReader["LOOKUPID"] != null ? objReader["LOOKUPID"].ToString() : string.Empty;
                        selectItem.Text = objReader["LOOKUPNAME"] != null ? objReader["LOOKUPNAME"].ToString() : string.Empty;
                        recognitionTypeList.Add(selectItem);
                    }

                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        LookupEO recognitionSource = new LookupEO();
                        recognitionSource.Value = objReader["LOOKUPID"] != null ? objReader["LOOKUPID"].ToString() : string.Empty;
                        recognitionSource.Text = objReader["LOOKUPNAME"] != null ? objReader["LOOKUPNAME"].ToString() : string.Empty;
                        recognitionSourceList.Add(recognitionSource);
                    }
                    recognitionTypeSource.RecognitionTypeList = recognitionTypeList;
                    recognitionTypeSource.RecognitionSourceList = recognitionSourceList;
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

            return recognitionTypeSource;
        }


        /// <summary>
        /// Insert Update Recognition
        /// </summary>
        /// <param name="model">Get Crew Recognition Model</param>
        /// <returns>Recognition Id</returns>
        public async Task<string> InsertUpdateRecognitionAsyc(CrewRecognitionEO model)
        {
            try
            {
                model.IsActive = Common.ISACTIVE;
                string recognitionId = string.Empty;
                if (model.RecognitionType.Type.Equals(Common.RecognitionType.Individual.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    foreach (StaffDetailsEO crDetails in model.StaffDetailsList)
                    {
                        recognitionId = InsertUpdateCrewRecognition(model).Result.ToString();
                        if (!string.IsNullOrEmpty(recognitionId))
                        {
                            crDetails.RecognitionId = recognitionId;
                            crDetails.IsActive = Common.ISACTIVE;
                            crDetails.CreatedBy = model.SubmittedBy;
                            string recognitionDetailsId = InsertUpdateCrewRecognitionDetails(crDetails).Result.ToString();
                            DeleteRecognitionQrValues(recognitionDetailsId);
                            foreach (RecognitionQRValueEO crData in crDetails.RecognitionQRValueList)
                            {
                                crData.RecognitionDetailsId = recognitionDetailsId.ToString();
                                crData.IsActive = Common.ISACTIVE;
                                crData.CreatedBy = model.SubmittedBy;
                                InsertUpdateCrewRecognitionData(crData);
                            }
                        }

                        var crDetailsHistory = new CrewRecognitionDetailsHistoryEO();
                        crDetailsHistory.RecognitionId = recognitionId;
                        crDetailsHistory.RecognitionStatus = model.RecognitionStatusId;
                        crDetailsHistory.IsActive = Common.ISACTIVE;
                        crDetailsHistory.CreatedBy = model.SubmittedBy;
                        InsertUpdateCrewRecognitionDetailsHistory(crDetailsHistory);

                        //if (model.AttachmentList != null)
                        //{
                        //    if (model.AttachmentList.Any(i => i.AttachmentID != "" && i.AttachmentID != null))
                        //    {
                        //        this.doCabinCrewRecognition.DeleteAllAttachments(recognitionId);
                        //    }

                        //    foreach (AttachmentModel crAttachment in model.AttachmentList)
                        //    {
                        //        crAttachment.ContentID = recognitionId;
                        //        crAttachment.ContentType = CCRConstants.CONTENTTYPE;
                        //        crAttachment.IsActive = CCRConstants.ISACTIVE;
                        //        crAttachment.CreatedBy = model.SubmittedBy;

                        //        this.doCabinCrewRecognition.AddAttachment(crAttachment);
                        //    }
                        //}
                    }
                }
                else
                {
                    recognitionId = InsertUpdateCrewRecognition(model).Result.ToString();
                    if (!string.IsNullOrEmpty(recognitionId))
                    {
                        foreach (StaffDetailsEO crDetails in model.StaffDetailsList)
                        {
                            crDetails.RecognitionId = recognitionId;
                            crDetails.IsActive = Common.ISACTIVE;
                            crDetails.CreatedBy = model.SubmittedBy;
                            string recognitionDetailsId = InsertUpdateCrewRecognitionDetails(crDetails).Result.ToString();
                            DeleteRecognitionQrValues(recognitionDetailsId);
                            foreach (RecognitionQRValueEO crData in crDetails.RecognitionQRValueList)
                            {
                                crData.RecognitionDetailsId = recognitionDetailsId.ToString();
                                crData.IsActive = Common.ISACTIVE;
                                crData.CreatedBy = model.SubmittedBy;
                                InsertUpdateCrewRecognitionData(crData);
                            }
                        }

                        var crDetailsHistory = new CrewRecognitionDetailsHistoryEO();
                        crDetailsHistory.RecognitionId = recognitionId;
                        crDetailsHistory.RecognitionStatus = model.RecognitionStatusId;
                        crDetailsHistory.IsActive = Common.ISACTIVE;
                        crDetailsHistory.CreatedBy = model.SubmittedBy;
                        InsertUpdateCrewRecognitionDetailsHistory(crDetailsHistory);

                        //if (model.AttachmentList != null)
                        //{
                        //    if (model.AttachmentList.Any(i => i.AttachmentID != "" && i.AttachmentID != null))
                        //    {
                        //        this.doCabinCrewRecognition.DeleteAllAttachments(recognitionId);
                        //    }

                        //    foreach (AttachmentModel crAttachment in model.AttachmentList)
                        //    {
                        //        crAttachment.ContentID = recognitionId;
                        //        crAttachment.ContentType = CCRConstants.CONTENTTYPE;
                        //        crAttachment.IsActive = CCRConstants.ISACTIVE;
                        //        crAttachment.CreatedBy = model.SubmittedBy;

                        //        this.doCabinCrewRecognition.AddAttachment(crAttachment);
                        //    }
                        //}
                    }
                }
                return recognitionId;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        #region PRIVATE Methods
        /// <summary>
        /// Insert or update crew recognition.
        /// </summary>
        /// <param name="model">The Crew Recognition Model.</param>
        /// <returns>Recognition Id</returns>
        private async Task<string> InsertUpdateCrewRecognition(CrewRecognitionEO model)
        {
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            DbCommand command;
            string recognitionId = string.Empty;

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("in_recognition_id", String.IsNullOrWhiteSpace(model.RecognitionId) ? "" : 
                    (model.RecognitionId), ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("in_recognition_type", model.RecognitionType.Id, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_recognition_source", model.RecognitionSource.Id, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_flightdetails_id", model.FlightDetails.FlightDetsID, ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("in_raised_by_staff_no", String.IsNullOrWhiteSpace(model.RaisedByStaffNo) ? "" : 
                    (model.RaisedByStaffNo), ParameterDirection.Input, OracleDbType.Varchar2));


                parameterList.Add(new ODPCommandParameter("in_passenger_details", String.IsNullOrWhiteSpace(model.PassengerDetails) ? "" :
                    (model.PassengerDetails), ParameterDirection.Input, OracleDbType.Varchar2));


                parameterList.Add(new ODPCommandParameter("in_evrno",(model.EvrNo == "0" || model.EvrNo == null ) ? "" : model.EvrNo, ParameterDirection.Input, OracleDbType.Varchar2));


                parameterList.Add(new ODPCommandParameter("in_recognition_comments", String.IsNullOrWhiteSpace(model.RecognitionComments) ? "" :
                    (model.RecognitionComments), ParameterDirection.Input, OracleDbType.Varchar2));


                parameterList.Add(new ODPCommandParameter("in_additional_comments", String.IsNullOrWhiteSpace(model.AdditionalComments) ? "" :
                    (model.AdditionalComments), ParameterDirection.Input, OracleDbType.Varchar2));


                parameterList.Add(new ODPCommandParameter("in_apv_rej_comments", String.IsNullOrWhiteSpace(model.ApvRejComments) ? "" :
                   (model.ApvRejComments), ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("in_recognition_status", String.IsNullOrWhiteSpace(model.RecognitionStatusId) ? "" :
                   (model.RecognitionStatusId), ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("in_is_active", model.IsActive, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_user_id", model.SubmittedBy, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_is_post_to_wall", String.IsNullOrWhiteSpace(model.IsPostToWall) ? "" :
                   (model.IsPostToWall), ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("out_recognition_id", ParameterDirection.Output, OracleDbType.Varchar2, 50));

                command = await dbframework.ExecuteSPNonQueryParmAsync("PN_CREW_RECOGNITION_PKG.SP_INSERT_UPDATE_RECOGNITION", parameterList);
                //command = dbframework.ExecuteSPNonQuery("PN_CREW_RECOGNITION_PKG.SP_INSERT_UPDATE_RECOGNITION", parameterList);

                recognitionId = command.Parameters["out_recognition_id"] != null ? command.Parameters["out_recognition_id"].Value.ToString() : string.Empty;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }

            return recognitionId;
        }

        /// <summary>
        /// Insert or update crew recognition details.
        /// </summary>
        /// <param name="model">The Crew Recognition Details Model.</param>
        /// <returns>Recognition Details Id</returns>
        private async Task<string> InsertUpdateCrewRecognitionDetails(StaffDetailsEO model)
        {

            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            DbCommand command;
            string recognitionDetailsId = string.Empty;

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("in_recognition_dtls_id", String.IsNullOrWhiteSpace(model.RecognitionDetailsId) ? "" :
                   (model.RecognitionDetailsId), ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("in_recognition_id", model.RecognitionId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_crewdetails_id", model.StaffDetails.CrewDetailsId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_grade_id", model.StaffDetails.StaffGradeId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_is_active", model.IsActive, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_user_id", model.CreatedBy, ParameterDirection.Input, OracleDbType.Varchar2));


                parameterList.Add(new ODPCommandParameter("out_recognition_dtls_id", ParameterDirection.Output, OracleDbType.Varchar2, 50));

                command = await dbframework.ExecuteSPNonQueryParmAsync("PN_CREW_RECOGNITION_PKG.SP_INSERT_UPDATE_RECOGN_DTLS", parameterList);

                recognitionDetailsId = command.Parameters["out_recognition_dtls_id"] != null ? command.Parameters["out_recognition_dtls_id"].Value.ToString() : string.Empty;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }

            return recognitionDetailsId;
        }

        /// <summary>
        /// Delete Recognition QR Values
        /// </summary>
        /// <param name="Id"></param>
        private async void DeleteRecognitionQrValues(string Id)
        {
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            DbCommand command;

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("in_recog_detls_id", Id, ParameterDirection.Input, OracleDbType.Varchar2));

                command = await dbframework.ExecuteSPNonQueryParmAsync("PN_CREW_RECOGNITION_PKG.SP_DELETE_RECOG_QRVALUES", parameterList);

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

        /// <summary>
        /// Insert or update crew recognition data.
        /// </summary>
        /// <param name="model">The Crew Recognition Data model.</param>
        private async void InsertUpdateCrewRecognitionData(RecognitionQRValueEO model)
        {
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            DbCommand command;

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("in_recognition_data_id", String.IsNullOrWhiteSpace(model.RecognitionDataId) ? "" :
                   (model.RecognitionDataId), ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_recognition_dtls_id", model.RecognitionDetailsId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_recognition_qr_VALUES_id", model.Id, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_is_active", model.IsActive, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_user_id", model.CreatedBy, ParameterDirection.Input, OracleDbType.Varchar2));

                command = await dbframework.ExecuteSPNonQueryParmAsync("PN_CREW_RECOGNITION_PKG.SP_INSERT_UPDATE_RECOGN_DATA", parameterList);

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

        /// <summary>
        /// Insert or update crew recognition details.
        /// </summary>
        /// <param name="model">The Crew Recognition Details Model.</param>
        /// <returns>Recognition Details Id</returns>
        private async void InsertUpdateCrewRecognitionDetailsHistory(CrewRecognitionDetailsHistoryEO model)
        {
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            DbCommand command;

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("in_recognition_history_id", model.RecognitionHistoryId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_recognition_dtls_id", model.RecognitionId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_recognition_status", model.RecognitionStatus, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_is_active", model.IsActive, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("in_user_id", model.CreatedBy, ParameterDirection.Input, OracleDbType.Varchar2));

                command = await dbframework.ExecuteSPNonQueryParmAsync("PN_CREW_RECOGNITION_PKG.SP_INSERT_UPDATE_RECOGN_HSTRY", parameterList);

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

        #endregion


        public async Task<List<FlightDetailsEO>> SearchFlightsForCCR(string userID, int pageIndex, int pageSize, string sortOrderColumn, string orderBy)
        {
             List<FlightDetailsEO> flightDetails = new List<FlightDetailsEO>();

            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            DbCommand command;

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("pi_user_id", userID, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_pageindex", "1", ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_pagesize", "30", ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_orderbycolumn", "", ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_sortorder", "", ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("cur_flights_detail", ParameterDirection.Output, OracleDbType.RefCursor));

                command = await dbframework.ExecuteSPNonQueryParmAsync("PN_CREW_RECOGNITION_PKG.SP_INSERT_UPDATE_RECOGN_HSTRY", parameterList);

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PN_CREW_RECOGNITION_PKG.SP_GET_RECOGNITION_TYPE_SOURCE", parameterList))
                {
                    while (objReader.Read())
                    {
                        var flightDetail = new FlightDetailsEO();

                        flightDetail.FlightDetsID = objReader["flightdetsid"].ToString();
                        flightDetail.FlightNumber = objReader["flightNumber"].ToString();
                        flightDetail.Sector = objReader["sector"].ToString();
                        flightDetail.SectorFrom = objReader["sectorfrom"].ToString();
                        flightDetail.SectorTo = objReader["sectorto"].ToString();

                        DateTime actualArrTime;
                        DateTime.TryParse(objReader["actarrdate"].ToString(), out actualArrTime);
                        flightDetail.ActArrTime = actualArrTime;
                        DateTime actualDeptTime;
                        DateTime.TryParse(objReader["actdeptdate"].ToString(), out actualDeptTime);
                        flightDetail.ActDeptTime = actualDeptTime;

                        flightDetail.AirCraftRegNo = objReader["aircraftregno"].ToString().ToUpper();
                        flightDetail.AirCraftType = objReader["aircrafttype"].ToString();
                        flightDetail.ScheArrTime = DateTime.Parse(objReader["schearrdate"].ToString());
                        flightDetail.ScheDeptTime = DateTime.Parse(objReader["schedeptdate"].ToString());

                        flightDetails.Add(flightDetail);
                    }  
                }

                return flightDetails;
                

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

        /// <summary>
        /// Get Crew Recognition By Flight
        /// </summary>
        /// <param name="flightIdList"></param>
        /// <param name="crewDetailsID"></param>
        /// <returns></returns>
        public async Task<List<CrewRecognitionOverviewEO>> GetCrewRecognitionByFlightAsyc(string flightIdList, string crewDetailsID)
        {
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            DbCommand command;

            List<CrewRecognitionOverviewEO> crewRecognitionList = new List<CrewRecognitionOverviewEO>();
            CrewRecognitionOverviewEO crewRecogn = null;

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("in_flightId", flightIdList, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_crewdetailsid", crewDetailsID, ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("cur_crew_recog", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PN_CREW_RECOGNITION_PKG.sp_get_recognition_by_flight", parameterList))
                {
                    while (objReader.Read())
                    {
                        crewRecogn = new CrewRecognitionOverviewEO();
                        crewRecogn.FlightID = (objReader["flightdetails_id"]) != DBNull.Value ? Convert.ToString(objReader["flightdetails_id"]) : string.Empty;
                        crewRecogn.RecognitionId = (objReader["recognition_id"]) != DBNull.Value ? Convert.ToString(objReader["recognition_id"]) : string.Empty;
                        crewRecogn.RecognitionStatus = (objReader["recognitionStatus"]) != DBNull.Value ? Convert.ToString(objReader["recognitionStatus"]) : string.Empty;
                        crewRecogn.RecognitionStatusID = (objReader["recognitionStatusID"]) != DBNull.Value ? Convert.ToString(objReader["recognitionStatusID"]) : string.Empty;
                        crewRecogn.RecognitionType = (objReader["recognitionType"]) != DBNull.Value ? Convert.ToString(objReader["recognitionType"]) : string.Empty;
                        crewRecogn.RecognitionTypeID = (objReader["recognitionTypeID"]) != DBNull.Value ? Convert.ToString(objReader["recognitionTypeID"]) : string.Empty;
                        crewRecogn.IndividualCrewCount = (objReader["CrewCount"]) != DBNull.Value ? Convert.ToString(objReader["CrewCount"]) : string.Empty;

                        crewRecogn.RecognizedCrewID = (objReader["recognCrewID"]) != DBNull.Value ? Convert.ToString(objReader["recognCrewID"]) : string.Empty;
                        crewRecogn.RecognizedStaffNumber = (objReader["recognStaffNo"]) != DBNull.Value ? Convert.ToString(objReader["recognStaffNo"]) : string.Empty;

                        crewRecognitionList.Add(crewRecogn);
                    }
                }

                return crewRecognitionList;
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
    }
}
