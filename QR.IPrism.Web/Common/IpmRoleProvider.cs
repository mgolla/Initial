using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using QR.IPrism.Models.Shared;
using QR.IPrism.Security.Authentication;

namespace QR.IPrism.Web.Common
{
    public class IpmRoleProvider : RoleProvider
    {
        ISecurityManager _securityManager=null;
        public IpmRoleProvider() { _securityManager = new SecurityManager(); }

        public UserContextModel UserContext {
            get { return _securityManager.GetLoggedinUserContext(); }
        }


        public override bool IsUserInRole(string username, string roleName)
        {
            return UserContext!=null && UserContext.Role!=null && UserContext.Role.Count>0
                && UserContext.Role.Any(rl=>rl.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }


        public override string[] GetRolesForUser(string username)
        {
            if (UserContext == null || UserContext.Role == null)
                return new string[0];
            
            return UserContext.Role.Select(r => r.Name).ToArray();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}