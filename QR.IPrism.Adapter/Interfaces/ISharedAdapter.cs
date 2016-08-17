using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Adapter.Helper;
using QR.IPrism.Models;
using QR.IPrism.Models.Shared;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using QR.IPrism.Models.Module;

namespace QR.IPrism.Adapter.Interfaces
{

    public interface ISharedAdapter
    {
        Task<bool> CreateAnalyticEntryAsync(AnalyticModel analytics);
        Task<List<MessageModel>> GetMessageListAsync();
        List<MessageModel> GetMessageList();
        Task<UserContextModel> GetUserContextAsync(string StaffNumber);
        UserContextModel GetUserContext(string StaffNumber);
        ClientModel FindAPIClient(string clientId);
        Task<bool> AddUserTokenAsync(UserTokenModel token);
        Task<bool> RemoveUserTokenAsync(string userTokenId);
        Task<UserTokenModel> FindUserTokenAsync(string userTokenId);
        Task<List<MenuModel>> GetUserMenuListAsyc(string staffNumber);
        Task<IEnumerable<LookupModel>> GetCommonInfoAsyc(string type);
        List<LookupModel> GetCommonInfo(string type);
        Task<IEnumerable<FileModel>> GetAttachments(string requestId);
        void InsertAttachment(FileModel docs, string lookupTypeName, string lookupValue, string userId);
        void DeleteAttachment_entity(List<String> docs);
        void DeleteAttachment_comn(List<String> docs);
        Task<IEnumerable<MessageModel>> GetAllComments(string requestId);
        Task<IEnumerable<LookupModel>> GetStayOutRequestType();
        Task<IEnumerable<LookupModel>> GetSwapRoomsRequestType();
        Task<UserContextModel> GetCrewPersonalDetailsAsync(string StaffNumber, string imagePath, string imageType);
        Task<UserContextModel> GetCrewPersonalDetailsHeaderAsync(string StaffNumber, string imagePath, string imageType);
        Task<IEnumerable<NotificationDetailsModel>> SearchNotifications(string id, string staffNumber);
        Task<IEnumerable<NotificationDetailsModel>> GetAllNotifications(NotificationDetailsModel input);
        Task<bool> UpdateCrewNotificationDetails(NotificationDetailsModel model);
        Task<IEnumerable<LookupModel>> GetAllLookUpDetails(string lookUpName);
        Task<IEnumerable<LookupModel>> GetAllLookUpWithParentDetails(string lookUpName, string parentlookuptypename, string parentlookupname);
        Task<HeaderViewModule> GetHeaderInfoAsyc(string staffNumber, string staffDetailsId);
        Task<IEnumerable<KeyContactsModel>> GetKeyContactsInfoAsyc();
        void EVR_SetDeletedDocInActive(List<String> docs);
        Task<string> GetConfigFromDB(string key);
    }
}
