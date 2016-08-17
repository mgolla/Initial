/*********************************************************************
 * Name         : IAssessmentDao.cs
 * Description  : Interface for Getting data from Database.
 * Create Date  : 29th Feb 2016
 * Last Modified: 29th Feb 2016
 * Copyright By : Qatar Airways
 *********************************************************************/
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.BusinessObjects.Interfaces
{
    public interface IAssessmentDao
    {
        /// <summary>
        /// Function that returns View Assement Details
        /// </summary>
        /// <param name="assessmentID"></param>
        /// <returns>ILIST with search results for viewing complete assessments</returns>
        ///<returns>EOAssessment entity contains list results of Objectives, Competencies Default, 
        /// Competencies EE, Ratings and IDP details along with Assessment IDP</returns>
        Task<AssessmentEO> GetAssessmentDetailsAsync(string assessmentID);

        /// <summary>
        /// Function that returns EOAssessment entity contains IList results of Objectives, Competencies Default, Competencies EE and Ratings
        /// </summary>
        /// <param name="grade"></param>
        /// <returns>EOAssessment entity contains IList results of Objectives, Competencies Default, Competencies EE and Ratings</returns>
        Task<AssessmentEO> GetAssessmnetByGradeAsync(string grade);

         /// <summary>
        /// Function returns Assessor, Assessee and Flight Details to display on Assessment screen
        /// </summary>
        /// <param name="assessmentID"></param>
        /// <returns>AssessmentEO entity object containing Assessor, Assessee and Flight Details</returns>
        Task<AssessmentEO> GetAssessorAssesseeFlightDetailsAsync(string assessmentID);

        /// <summary>
        /// Fetch all rating guidelines for crew
        /// </summary>
        /// <returns>List of EORatingGuideLine</returns>
        Task<List<RatingGuideLineEO>> GetRatingGuidelinesAsync();

         /// <summary>
        /// Function to get the list of IDP type
        /// </summary>
        /// <returns>type name and type id</returns>
        Task<Dictionary<string, string>> GetIDPTypeAsync();

        /// <summary>
        /// Function to get Assessment Objective Percentages
        /// </summary>
        /// <returns>Percentage & Id</returns>
        Task<Dictionary<string, string>> GetAssessmentObjectivePercentagesAsync();

        /// <summary>
        /// Function that insert or Update Assessment Details, Objective Details and Competency details
        /// </summary>
        /// <param name="asssessDet">AssessmentDetails</param>
        /// <returns>AssessmentID</returns>
        Task<String> Insert_UpdateAssessmentDetAsync(AssessmentEO asssessDet);

        /// <summary>
        /// Function that insert or Update Assessment Objective Details
        /// </summary>
        /// <param name="objectDet">ObjectiveEO</param>
        /// <returns>AssessmentObjectID</returns>
        Task<String> Insert_UpdateAssessObjectDetAsync(ObjectiveEO objectDet);

        /// <summary>
        /// Function that insert or Update Assessment Competency Details
        /// </summary>
        /// <param name="compDet">CompetencyEO</param>
        /// <returns>assessCompID</returns>
        Task<String> Insert_UpdateAssessCompDetAsync(CompetencyEO compDet);

        /// <summary>
        /// Function that insert Assessment IDP
        /// </summary>
        /// <param name="assessIDP">AssessmentIDPEO</param>
        /// <returns>AssessmentIDPEO</returns>
        Task<AssessmentIDPEO> Insert_AssessIDPAsync(AssessmentIDPEO assessIDP);

        /// <summary>
        /// Function that insert Assessment IDP Details
        /// </summary>
        /// <param name="idpDetails">AssessmentIDPEO</param>
        /// <returns>AssessmentIDPEO</returns>
        Task<AssessmentIDPEO> Insert_AssessIDPDetailsAsync(AssessmentIDPEO idpDetails);

        /// <summary>
        /// Function that Delete Assessment IDP Details
        /// </summary>
        /// <param name="assessmentID">assessmentID</param>
        void Delete_AssessIDPDetailsAsync(string assessmentID);

        Task<AssessmentBehaviourEO> GetBehaviourActionIssue(string behaviourID);
        Task<string> InsertIDPCrewComment(string requestId, string createdBy, string crewCommentsRequired, string crewComments);
        Task<int> GetAssessmentSlaConfiguration();
        void DeleteAssessment(string assessmentID);
    }
}
