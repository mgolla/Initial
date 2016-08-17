/*********************************************************************
 * Name          : IAssessmentListAdapter.cs
 * Description   : Interface for Getting Housing details for DAO layer.
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
   public interface IAssessmentListAdapter
    {
        /// <summary>
        /// Gets the Assessment request search result from DAO layer.
        /// </summary>
        /// <param name="inputs">Assessment request filter</param>
        /// <returns>List of HousingModel</returns>
       Task<IEnumerable<AssessmentSearchModel>> GetAssessmentListResultAsync(String id);
       Task<AssessmentPOViewModel> GetPOAssessmentDataAsync(AssessmentSearchRequestFilterModel filterInput);
       //Task<AssessmentPOViewModel> GetSearchCSDCSResultAsync(AssessmentSearchRequestFilterModel filterInput);
       void CancelSchedluedAsmnt(string requestId);
       Task<ResponseModel> ScheduleAssessment(AssessmentModel input);
       Task<IEnumerable<IGrouping<string,AssessmentModel>>> ViewAssmtAsync(string id);
       
       Task<IEnumerable<AssessmentSearchModel>> GetPreviousAssessmentsAsync(AssessmentSearchModel model, string LoggedInStaffDetailId);
    }
}
