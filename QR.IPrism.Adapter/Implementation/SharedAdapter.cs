using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using QR.IPrism.Adapter.Helper;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.BusinessObjects.Data_Layer.Shared;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using QR.IPrism.Models;
using QR.IPrism.Models.Shared;
using QR.IPrism.Utility;
using QR.IPrism.Models.Module;

namespace QR.IPrism.Adapter.Implementation
{
    public class SharedAdapter : ISharedAdapter
    {
        private ISharedDao _doShared = new SharedDao();
        #region Analytics
        public async Task<bool> CreateAnalyticEntryAsync(AnalyticModel analytics)
        {
            AnalyticEO eoAnalytics = new AnalyticEO();
            Mapper.Map<AnalyticModel, AnalyticEO>(analytics, eoAnalytics);
            return await _doShared.CreateAnalyticEntryAsync(eoAnalytics);
        }
        //public async Task<bool> CreateAnalyticEntryAsync(string category, string action, string label, string value)
        //{
        //    AnalyticEO eoAnalytics = new AnalyticEO()
        //    {
        //        Category=category,
        //        Action=action,
        //        OptionLabel=label,
        //        OptionValue=value
        //    }
            
        //    //Mapper.Map<AnalyticModel, AnalyticEO>(analytics, eoAnalytics);
        //    //return await _doShared.CreateAnalyticEntryAsync(eoAnalytics);
        //}
        #endregion
        #region User
        public async Task<UserContextModel> GetUserContextAsync(string StaffNumber)
        {
            return Mapper.Map<UserContextEO, UserContextModel>(await _doShared.GetUserContextAsync(StaffNumber));
        }
        public UserContextModel GetUserContext(string StaffNumber)
        {
            return Mapper.Map<UserContextEO, UserContextModel>(_doShared.GetUserContext(StaffNumber));
        }
        public ClientModel FindAPIClient(string clientId)
        {
            return Mapper.Map<ClientEO, ClientModel>(_doShared.FindAPIClient(clientId));
        }
        public async Task<bool> AddUserTokenAsync(UserTokenModel token)
        {
            return await _doShared.AddUserTokenAsync(Mapper.Map(token, new UserTokenEO()));
        }
        public async Task<bool> RemoveUserTokenAsync(string userTokenId)
        {
            return await _doShared.RemoveUserTokenAsync(userTokenId);
        }
        public async Task<UserTokenModel> FindUserTokenAsync(string userTokenId)
        {
            return Mapper.Map<UserTokenEO, UserTokenModel>(await _doShared.FindUserTokenAsync(userTokenId));
        }
        #endregion
        #region Common
        public async Task<List<MessageModel>> GetMessageListAsync()
        {
            return Mapper.Map<List<MessageEO>, List<MessageModel>>(await _doShared.GetMessageListAsyc());
        }
        public List<MessageModel> GetMessageList()
        {
            return Mapper.Map<List<MessageEO>, List<MessageModel>>(_doShared.GetMessageList());
        }
        /// <summary>
        /// Common Info
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<LookupModel>> GetCommonInfoAsyc(string type)
        {
            return Mapper.Map<IEnumerable<LookupEO>, IEnumerable<LookupModel>>(await _doShared.GetCommonInfoAsyc(type));
        }
        public List<LookupModel> GetCommonInfo(string type)
        {
            return Mapper.Map<List<LookupEO>, List<LookupModel>>(_doShared.GetCommonInfo(type));
        }
        #endregion
        #region Menu
        public async Task<List<MenuModel>> GetUserMenuListAsyc(string staffNumber)
        {
            List<MenuEO> menuList = await _doShared.GetUserMenuListAsyc(staffNumber);
            List<MenuModel> userMenuList = null;
            if (menuList != null)
            {
                userMenuList = menuList.RecursiveJoin(menu => menu.MenuId, menu => menu.ParentId, (MenuEO menu, int index, int depth, IEnumerable<MenuModel> subMenus) =>
                   {
                       return new MenuModel()
                        {
                            MenuName = menu.MenuName,
                            MenuId = menu.MenuId,
                            Rank = menu.Rank,
                            Url = menu.Url,
                            Template = menu.Template,
                            Sref = menu.Sref,
                            IsMobileLink = menu.IsMobileLink,
                            IconClass = menu.IconClass,
                            Controller = menu.Controller,
                            SubMenus = subMenus.ToList()
                        };
                   }).ToList();
            }
            return userMenuList;
        }

        public async Task<HeaderViewModule> GetHeaderInfoAsyc(string staffNumber, string staffDetailsId)
        {
            HeaderViewModule headerViewModule = new HeaderViewModule();
            headerViewModule.Menus = (List<MenuModel>)GetUserMenuListAsyc(staffDetailsId).Result;
           // headerViewModule.AlertCounter = await _doShared.GetNotificationsCount(staffNumber);
            var notifications = await SearchNotifications("0", staffNumber);
            foreach (var item in notifications)
            {
                if (item.Type.ToLower() == "assessments" && item.Date.Value.AddDays(12) > DateTime.Today)
                {
                    headerViewModule.NotificationDetails = item;
                }
            }
            return headerViewModule;
        }

        #endregion

        public void InsertAttachment(FileModel docs, string lookupTypeName, string lookupValue, string userId)
        {
            switch (lookupValue)
            {
                case UploadType.Housing:

                    docs.ClassType = GetConfigLookUpDetails(lookupTypeName).Result
                        .Where(i => i.Text == lookupValue).FirstOrDefault().Value;

                    _doShared.InsertAttachment(Mapper.Map(docs, new FileEO()));
                    break;
                case UploadType.RecordAssessment:

                    docs.ClassType = "AssessmentAttachment";
                    //_doShared.DeleteAttachments_comn(docs.ClassKeyId);
                    docs.FileNamePrefix = docs.ClassKeyId;
                    _doShared.InsertEVRBlobAttachment(Mapper.Map(docs, new FileEO()), userId);
                    break;

                case UploadType.EVRSave:
                    docs.ClassType = "PRISM.WEB";
                    _doShared.InsertEVRAttachment(Mapper.Map(docs, new FileEO()), userId);
                    break;
                case UploadType.Kafou:
                    docs.ClassType = "CCRAttachment";
                    docs.FileNamePrefix = docs.ClassKeyId;
                    _doShared.InsertEVRBlobAttachment(Mapper.Map(docs, new FileEO()), userId);
                    break;

            }
        }

        public void DeleteAttachment_entity(List<String> docs)
        {
            _doShared.DeleteAttachments_entity(docs);
        }

        public void DeleteAttachment_comn(List<String> docs)
        {
            _doShared.DeleteAttachments_comn(docs);
        }

        public async Task<IEnumerable<FileModel>> GetAttachments(string requestId)
        {
            return Mapper.Map(await _doShared.GetAttachments(requestId), new List<FileModel>());
        }

        private async Task<List<LookupModel>> GetConfigLookUpDetails(string lookUpTypeName)
        {
            return Mapper.Map(await _doShared.GetConfigLookUpDetails(lookUpTypeName), new List<LookupModel>());
        }

        public async Task<IEnumerable<MessageModel>> GetAllComments(string requestId)
        {
            return Mapper.Map(await _doShared.GetAllComments(requestId), new List<MessageModel>());
        }

        public async Task<IEnumerable<LookupModel>> GetStayOutRequestType()
        {
            return Mapper.Map(await _doShared.GetStayOutRequestType(), new List<LookupModel>());
        }

        public async Task<IEnumerable<LookupModel>> GetSwapRoomsRequestType()
        {
            return Mapper.Map(await _doShared.GetSwapRoomsRequestType(), new List<LookupModel>());
        }

        // CrewProfile - Method to get Crew Personal Details
        public async Task<UserContextModel> GetCrewPersonalDetailsAsync(string StaffNumber, string imagePath, string imageType)
        {
            return Mapper.Map<UserContextEO, UserContextModel>(await _doShared.GetCrewPersonalDetailsAsync(StaffNumber, imagePath,imageType));
        }

        public async Task<UserContextModel> GetCrewPersonalDetailsHeaderAsync(string StaffNumber, string imagePath,string imageType)
        {
            return Mapper.Map<UserContextEO, UserContextModel>(await _doShared.GetCrewPersonalDetailsHeaderAsync(StaffNumber,imagePath,imageType));
        }

        public async Task<IEnumerable<NotificationDetailsModel>> SearchNotifications(string id, string staffNumber)
        {
            var input = new NotificationDetailsModel()
            {
                Id = id,
                ToCrewId = staffNumber,
                Date = DateTime.MinValue,
                ActionByDate = DateTime.MinValue
            };

            return Mapper.Map(await _doShared.SearchNotifications(Mapper.Map(input, new NotificationDetailsEO())), new List<NotificationDetailsModel>());
        }

        public async Task<IEnumerable<NotificationDetailsModel>> GetAllNotifications(NotificationDetailsModel input)
        {
            if (input.Date == null)
            {
                input.Date = DateTime.MinValue;
            }

            if (input.ActionByDate == null)
            {
                input.ActionByDate = DateTime.MinValue;
            }
            return Mapper.Map(await _doShared.SearchNotifications(Mapper.Map(input, new NotificationDetailsEO())), new List<NotificationDetailsModel>());
        }



        public async Task<bool> UpdateCrewNotificationDetails(NotificationDetailsModel model)
        {
            return await _doShared.UpdateCrewNotificationDetails(Mapper.Map(model, new NotificationDetailsEO()));
        }

        public async Task<IEnumerable<LookupModel>> GetAllLookUpDetails(string lookUpName)
        {
            return Mapper.Map(await _doShared.GetAllLookUpDetails(lookUpName), new List<LookupModel>());
        }

        public async Task<IEnumerable<LookupModel>> GetAllLookUpWithParentDetails(string lookUpName, string parentlookuptypename, string parentlookupname)
        {
            return Mapper.Map(await _doShared.GetAllLookUpWithParentDetails(lookUpName, parentlookuptypename, parentlookupname), new List<LookupModel>());
        }

        public async Task<IEnumerable<KeyContactsModel>> GetKeyContactsInfoAsyc()
        {
            return Mapper.Map(await _doShared.GetKeyContactsAsync(), new List<KeyContactsModel>());
        }


        public void EVR_SetDeletedDocInActive(List<string> docs)
        {
            _doShared.EVR_SetDeletedDocInActive(docs);
        }


        public Task<string> GetConfigFromDB(string key)
        {
            return _doShared.GetConfigFromDBAsyc(key);
        }
    }
}
