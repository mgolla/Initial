using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace QR.IPrism.Web.Helper
{
    public class SharedBundleHelper
    {
        const string MessageScriptFileTemplate = "<script id='ipmClientTransform' src=\"{0}\"/>";

        public static MvcHtmlString ResolveBundleUrl(string bundleUrl, bool bundle)
        {
            return BundledFiles(BundleTable.Bundles.ResolveBundleUrl(bundleUrl));
        }

        private static MvcHtmlString BundledFiles(string bundleVirtualPath)
        {
            return new MvcHtmlString(string.Format(MessageScriptFileTemplate, bundleVirtualPath));
        }
        public static MvcHtmlString Render(string bundleUrl, bool bundle)
        {
            return ResolveBundleUrl(bundleUrl, bundle);
        }
    }
}