/*********************************************************************
 * Name         : AssessmentDao.cs
 * Description  : Implementation of IAssessmentDao.
 * Create Date  : 14th Jan 2016
 * Last Modified: 15th Jan 2016
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
    public class AssessmentDao : IAssessmentDao
    {
        #region View Assessment Details
        /// <summary>
        /// Function that returns View Assement Details
        /// </summary>
        /// <param name="assessmentID">assessmentID</param>
        /// <returns>EOAssessment entity contains list results of Objectives, Competencies Default, 
        /// Competencies EE, Ratings and IDP details along with Assessment IDP</returns>
        public async Task<AssessmentEO> GetAssessmentDetailsAsync(string assessmentID)
        {
            AssessmentEO objAssessment = new AssessmentEO();
            ObjectiveEO objObjective = new ObjectiveEO();
            List<CompetencyEO> compList = new List<CompetencyEO>();
            List<CompetencyEO> eeCompList = new List<CompetencyEO>();
            List<AssessmentIDPEO> assmIDP = new List<AssessmentIDPEO>();
            objAssessment.Objectives = new List<ObjectiveEO>();
            objAssessment.RatingGuidelines = new List<RatingGuideLineEO>();
            objAssessment.RatingForDropDown = new List<RatingGuideLineEO>();
            objAssessment.Attachments = new List<FileEO>();
            List<ODPCommandParameter> parameterList = null;


            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_assessment_id", assessmentID, ParameterDirection.Input, OracleDbType.Varchar2));
                List<CommandCursorParameter> commandCursorParameter = new List<CommandCursorParameter>();

                parameterList.Add(new ODPCommandParameter("cur_ratings", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_assm_dets", ParameterDirection.Output, OracleDbType.RefCursor));

                parameterList.Add(new ODPCommandParameter("cur_objetives", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_competancy_default", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_competancy_ee", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_assm_idp_details", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_assm_idp", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_attachments", ParameterDirection.Output, OracleDbType.RefCursor));

                //pbms4_assessment_pkg.sp_view_asseessment
                //PRISM_MIG_ASSESSMENT.sp_view_asseessment
                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PRISM_MIG_ASSESSMENT.sp_view_asseessment", parameterList))
                {
                    while (objReader.Read())
                    {
                        RatingGuideLineEO ratGlDetails = new RatingGuideLineEO();
                        ratGlDetails.RatingGuidelineID = objReader["assm_rating_id"] != null ? objReader["assm_rating_id"].ToString() : string.Empty;
                        ratGlDetails.Rating = objReader["rating"] != null ? objReader["rating"].ToString() : string.Empty;

                        ratGlDetails.RatingDescription = objReader["RATING_DESC"] != null ? objReader["RATING_DESC"].ToString() : string.Empty;
                        ratGlDetails.GuidelineDesc = objReader["GL_DESCRIPTION"] != null ? objReader["GL_DESCRIPTION"].ToString() : string.Empty;
                        ratGlDetails.ComplianceDesc = objReader["COMPL_DESC"] != null ? objReader["COMPL_DESC"].ToString() : string.Empty;

                        objAssessment.RatingForDropDown.Add(ratGlDetails);
                    }

                    objReader.NextResult();
                    while (objReader.Read())
                    {

                        objAssessment.AdditionalComments = objReader["ADDITIONALCOMMENTS"] != null && objReader["ADDITIONALCOMMENTS"].ToString() != string.Empty ? objReader["ADDITIONALCOMMENTS"].ToString() : string.Empty;
                        objAssessment.AssessmentID = objReader["assessmentid"] != null ? objReader["assessmentid"].ToString() : string.Empty;
                        objAssessment.FlightDetID = objReader["flightdetsid"] != null ? objReader["flightdetsid"].ToString() : string.Empty;
                        objAssessment.AssesseeCrewDetID = objReader["assesseecrewdetailsid"] != null ? objReader["assesseecrewdetailsid"].ToString() : string.Empty;
                        objAssessment.AssessorCrewDetID = objReader["assessorcrewdetailsid"] != null ? objReader["assessorcrewdetailsid"].ToString() : string.Empty;
                        objAssessment.AssessorStaffName = objReader["assessorname"] != null ? objReader["assessorname"].ToString() : string.Empty;
                        objAssessment.AssessorStaffNo = objReader["assessorstaffno"] != null ? objReader["assessorstaffno"].ToString() : string.Empty;
                        objAssessment.AssessorGrade = objReader["assessorgrade"] != null ? objReader["assessorgrade"].ToString() : string.Empty;
                        objAssessment.AssesseeStaffName = objReader["assesseename"] != null ? objReader["assesseename"].ToString() : string.Empty;
                        objAssessment.AssesseeStaffNo = objReader["assesseestaffno"] != null ? objReader["assesseestaffno"].ToString() : string.Empty;
                        objAssessment.AssesseeGrade = objReader["assesseegrade"] != null ? objReader["assesseegrade"].ToString() : string.Empty;
                        objAssessment.ManagerStaffNo = objReader["managerno"] != null ? objReader["managerno"].ToString() : string.Empty;
                        objAssessment.ManagerStaffName = objReader["manager"] != null ? objReader["manager"].ToString() : string.Empty;
                        objAssessment.AssessmentDate = !string.IsNullOrEmpty(objReader["dateofassessment"].ToString()) ? Convert.ToDateTime(objReader["dateofassessment"].ToString()) : DateTime.MinValue;
                        objAssessment.SectorFrom = objReader["sectorfrom"] != null ? objReader["sectorfrom"].ToString() : string.Empty;
                        objAssessment.SectorTo = objReader["sectorto"] != null ? objReader["sectorto"].ToString() : string.Empty;
                        objAssessment.FlightNumber = objReader["flightnum"] != null ? objReader["flightnum"].ToString() : string.Empty;
                        objAssessment.FlightDate = !string.IsNullOrEmpty(objReader["flightdate"].ToString()) ? Convert.ToDateTime(objReader["flightdate"].ToString()) : DateTime.MinValue;
                        objAssessment.AssessmentStatus = objReader["status"] != null ? objReader["status"].ToString() : string.Empty;
                        objAssessment.AssessmentType = objReader["assessmenttype"] != null ? objReader["assessmenttype"].ToString() : string.Empty;
                        //assessmentDetails.TotalScore = objReader["totalscore"] != null ? Convert.ToDecimal(objReader["totalscore"]) : 0;
                        objAssessment.ReasonForNonSubmission = objReader["reasonfornonsubmission"] != null ? objReader["reasonfornonsubmission"].ToString() : string.Empty;
                        objAssessment.Pax_Load_F = objReader["PAX_LOAD_F"] != null && objReader["PAX_LOAD_F"].ToString() != string.Empty ? Convert.ToInt32(objReader["PAX_LOAD_F"]) : 0;
                        objAssessment.Pax_Load_J = objReader["PAX_LOAD_J"] != null && objReader["PAX_LOAD_J"].ToString() != string.Empty ? Convert.ToInt32(objReader["PAX_LOAD_J"]) : 0;
                        objAssessment.Pax_Load_Y = objReader["PAX_LOAD_Y"] != null && objReader["PAX_LOAD_Y"].ToString() != string.Empty ? Convert.ToInt32(objReader["PAX_LOAD_Y"]) : 0;
                        objAssessment.Infants = objReader["infants"] != null && objReader["infants"].ToString() != string.Empty ? Convert.ToInt32(objReader["infants"]) : 0;
                        objAssessment.CrewPosition = objReader["crewposition"] != null && objReader["crewposition"].ToString() != string.Empty ? objReader["crewposition"].ToString() : string.Empty;
                        objAssessment.AircraftType = objReader["actype"] != null && objReader["actype"].ToString() != string.Empty ? objReader["actype"].ToString() : string.Empty;
                        objAssessment.AssesseeDesignation = objReader["designation"] != null && objReader["designation"].ToString() != string.Empty ? objReader["designation"].ToString() : string.Empty;


                    }
                    objReader.NextResult();


                    while (objReader.Read())
                    {
                        ObjectiveEO objDetails = new ObjectiveEO();
                        objDetails.AssessmentID = objReader["assessmentid"] != null ? objReader["assessmentid"].ToString() : string.Empty;
                        objDetails.ObjectiveID = objReader["Objective_ID"] != null ? objReader["Objective_ID"].ToString() : string.Empty;
                        objDetails.ObjectiveName = objReader["Objective_Name"] != null ? objReader["Objective_Name"].ToString() : string.Empty;
                        objDetails.ObjectiveImage = objReader["Objective_Image"] != null && objReader["Objective_Image"].ToString() != string.Empty ? (byte[])objReader["Objective_Image"] : null;
                        objDetails.Description = objReader["Objective_Description"] != null ? objReader["Objective_Description"].ToString() : string.Empty;
                        objDetails.Caption = objReader["objective_caption"] != null ? objReader["objective_caption"].ToString() : string.Empty;
                        objDetails.Rank = objReader["obj_rank"] != null ? Convert.ToInt32(objReader["obj_rank"]) : 0;
                        objDetails.Weightage = objReader["obj_weightage"] != null ? Convert.ToInt32(objReader["obj_weightage"]) : 0;
                        objDetails.AssessmentObjectiveID = objReader["ASSESSMENT_OBJECT_ID"] != null ? objReader["ASSESSMENT_OBJECT_ID"].ToString() : string.Empty;
                        objDetails.ObjectiveDetailedID = objReader["OBJECT_DETAIL_ID"] != null ? objReader["OBJECT_DETAIL_ID"].ToString() : string.Empty;
                        objDetails.Comments = objReader["OBJ_COMMENTS"] != null ? objReader["OBJ_COMMENTS"].ToString() : string.Empty;
                        objDetails.ObjScore = objReader["OBJ_SCORE"] != null ? Convert.ToDecimal(objReader["OBJ_SCORE"]) : 0;
                        objDetails.Rating = objReader["OBJ_RATING"] != null ? objReader["OBJ_RATING"].ToString() : string.Empty;
                        objDetails.RatingDescription = objReader["OBJ_RATING_DESC"] != null ? objReader["OBJ_RATING_DESC"].ToString() : string.Empty;
                        objAssessment.Objectives.Add(objDetails);
                    }
                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        CompetencyEO compDetails = new CompetencyEO();
                        compDetails.CompetencyId = objReader["Competancy_ID"] != null ? objReader["Competancy_ID"].ToString() : string.Empty;
                        compDetails.CompetencyName = objReader["Competancy_Name"] != null ? objReader["Competancy_Name"].ToString() : string.Empty;
                        compDetails.CompetencyConfigId = objReader["competancy_config_id"] != null ? objReader["competancy_config_id"].ToString() : string.Empty;
                        compDetails.Type = objReader["comp_type"] != null ? objReader["comp_type"].ToString() : string.Empty;
                        compDetails.AssessmentObjectiveID = objReader["ASSESSMENT_OBJECT_ID"] != null ? objReader["ASSESSMENT_OBJECT_ID"].ToString() : string.Empty;
                        compDetails.AssessmentCompID = objReader["ASSESSMENT_COMP_ID"] != null ? objReader["ASSESSMENT_COMP_ID"].ToString() : string.Empty;
                        compDetails.Comments = objReader["COMP_COMMENTS"] != null ? objReader["COMP_COMMENTS"].ToString() : string.Empty;
                        compDetails.ObjectiveDetID = objReader["OBJECT_DETAIL_ID"] != null ? objReader["OBJECT_DETAIL_ID"].ToString() : string.Empty;
                        compDetails.CompetencyScore = objReader["COMP_SCORE"] != null ? Convert.ToDecimal(objReader["COMP_SCORE"]) : 0;
                        compDetails.Rating = objReader["COMP_RATING"] != null ? objReader["COMP_RATING"].ToString() : string.Empty;
                        compDetails.Description = objReader["Competancy_Description"] != null ? objReader["Competancy_Description"].ToString() : string.Empty;
                        compList.Add(compDetails);

                    }
                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        CompetencyEO eeDetails = new CompetencyEO();
                        eeDetails.CompetencyId = objReader["Competancy_ID"] != null ? objReader["Competancy_ID"].ToString() : string.Empty;
                        eeDetails.CompetencyName = objReader["Competancy_Name"] != null ? objReader["Competancy_Name"].ToString() : string.Empty;
                        eeDetails.CompetencyConfigId = objReader["competancy_config_id"] != null ? objReader["competancy_config_id"].ToString() : string.Empty;
                        eeDetails.Type = objReader["comp_type"] != null ? objReader["comp_type"].ToString() : string.Empty;
                        eeDetails.AssessmentObjectiveID = objReader["ASSESSMENT_OBJECT_ID"] != null ? objReader["ASSESSMENT_OBJECT_ID"].ToString() : string.Empty;
                        eeDetails.AssessmentCompID = objReader["ASSESSMENT_COMP_ID"] != null ? objReader["ASSESSMENT_COMP_ID"].ToString() : string.Empty;
                        eeDetails.ObjectiveDetID = objReader["OBJECT_DETAIL_ID"] != null ? objReader["OBJECT_DETAIL_ID"].ToString() : string.Empty;
                        eeDetails.Comments = objReader["COMP_COMMENTS"] != null ? objReader["COMP_COMMENTS"].ToString() : string.Empty;
                        eeDetails.CompetencyScore = objReader["COMP_SCORE"] != null ? Convert.ToDecimal(objReader["COMP_SCORE"]) : 0;
                        eeDetails.Rating = objReader["COMP_RATING"] != null ? objReader["COMP_RATING"].ToString() : string.Empty;
                        eeDetails.IsEEChecked = objReader["IS_EE_CHECKED"] != null ? objReader["IS_EE_CHECKED"].ToString() : string.Empty;
                        eeCompList.Add(eeDetails);
                    }

                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        AssessmentIDPEO assmIDPDet = new AssessmentIDPEO();
                        assmIDPDet.AssessmentID = objReader["ASSESSMENT_ID"] != null ? objReader["ASSESSMENT_ID"].ToString() : string.Empty;
                        assmIDPDet.AssessmentIDPDetID = objReader["ASSESSMENT_IDP_DET_ID"] != null ? objReader["ASSESSMENT_IDP_DET_ID"].ToString() : string.Empty;
                        assmIDPDet.CompConfigID = objReader["COMPETANCY_CONFIG_ID"] != null ? objReader["COMPETANCY_CONFIG_ID"].ToString() : string.Empty;
                        //assmIDPDet.CompetencyName = compList.FirstOrDefault(com => com.CompetencyConfigId.Equals(assmIDPDet.CompConfigID)).CompetencyName;
                        assmIDPDet.CompetencyName = objReader["COMP_DISPLAY_NAME"] != null ? objReader["COMP_DISPLAY_NAME"].ToString() : string.Empty;
                        assmIDPDet.Observation = objReader["OBSERVATION"] != null ? objReader["OBSERVATION"].ToString() : string.Empty;
                        assmIDPDet.Situation = objReader["SITUATION"] != null ? objReader["SITUATION"].ToString() : string.Empty;
                        assmIDPDet.ExpectedResult = objReader["EXPECTED_RESULT"] != null ? objReader["EXPECTED_RESULT"].ToString() : string.Empty;
                        assmIDPDet.ObjectiveName = objReader["OBJ_DISPLAY_NAME"] != null ? objReader["OBJ_DISPLAY_NAME"].ToString() : string.Empty;
                        assmIDP.Add(assmIDPDet);
                    }

                    objReader.NextResult();
                    while (objReader.Read())
                    {

                        objAssessment.AssessmentID = objReader["assessmentid"] != null ? objReader["assessmentid"].ToString() : string.Empty;
                        objAssessment.IdpId = objReader["idp_id"] != null ? objReader["idp_id"].ToString() : string.Empty;
                        objAssessment.IdpComnNo = objReader["idp_comn_no"] != null ? objReader["idp_comn_no"].ToString() : string.Empty;
                        objAssessment.AssesseeCrewDetID = objReader["crewdetailsid"] != null ? objReader["crewdetailsid"].ToString() : string.Empty;
                        objAssessment.IdpTypeID = objReader["idp_type_id"] != null ? objReader["idp_type_id"].ToString() : string.Empty;


                    }

                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        FileEO attachment = new FileEO();
                        attachment.FileId = objReader["attachmentid"].ToString();
                        attachment.FileContent = objReader["attachment"] as byte[];
                        attachment.FileName = objReader["attachmentname"].ToString();
                        attachment.FileType = objReader["contenttype"].ToString();
                        attachment.ClassKeyId = objReader["contentid"].ToString();

                        objAssessment.Attachments.Add(attachment);

                    }

                }
                dbframework.CloseConnection();
                dbframework = null;

                dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
                List<ODPCommandParameter> parameterListNew = new List<ODPCommandParameter>();
                parameterListNew.Add(new ODPCommandParameter("cur_rating_guideline", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("pbms4_assessment_pkg.sp_get_assm_rating", parameterListNew))
                {
                    while (objReader.Read())
                    {
                        RatingGuideLineEO ratGlDetails = new RatingGuideLineEO();
                        ratGlDetails.RatingGuidelineID = objReader["assm_rating_id"] != null ? objReader["assm_rating_id"].ToString() : string.Empty;
                        ratGlDetails.Rating = objReader["rating"] != null ? objReader["rating"].ToString() : string.Empty;
                        ratGlDetails.RatingDescription = objReader["RATING_DESC"] != null ? objReader["RATING_DESC"].ToString() : string.Empty;
                        ratGlDetails.GuidelineDesc = objReader["GL_DESCRIPTION"] != null ? objReader["GL_DESCRIPTION"].ToString() : string.Empty;
                        ratGlDetails.ComplianceDesc = objReader["COMPL_DESC"] != null ? objReader["COMPL_DESC"].ToString() : string.Empty;
                        objAssessment.RatingGuidelines.Add(ratGlDetails);
                    }
                }

                objAssessment.Objectives.ForEach(obj =>
                {
                    obj.Competencies = new List<CompetencyEO>();
                    obj.Competencies.AddRange(compList.Where(comp => comp.AssessmentObjectiveID.Equals(obj.AssessmentObjectiveID)).ToList());
                    obj.ExceedsExpectations = new List<CompetencyEO>();
                    obj.ExceedsExpectations.AddRange(eeCompList.Where(comp => comp.AssessmentObjectiveID.Equals(obj.AssessmentObjectiveID)).ToList());
                    obj.AssmIDPs = new List<AssessmentIDPEO>();
                    obj.AssmIDPs.AddRange(assmIDP.Where(comp => comp.AssessmentID.Equals(obj.AssessmentID) && comp.ObjectiveName.Equals(obj.ObjectiveName)).ToList());
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }

            return objAssessment;

        }
        #endregion

        #region get the list of Objectives, Competencies Default, Competencies EE and Ratings
        /// <summary>
        /// Function that returns EOAssessment entity contains IList results of Objectives, Competencies Default, Competencies EE and Ratings
        /// </summary>
        /// <param name="grade"></param>
        /// <returns>EOAssessment entity contains IList results of Objectives, Competencies Default, Competencies EE and Ratings</returns>
        public async Task<AssessmentEO> GetAssessmnetByGradeAsync(string grade)
        {
            AssessmentEO objAssessmentList = new AssessmentEO();
            objAssessmentList.Objectives = new List<ObjectiveEO>();

            //ObjectiveEO objCompEERatList = new ObjectiveEO();
            List<CompetencyEO> compList = new List<CompetencyEO>();
            List<CompetencyEO> eeCompList = new List<CompetencyEO>();
            objAssessmentList.RatingGuidelines = new List<RatingGuideLineEO>();
            objAssessmentList.RatingForDropDown = new List<RatingGuideLineEO>();
            List<AssessmentIDPEO> assmIDPList = new List<AssessmentIDPEO>();
            objAssessmentList.Attachments = new List<FileEO>();
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_grade", grade, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("cur_objectives", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_competancy_default", ParameterDirection.Output,  OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_competancy_ee", ParameterDirection.Output,  OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_rating", ParameterDirection.Output,  OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PRISM_MIG_ASSESSMENT.sp_get_obj_comp_rating", parameterList))
                {
                    while (objReader.Read())
                    {
                        ObjectiveEO objDetails = new ObjectiveEO();
                        objDetails.ObjectiveID = objReader["Objective_ID"] != null ? objReader["Objective_ID"].ToString() : string.Empty;
                        objDetails.ObjectiveName = objReader["Objective_Name"] != null ? objReader["Objective_Name"].ToString() : string.Empty;
                        objDetails.ObjectiveDetailedID = objReader["objective_detail_id"] != null ? objReader["objective_detail_id"].ToString() : string.Empty;
                        objDetails.Description = objReader["objective_description"] != null ? objReader["objective_description"].ToString() : string.Empty;
                        objDetails.ObjectiveImage = objReader["Objective_Image"] != null ? (byte[])objReader["Objective_Image"] : null;
                        objDetails.Caption = objReader["objective_caption"] != null ? objReader["objective_caption"].ToString() : string.Empty;
                        objDetails.Rank = objReader["obj_rank"] != null ? Convert.ToInt32(objReader["obj_rank"]) : 0;
                        objDetails.Weightage = objReader["obj_weightage"] != null ? Convert.ToInt32(objReader["obj_weightage"]) : 0;
                        objDetails.IsActive = objReader["IS_ACTIVE"] != null ? objReader["IS_ACTIVE"].ToString() : string.Empty;
                        objAssessmentList.Objectives.Add(objDetails);
                    }
                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        CompetencyEO compDetails = new CompetencyEO();
                        compDetails.CompetencyId = objReader["Competancy_ID"] != null ? objReader["Competancy_ID"].ToString() : string.Empty;
                        compDetails.CompetencyConfigId = objReader["competancy_config_id"] != null ? objReader["competancy_config_id"].ToString() : string.Empty;
                        compDetails.ObjectiveID = objReader["objective_id"] != null ? objReader["objective_id"].ToString() : string.Empty;
                        compDetails.ObjectiveDetID = objReader["objective_detail_id"] != null ? objReader["objective_detail_id"].ToString() : string.Empty;
                        compDetails.CompetencyName = objReader["Competancy_Name"] != null ? objReader["Competancy_Name"].ToString() : string.Empty;
                        compDetails.Type = objReader["type"] != null ? objReader["type"].ToString() : string.Empty;
                        compDetails.IsActive = objReader["Is_Active"] != null ? objReader["Is_Active"].ToString() : string.Empty;
                        compDetails.Description = objReader["Competancy_Description"] != null ? objReader["Competancy_Description"].ToString() : string.Empty;
                        compDetails.Rank = objReader["sort_order"] != null ? Convert.ToInt32(objReader["sort_order"]) : 0;
                        compList.Add(compDetails);

                    }


                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        CompetencyEO eeDetails = new CompetencyEO();
                        eeDetails.CompetencyId = objReader["Competancy_ID"] != null ? objReader["Competancy_ID"].ToString() : string.Empty;
                        eeDetails.CompetencyConfigId = objReader["competancy_config_id"] != null ? objReader["competancy_config_id"].ToString() : string.Empty;
                        eeDetails.ObjectiveID = objReader["objective_id"] != null ? objReader["objective_id"].ToString() : string.Empty;
                        eeDetails.ObjectiveDetID = objReader["objective_detail_id"] != null ? objReader["objective_detail_id"].ToString() : string.Empty;
                        eeDetails.CompetencyName = objReader["Competancy_Name"] != null ? objReader["Competancy_Name"].ToString() : string.Empty;
                        eeDetails.Type = objReader["type"] != null ? objReader["type"].ToString() : string.Empty;
                        eeDetails.IsActive = objReader["Is_Active"] != null ? objReader["Is_Active"].ToString() : string.Empty;
                        eeDetails.Rank = objReader["sort_order"] != null ? Convert.ToInt32(objReader["sort_order"]) : 0;
                        eeCompList.Add(eeDetails);
                    }
                    //objAssessmentList.Objectives.Add(objCompEERatList);
                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        RatingGuideLineEO ratGlDetails = new RatingGuideLineEO();
                        ratGlDetails.RatingGuidelineID = objReader["assm_rating_id"] != null ? objReader["assm_rating_id"].ToString() : string.Empty;
                        ratGlDetails.Rating = objReader["rating"] != null ? objReader["rating"].ToString() : string.Empty;
                        ratGlDetails.RatingDescription = objReader["RATING_DESC"] != null ? objReader["RATING_DESC"].ToString() : string.Empty;
                        ratGlDetails.GuidelineDesc = objReader["GL_DESCRIPTION"] != null ? objReader["GL_DESCRIPTION"].ToString() : string.Empty;
                        ratGlDetails.ComplianceDesc = objReader["COMPL_DESC"] != null ? objReader["COMPL_DESC"].ToString() : string.Empty;

                        objAssessmentList.RatingForDropDown.Add(ratGlDetails);
                    }

                }

                dbframework.CloseConnection();
                dbframework = null;
                dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
                List<ODPCommandParameter> parameterListNew = new List<ODPCommandParameter>();
                parameterListNew.Add(new ODPCommandParameter("cur_rating_guideline", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("pbms4_assessment_pkg.sp_get_assm_rating", parameterListNew))
                {
                 while (objReader.Read())
                    {
                        RatingGuideLineEO ratGlDetails = new RatingGuideLineEO();
                        ratGlDetails.RatingGuidelineID = objReader["assm_rating_id"] != null ? objReader["assm_rating_id"].ToString() : string.Empty;
                        ratGlDetails.Rating = objReader["rating"] != null ? objReader["rating"].ToString() : string.Empty;
                        ratGlDetails.RatingDescription = objReader["RATING_DESC"] != null ? objReader["RATING_DESC"].ToString() : string.Empty;
                        ratGlDetails.GuidelineDesc = objReader["GL_DESCRIPTION"] != null ? objReader["GL_DESCRIPTION"].ToString() : string.Empty;
                        ratGlDetails.ComplianceDesc = objReader["COMPL_DESC"] != null ? objReader["COMPL_DESC"].ToString() : string.Empty;

                        objAssessmentList.RatingGuidelines.Add(ratGlDetails);
                    }

                }

                objAssessmentList.Objectives.ForEach(obj =>
                {
                    obj.Competencies = new List<CompetencyEO>();
                    obj.Competencies.AddRange(compList.Where(comp => comp.ObjectiveID.Equals(obj.ObjectiveID)).ToList());
                    obj.ExceedsExpectations = new List<CompetencyEO>();
                    obj.ExceedsExpectations.AddRange(eeCompList.Where(comp => comp.ObjectiveID.Equals(obj.ObjectiveID)).ToList());
                });
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }
            return objAssessmentList;

        }
        #endregion

        #region get Assessor, Assessee and Flight Details  for crew
        /// <summary>
        /// Function returns Assessor, Assessee and Flight Details to display on Assessment screen
        /// </summary>
        /// <param name="assessmentID"></param>
        /// <returns>AssessmentEO entity object containing Assessor, Assessee and Flight Details</returns>
        public async Task<AssessmentEO> GetAssessorAssesseeFlightDetailsAsync(string assessmentID)
        {
            AssessmentEO assessmentDetails = new AssessmentEO();
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_assessmentid", assessmentID, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("cur_flt_dets", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PRISM_MIG_ASSESSMENT.sp_get_assm_flight_details", parameterList))
                {
                    while (objReader.Read())
                    {
                        assessmentDetails.AssessmentID = objReader["assessmentid"] != null ? objReader["assessmentid"].ToString() : string.Empty;
                        assessmentDetails.FlightDetID = objReader["flightdetsid"] != null ? objReader["flightdetsid"].ToString() : string.Empty;
                        assessmentDetails.AssesseeCrewDetID = objReader["assesseecrewdetailsid"] != null ? objReader["assesseecrewdetailsid"].ToString() : string.Empty;
                        assessmentDetails.AssessorCrewDetID = objReader["assessorcrewdetailsid"] != null ? objReader["assessorcrewdetailsid"].ToString() : string.Empty;
                        assessmentDetails.AssessorStaffName = objReader["assessorname"] != null ? objReader["assessorname"].ToString() : string.Empty;
                        assessmentDetails.AssessorStaffNo = objReader["assessorstaffno"] != null ? objReader["assessorstaffno"].ToString() : string.Empty;
                        assessmentDetails.AssessorGrade = objReader["assessorgrade"] != null ? objReader["assessorgrade"].ToString() : string.Empty;
                        assessmentDetails.AssesseeStaffName = objReader["assesseename"] != null ? objReader["assesseename"].ToString() : string.Empty;
                        assessmentDetails.AssesseeStaffNo = objReader["assesseestaffno"] != null ? objReader["assesseestaffno"].ToString() : string.Empty;
                        assessmentDetails.AssesseeGrade = objReader["assesseegrade"] != null ? objReader["assesseegrade"].ToString() : string.Empty;
                        assessmentDetails.ManagerStaffNo = objReader["managerno"] != null ? objReader["managerno"].ToString() : string.Empty;
                        assessmentDetails.ManagerStaffName = objReader["manager"] != null ? objReader["manager"].ToString() : string.Empty;
                        assessmentDetails.AssessmentDate = !string.IsNullOrEmpty(objReader["dateofassessment"].ToString()) ? Convert.ToDateTime(objReader["dateofassessment"].ToString()) : DateTime.MinValue;
                        assessmentDetails.SectorFrom = objReader["sectorfrom"] != null ? objReader["sectorfrom"].ToString() : string.Empty;
                        assessmentDetails.SectorTo = objReader["sectorto"] != null ? objReader["sectorto"].ToString() : string.Empty;
                        assessmentDetails.FlightNumber = objReader["flightnum"] != null ? objReader["flightnum"].ToString() : string.Empty;
                        assessmentDetails.FlightDate = !string.IsNullOrEmpty(objReader["flightdate"].ToString()) ? Convert.ToDateTime(objReader["flightdate"].ToString()) : DateTime.MinValue;
                        assessmentDetails.AssessmentStatus = objReader["status"] != null ? objReader["status"].ToString() : string.Empty;
                        assessmentDetails.AssessmentType = objReader["assessmenttype"] != null ? objReader["assessmenttype"].ToString() : string.Empty;
                        //assessmentDetails.TotalScore = objReader["totalscore"] != null ? Convert.ToDecimal(objReader["totalscore"]) : 0;
                        assessmentDetails.AdditionalComments = objReader["additionalcomments"] != null ? objReader["additionalcomments"].ToString() : string.Empty;
                        assessmentDetails.ReasonForNonSubmission = objReader["reasonfornonsubmission"] != null ? objReader["reasonfornonsubmission"].ToString() : string.Empty;
                        assessmentDetails.Pax_Load_F = objReader["PAX_LOAD_F"] != null && objReader["PAX_LOAD_F"].ToString() != string.Empty ? Convert.ToInt32(objReader["PAX_LOAD_F"]) : 0;
                        assessmentDetails.Pax_Load_J = objReader["PAX_LOAD_J"] != null && objReader["PAX_LOAD_J"].ToString() != string.Empty ? Convert.ToInt32(objReader["PAX_LOAD_J"]) : 0;
                        assessmentDetails.Pax_Load_Y = objReader["PAX_LOAD_Y"] != null && objReader["PAX_LOAD_Y"].ToString() != string.Empty ? Convert.ToInt32(objReader["PAX_LOAD_Y"]) : 0;
                        assessmentDetails.Infants = objReader["infants"] != null && objReader["infants"].ToString() != string.Empty ? Convert.ToInt32(objReader["infants"]) : 0;
                        assessmentDetails.CrewPosition = objReader["crewposition"] != null && objReader["crewposition"].ToString() != string.Empty ? objReader["crewposition"].ToString() : string.Empty;
                        assessmentDetails.AircraftType = objReader["actype"] != null && objReader["actype"].ToString() != string.Empty ? objReader["actype"].ToString() : string.Empty;
                        assessmentDetails.AssesseeDesignation = objReader["designation"] != null && objReader["designation"].ToString() != string.Empty ? objReader["designation"].ToString() : string.Empty;

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

            return assessmentDetails;
        }

        #endregion

        #region get all rating guideline for crew
        /// <summary>
        /// Fetch all rating guidelines for crew
        /// </summary>
        /// <returns>List of RatingGuideLine</returns>
        public async Task<List<RatingGuideLineEO>> GetRatingGuidelinesAsync()
        {
            List<RatingGuideLineEO> ratingGLList = new List<RatingGuideLineEO>();
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("cur_rating_guideline", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("pbms4_assessment_pkg.sp_get_assm_rating", parameterList))
                {
                    while (objReader.Read())
                    {
                        RatingGuideLineEO ratingGuideline = new RatingGuideLineEO();
                        ratingGuideline.RatingGuidelineID = objReader["ASSM_RATING_ID"].ToString();
                        ratingGuideline.Compliance = objReader["COMPLIANCE"].ToString();
                        ratingGuideline.ComplianceDesc = objReader["COMPL_DESC"].ToString();
                        ratingGuideline.GuidelineDesc = objReader["GL_DESCRIPTION"].ToString();
                        ratingGuideline.Rating = objReader["RATING"].ToString();
                        ratingGuideline.IsActive = objReader["IS_ACTIVE"].ToString();
                        ratingGLList.Add(ratingGuideline);
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

            return ratingGLList;
        }

        #endregion

        #region Get IDP type list
        /// <summary>
        /// Function to get the list of IDP type
        /// </summary>
        /// <returns>type name and type id</returns>
        public async Task<Dictionary<string, string>> GetIDPTypeAsync()
        {
            Dictionary<string, string> lstIDPType = new Dictionary<string, string>();
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("prc_idptype", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("pbms4_assessment_pkg.sp_get_idp_type", parameterList))
                {
                    while (objReader.Read())
                    {

                        lstIDPType.Add(objReader["idp_type_name"].ToString(), objReader["idp_type_id"].ToString());

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

            return lstIDPType;
        }
        #endregion

        #region Get Assessment Objective Percentages
        /// <summary>
        /// Function to get Assessment Objective Percentages
        /// </summary>
        /// <returns>Percentage & Id</returns>
        public async Task<Dictionary<string, string>> GetAssessmentObjectivePercentagesAsync()
        {
            Dictionary<string, string> dictPercentages = new Dictionary<string, string>();
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("prc_obj_percentage", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("pbms4_assessment_pkg.sp_get_obj_percentage", parameterList))
                {
                    while (objReader.Read())
                    {

                        dictPercentages.Add(objReader["obj_percentage_id"].ToString(), objReader["obj_percentage_name"].ToString());

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
            return dictPercentages;

        }


        #endregion

        #region Insert or Update Assessment Details, Objective Details and Competency details
        /// <summary>
        /// Function that insert or Update Assessment Details, Objective Details and Competency details
        /// </summary>
        /// <param name="asssessDet">AssessmentDetails</param>
        /// <returns>AssessmentID</returns>
        public async Task<String> Insert_UpdateAssessmentDetAsync(AssessmentEO asssessDet)
        {
            string assessmentID = string.Empty;
            List<ODPCommandParameter> parameterList = null;
            string response = string.Empty;

            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {

                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("pi_assessmentid", string.IsNullOrEmpty(asssessDet.AssessmentID) ? String.Empty : asssessDet.AssessmentID, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_assesseecrewdetailsid", string.IsNullOrEmpty(asssessDet.AssesseeCrewDetID) ? String.Empty : asssessDet.AssesseeCrewDetID, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_assessorcrewdetailsid", string.IsNullOrEmpty(asssessDet.AssessorCrewDetID) ? String.Empty : asssessDet.AssessorCrewDetID, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_assessmenttype", string.IsNullOrEmpty(asssessDet.AssessmentType) ? String.Empty : asssessDet.AssessmentType, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_assessmentstatus", string.IsNullOrEmpty(asssessDet.AssessmentStatus) ? String.Empty : asssessDet.AssessmentStatus, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_dateofassessment", Common.ToOracleDate(asssessDet.AssessmentDate), ParameterDirection.Input, OracleDbType.Date));

                // parameterList.Add(new ODPCommandParameter("pi_dateofassessment", string.IsNullOrEmpty(asssessDet.AssessmentDate.ToString()) ? String.Empty :asssessDet.AssessmentDate.ToString(), ParameterDirection.Input, DbType.DateTime));


                parameterList.Add(new ODPCommandParameter("pi_totalscore", string.IsNullOrEmpty(asssessDet.TotalScore.ToString()) ? String.Empty : asssessDet.TotalScore.ToString(), ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_reasonfornonsubmission", string.IsNullOrEmpty(asssessDet.ReasonForNonSubmission) ? String.Empty : asssessDet.ReasonForNonSubmission, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_additionalcomments", string.IsNullOrEmpty(asssessDet.AdditionalComments) ? String.Empty : asssessDet.AdditionalComments, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_flightdetsid", string.IsNullOrEmpty(asssessDet.FlightDetID) ? String.Empty : asssessDet.FlightDetID, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_assesseegrade", string.IsNullOrEmpty(asssessDet.AssesseeGrade) ? String.Empty : asssessDet.AssesseeGrade, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_assessorgrade", string.IsNullOrEmpty(asssessDet.AssessorGrade) ? String.Empty : asssessDet.AssessorGrade, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_is_active", string.IsNullOrEmpty(asssessDet.IsActive) ? String.Empty : asssessDet.IsActive, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_created_by", string.IsNullOrEmpty(asssessDet.CreatedBy) ? String.Empty : asssessDet.CreatedBy, ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("pi_flightnum", string.IsNullOrEmpty(asssessDet.FlightNumber) ? String.Empty : asssessDet.FlightNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_flightdate", string.IsNullOrEmpty(asssessDet.FlightDate.ToString()) ? String.Empty : Common.OracleDateFormateddMMMyyyy(asssessDet.FlightDate), ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_sectorfrom", string.IsNullOrEmpty(asssessDet.SectorFrom) ? String.Empty : asssessDet.SectorFrom, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_sectorto", string.IsNullOrEmpty(asssessDet.SectorTo) ? String.Empty : asssessDet.SectorTo, ParameterDirection.Input, OracleDbType.Varchar2));


                parameterList.Add(new ODPCommandParameter("po_assessment_id", ParameterDirection.Output, OracleDbType.Varchar2, 250));

                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("prism_mig_assessment.sp_insert_update_assessment", parameterList);
                response = !command.Parameters["po_assessment_id"].Value.Equals(Oracle.DataAccess.Types.OracleString.Null) ? command.Parameters["po_assessment_id"].Value.ToString() : string.Empty;
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

        #region Insert or Update Assessment Objective Details
        /// <summary>
        /// Function that insert or Update Assessment Objective Details
        /// </summary>
        /// <param name="objectDet">ObjectiveEO</param>
        /// <returns>AssessmentObjectID</returns>
        public async Task<String> Insert_UpdateAssessObjectDetAsync(ObjectiveEO objectDet)
        {

            string assessObjectID = string.Empty;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {

                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_assessment_object_id", objectDet.AssessmentObjectiveID ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_assessment_id", objectDet.AssessmentID ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_object_detail_id", objectDet.ObjectiveDetailedID ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_obj_comments", objectDet.Comments ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_obj_score", objectDet.ObjScore, ParameterDirection.Input, OracleDbType.Decimal));
                parameterList.Add(new ODPCommandParameter("pi_obj_rating", objectDet.Rating ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_is_active", objectDet.IsActive ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_created_by", objectDet.CreatedBy ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_assessment_object_id", ParameterDirection.Output, OracleDbType.Varchar2, 250));
                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("pbms4_assessment_pkg.sp_insert_update_assm_det_obj", parameterList);
                assessObjectID = !command.Parameters["po_assessment_object_id"].Value.Equals(Oracle.DataAccess.Types.OracleString.Null) ? command.Parameters["po_assessment_object_id"].Value.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }



            return assessObjectID;
        }
        #endregion

        #region Insert or Update Assessment Competency Details
        /// <summary>
        /// Function that insert or Update Assessment Competency Details
        /// </summary>
        /// <param name="compDet">CompetencyEO</param>
        /// <returns>assessCompID</returns>
        public async Task<String> Insert_UpdateAssessCompDetAsync(CompetencyEO compDet)
        {
            string assessCompID = string.Empty;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {

                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_assessment_comp_id", compDet.AssessmentCompID ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_assessment_object_id", compDet.AssessmentObjectiveID ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_competancy_config_id", compDet.CompetencyConfigId ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_comp_comments", compDet.Comments ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_comp_score", compDet.CompetencyScore, ParameterDirection.Input, OracleDbType.Decimal));
                parameterList.Add(new ODPCommandParameter("pi_comp_rating", compDet.Rating ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_EE_CHECKED", compDet.IsEEChecked ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_is_active", compDet.IsActive ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_created_by", compDet.CreatedBy ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_assessment_comp_id", ParameterDirection.Output, OracleDbType.Varchar2, 250));
                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("pbms4_assessment_pkg.sp_insert_update_assm_det_comp", parameterList);
                assessCompID = !command.Parameters["po_assessment_comp_id"].Value.Equals(Oracle.DataAccess.Types.OracleString.Null) ? command.Parameters["po_assessment_comp_id"].Value.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }


            return assessCompID;
        }
        #endregion

        #region Insert Assessment IDP Details
        /// <summary>
        /// Function that insert Assessment IDP Details
        /// </summary>
        /// <param name="idpDetails">AssessmentIDPEO</param>
        /// <returns>AssessmentIDPEO</returns>
        public async Task<AssessmentIDPEO> Insert_AssessIDPDetailsAsync(AssessmentIDPEO idpDetails)
        {
            AssessmentIDPEO idpDet = new AssessmentIDPEO();
            idpDet = idpDetails;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {

                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_assessment_id", idpDetails.AssessmentID ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_competancy_config_id", idpDetails.CompConfigID ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_observation", idpDetails.Observation ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_situation", idpDetails.Situation ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_expectedresult", idpDetails.ExpectedResult ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_is_active", idpDetails.IsActive ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_created_by", idpDetails.CreatedBy ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_assessment_idp_det_id", ParameterDirection.Output, OracleDbType.Varchar2, 250));
                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync(" pbms4_assessment_pkg.sp_insert_assm_idp_details", parameterList);
                idpDet.AssessmentIDPDetID = !command.Parameters["po_assessment_idp_det_id"].Value.Equals(Oracle.DataAccess.Types.OracleString.Null) ? command.Parameters["po_assessment_idp_det_id"].Value.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }

            return idpDet;
        }
        #endregion

        #region Insert Assessment IDP
        /// <summary>
        /// Function that insert Assessment IDP
        /// </summary>
        /// <param name="assessIDP">AssessmentIDPEO</param>
        /// <returns>AssessmentIDPEO</returns>
        public async Task<AssessmentIDPEO> Insert_AssessIDPAsync(AssessmentIDPEO assessIDP)
        {
            AssessmentIDPEO assessmentIDP = new AssessmentIDPEO();
            assessmentIDP = assessIDP;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {

                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_assessment_id", assessmentIDP.AssessmentID ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_idp_type", assessmentIDP.IDP_Type_ID ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_assesseecrewdetailsid", assessmentIDP.AssesseeCrewDetailsID ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_is_active", assessmentIDP.IsActive ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_created_by", assessmentIDP.CreatedBy ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_idp_comn_no", ParameterDirection.Output, OracleDbType.Varchar2, 250));
                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync(" pbms4_assessment_pkg.sp_insert_assm_idp", parameterList);
                assessmentIDP.IDP_Comn_No = !command.Parameters["po_idp_comn_no"].Value.Equals(Oracle.DataAccess.Types.OracleString.Null) ? command.Parameters["po_idp_comn_no"].Value.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }


            return assessmentIDP;
        }
        #endregion

        #region Delete Assessment IDP Details
        /// <summary>
        /// Function that Delete Assessment IDP Details
        /// </summary>
        /// <param name="assessmentID">assessmentID</param>
        public async void Delete_AssessIDPDetailsAsync(string assessmentID)
        {
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            List<ODPCommandParameter> parameterList = null;
            try
            {

                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_assessment_id", assessmentID, ParameterDirection.Input, OracleDbType.Varchar2));
                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync(" pbms4_assessment_pkg.sp_delete_assm_idp_details", parameterList);
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
        #region Delete unscheduled assessment
        public async void DeleteAssessment(string assessmentID)
        {
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            List<ODPCommandParameter> parameterList = null;
            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_assessment_id", assessmentID, ParameterDirection.Input, OracleDbType.Varchar2));
                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("Prism_Mig_Assessment.Delete_Assessment_Mig", parameterList);
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

        #region GetBehaviourActionIssue
        /// <summary>
        /// Get the details of Crew Behavior for File notes
        /// </summary>
        /// <param name="behaviourID"></param>
        /// <returns></returns>
        public async Task<AssessmentBehaviourEO> GetBehaviourActionIssue(string behaviourID)
        {

            AssessmentBehaviourEO assmt = new AssessmentBehaviourEO();
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_behavior_id", behaviourID, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("cur_behavior", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_idp", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("cur_momnotes", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("pbms4_behavior_pkg.sp_get_behavior_idp_mom", parameterList))
                {
                    while (objReader.Read())
                    {
                        assmt.ReportingDate = objReader["Reporting_Date"] != null ? Convert.ToDateTime(objReader["Reporting_Date"].ToString()) : (DateTime?)null;
                        assmt.StaffName = objReader["StaffName"] != null ? objReader["StaffName"].ToString() : String.Empty;
                        assmt.ObjectiveName = objReader["objective_name"] != null ? objReader["objective_name"].ToString() : String.Empty;
                        assmt.CategoryName = objReader["category_name"] != null ? objReader["category_name"].ToString() : String.Empty;

                        assmt.RootCauseName = objReader["rootcause_name"] != null ? objReader["rootcause_name"].ToString() : String.Empty;
                        assmt.FinalValidity = objReader["FINAL_VALIDITY"] != null ? objReader["FINAL_VALIDITY"].ToString() : String.Empty;
                        assmt.FinalAction = objReader["Final_Action"] != null ? objReader["Final_Action"].ToString() : String.Empty;
                        assmt.DateOfInvestigation = objReader["DATEOFINVESTIGATION"] != null ? Convert.ToDateTime(objReader["DATEOFINVESTIGATION"].ToString()) : (DateTime?)null;
                        assmt.DateOfEffectiveLetter = objReader["DATEOFEFFECTIVELETTER"] != null ? Convert.ToDateTime(objReader["DATEOFEFFECTIVELETTER"].ToString()) : (DateTime?)null;
                        assmt.Situation = objReader["SITUATION"] != null ? objReader["SITUATION"].ToString() : String.Empty;
                        assmt.Observation = objReader["OBSERVATION"] != null ? objReader["OBSERVATION"].ToString() : String.Empty;
                        assmt.ExpectedResult = objReader["EXPECTED_RESULT"] != null ? objReader["EXPECTED_RESULT"].ToString() : String.Empty;
                    }

                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        assmt.IdpObjectiveName = objReader["objective_name"] != null ? objReader["objective_name"].ToString() : String.Empty;
                        assmt.IdpObservation = objReader["OBSERVATION"] != null ? objReader["OBSERVATION"].ToString() : String.Empty;
                        assmt.IdpSituation = objReader["SITUATION"] != null ? objReader["SITUATION"].ToString() : String.Empty;
                        assmt.IdpExpectedResult = objReader["EXPECTED_RESULT"] != null ? objReader["EXPECTED_RESULT"].ToString() : String.Empty;
                        assmt.IdpStartDate = objReader["Start_Date"] != null ? Convert.ToDateTime(objReader["Start_Date"].ToString()) : (DateTime?)null;
                        assmt.IdpEndDate = objReader["End_Date"] != null ? Convert.ToDateTime(objReader["End_Date"].ToString()) : (DateTime?)null;
                        assmt.IdpIsReviewApplicable = objReader["Is_ReviewApplicable"] != null ? objReader["Is_ReviewApplicable"].ToString() : string.Empty;
                        assmt.IsCrewCommentsRequired = objReader["is_crew_comments_reqd"] != null ? objReader["is_crew_comments_reqd"].ToString() : string.Empty;
                        assmt.IdpComment = objReader["Comments"] != null ? objReader["Comments"].ToString() : string.Empty;
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

            return assmt;
        }
        #endregion

        #region Get Assessment Sla Configuration
     
        /// <summary>
        /// Get Assessment Sla Configuration
        /// </summary>
        /// <returns>type name and type id</returns>
        public async Task<int> GetAssessmentSlaConfiguration()
        {
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("po_assm_sla", ParameterDirection.Output, OracleDbType.Varchar2, 100));
                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("pbms4_assessment_pkg.sp_get_assm_slaconfig", parameterList);
                return !command.Parameters["po_assm_sla"].Value.Equals(Oracle.DataAccess.Types.OracleString.Null) ? Convert.ToInt32(command.Parameters["po_assm_sla"].Value.ToString()) : 0;
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

        public async Task<string> InsertIDPCrewComment(string requestId, string createdBy, string crewCommentsRequired, string crewComments)
        {
            string response = string.Empty;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("pi_request_id", requestId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_created_by", createdBy, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_crew_comments_reqd", crewCommentsRequired, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_crew_comments", crewComments, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_msg", ParameterDirection.Output, OracleDbType.Varchar2, 128));

                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("pbms4_behavior_pkg.sp_insert_idp_crewcomments", parameterList);
                response = !command.Parameters["po_msg"].Value.Equals(Oracle.DataAccess.Types.OracleString.Null) ? command.Parameters["po_msg"].Value.ToString() : string.Empty;
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
    }
}
