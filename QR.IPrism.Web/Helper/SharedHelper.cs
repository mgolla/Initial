using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using RestSharp;

namespace QR.IPrism.Web.Helper
{
    public static class SharedHelper
    {
       

        public static List<T> ExecuteAction<T>(string apiAction)
        {
            List<T> result = default(List<T>);
            string webApiUrl = ConfigurationManager.AppSettings["WebApiUrl"].ToString();

            RestClient restClient = new RestClient(webApiUrl);
            IRestRequest request = new RestRequest(apiAction, Method.GET);

            IRestResponse response = restClient.Execute(request);
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            result = jsonSerializer.Deserialize<List<T>>(response.Content);

            return result;
        }

        public static string ExecuteAction(string apiAction)
        {
            string webApiUrl = ConfigurationManager.AppSettings["WebApiUrl"].ToString();

            RestClient restClient = new RestClient(webApiUrl);
            IRestRequest request = new RestRequest(apiAction, Method.GET);

            IRestResponse response = restClient.Execute(request);
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return response.Content;
        }

       
    }
}