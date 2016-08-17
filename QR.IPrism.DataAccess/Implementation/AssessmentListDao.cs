/*********************************************************************
 * Name         : ViewAssessmentDetailDao.cs
 * Description  : Implementation of IViewAssessmentDetailDao.
 * Create Date  : 25th Jan 2016
 * Last Modified: 26th Jan 2016
 * Copyright By : Qatar Airways
 *********************************************************************/

using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.Enterprise;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using QR.IPrism.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;
using Oracle.DataAccess.Client;


namespace QR.IPrism.BusinessObjects.Implementation
{
    public class AssessmentListDao : IAssessmentListDao
    {
        #region View complete assessments for a crew

        /// <summary>
        /// Function that returns ILIST results for completed assessment 
        /// </summary>
        /// <param name="Inputs"></param>
        /// <returns>ILIST with search results for viewing complete assessments</returns>
        /// 
        public async Task<List<AssessmentSearchEO>> GetAssessmentListResultAsync(string id)
        {
            List<AssessmentSearchEO> response = new List<AssessmentSearchEO>();
            AssessmentSearchEO assessmentList = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_assessoruserid", id, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_crewsearch", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_assessment.get_assessmentlist_new", parameterList))
                {
                    while (objReader.Read())
                    {
                        assessmentList = new AssessmentSearchEO();
                        assessmentList.AssessmentID = objReader["assessmentid"] != null ? objReader["assessmentid"].ToString() : string.Empty;
                        assessmentList.StaffNumber = objReader["staffno"] != null ? objReader["staffno"].ToString() : string.Empty;
                        assessmentList.CrewName = objReader["crewname"] != null ? objReader["crewname"].ToString() : string.Empty;
                        assessmentList.AssessmentStatus = objReader["status"] != null ? objReader["status"].ToString() : string.Empty;
                        assessmentList.Grade = objReader["grade"] != null ? objReader["grade"].ToString() : string.Empty;
                        assessmentList.CustomStatus = objReader["customstatus"] != null ? objReader["customstatus"].ToString() : string.Empty;
                        assessmentList.FlightNo = objReader["flightno"] != null ? objReader["flightno"].ToString() : string.Empty;
                        assessmentList.SectorFrom = objReader["sectorfrom"] != null ? objReader["sectorfrom"].ToString() : string.Empty;
                        assessmentList.SectorTo = objReader["sectorto"] != null ? objReader["sectorto"].ToString() : string.Empty;
                        assessmentList.DateofAssessment = !string.IsNullOrEmpty(objReader["dateofassessment"].ToString()) ? Convert.ToDateTime(objReader["dateofassessment"]) : DateTime.MinValue;
                        assessmentList.FormatedDate = objReader["dateofassessment"] != null && objReader["dateofassessment"].ToString() != String.Empty ?
                          Convert.ToDateTime(objReader["dateofassessment"].ToString()) : (DateTime?)null;

                        assessmentList.ExpectedDate = (!string.IsNullOrEmpty(objReader["targetdate"].ToString())) ? string.Format("{0:dd-MMM-yyyy}", objReader["targetdate"]) : string.Empty;
                        response.Add(assessmentList);
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
        /// Function that returns ILIST results for CS and CSD list For PO assessment 
        /// </summary>
        /// <param name="Inputs"></param>
        /// <returns>ILIST with results for CS and CSD list For PO assessment </returns>
        /// 
        public async Task<List<AssessmentSearchEO>> GetCSDCSListResultAsync(AssessmentSearchRequestFilterEO inputs)
        {
            List<AssessmentSearchEO> response = new List<AssessmentSearchEO>();
            AssessmentSearchEO csdCSList = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_assessoruserid", inputs.AssessorUserID, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_staffNos", inputs.StaffNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_grade", inputs.Grade, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_months", inputs.PendingAssessment, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_csdcslist", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PRISM_MIG_ASSESSMENT.GET_PO_PENDINGASSMT_LIST", parameterList))
                {
                    while (objReader.Read())
                    {
                        csdCSList = new AssessmentSearchEO();
                        csdCSList.StaffNumber = objReader["staffno"] != null ? objReader["staffno"].ToString() : string.Empty;
                        csdCSList.StaffName = objReader["staffname"] != null ? objReader["staffname"].ToString() : string.Empty;
                        csdCSList.LastAssessment = (!string.IsNullOrEmpty(objReader["lastasmntdate"].ToString())) ? string.Format("{0:dd-MMM-yyyy}", objReader["lastasmntdate"]) : string.Empty;
                        csdCSList.ExpectedDate = (!string.IsNullOrEmpty(objReader["expecteddate"].ToString())) ? string.Format("{0:dd-MMM-yyyy}", objReader["expecteddate"]) : string.Empty;
                        csdCSList.AssessmentsScheduled = (!string.IsNullOrEmpty(objReader["ScheduledDate"].ToString())) ? string.Format("{0:dd-MMM-yyyy}", objReader["ScheduledDate"]) : string.Empty;
                        csdCSList.PmName = objReader["pm_staffname"] != null ? objReader["pm_staffname"].ToString() : string.Empty;
                        csdCSList.Grade = objReader["grade"] != null ? objReader["grade"].ToString() : string.Empty;

                        response.Add(csdCSList);
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
        /// Function that returns ILIST searched results for CS and CSD list For which PO has to do assessment 
        /// </summary>
        /// <param name="Inputs"></param>
        /// <returns>ILIST with results searched results for CS and CSD list For which PO has to do assessment </returns>
        /// 
        //public async Task<List<AssessmentSearchEO>> GetSearchCSDCSResultAsync(AssessmentSearchRequestFilterEO inputs)
        //{
        //    List<AssessmentSearchEO> response = new List<AssessmentSearchEO>();
        //    AssessmentSearchEO csdCSList = null;
        //    List<ODPCommandParameter> parameterList = null;
        //    ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

        //    try
        //    {
        //        parameterList = new List<ODPCommandParameter>();

        //        parameterList.Add(new ODPCommandParameter("pi_assessoruserid", inputs.AssessorUserID, ParameterDirection.Input, OracleDbType.Varchar2));
        //        parameterList.Add(new ODPCommandParameter("pi_assessestaffno", inputs.StaffNumber ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
        //        parameterList.Add(new ODPCommandParameter("po_csdcslist", ParameterDirection.Output, OracleDbType.RefCursor));

        //        using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("pbms_recordassessment_pkg.get_CSDCS_AssessList", parameterList))
        //        {
        //            while (objReader.Read())
        //            {
        //                csdCSList = new AssessmentSearchEO();
        //                csdCSList.StaffNumber = objReader["staffno"] != null ? objReader["staffno"].ToString() : string.Empty;
        //                csdCSList.StaffName = objReader["staffname"] != null ? objReader["staffname"].ToString() : string.Empty;
        //                csdCSList.LastAssessment = (!string.IsNullOrEmpty(objReader["lastasmntdate"].ToString())) ? string.Format("{0:dd-MMM-yyyy}", objReader["lastasmntdate"]) : string.Empty;
        //                csdCSList.ExpDateOfCompletion = (!string.IsNullOrEmpty(objReader["expectedasmntdate"].ToString())) ? string.Format("{0:dd-MMM-yyyy}", objReader["expectedasmntdate"]) : string.Empty;
        //                csdCSList.AssessmentsScheduled = (!string.IsNullOrEmpty(objReader["ScheduledDate"].ToString())) ? string.Format("{0:dd-MMM-yyyy}", objReader["ScheduledDate"]) : string.Empty;
        //                csdCSList.PmName = objReader["pm_staffname"] != null ? objReader["pm_staffname"].ToString() : string.Empty;
        //                csdCSList.Grade = objReader["grade"] != null ? objReader["grade"].ToString() : string.Empty;

        //                response.Add(csdCSList);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbframework.CloseConnection();
        //    }


        //    return response;

        //}

        /// <summary>
        /// Function that returns assessment to which PO can schedule assessment for the assesseee 
        /// </summary>
        /// <param name="Inputs"></param>
        /// <returns>Function that returns assessment to which PO can schedule assessment for the assesseee  </returns>
        /// 
        public async Task<ResponseEO> ScheduleAssessment(AssessmentEO filterInput)
        {

            ResponseEO response = new ResponseEO() { IsSuccess = true};


            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                var flightDate = Common.OracleDateFormateddMMMyyyy(filterInput.FlightDate);
                try
                {
                    //if (filterInput.StaffId != null && filterInput.StaffId != string.Empty)
                    //{
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_assesseeuserid", filterInput.AssesseeStaffNo, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_assessoruserid", filterInput.AssessorStaffNo, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_flight_number", "QR" + filterInput.FlightNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_flight_date", flightDate, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_sector_from", filterInput.SectorFrom, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_sector_to", filterInput.SectorTo, ParameterDirection.Input, OracleDbType.Varchar2));

                    DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("pbms_recordassessment_pkg.insert_csdcs_assessment", perametersList);
                    //}
                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    if (ex.Message.Contains("FlightNotExits"))
                    {
                        response.Message = "Flight Number not found.";
                    }
                    else if (ex.Message.Contains("Duplicate"))
                    {
                        response.Message = "Already assessment has been scheduled.";
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

        /// <summary>
        /// Function that returns assessment to which PO can cancel the assessment for the assesseee 
        /// </summary>
        /// <param name="Inputs"></param>
        /// <returns>Function that returns assessment to which PO can cancel the assessment for the assesseee  </returns>
        /// 
        public async void CancelSchedluedAsmnt(string requestId)
        {
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_assessment_id", requestId, ParameterDirection.Input, OracleDbType.Varchar2));
                await dbframework.ExecuteSPNonQueryParmAsync("pbms_recordassessment_pkg.cancel_csdcs_assessment", parameterList);
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

        #region My old assessment

        public async Task<IEnumerable<AssessmentEO>> ViewAssmtAsync(string id)
        {
            AssessmentEO row = null;
            List<AssessmentEO> lst = new List<AssessmentEO>();

            List<ODPCommandParameter> parameterList = new List<ODPCommandParameter>();
          
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList.Add(new ODPCommandParameter("pi_assessmentid", id, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_searchresult", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("po_document", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("pbms_performance_search_pkg.getassmtdel", parameterList))
                {
                    while (objReader.Read())
                    {
                        row = new AssessmentEO() { Attachments = new List<FileEO>() };
                        row.AssesseeCrewDetID = objReader["assesseecrewdetailsid"] != null ? objReader["assesseecrewdetailsid"].ToString() : string.Empty;
                        row.LevelId = objReader["levelid"] != null ? objReader["levelid"].ToString() : string.Empty;
                        row.LevelName = objReader["levelname"] != null ? objReader["levelname"].ToString() : string.Empty;
                        row.CompetencyID = objReader["competencyid"] != null ? objReader["competencyid"].ToString() : string.Empty;
                        row.CompetencyName = objReader["competencyname"] != null ? objReader["competencyname"].ToString() : string.Empty;
                        row.RequirementId = objReader["requirementid"] != null ? objReader["requirementid"].ToString() : string.Empty;
                        row.RequirementName = objReader["requirementname"] != null ? objReader["requirementname"].ToString() : string.Empty;
                        row.RequirementDesc = objReader["requirementnamedet"] != null ? objReader["requirementnamedet"].ToString() : string.Empty;
                        row.FlightNumber = objReader["flightnum"] != null ? objReader["flightnum"].ToString() : string.Empty;
                        row.FlightDate = objReader["flightdate"] != null && objReader["flightdate"].ToString() != string.Empty ? Convert.ToDateTime(objReader["flightdate"].ToString()) : (DateTime?)null;
                        row.SectorFrom = objReader["sectorfrom"] != null ? objReader["sectorfrom"].ToString() : string.Empty;
                        row.SectorTo = objReader["sectorto"] != null ? objReader["sectorto"].ToString() : string.Empty;
                        row.AssessorStaffName = objReader["assessorname"] != null ? objReader["assessorname"].ToString() : string.Empty;
                        row.AssessorGrade = objReader["assessorgrade"] != null ? objReader["assessorgrade"].ToString() : string.Empty;
                        row.AssessorGradeSince = objReader["assessorgradesince"] != null ? objReader["assessorgradesince"].ToString() : string.Empty;
                        row.AssesseeStaffName = objReader["assesseename"] != null ? objReader["assesseename"].ToString() : string.Empty;
                        row.AssesseeGrade = objReader["assesseegrade"] != null ? objReader["assesseegrade"].ToString() : string.Empty;
                        row.AssesseeGradeSince = objReader["assesseegradesince"] != null ? objReader["assesseegradesince"].ToString() : string.Empty;

                        row.AssessmentDate = objReader["dateofassessment"] != null ? Convert.ToDateTime(objReader["dateofassessment"].ToString()) : (DateTime?)null;
                        row.POBriefingComment = objReader["pobriefcomm"] != null ? objReader["pobriefcomm"].ToString() : string.Empty;
                        row.POOnBoardComment = objReader["poobccomm"] != null ? objReader["poobccomm"].ToString() : string.Empty;
                        row.CSDCSBriefingComment = objReader["csdbriefcomm"] != null ? objReader["csdbriefcomm"].ToString() : string.Empty;
                        row.CSDCSOnBoardComment = objReader["csdobccomm"] != null ? objReader["csdobccomm"].ToString() : string.Empty;

                        row.Rating = objReader["rating"] != null ? objReader["rating"].ToString() : string.Empty;
                        row.Comments = objReader["comments"] != null ? objReader["comments"].ToString() : string.Empty;
                        row.TotalScore = objReader["totalscore"] != null ? Convert.ToInt32(objReader["totalscore"].ToString()) : 0;
                        row.CabinComments = objReader["cabincomments"] != null ? objReader["cabincomments"].ToString() : string.Empty;
                        row.AdditionalComments = objReader["addcomments"] != null ? objReader["addcomments"].ToString() : string.Empty;
                        row.Dispute = objReader["dispute"] != null ? objReader["dispute"].ToString() : string.Empty;
                        row.AssessmentStatus = objReader["status"] != null ? objReader["status"].ToString() : string.Empty;
                        row.AssessmentType = objReader["assessmenttype"] != null ? objReader["assessmenttype"].ToString() : string.Empty;
                        row.Isrequirementselected = objReader["isrequirementselected"] != null ? objReader["isrequirementselected"].ToString() : string.Empty;
                        row.ReasonForNonSubmission = objReader["nonsubmission"] != null ? objReader["nonsubmission"].ToString() : string.Empty;
                        row.RequestReason = objReader["requestreason"] != null ? objReader["requestreason"].ToString() : string.Empty;
                        row.RequestedBy = objReader["requestedby"] != null ? objReader["requestedby"].ToString() : string.Empty;
                        row.DelayReasonComments = objReader["delayreasoncomments"] != null ? objReader["delayreasoncomments"].ToString() : string.Empty;
                        lst.Add(row);
                    }

                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        FileEO attachment = new FileEO();
                        attachment.FileId = objReader["attachmentid"] != null ? objReader["attachmentid"].ToString() : string.Empty;
                        attachment.FileContent = objReader["attachment"] as byte[];
                        attachment.FileName = objReader["attachmentname"] != null ? objReader["attachmentname"].ToString() : string.Empty;
                        row.Attachments.Add(attachment);
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

            return lst;
        }

        public async Task<IEnumerable<AssessmentSearchEO>> GetPreviousAssessmentsAsync(AssessmentSearchEO input, string staffId)
        {
            List<AssessmentSearchEO> response = new List<AssessmentSearchEO>();
            AssessmentSearchEO row = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_from_date", Common.ToOracleDate(input.FromDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("pi_to_date", Common.ToOracleDate(input.ToDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("pi_crewdetails_id", staffId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_assmt", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_assessment.get_prev_assessments_proc", parameterList))
                {
                    while (objReader.Read())
                    {
                        row = new AssessmentSearchEO();
                        row.AssessmentID = objReader["assessmentid"] != null ? objReader["assessmentid"].ToString() : string.Empty;
                        row.FlightNo = objReader["flightnumber"] != null ? objReader["flightnumber"].ToString() : string.Empty;
                        row.FormatedDate = objReader["schedeptdate"] != null && objReader["schedeptdate"].ToString() != String.Empty ?
                           Convert.ToDateTime(objReader["schedeptdate"].ToString()) : (DateTime?)null;
                        row.SectorTo = objReader["sector"] != null ? objReader["sector"].ToString() : string.Empty;
                        row.DateofAssessment = objReader["dateofassessment"] != null && objReader["dateofassessment"].ToString() != string.Empty ?
                            Convert.ToDateTime(objReader["dateofassessment"].ToString()) : (DateTime?)null;
                        row.CrewName = objReader["assessor"] != null ? objReader["assessor"].ToString() : string.Empty;
                        row.Grade = objReader["assesseegrade_ass_date"] != null ? objReader["assesseegrade_ass_date"].ToString() : string.Empty;
                        row.CustomStatus = objReader["assessmentstatus"] != null ? objReader["assessmentstatus"].ToString() : string.Empty;
                        row.CutOffDate = objReader["cutoffdate"] != null ? Convert.ToDateTime(objReader["cutoffdate"].ToString()) : (DateTime?)null;

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

        #endregion
    }
}
