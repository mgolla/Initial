using QR.IPrism.EntityObjects.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.BusinessObjects.Interfaces
{
    public interface ICrewProfileDao
    {
        Task<List<TrainingHistoryEO>> GetCrewTrainingHistoryAsync(string staffNumber);
        Task<List<QualnVisaEO>> GetCrewQualnVisaAsync(string staffNumber);
        Task<List<CareerPathEO>> GetCrewCareerPathAsync(string staffNumber);
        Task<List<IDPEO>> GetCrewIDPAsync(string staffNumber);
        Task<List<FileEO>> GetCrewMyDocAsync(string staffNumber);
        Task<List<DestinationsVisitedEO>> GetCrewDstVstdAsync(string staffNumber);
    }
}
