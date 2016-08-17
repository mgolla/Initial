/*********************************************************************
 * Name          : IAssessmentAdapter.cs
 * Description   : Interface for Assessment details for DAO layer.
 * Create Date   : 29th Feb 2016
 * Last Modified : 29th Feb 2016
 * Copyright By  : Qatar Airways
 *********************************************************************/

using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;
using QR.IPrism.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Adapter.Interfaces
{
   public interface IAssessmentAdapter
    {
        /// <summary>
        /// Function that returns View Assement Details
        /// </summary>
        /// <param name="assessmentID">assessmentID</param>
        /// <returns>AssessmentViewModel entity contains list results of Objectives, Competencies Default, 
        /// Competencies EE, Ratings and IDP details along with Assessment IDP</returns>
       Task<AssessmentViewModel> GetAssessmentDetailsAsync(String assessmentID);
        

        /// <summary>
        /// Function that returns EOAssessment entity contains IList results of Objectives, Competencies Default, Competencies EE and Ratings
        /// </summary>
        /// <param name="grade"></param>
       /// <returns>AssessmentViewModel entity contains IList results of Objectives, Competencies Default, Competencies EE and Ratings</returns>
        Task<AssessmentViewModel> GetAssessmnetByGradeAsync(string grade);

        /// <summary>
        /// Function returns Assessor, Assessee and Flight Details to display on Assessment screen
        /// </summary>
        /// <param name="assessmentID"></param>
        /// <returns>AssessmentModel entity object containing Assessor, Assessee and Flight Details</returns>
        Task<AssessmentModel> GetAssessorAssesseeFlightDetailsAsync(string assessmentID);
        

        /// <summary>
        /// Fetch all rating guidelines for crew
        /// </summary>
        /// <returns>List of RatingGuideLineModel</returns>
        Task<IEnumerable<RatingGuideLineModel>> GetRatingGuidelinesAsync();

        /// <summary>
        /// Function to get the list of IDP type
        /// </summary>
        /// <returns>type name and type id</returns>
        Task<Dictionary<string, string>> GetIDPTypeAsync();

        ///// <summary>
        ///// Function to get Assessment Objective Percentages
        ///// </summary>
        ///// <returns>Percentage & Id</returns>
        //Task<Dictionary<string, string>> GetAssessmentObjectivePercentagesAsync();

        /// <summary>
        /// Function that Delete Assessment IDP Details
        /// </summary>
        /// <param name="assessmentID">assessmentID</param>
       // void Delete_AssessIDPDetailsAsync(string assessmentID);

        /// <summary>
        /// Function Insert or Update Assessement details along with Assessment Objective details and Assessment Competency details
        /// </summary>
        /// <param name="objAssessment">AssessmentEO</param>
        /// <returns>AssessmentModel</returns
        Task<AssessmentModel> Insert_UpdateAssessObjectCompDetailsAsync(AssessmentModel objAssessment, string staffNo);

        Task<AssessmentBehaviourModel> GetBehaviourActionIssue(string behaviourID);
        Task<string> InsertIDPCrewComment(string requestId, string createdBy, string crewCommentsRequired, string crewComments);
        void DeleteAssessment(string id);
    }
}
