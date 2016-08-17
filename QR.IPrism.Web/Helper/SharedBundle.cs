using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace QR.IPrism.Web.Helper
{
    public class SharedBundle : Bundle
    {
        public SharedBundle(string moduleName,string staffNumber, string virtualPath)
            : base(virtualPath, null, new[] { (IBundleTransform)new SharedBundleTransform(moduleName, staffNumber) })
        {
        }
    }
}