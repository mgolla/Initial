using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Web.API.Shared;
using QR.IPrism.Models;
using QR.IPrism.Models.Shared;
using System.Web;
using System.IO;
using QR.IPrism.Models.Module;
using System.Threading.Tasks;
using QR.IPrism.Web.Helper;
using System.Text;
using QR.IPrism.Web.Models;
using QR.IPrism.Utility;
using QR.IPrism.Security.Authentication;
using QR.IPrism.Web.Common;

namespace QR.IPrism.Web.API.Controllers.Shared
{
    [Authorize(Roles = "F1, F2, CS, CSD, PO, LT, Admin, CrewLocator")]
    public class SharedController : ApiBaseController
    {
        private readonly ISharedAdapter _srdAdapter;
        private readonly ISecurityManager _securityManager;
        private readonly ITokenManager _tokenManager;
        public SharedController(ISharedAdapter srdAdapter, ISecurityManager securityManager, ITokenManager tokenManager)
        {
            _srdAdapter = srdAdapter;
            _securityManager = securityManager;
            _tokenManager = tokenManager;
        }
        [Route("api/analytics/post")]
        public HttpResponseMessage PostAnalytics(AnalyticModel analytics)
        {
            bool isSuccess = Convert.ToBoolean(_srdAdapter.CreateAnalyticEntryAsync(analytics).Result);
            return Request.CreateResponse(isSuccess ? HttpStatusCode.OK : HttpStatusCode.BadRequest, "Success");
        }

        [Route("api/messages/get")]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _srdAdapter.GetMessageListAsync().Result);
        }
        [Route("api/usermenu/get")]
        public HttpResponseMessage GetUserMenu()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _srdAdapter.GetUserMenuListAsyc(LoggedInStaffDetailId).Result);
        }
        [Route("api/commoninfo/{id}")]
        public HttpResponseMessage GetCommonInfo(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _srdAdapter.GetCommonInfoAsyc(id).Result);
        }

        [Route("api/crewpersonaldetails/")]
        public HttpResponseMessage PostCrewProfileInfo(CommonFilterModel crewInfo)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _srdAdapter.GetCrewPersonalDetailsAsync(LoggedInStaffNo, crewInfo.CrewImagePath, crewInfo.CrewImageType).Result);
        }

        //[Route("api/crewpersonaldetailsheader/")]
        //public HttpResponseMessage GetCrewProfileInfoHeader()
        //{
        //    return Request.CreateResponse(HttpStatusCode.OK, _srdAdapter.GetCrewPersonalDetailsHeaderAsync(LoggedInStaffNo).Result);
        //}

        [Route("api/headerInfoAll/")]
        public HttpResponseMessage PostHeaderInfo(CommonFilterModel crewInfo)
        {
            HeaderViewModule headerViewModule = _srdAdapter.GetHeaderInfoAsyc(LoggedInStaffNo, LoggedInStaffDetailId).Result;
            headerViewModule.StaffInfo = UserContext;
            headerViewModule.StaffInfo.CrewPhotoUrl = crewInfo.CrewImagePath + "/" + LoggedInStaffNo + crewInfo.CrewImageType;
            return Request.CreateResponse(HttpStatusCode.OK, headerViewModule);
        }

        [Route("api/document/postdocument")]
        public HttpResponseMessage PostDocument()
        {
            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;

            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);

                var path = Path.Combine(
                    HttpContext.Current.Server.MapPath("~/uploads"),
                    fileName
                );
            }

            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        [Route("api/fileupload")]
        [HttpPost]
        public HttpResponseMessage PostData()
        {
            var fileModel = new FileModel();

            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
            fileModel.FileName = file.FileName;
            fileModel.CreatedBy = LoggedInStaffDetailId;
            var uploadType = string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                fileModel.FileContent = memoryStream.ToArray();
            }

            if (IsValidFileExtension(fileModel.FileContent))
            {
                StreamReader reader = new StreamReader(HttpContext.Current.Request.InputStream);
                string requestFromPost = reader.ReadToEnd();

                foreach (string key in HttpContext.Current.Request.Form.AllKeys)
                {
                    if (key == "RequestId")
                        fileModel.ClassKeyId = HttpContext.Current.Request.Form[key];

                    if (key == "UploadType")
                        uploadType = HttpContext.Current.Request.Form[key];

                    if (key == "fileNamePrefix")
                        fileModel.FileNamePrefix = HttpContext.Current.Request.Form[key];
                }

                _srdAdapter.InsertAttachment(fileModel, HousingConstants.EntityType, uploadType, LoggedInUserId);

                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Error");
        }


        private bool IsValidFileExtension(byte[] sixBytes)
        {
            //var validFileExtantionSignatures = new List<byte[]> {
            //    new byte[] { 208,207,17, 224, 161, 177,26,225,0,0,0,0,0,0 }, // DOC, XLS, PPT
            //    new byte[] { 80,75,3,4,20,0,6,0,8,0,0,0,33,0 }, //DOCX, XLSX, PPTX
            //    new byte[] { 80,75,3,4,10,0,6,0,8,0,0,0,33,0 }, // XLSX new
            //    new byte[] { 37,80,68,70,45,49,46,53,13,10,37,181,181,181 },  // PDF
            //    new byte[] { 37,80,68,70,45,49,46,52,10,37,226,227,207,211 },  // PDF new 
            //    new byte[] { 66,77,134,68,17,0,0,0,0,0,54,0,0,0 },  // BMP,
            //    new byte[] { 71,73,70,56,57,97,50,0,50,0,242,0,0,255 },  // GIF
            //    new byte[] { 255,216,255,224,0,16,74,70,73,70,0,1,1,1 },  // JPG
            //    new byte[] { 255,216,255,225,0,24,69,120,105,102,0,0,73,73 }, // JPG new 
            //    new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72 }  // PNG
            //};


            var validFileExtantionSignatures = new List<byte[]> {
                new byte[] { 208,207,17,224 }, // DOC, XLS, PPT
                new byte[] { 80,75,3,4 }, //DOCX, XLSX, PPTX
                //new byte[] { 80,75,3,4 }, // XLSX new
                new byte[] { 37,80,68,70 },  // PDF
                //new byte[] { 37,80,68,70 },  // PDF new 
                new byte[] { 66,77,134,68},  // BMP,
                new byte[] { 71,73,70,56 },  // GIF
                new byte[] { 255,216,255,224 },  // JPG
                new byte[] { 255,216,255,225 }, // JPG new 
                new byte[] { 137,80,78,71}  // PNG
            };


            byte[] validSignature = validFileExtantionSignatures.Where(signature => sixBytes.Take(4).SequenceEqual(signature)).FirstOrDefault();

            return validSignature == null ? false : true;

        }


        [Route("api/usercontext")]
        public HttpResponseMessage GetUserContext()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _securityManager.GetSecurityContext());
        }

        [Route("api/refreshecontext")]
        public HttpResponseMessage GetRefreshedContext()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _tokenManager.GetAntiForgeryToken());
        }

        [Route("api/shiftdltcntxt")]
        public HttpResponseMessage ShiftDeleteContext()
        {
            _securityManager.ClearSecurityContext();
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        [Route("api/getLoggedInStaffNoAndGrade/")]
        public HttpResponseMessage GetLoggedInStaffNoAndGrade()
        {
            //Check for Acting CS/CSD
            return Request.CreateResponse(HttpStatusCode.OK, UserContext.Grade + "_$$_" + UserContext.StaffNumber + "_$$_" + UserContext.JoiningDate);
        }

        [Route("api/getimpusrtoken/{id}")]
        public HttpResponseMessage GetImpUsrToken(string id)
        {
            string errMessage = _securityManager.SetImpersoneteUserContext(id);
            return Request.CreateResponse(HttpStatusCode.OK, errMessage);
        }

        [Route("api/getkeycontacts/")]
        public HttpResponseMessage GetKeyContactsInfo()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _srdAdapter.GetKeyContactsInfoAsyc().Result);
        }

        [Route("api/deleteComnDoc")]
        public HttpResponseMessage PostDeleteDocument(List<String> docs)
        {
            _srdAdapter.DeleteAttachment_comn(docs);
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }


        [Route("api/getconfigfromdb/{key}")]
        public HttpResponseMessage GetConfigFromDBForKey(string key)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _srdAdapter.GetConfigFromDB(key).Result);
        }
    }
}