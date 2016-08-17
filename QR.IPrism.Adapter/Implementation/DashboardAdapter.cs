using AutoMapper;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.BusinessObjects.Implementation;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QR.IPrism.Adapter.Implementation
{
    public class DashboardAdapter : IDashboardAdapter
    {
        private IDashboardDao _iDashboardDao = new DashboardDao();

        /// <summary>
        /// Get NotificationAlertSVPs 
        /// </summary>
        /// <returns></returns>
        public async Task<NotificationAlertSVPViewModel> GetNotificationAlertsAsyc(NotificationAlertSVPFilterModel filterInput)
        {
            //Define variables 
            List<NotificationAlertSVPModel> notificationAlertSVPList = new List<NotificationAlertSVPModel>();
            NotificationAlertSVPViewModel vm = new NotificationAlertSVPViewModel();


            List<NotificationAlertSVPEO> notificationAlertSVP = await _iDashboardDao.GetNotificationAlertSVPsAsyc(Mapper.Map(filterInput, new NotificationAlertSVPFilterEO())); //Get NotificationAlertSVP data from stored procedure                        
            Mapper.Map<List<NotificationAlertSVPEO>, List<NotificationAlertSVPModel>>(notificationAlertSVP, notificationAlertSVPList);


            vm.NotificationAlertSVPs = notificationAlertSVPList;


            return vm;
        }

        public async Task<List<AirlinesNewsModel>> GetAirlinesNewsAsyc(NewsFilterModel filter)
        {
            List<AirlinesNewsModel> vm = new List<AirlinesNewsModel>();

            //var RSSURL = "http://www.flightglobal.com/rss/airlines-news-rss/";
            //WebProxy proxyObject =
            //    //new WebProxy(Convert.ToString(ConfigurationManager.AppSettings["ProxyURL"]), true);
            //    new WebProxy("http://proxy.qatarairways.com.qa:8080", true);
            //WebRequest req = WebRequest.Create(RSSURL);
            //proxyObject.Credentials =
            //    new System.Net.NetworkCredential("iprismsupport", "Doha123", "QR");
            ////new System.Net.NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["ProxyAccount"]), Convert.ToString(ConfigurationManager.AppSettings["ProxyPassword"]), Convert.ToString(ConfigurationManager.AppSettings["ProxyDomain"]));


            var RSSURL = filter.RSSUrl.ToString().Trim();
            WebProxy proxyObject =

                new WebProxy(filter.ProxyURL.ToString().Trim(), true);
            WebRequest req = WebRequest.Create(RSSURL.ToString().Trim());
            proxyObject.Credentials =
                new System.Net.NetworkCredential(filter.ProxyAccount.ToString().Trim(), filter.ProxyPassword.ToString().Trim(), filter.ProxyDomain.ToString().Trim());
           

            req.Proxy = proxyObject;

            IWebProxy objIWebProxy = GlobalProxySelection.Select;
            GlobalProxySelection.Select = proxyObject;

            XElement airlineNews = XElement.Load(RSSURL);

            int count = 1;
            foreach (XElement items in airlineNews.Element("channel").Elements("item"))
            {
                AirlinesNewsModel airlinesNewsModel = new AirlinesNewsModel();

                string title = items.Element("title").Value;
                if (title.Length > 28)
                {
                    title = title.Substring(0, 25) + "...";
                }

                airlinesNewsModel.Title = title;
                airlinesNewsModel.Link = items.Element("link").Value;
                airlinesNewsModel.Description = items.Element("description").Value;
                airlinesNewsModel.PostedDate = items.Element("pubDate").Value;
                airlinesNewsModel.Tooltip = items.Element("title").Value;
                          

                //string imagePath = GetGroupAirlineImage(count);
                //airlinesNewsModel.ImageSrc = imagePath;
                //count++;
                vm.Add(airlinesNewsModel);
            }



            return vm;
        }

        //private string GetGroupAirlineImage(int _index)
        //{
        //    try
        //    {
        //        using (SPWeb site = SPContext.Current.Site.OpenWeb())
        //        {
        //            //Get the IDs of the visited alerts for the logged in crew
        //            SPList groupAirlinesImages = site.Lists["Group_Airlines_News_Images"];
        //            SPQuery query = new SPQuery();

        //            int imageIndex = _index % 10;
        //            query.Query = "<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + imageIndex.ToString() + "</Value></Eq></Where>";

        //            SPListItemCollection imageCol = groupAirlinesImages.GetItems(query);

        //            //To hold the alerts...
        //            foreach (SPListItem lstImage in imageCol)
        //            {
        //                //For displaying the image
        //                //To get the images from attachments in Group_Airlines_News_Images list
        //                SPAttachmentCollection attachments = lstImage.Attachments;

        //                if (attachments.Count != 0)
        //                {
        //                    Uri imageUri;
        //                    string imagePath = attachments.UrlPrefix + attachments[0];
        //                    imageUri = new Uri(imagePath, UriKind.RelativeOrAbsolute);
        //                    return imageUri.ToString();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        CGErrorLog.WriteIntoLogFile("Layer : User Control , Class Name : UCGroupDepAirlineNews ,Method Name : GetGroupAirlineImage ", exc.Message);
        //    }
        //    return "/_layouts/Images/cabincrew_images/QR1.gif";
        //}
       
        public async Task<NotificationAlertSVPViewModel> GetAlertNotificationHeaderAsyc(NotificationAlertSVPFilterModel filterInput)
        {
            //Define variables 
            List<NotificationAlertSVPModel> notificationAlertSVPList = new List<NotificationAlertSVPModel>();
            NotificationAlertSVPViewModel vm = new NotificationAlertSVPViewModel();


            List<NotificationAlertSVPEO> notificationAlertSVP = await _iDashboardDao.GetAlertNotificationHeaderAsyc(Mapper.Map(filterInput, new NotificationAlertSVPFilterEO())); //Get NotificationAlertSVP data from stored procedure                        
            Mapper.Map<List<NotificationAlertSVPEO>, List<NotificationAlertSVPModel>>(notificationAlertSVP, notificationAlertSVPList);


            vm.NotificationAlertSVPs = notificationAlertSVPList;


            return vm;
        }

        public async Task<bool> UpdateAlertNotificationOnHeader(NotificationAlertSVPFilterModel filterInput)
        {
            return await _iDashboardDao.UpdateAlertNotificationOnHeader(Mapper.Map(filterInput, new NotificationAlertSVPFilterEO()));
        }
        /// <summary>
        /// Get SVP Messages
        /// </summary>
        /// <returns></returns>
        public async Task<NotificationAlertSVPViewModel> GetSVPMessagesAsyc()
        {
            //Define variables 
            List<NotificationAlertSVPModel> notificationAlertSVPList = new List<NotificationAlertSVPModel>();
            NotificationAlertSVPViewModel vm = new NotificationAlertSVPViewModel();


            List<NotificationAlertSVPEO> notificationAlertSVP = await _iDashboardDao.GetSVPMessagesAsyc(); //Get NotificationAlertSVP data from stored procedure                        
            Mapper.Map<List<NotificationAlertSVPEO>, List<NotificationAlertSVPModel>>(notificationAlertSVP, notificationAlertSVPList);


            vm.NotificationAlertSVPs = notificationAlertSVPList;
                //.Where(v => v.DocType.ToString().Trim() != "FOLDER" && v.DocType.ToString().Trim() != "PDF" && v.File != null && v.File.FileName != null).ToList();


            return vm;
        }
        /// <summary>
        /// Get Documents 
        /// </summary>
        /// <returns></returns>
        public async Task<DocumentViewModel> GetDocumentsAsyc(DocumentFilterModel filterInput)
        {
            //Define variables 
            List<DocumentModel> documentList = new List<DocumentModel>();
            DocumentViewModel vm = new DocumentViewModel();


            List<DocumentEO> document = await _iDashboardDao.GetDocumentsAsyc(Mapper.Map(filterInput, new DocumentFilterEO())); //Get Document data from stored procedure                        
            Mapper.Map<List<DocumentEO>, List<DocumentModel>>(document, documentList);

         

            vm.Documents = documentList;
            return vm;
        }
        public async Task<DocumentViewModel> GetDocumentsFileMgAsyc(DocumentFilterModel filterInput)
        {
            //Define variables 
            List<DocumentModel> documentList = new List<DocumentModel>();
            DocumentViewModel docVModel = new DocumentViewModel();
            List<DocumentEO> document = await _iDashboardDao.GetDocumentsAsyc(Mapper.Map(filterInput, new DocumentFilterEO())); //Get Document data from stored procedure                        
            Mapper.Map<List<DocumentEO>, List<DocumentModel>>(document, documentList);

            documentList.ForEach(x =>
            {
                x.name = x.DocumentName; if (x.RowType.ToUpper() == "FOLDER") { x.type = "dir"; } else { x.type = "file"; }
                x.date = "2015-11-01 11:44:13";
                x.rights = "drwxr-xr-x";
                x.size = "4096";
                x.DocumentCount = x.DocumentCount;
            });
            docVModel.result = documentList;

            return docVModel;
        }
        /// <summary>
        /// Get DepartmentNews
        /// </summary>
        /// <returns></returns>
        public async Task<DepartmentNewsIFEGuideViewModel> GetNewsAsycByType(NewsFilterModel filter)
        {
            //Define variables 
            List<DepartmentNewsIFEGuideModel> departmentNewsIFEGuideList = new List<DepartmentNewsIFEGuideModel>();
            DepartmentNewsIFEGuideViewModel vm = new DepartmentNewsIFEGuideViewModel();

            List<DepartmentNewsIFEGuideEO> departmentNewsIFEGuide = await _iDashboardDao.GetNewsAsycByType(Mapper.Map(filter, new NewsFilterEO())); //Get DepartmentNewsIFEGuide data from stored procedure                        
            Mapper.Map<List<DepartmentNewsIFEGuideEO>, List<DepartmentNewsIFEGuideModel>>(departmentNewsIFEGuide, departmentNewsIFEGuideList);
            
            vm.DepartmentNewsIFEGuides = departmentNewsIFEGuideList;

            return vm;
        }
       
        /// <summary>
        /// Get IFEGuides 
        /// </summary>
        /// <returns></returns>
        public async Task<DepartmentNewsIFEGuideViewModel> GetIFEGuidesAsyc()
        {
            //Define variables 
            List<DepartmentNewsIFEGuideModel> departmentNewsIFEGuideList = new List<DepartmentNewsIFEGuideModel>();
            DepartmentNewsIFEGuideViewModel vm = new DepartmentNewsIFEGuideViewModel();
            
            List<DepartmentNewsIFEGuideEO> departmentNewsIFEGuide = await _iDashboardDao.GetIFEGuidesAsyc(); //Get DepartmentNewsIFEGuide data from stored procedure                        
            Mapper.Map<List<DepartmentNewsIFEGuideEO>, List<DepartmentNewsIFEGuideModel>>(departmentNewsIFEGuide, departmentNewsIFEGuideList);
            
            vm.DepartmentNewsIFEGuides = departmentNewsIFEGuideList;

            return vm;
        }
        /// <summary>
        /// Get Files 
        /// </summary>
        /// <returns></returns>
        public async Task<FileViewModel> GetFilesAsyc(FileFilterModel filterInput)
        {
            //Define variables 
            List<FileModel> fileList = new List<FileModel>();
            FileViewModel vm = new FileViewModel();
            if (filterInput.FileCode != null && filterInput.FileId != null)
            {
            List<FileEO> file = await _iDashboardDao.GetFilesAsyc(Mapper.Map(filterInput, new FileFilterEO())); //Get File data from stored procedure                        
            Mapper.Map<List<FileEO>, List<FileModel>>(file, fileList);
            }

            vm.Files = fileList;


            return vm;
        }
        /// <summary>
        /// Get Links 
        /// </summary>
        /// <returns></returns>
        public async Task<LinkViewModel> GetLinksAsyc()
        {
            //Define variables 
            List<LinkModel> linkList = new List<LinkModel>();
            LinkViewModel vm = new LinkViewModel();

            List<LinkEO> link = await _iDashboardDao.GetLinksAsyc(); //Get Link data from stored procedure                        
            Mapper.Map<List<LinkEO>, List<LinkModel>>(link, linkList);

            vm.Links = linkList;

            return vm;
        }
        /// <summary>
        /// Get VisionMissions 
        /// </summary>
        /// <returns></returns>
        public async Task<VisionMissionViewModel> GetVisionMissionsAsyc()
        {
            //Define variables 
            List<VisionMissionModel> visionMissionList = new List<VisionMissionModel>();
            VisionMissionViewModel vm = new VisionMissionViewModel();

            List<VisionMissionEO> visionMission = await _iDashboardDao.GetVisionMissionsAsyc(); //Get VisionMission data from stored procedure                        
            Mapper.Map<List<VisionMissionEO>, List<VisionMissionModel>>(visionMission, visionMissionList);

            vm.VisionMissions = visionMissionList;
            return vm;
        }
        /// <summary>
        /// Get MyRequests 
        /// </summary>
        /// <returns></returns>
        public async Task<MyRequestViewModel> GetMyRequestsAsyc(MyRequestFilterModel filterInput)
        {
            //Define variables 
            List<MyRequestModel> myRequestList = new List<MyRequestModel>();
            MyRequestViewModel vm = new MyRequestViewModel();

            List<MyRequestEO> myRequest = await _iDashboardDao.GetMyRequestsAsyc(Mapper.Map(filterInput, new MyRequestFilterEO())); //Get MyRequest data from stored procedure                        
            Mapper.Map<List<MyRequestEO>, List<MyRequestModel>>(myRequest, myRequestList);

            vm.MyRequests = myRequestList;
            return vm;
        }
    }
}
