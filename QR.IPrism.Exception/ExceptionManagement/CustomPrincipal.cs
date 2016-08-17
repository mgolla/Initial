using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace QR.IPrism.Exception.ExceptionManagement
{
    public class CustomPrincipal : System.Security.Principal.IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public CustomPrincipal(string username)
        {
            this.Identity = new GenericIdentity(username);
        }

        public bool IsInRole(string role)
        {
            return Identity != null && Identity.IsAuthenticated &&
               !string.IsNullOrWhiteSpace(role) && Roles.IsUserInRole(Identity.Name, role);
        }

        public string FirstName { get; set; }
        public int Id { get; set; }

        public string LastName { get; set; }

        public string FullName { get { return FirstName + " " + LastName; } }
    }
}