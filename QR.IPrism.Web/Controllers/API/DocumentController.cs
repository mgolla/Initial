
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
using System.Web.Script.Serialization;


namespace QR.IPrism.API.Controllers.Module
{
     [Authorize(Roles = "F1, F2, CS, CSD, PO, LT, Admin, CrewLocator")]
    public class DocumentController : ApiBaseController
    {
        private readonly IDashboardAdapter _iDashboardAdapter;
        public DocumentController(IDashboardAdapter iDashboardAdapter)
        {
            _iDashboardAdapter = iDashboardAdapter;
        }
        public HttpResponseMessage Get()
        {
            DocumentFilterModel filter = new DocumentFilterModel();
            filter.DocumentId = "";

            return Request.CreateResponse(HttpStatusCode.OK, _iDashboardAdapter.GetDocumentsAsyc(filter).Result);
        }
        public HttpResponseMessage Post(DocumentFilterModel filter)
        {
           
            if (filter.DocumentId == string.Empty)
            {
                filter.DocumentPath = @"/Documents";
            }
            return Request.CreateResponse(HttpStatusCode.OK, _iDashboardAdapter.GetDocumentsAsyc(filter).Result);
        }
        [Route("api/filemg")]
        public HttpResponseMessage PostFileMg(dynamic filter)
        {
            //Test Data for folder structure 
            //FileManagerResult resultFM = new FileManagerResult();
            //List<FileManagerData> list = new List<FileManagerData>();
            //list.Add(new FileManagerData()
            //{
            //    DocumentId = "2",
            //    name = "joomla",
            //    rights = "drwxr-xr-x",
            //    size = "4096",
            //    date = "2013-11-01 11:44:13",
            //    type = "dir"
            //});


            //var file = _iDashboardAdapter.GetDocumentsAsyc(filterd).Result.Documents.FirstOrDefault();
            //return Request.CreateResponse(HttpStatusCode.OK, _iDashboardAdapter.GetDocumentsAsyc(filterd).Result);
            //list.Add(new FileManagerData()
            //{
            //    DocumentId = file.DocumentId,
            //    FileCode = file.FileCode,
            //    FileId = file.FileId,
            //    DocType = file.RowType,
            //    name = file.DocumentName,                              
            //    rights = "-rw-r--r--",
            //    size = "",
            //    date = "2013-11-01 11:44:13",
            //    type = "file"
            //});
            //filterd.DocumentId = "7";
            //var file = _iDashboardAdapter.GetDocumentsAsyc(filterd).Result.Documents.FirstOrDefault();
            ////return Request.CreateResponse(HttpStatusCode.OK, _iDashboardAdapter.GetDocumentsAsyc(filterd).Result);
            //list.Add(new FileManagerData()
            //{
            //    DocumentId = file.DocumentId,
            //    FileCode = file.FileCode,
            //    FileId = file.FileId,
            //    DocType = file.RowType,
            //    name = file.DocumentName,
            //    rights = "-rw-r--r--",
            //    size = "",
            //    date = "2013-11-01 11:44:13",
            //    type = "file"
            //});
            //resultFM.result = list;
            DocumentFilterModel filterd = new DocumentFilterModel();
           
             
          

            foreach (Newtonsoft.Json.Linq.JProperty para in filter)
            {
                foreach (Newtonsoft.Json.Linq.JProperty item in para.Value)
                {
                    if (item.Name == "documentId")//property name
                    {
                        filterd.DocumentId = item.Value.ToString();

                    }
                    if (item.Name == "path")//property name
                    {
                        filterd.DocumentPath = item.Value.ToString();

                    }

                }
            }
            if (filterd.DocumentPath == @"/")
            {
                filterd.DocumentPath = filterd.DocumentPath + "Documents";
                filterd.DocumentId = "";
                if (filterd.DocumentId != "")
                {
                    // filterd.DocumentPath = "";
                }
                else
                {

                }
            }
            else
            {
                filterd.DocumentPath = "/Documents" + filterd.DocumentPath;
                filterd.DocumentId = "";
            }

            return Request.CreateResponse(HttpStatusCode.OK, _iDashboardAdapter.GetDocumentsFileMgAsyc(filterd).Result);


        }
    }
}

