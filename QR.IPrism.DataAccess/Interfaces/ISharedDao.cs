using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.EntityObjects.Shared;
using QR.IPrism.EntityObjects.Module;

namespace QR.IPrism.BusinessObjects.Interfaces
{
    public interface ISharedDao
    {
        Task<List<MessageEO>> GetMessageListAsyc();
        List<MessageEO> GetMessageList();
        Task<UserContextEO> GetUserContextAsync(string staffNumber);
        UserContextEO GetUserContext(string staffNumber);
        Task<bool> CreateAnalyticEntryAsync(AnalyticEO analytics);
        ClientEO FindAPIClient(string clientId);
        Task<bool> AddUserTokenAsync(UserTokenEO token);
        Task<bool> RemoveUserTokenAsync(string userTokenId);
        Task<UserTokenEO> FindUserTokenAsync(string userTokenId);
        Task<List<MenuEO>> GetUserMenuListAsyc(string staffNumber);
        Task<IEnumerable<LookupEO>> GetCommonInfoAsyc(string type);
        List<LookupEO> GetCommonInfo(string type);
        Task<IEnumerable<LookupEO>> GetHousingRequestTypeAsync();
        Task<IEnumerable<LookupEO>> GetHousingStatusAsync();
        Task<IEnumerable<LookupEO>> GetHousingRequestTypeByStaffAsync(string staffNo);
        void InsertAttachment(FileEO docs);
        void DeleteAttachments_entity(List<String> docs);
        void DeleteAttachments_comn(List<String> docs);
        Task<IEnumerable<FileEO>> GetAttachments(string requestId);
        Task<IEnumerable<LookupEO>> GetConfigLookUpDetails(string lookUpTypeName);
        Task<bool> InsertCrewNotificationDetails(NotificationDetailsEO input, string staffNo);
        Task<bool> UpdateCrewNotificationDetails(NotificationDetailsEO input);
        Task<IEnumerable<NotificationDetailsEO>> SearchNotifications(NotificationDetailsEO input);
        Task<IEnumerable<MessageEO>> GetAllComments(string requestId);
        void InsertComment(MessageEO comments);
        Task<IEnumerable<LookupEO>> GetStayOutRequestType();
        Task<IEnumerable<LookupEO>> GetSwapRoomsRequestType();
        Task<UserContextEO> GetCrewPersonalDetailsAsync(string staffNumber, string imagePath, string imageType);
        Task<UserContextEO> GetCrewPersonalDetailsHeaderAsync(string staffNumber, string imagePath, string imageType);
        Task<IEnumerable<LookupEO>> GetAllVRStatusAsync();
        //Task<IEnumerable<LookupEO>> GetAllVRSectorsAsync();
        Task<IEnumerable<LookupEO>> GetAllCoutriesAsync();
        Task<IEnumerable<LookupEO>> GetAllAirportCodesAsync();
        Task<IEnumerable<LookupEO>> GetAllCurrencyCodesAsync();

        Task<IEnumerable<LookupEO>> GetAllReportAboutAsync();
        Task<IEnumerable<LookupEO>> GetAllCategoryForReportAboutAsync(string reportAbtId);
        Task<IEnumerable<LookupEO>> GetAllCategoryForSubCategoryAsync(string categoryId);
        Task<IEnumerable<LookupEO>> GetAllLookUpDetails(string lookUpName);
        //Task<IEnumerable<LookupEO>> GetAllDepartForEVRAsync(string reportId, string categoryId, string subCategoryId);
        Task<IEnumerable<LookupEO>> GetAllDepartForEVRAsync(string[] filterIds);    
        void InsertEVRAttachment(FileEO docs, string userId);
        void InsertEVRBlobAttachment(FileEO docs, string userId);
        Task<IEnumerable<LookupEO>> GetAllLookUpWithParentDetails(string lookUpName, string parentlookuptypename, string parentlookupname);
        Task<IEnumerable<LookupEO>> GetGradeAsync();
       
        Task<IEnumerable<LookupEO>> GetAssmtStatusAsync();
        Task<IEnumerable<LookupEO>> GetPendingAssessmentAsync();
        Task<IEnumerable<LookupEO>> GetGradeCSDCS();
        //Task<IEnumerable<LookupEO>> GetSectorFrom();
        //Task<IEnumerable<LookupEO>> GetSectorTo();
        Task<IEnumerable<LookupEO>> GetSectorAsync();

        Task<int> GetNotificationsCount(string staffID);
        Task<IEnumerable<LookupEO>> GetEVROwnerNonOwnerAsyc(string[] filterIds);
        Task<IEnumerable<LookupEO>> GetReasonForRecNonAsmtAsync();
        Task<IEnumerable<KeyContactsEO>> GetKeyContactsAsync();
        void EVR_SetDeletedDocInActive(List<String> docs);
        Task<string> GetConfigFromDBAsyc(string key);

    }
}
