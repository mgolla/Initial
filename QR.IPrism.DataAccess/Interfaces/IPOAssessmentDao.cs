using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;

namespace QR.IPrism.BusinessObjects.Interfaces
{
    public interface IPOAssessmentDao
    {
        Task<List<AssessmentEO>> GetPOAssmtListAsync(AssessmentSearchRequestFilterEO filter);
        //Task<List<AssessmentEO>> GetCSDListAsync(AssessmentSearchRequestFilterEO filter);
        //Task<List<AssessmentEO>> GetCSListAsync(AssessmentSearchRequestFilterEO filter);
    }
}
