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
using QR.IPrism.EntityObjects.Shared;
using System.Data.OracleClient;
using System.Data.Common;
using Oracle.DataAccess.Client;


namespace QR.IPrism.BusinessObjects.Implementation
{
    public class AssessmentSearchDao : IAssessmentSearchDao
    {
        /// <summary>
        /// Function that returns ILIST results for searched assessment list
        /// </summary>
        /// <param name="Inputs"></param>
        /// <returns>ILIST with search results for Searched list assessments</returns>
        /// 
        public async Task<List<AssessmentSearchEO>> GetAssmtSearchResultAsync(AssessmentSearchRequestFilterEO assmtSearchFilterEO)
        {
            List<AssessmentSearchEO> assmtSearchresultList = new List<AssessmentSearchEO>();
            AssessmentSearchEO assmtSearchresult = null;

            ODPDataAccess dbFramework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> parameterList = null;
                try
                {
                    parameterList = new List<ODPCommandParameter>();

                    parameterList.Add(new ODPCommandParameter("pi_assesse_staffno", assmtSearchFilterEO.StaffNumber ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                    parameterList.Add(new ODPCommandParameter("pi_assessor_staffno", assmtSearchFilterEO.AssessorUserID ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                    parameterList.Add(new ODPCommandParameter("pi_grade", assmtSearchFilterEO.Grade ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                    parameterList.Add(new ODPCommandParameter("pi_fromdate",  Common.OracleDateFormateddMMMyyyy(assmtSearchFilterEO.FromDate), ParameterDirection.Input, OracleDbType.Varchar2));
                    parameterList.Add(new ODPCommandParameter("pi_todate", Common.OracleDateFormateddMMMyyyy(assmtSearchFilterEO.ToDate), ParameterDirection.Input, OracleDbType.Varchar2));
                    parameterList.Add(new ODPCommandParameter("pi_assmtstatus", assmtSearchFilterEO.AssessmentStatus ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                    parameterList.Add(new ODPCommandParameter("po_searchresult", ParameterDirection.Output, OracleDbType.RefCursor));

                    using (IDataReader objReader = await dbFramework.ExecuteSPReaderAsync("prism_mig_assessment.searchassessment_new", parameterList))
                    {
                        while (objReader.Read())
                        {
                            assmtSearchresult = new AssessmentSearchEO();
                            assmtSearchresult.StaffNumber = objReader["Assessee_StaffNum"] != null ? objReader["Assessee_StaffNum"].ToString() : string.Empty;
                            assmtSearchresult.StaffName = objReader["Assessee_Name"] != null ? objReader["Assessee_Name"].ToString() : string.Empty;
                            assmtSearchresult.AssessmentStatus = objReader["Assessment_Status"] != null ? objReader["Assessment_Status"].ToString() : string.Empty;
                            assmtSearchresult.AssessmentID = objReader["ASSESSMENT_ID"] != null ? objReader["ASSESSMENT_ID"].ToString() : string.Empty;
                            assmtSearchresult.Grade = objReader["STAFF_GRADE"] != null ? objReader["STAFF_GRADE"].ToString() : string.Empty;
                            assmtSearchresult.AssessmentType = objReader["ASSESSMENT_TYPE"] != null ? objReader["ASSESSMENT_TYPE"].ToString() : string.Empty;
                            assmtSearchresult.CutOffDate = objReader["cutoffdate"] != null ? Convert.ToDateTime(objReader["cutoffdate"]) : default(DateTime);
                            //assmtSearchresult.AssessmentDate = objReader["Assessment_Date"] != null ? Convert.ToDateTime(objReader["Assessment_Date"]) : default(DateTime);
                            //assmtSearchresult.ExpDateOfCompletion = objReader["Expected_Date"] != null ? Convert.ToDateTime(objReader["Expected_Date"]) : default(DateTime);

                            assmtSearchresult.AssessmentDate = (!string.IsNullOrEmpty(objReader["Assessment_Date"].ToString())) ? string.Format("{0:dd-MMM-yyyy}", objReader["Assessment_Date"]) : string.Empty;
                            //assmtSearchresult.AssessmentDate = !string.IsNullOrEmpty(objReader["Assessment_Date"].ToString()) ? Convert.ToDateTime(objReader["Assessment_Date"]) : DateTime.MinValue;
                            //assmtSearchresult.FormatedDate = assmtSearchresult.AssessmentDate != DateTime.MinValue ? assmtSearchresult.AssessmentDate.ToString(Constants.DATEFORMAT) : string.Empty;
                            assmtSearchresult.ExpDateOfCompletion = (!string.IsNullOrEmpty(objReader["Expected_Date"].ToString())) ? string.Format("{0:dd-MMM-yyyy}", objReader["Expected_Date"]) : string.Empty;
                            
                            assmtSearchresultList.Add(assmtSearchresult);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbFramework.CloseConnection();
                }

                return assmtSearchresultList;
            }

        }

        /// <summary>
        /// Function that returns ILIST results for searched assessment list
        /// </summary>
        /// <param name="Inputs"></param>
        /// <returns>ILIST with search results for Searched list assessments</returns>
        /// 
        public async Task<List<AssessmentEO>> GetAllPreviousAssessment(string id)
        {
            List<AssessmentEO> assmtSearchresultList = new List<AssessmentEO>();
            AssessmentEO row = null;
            
            ODPDataAccess dbFramework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> parameterList = null;
                try
                {
                    parameterList = new List<ODPCommandParameter>();
                    parameterList.Add(new ODPCommandParameter("pi_staffno", id, ParameterDirection.Input, OracleDbType.Varchar2));
                    parameterList.Add(new ODPCommandParameter("po_searchresult", ParameterDirection.Output, OracleDbType.RefCursor));

                    using (IDataReader objReader = await dbFramework.ExecuteSPReaderAsync("prism_mig_assessment.getAllAssessment", parameterList))
                    {
                        while (objReader.Read())
                        {
                            row = new AssessmentEO();
                            row.FlightNumber = objReader["FLIGHTNUMBER"] != null ? objReader["FLIGHTNUMBER"].ToString() : string.Empty;
                            row.FlightDate = objReader["FLIGHTDATE"] != null ? Convert.ToDateTime(objReader["FLIGHTDATE"].ToString()) : (DateTime?)null;
                            row.SectorFrom = objReader["SECTOR"] != null ? objReader["SECTOR"].ToString() : string.Empty;
                            row.AssessmentType = objReader["ASSESSMENTTYPE"] != null ? objReader["ASSESSMENTTYPE"].ToString() : string.Empty;
                            row.AssessmentStatus = objReader["ASSESSMENTSTATUS"] != null ? objReader["ASSESSMENTSTATUS"].ToString() : string.Empty;
                            row.AssessmentDate = objReader["DATEOFASSESSMENT"] != null ? Convert.ToDateTime(objReader["DATEOFASSESSMENT"].ToString()) : (DateTime?)null;
                            row.AssessorStaffName = objReader["ASSESSORNAME"] != null ? objReader["ASSESSORNAME"].ToString() : string.Empty;
                            row.AssesseeGrade = objReader["ASSESSEEGRADE"] != null ? objReader["ASSESSEEGRADE"].ToString() : string.Empty;
                            row.AssesseeStaffNo = objReader["ASSESSEENO"] != null ? objReader["ASSESSEENO"].ToString() : string.Empty;

                            assmtSearchresultList.Add(row);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbFramework.CloseConnection();
                }

                return assmtSearchresultList;
            }

        }

        /// <summary>
        /// Function that returns ILIST results for unscheduled assessments for logged in assessor
        /// </summary>
        /// <param name="Inputs"></param>
        /// <returns>ILIST with search results for unscheduled assessments for logged in assessor</returns>
        /// 
        public async Task<List<AssessmentSearchEO>> SavedUnscheduledAssessmentAsync(AssessmentSearchEO filter)
        {
            List<AssessmentSearchEO> savedassmtList = new List<AssessmentSearchEO>();
            AssessmentSearchEO savedassmtresult = null;



            ODPDataAccess dbFramework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> parameterList = null;
                // var dateOfAssessment = Common.OracleDateFormateddMMMyyyy(filter.DateofAssessment);
                // var toDate = Common.OracleDateFormateddMMMyyyy(assmtSearchFilterEO.ToDate);
                try
                {
                    parameterList = new List<ODPCommandParameter>();
                    parameterList.Add(new ODPCommandParameter("pi_staffno", filter.StaffNumber ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                    parameterList.Add(new ODPCommandParameter("po_searchresult", ParameterDirection.Output, OracleDbType.RefCursor));

                    using (IDataReader objReader = await dbFramework.ExecuteSPReaderAsync("pbms_performance_search_pkg.getUnscheduledSavedAssessments", parameterList))
                    {
                        while (objReader.Read())
                        {
                            savedassmtresult = new AssessmentSearchEO();
                            savedassmtresult.StaffNumber = objReader["staffno"] != null ? objReader["staffno"].ToString() : string.Empty;
                            savedassmtresult.FlightNo = objReader["flightdetsid"] != null ? objReader["flightdetsid"].ToString() : string.Empty;
                            savedassmtresult.AssessmentStatus = objReader["assessmentstatus"] != null ? objReader["assessmentstatus"].ToString() : string.Empty;
                            savedassmtresult.AssessmentID = objReader["ASSESSMENTID"] != null ? objReader["ASSESSMENTID"].ToString() : string.Empty;
                            savedassmtresult.Grade = objReader["ASSESSEEGRADE"] != null ? objReader["ASSESSEEGRADE"].ToString() : string.Empty;
                            savedassmtresult.AssessmentType = objReader["ASSESSMENTTYPE"] != null ? objReader["ASSESSMENTTYPE"].ToString() : string.Empty;

                            // savedassmtresult.AssessmentDate = (!string.IsNullOrEmpty(objReader["dateofassessment"].ToString())) ? string.Format("{0:dd-MMM-yyyy}", objReader["dateofassessment"]) : string.Empty;
                            savedassmtresult.DateofAssessment = !string.IsNullOrEmpty(objReader["dateofassessment"].ToString()) ? Convert.ToDateTime(objReader["dateofassessment"]) : DateTime.MinValue;
                            savedassmtresult.FormatedDate = objReader["dateofassessment"] != null && objReader["dateofassessment"].ToString() != String.Empty ?
                                Convert.ToDateTime(objReader["dateofassessment"].ToString()) : (DateTime?)null;



                            savedassmtList.Add(savedassmtresult);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbFramework.CloseConnection();
                }

                return savedassmtList;
            }

        }

        /// <summary>
        /// Function that returns true or false for the given search criteria to validate inputs
        /// </summary>
        /// <param name="Inputs"></param>
        /// <returns>Function that returns true or false for the given search criteria to validate inputs</returns>
        /// 
        public async Task<ResponseEO> ValidateUnscheduledData(AssessmentSearchEO filter)
        {

            ResponseEO response = new ResponseEO();
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                var flightDate = filter.FlightDate != null ? Common.OracleDateFormateddMMMyyyy(Convert.ToDateTime(filter.FlightDate)) : "";
                var dateofAssessment = filter.DateofAssessment != null ? Common.OracleDateFormateddMMMyyyy(Convert.ToDateTime(filter.DateofAssessment)) : "";

                try
                {
                    perametersList = new List<ODPCommandParameter>();

                    perametersList.Add(new ODPCommandParameter("pi_assesseeuserid", filter.StaffNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_assessoruserid", filter.AssessorStaffNo, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_assessmenttype", filter.AssessmentType ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_flightnum", filter.FlightNo, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_flightdate", flightDate, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_dateofassmnt", dateofAssessment, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_sectorfrom", filter.SectorFrom, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_sectorto", filter.SectorTo, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_validated", ParameterDirection.Output, OracleDbType.Varchar2, 50));



                    System.Data.Common.DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("pbms_createreqassmnt_pkg.validate_reqassessment", perametersList);
                    response.ResponseId = !command.Parameters["po_validated"].Value.Equals(Oracle.DataAccess.Types.OracleString.Null) ? Convert.ToString(command.Parameters["po_validated"].Value) : string.Empty;
                }
                catch (Exception ex)
                {
                    response.ResponseId = "N";
                    if (ex.Message.Contains("FlightNotExits"))
                    {
                        response.Message = "Flight number does not exist.";
                    }
                    else if (ex.Message.Contains("Duplicate"))
                    {
                        response.Message = "The assessment is already scheduled for staff.";
                    }
                    else if (ex.Message.Contains("StaffNotFound"))
                    {
                        response.Message = "The staff not found in the flight.";
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
            }
            return response;
        }

        #region Function to get the last and expected assessment dates for the staff

        /// <summary>
        /// Function to get the last and expected assessment dates for the staff
        /// </summary>
        /// <param name="staffNo"></param>
        /// <returns></returns>
        public async Task<List<AssessmentSearchEO>> GetCrewExpectedAsmnt(AssessmentSearchRequestFilterEO filter)
        {
            List<AssessmentSearchEO> crewAsmntDates = new List<AssessmentSearchEO>();
            AssessmentSearchEO crewAsmntDate = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_staffNos", filter.StaffNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_searchresult", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objSearch = await dbframework.ExecuteSPReaderAsync("pbms_performance_search_pkg.GetCrewAssessmentDates", parameterList))
                {
                    while (objSearch.Read())
                    {
                        crewAsmntDate = new AssessmentSearchEO();
                        crewAsmntDate.StaffName = objSearch["staffName"] != null ? objSearch["staffName"].ToString() : string.Empty;
                        crewAsmntDate.ToDate = DateTime.Parse(objSearch["dateofassessment"].ToString());
                        crewAsmntDate.FromDate = DateTime.Parse(objSearch["expectedassessmentdate"].ToString());

                        crewAsmntDates.Add(crewAsmntDate);
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


            return crewAsmntDates;
        }

        #endregion

        /// <summary>
        /// Method to get the crew on board from AIMS
        /// </summary>
        /// <param name="crewDetails"></param>
        /// <returns></returns>
        public async Task<List<AssessmentSearchRequestFilterEO>> GetCrewOnBoard(AssessmentSearchRequestFilterEO filter)
        {
            List<AssessmentSearchRequestFilterEO> responseList = new List<AssessmentSearchRequestFilterEO>();
            AssessmentSearchRequestFilterEO response = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.AIMS_CONNECTION_STR);
            {
                List<ODPCommandParameter> parametersList = null;
                try
                {
                    parametersList = new List<ODPCommandParameter>();
                    parametersList.Add(new ODPCommandParameter("flight_no", filter.FlightNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("v_arr", filter.SectorTo, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("flight_date", Common.OracleDateFormateddMMMyyyy(filter.ToDate), ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("fpis_crew", ParameterDirection.Output, OracleDbType.RefCursor));

                    using (IDataReader command = await dbframework.ExecuteSPReaderAsync("getCrewOnBoard", parametersList))
                    {
                        while (command.Read())
                        {
                            response = new AssessmentSearchRequestFilterEO();
                            response.StaffNumber = command["CREWID"] != null ? command["CREWID"].ToString() : string.Empty;
                            response.Grade = command["POS"] != null ? command["POS"].ToString() : string.Empty;
                            response.StaffName = command["NAME"] != null ? command["NAME"].ToString() : string.Empty;
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
