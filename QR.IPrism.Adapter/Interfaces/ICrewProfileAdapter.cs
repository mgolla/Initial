using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Adapter.Interfaces
{
    public interface ICrewProfileAdapter
    {
        Task<List<TrainingHistoryModel>> GetTrainingHistory(string StaffNumber);
        Task<List<QualnVisaModel>> GetQualnVisaDetails(string StaffNumber);
        Task<List<CareerPathModel>> GetCareerPathDetails(string StaffNumber);
        Task<List<IDPModel>> GetIDPDetails(string StaffNumber);
        Task<List<FileModel>> GetMyDocDetails(string StaffNumber);
        Task<List<DestinationsVisitedModel>> GetDestinationsVisitedDetails(string StaffNumber);
    }
}
