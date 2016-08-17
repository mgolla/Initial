/*********************************************************************
 * Name         : IAssessmentListDao.cs
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
   public interface IAssessmentListDao
    {
        /// <summary>
        /// Function that returns ILIST results for completed assessment 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>ILIST with search results for viewing complete assessments</returns>
        ///
       Task<List<AssessmentSearchEO>> GetAssessmentListResultAsync(string id);
       Task<List<AssessmentSearchEO>> GetCSDCSListResultAsync(AssessmentSearchRequestFilterEO inputs);
       //Task<List<AssessmentSearchEO>> GetSearchCSDCSResultAsync(AssessmentSearchRequestFilterEO inputs);
       void CancelSchedluedAsmnt(string requestId);
       Task<ResponseEO> ScheduleAssessment(AssessmentEO inputts);

       Task<IEnumerable<AssessmentEO>> ViewAssmtAsync(string id);

       Task<IEnumerable<AssessmentSearchEO>> GetPreviousAssessmentsAsync(AssessmentSearchEO filter, string staffId);
    }
}
