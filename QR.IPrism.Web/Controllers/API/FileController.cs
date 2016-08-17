using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Web.API.Shared;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QR.IPrism.Adapter.Interfaces;
using System.IO;

namespace QR.IPrism.API.Controllers.Module
{
     [Authorize(Roles = "F1, F2, CS, CSD, PO, LT, Admin, CrewLocator")]
    public class FileController : ApiBaseController
    {
        private readonly IDashboardAdapter _iDashboardAdapter;
        public FileController(IDashboardAdapter iDashboardAdapter)
        {
            _iDashboardAdapter = iDashboardAdapter;
        }
        public HttpResponseMessage Get()
        {
            FileFilterModel filter = new FileFilterModel();
            filter.FileId = "8";
            filter.FileCode = "DOCUMENTS";
            byte[] content = null;
            HttpResponseMessage result = null;

            var files = _iDashboardAdapter.GetFilesAsyc(filter).Result.Files;
            if (files.Count == 0)
            {
                result = Request.CreateResponse(HttpStatusCode.Gone);
            }
            else
            {
                var file = files.FirstOrDefault();
                if (file != null)
                {
                    // sendo file to client
                    // byte[] bytes = Convert.FromBase64String(file.pdfBase64);
                    byte[] bytes = file.FileContent;

                    result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = file.FileName + ".pdf";
                    return result;
                }
            }
            result = Request.CreateResponse(HttpStatusCode.Gone);
            return result;
        }

        [Route("api/File/post")]
        [HttpPost]
        public HttpResponseMessage PostFile(FileFilterModel filter)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _iDashboardAdapter.GetFilesAsyc(filter).Result);
        }

        [Route("api/FileMgDownload/post")]
        [HttpPost]
        public HttpResponseMessage PostFile(dynamic filterdata )
        {
            FileFilterModel filter = new FileFilterModel();
            filter.FileCode = "DOCUMENTS";
            filter.FileId = "7";
            byte[] content = null;

            HttpResponseMessage result = null;

            var files = _iDashboardAdapter.GetFilesAsyc(filter).Result.Files;
            if (files.Count == 0)
            {
                result = Request.CreateResponse(HttpStatusCode.Gone);
            }
            else
            {
                var file = files.FirstOrDefault();
                if (file != null)
                {
                    byte[] bytes = file.FileContent;
                   bytes=  File.ReadAllBytes("D:/Temp/pdf/Koala.jpg");

                    //result = Request.CreateResponse(HttpStatusCode.OK);
                    //result.Content = new ByteArrayContent(bytes);
                    //result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    //result.Content.Headers.ContentDisposition.FileName = file.FileName + ".pdf";

                    FileManagerResultContecnt resultFM = new FileManagerResultContecnt();
                    resultFM.result = System.Text.Encoding.ASCII.GetString(bytes);
                   

                    return Request.CreateResponse(HttpStatusCode.OK, resultFM);
                   

                }
            }
            result = Request.CreateResponse(HttpStatusCode.Gone);
            return result;
        }

        [Route("api/pdf/post")]
        [HttpPost]
        public HttpResponseMessage PostPDF(FileFilterModel filter)
        {
          
            byte[] content = null;
           
            HttpResponseMessage result = null;
           
                var files = _iDashboardAdapter.GetFilesAsyc(filter).Result.Files;
                //return Request.CreateResponse(HttpStatusCode.NotFound, "File not found!!!");
                

                if (files.Count==0) {
                    result = Request.CreateResponse(HttpStatusCode.NoContent);
                    result.Content = null; 
                    
                    return result;
                }
                else
                {
                    var file = files.FirstOrDefault();
                    if (file != null)
                    {
                   
                        byte[] bytes = file.FileContent;
                        //System.IO.File.WriteAllBytes("D:\\Mack\\Temphello" + DateTime.Now.ToString().Replace(" ", "-").Replace(":", "-").Replace("/", "-") + ".pdf", bytes);
                        result = Request.CreateResponse(HttpStatusCode.OK);
                        result.Content = new ByteArrayContent(bytes);
                        result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                        result.Content.Headers.ContentDisposition.FileName = file.FileName + ".pdf";
                        return result;
                    }
                }
                result = Request.CreateResponse(HttpStatusCode.Gone);
                return result;
           
        }
    }
    public class FileManagerResultContecnt
    {
        public string result { get; set; }
    }
}

