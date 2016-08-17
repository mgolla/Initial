using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using QR.IPrism.Adapter.Helper;
using QR.IPrism.Adapter.Implementation;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Models.Shared;
using QR.IPrism.Security.Authentication;
using QR.IPrism.Utility;

namespace QR.IPrism.Security
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        ISharedAdapter _srdAdapter = null;
        ISecurityManager _securityManager = null;
        public AuthorizationServerProvider()
        {
            _srdAdapter = new SharedAdapter();
            _securityManager = new SecurityManager();
        }
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            ClientModel client = null;
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }
            client = _srdAdapter.FindAPIClient(context.ClientId.ToString().Replace("QR\\", ""));

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == Convert.ToInt32(ApplicationTypes.Secured))
            {
                if (string.IsNullOrWhiteSpace(client.Key))
                {
                    context.SetError("invalid_clientId", "Client key should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    //if (Common.Decrypt(client.Key, SecuredConstants.ClientKey) != Common.Decrypt(clientSecret, SecuredConstants.ClientKey))
                    //{
                    //    context.SetError("invalid_clientId", "Client key is invalid.");
                    //    return Task.FromResult<object>(null);
                    //}
                }
            }

            if (client.IsActive.Equals(Constants.IN_ACTIVE,StringComparison.OrdinalIgnoreCase))
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.Options.AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(client.TokenLifeTime);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.TokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
  
            UserContextModel user = await _srdAdapter.GetUserContextAsync(Common.Decrypt(context.UserName, SecuredConstants.UserIdKey));
            if (user == null)
            {
                context.SetError("invalid_grant", "Invalid User.");
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, Common.Decrypt(context.UserName, SecuredConstants.UserIdKey)));

            //identity.AddClaim(new Claim("role", "user"));

            var props = new AuthenticationProperties(new Dictionary<string, string>
                 {
                     { 
                         "as:client_id", string.IsNullOrEmpty(context.ClientId) ? string.Empty : context.ClientId
                     },
                     { 
                         "LoggedInUser",  Common.Decrypt(context.UserName, SecuredConstants.UserIdKey)
                     },
                      { 
                         "LoggedInUserDetailsId", user.CrewDetailsId
                     },
                      { 
                         "UserId", user.UserId
                     }
                 });

            var ticket = new AuthenticationTicket(identity, props);
            bool isValidated=context.Validated(ticket);
            //if (isValidated)
            //    _securityManager.SetAuthenticatedUserContext(user);

        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
    }
}
