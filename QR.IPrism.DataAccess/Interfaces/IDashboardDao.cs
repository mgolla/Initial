using QR.IPrism.EntityObjects.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.BusinessObjects.Interfaces
{
    public interface IDashboardDao
    {
        Task<List<NotificationAlertSVPEO>> GetNotificationAlertSVPsAsyc(NotificationAlertSVPFilterEO filterInput);
        Task<List<NotificationAlertSVPEO>> GetSVPMessagesAsyc();
        Task<List<DocumentEO>> GetDocumentsAsyc(DocumentFilterEO filterInput);
        Task<List<DepartmentNewsIFEGuideEO>> GetNewsAsycByType(NewsFilterEO filter);
       
        Task<List<DepartmentNewsIFEGuideEO>> GetIFEGuidesAsyc();
        Task<List<FileEO>> GetFilesAsyc(FileFilterEO filterInput);
        Task<List<LinkEO>> GetLinksAsyc();
        Task<List<VisionMissionEO>> GetVisionMissionsAsyc();
        Task<List<MyRequestEO>> GetMyRequestsAsyc(MyRequestFilterEO filterInput);
        Task<List<NotificationAlertSVPEO>> GetAlertNotificationHeaderAsyc(NotificationAlertSVPFilterEO filterInput);
        Task<bool> UpdateAlertNotificationOnHeader(NotificationAlertSVPFilterEO filterInput);
    }
}
