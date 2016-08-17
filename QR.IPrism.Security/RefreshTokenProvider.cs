using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Infrastructure;
using QR.IPrism.Adapter.Helper;
using QR.IPrism.Adapter.Implementation;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Models.Shared;
using QR.IPrism.Utility;

namespace QR.IPrism.Security
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        ISharedAdapter _srdAdapter = null;
        public RefreshTokenProvider()
        {
            _srdAdapter = new SharedAdapter();
        }
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

            var token = new UserTokenModel()
            {
                UserTokenId = Common.Encrypt(refreshTokenId, SecuredConstants.TokenIdKey),
                ClientId = clientid,
                Subject="iPrim",
                StaffNo = context.Ticket.Identity.Name,
                IssuedOn = DateTime.Now,
                ExpiresOn = DateTime.Now.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedOn;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresOn;

            token.Token = context.SerializeTicket();

            var result = await _srdAdapter.AddUserTokenAsync(token);

            if (result)
            {
                context.SetToken(refreshTokenId);
            }
        }
        public void Create(AuthenticationTokenCreateContext context) { }
        public void Receive(AuthenticationTokenReceiveContext context) { }
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string tokenId = Common.Encrypt(context.Token, SecuredConstants.TokenIdKey);

            var refreshToken = await _srdAdapter.FindUserTokenAsync(tokenId);

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.Token);
                var result = await _srdAdapter.RemoveUserTokenAsync(tokenId);
            }
        }

    }
}
