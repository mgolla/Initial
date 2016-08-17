using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.ViewModels;

namespace QR.IPrism.Adapter.Interfaces
{
    public interface IPOAssessmentAdapter
    {
        Task<AssessmentViewModel> GetPOAssessmentDataAsync(AssessmentSearchRequestFilterModel filterInput);
    }
}
