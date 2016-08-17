using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;

namespace QR.IPrism.BusinessObjects.Interfaces
{
    public interface IAssessmentSearchDao
    {
        /// <summary>
        /// Function that returns ILIST results for searched assessment list
        /// </summary>
        /// <param name="input"></param>
        /// <returns>ILIST with search results for Searched list assessments</returns>
        ///
        Task<List<AssessmentSearchEO>> GetAssmtSearchResultAsync(AssessmentSearchRequestFilterEO assmtSearchFilterEO);
        Task<List<AssessmentSearchEO>> SavedUnscheduledAssessmentAsync(AssessmentSearchEO filter);
        Task<ResponseEO> ValidateUnscheduledData(AssessmentSearchEO filter);
        Task<List<AssessmentSearchEO>> GetCrewExpectedAsmnt(AssessmentSearchRequestFilterEO filter);
        Task<List<AssessmentSearchRequestFilterEO>> GetCrewOnBoard(AssessmentSearchRequestFilterEO filter);
        Task<List<AssessmentEO>> GetAllPreviousAssessment(string id);
    }
}
