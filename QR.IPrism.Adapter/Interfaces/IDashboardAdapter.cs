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
    public interface IDashboardAdapter
    {
        Task<NotificationAlertSVPViewModel> GetNotificationAlertsAsyc(NotificationAlertSVPFilterModel filterInput);
        Task<NotificationAlertSVPViewModel> GetSVPMessagesAsyc();
        Task<DocumentViewModel> GetDocumentsAsyc(DocumentFilterModel filterInput);
        Task<DocumentViewModel> GetDocumentsFileMgAsyc(DocumentFilterModel filterInput);
        Task<DepartmentNewsIFEGuideViewModel> GetNewsAsycByType(NewsFilterModel filter);
        Task<DepartmentNewsIFEGuideViewModel> GetIFEGuidesAsyc();
        Task<FileViewModel> GetFilesAsyc(FileFilterModel filterInput);
        Task<LinkViewModel> GetLinksAsyc();
        Task<VisionMissionViewModel> GetVisionMissionsAsyc();
        Task<MyRequestViewModel> GetMyRequestsAsyc(MyRequestFilterModel filterInput);
        Task<NotificationAlertSVPViewModel> GetAlertNotificationHeaderAsyc(NotificationAlertSVPFilterModel filterInput);
        Task<bool> UpdateAlertNotificationOnHeader(NotificationAlertSVPFilterModel filterInput);
        Task<List<AirlinesNewsModel>> GetAirlinesNewsAsyc(NewsFilterModel filter);
    }
}
