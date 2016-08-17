using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Script.Serialization;
using QR.IPrism.Adapter.Implementation;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Web.API.Controllers.Shared;
using QR.IPrism.Web.Models;
using RestSharp;

namespace QR.IPrism.Web.Helper
{
    public class SharedBundleTransform : IBundleTransform
    {
        private const string JsContentType = "text/javascript";
        private readonly string moduleName;
        private ISharedAdapter _srdAdapter = DependencyResolver.Current.GetService<SharedAdapter>();
        private string _staffNumber = string.Empty;

        public SharedBundleTransform(string moduleName,  string staffNumber)
        {
            this.moduleName = moduleName;
            this._staffNumber = staffNumber;
        }

        public virtual void Process(BundleContext context, BundleResponse response)
        {
            context.HttpContext.Response.Cache.SetLastModified(DateTime.Now);

            if (string.IsNullOrWhiteSpace(this.moduleName))
            {
                response.Content = "// No or wrong app name defined";
                response.ContentType = JsContentType;
                return;
            }
            var messages = _srdAdapter.GetMessageList();
            int index = 0;
            var contentBuilder = new StringBuilder();
            contentBuilder.Append("(function(){");
            contentBuilder.Append("angular.module('" + this.moduleName + "').constant('messages',{");

            var last = messages.Last();
            if (messages != null && messages.Count > 0)
            {
                messages.ForEach(message =>
                {
                    if (index != 0)
                        contentBuilder.Append(",");
                    contentBuilder.AppendFormat("'{0}':'{1}'",
                         message.MessageCode.Replace(":", "_"),
                            HttpUtility.JavaScriptStringEncode(message.Message));
                    index++;
                });
                contentBuilder.Append(",");
                contentBuilder.AppendFormat("'{0}':'{1}'",
                        "TimeOutMinutes",
                           HttpUtility.JavaScriptStringEncode(Convert.ToString(ConfigurationManager.AppSettings["TimeOutMinutes"])));
            }

            contentBuilder.Append("});");
            contentBuilder.Append("})();");

            //index = 0;
            //var appSettings = _srdAdapter.GetCommonInfo("APPCONFIG");
            //contentBuilder.Append("(function(){");
            //contentBuilder.Append("angular.module('" + this.moduleName + "').constant('appSettings',{");

            //if (appSettings != null && appSettings.Count() > 0)
            //{
            //    foreach (var setting in appSettings)
            //    {
            //        if (index != 0)
            //            contentBuilder.Append(",");
            //        contentBuilder.AppendFormat("'{0}':'{1}'",
            //             setting.Value.Replace(":", "_"),
            //                HttpUtility.JavaScriptStringEncode(setting.Text));
            //        index++;
            //    }
            //    contentBuilder.Append(",");
            //    contentBuilder.AppendFormat("'{0}':'{1}'",
            //             "API", ConfigurationManager.AppSettings["WebApiUrl"].ToString());
            //}

            //contentBuilder.Append("});");
            //contentBuilder.Append("})();");

            //index = 0;
            //var userMenu = await _srdAdapter.GetUserMenuListAsyc(_staffNumber);
            //contentBuilder.Append("(function(){");
            //contentBuilder.Append("angular.module('" + this.moduleName + "').constant('user_menu',{");

            //contentBuilder.AppendFormat("'{0}':{1}",
            //     "UserMenu", userMenu);
            //contentBuilder.Append("});");
            //contentBuilder.Append("})();");

            context.UseServerCache = false;
            response.Cacheability = HttpCacheability.Public;
            response.Content = contentBuilder.ToString();
            response.ContentType = JsContentType;
        }
    }
}