using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.ViewModels;
using QR.IPrism.Models.Shared;

namespace QR.IPrism.Adapter.Interfaces
{
    public  interface IAssessmentSearchAdapter
    {
        /// <summary>
        /// Function that returns ILIST results for searched assessment list
        /// </summary>
        /// <param name="input"></param>
        /// <returns>ILIST with search results for Searched list assessments</returns>
        ///
        Task<IEnumerable<AssessmentSearchModel>> GetAssmtSearchResultAsync(AssessmentSearchRequestFilterModel assessmentSearchFilterModel);

        Task<IEnumerable<AssessmentSearchModel>> SavedUnscheduledAssessmentAsync(AssessmentSearchModel filter);
        Task<ResponseModel> ValidateUnscheduledData(AssessmentSearchModel filter);
        Task<List<AssessmentSearchModel>> GetCrewExpectedAsmnt(AssessmentSearchRequestFilterModel assessmentSearchFilterModel);
        Task<List<AssessmentModel>> GetAllPreviousAssessment(string id);

    }
}
