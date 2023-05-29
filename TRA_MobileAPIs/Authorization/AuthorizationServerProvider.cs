
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using TRA_MobileAPIs.ConfigSettings;
using System;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Tasks;
namespace TRA_MobileAPIs.Authorization
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider

    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
           await Task.Run(()=> context.Validated()); // 
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {

            // Change authentication ticket for refresh token requests  
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);

        }


        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            ConfigData _serviceConfiguration = ConfigEncrypt.GetCrmCredentials();

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            //  Accounts acc = new Accounts();

            string username = Encryption.Auth_Decrypt(_serviceConfiguration.Auth_UserName);
            string password = Encryption.Auth_Decrypt(_serviceConfiguration.Auth_Password);


            if (context.UserName == username && context.Password == password)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));

                identity.AddClaim(new Claim("username", username));

                identity.AddClaim(new Claim(ClaimTypes.Name, "demo"));
                await Task.Run(()=> context.Validated(identity));
            }
            else if (context.UserName == "user" && context.Password == "user")
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                identity.AddClaim(new Claim("username", "user"));
                identity.AddClaim(new Claim(ClaimTypes.Name, "demo data"));
                await Task.Run(() => context.Validated(identity));

            }
            else
            {
                context.SetError("invalid_grant", "Authentication Failed");
                return;
            }
        }
    }


    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {

            var guid = Guid.NewGuid().ToString();

            // copy all properties and set the desired lifetime of refresh token  
            var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
            {
                IssuedUtc = context.Ticket.Properties.IssuedUtc,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
            };

            var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);

            _refreshTokens.TryAdd(guid, refreshTokenTicket);

            // consider storing only the hash of the handle  
            await Task.Run(()=>context.SetToken(guid));
        }


        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            // context.DeserializeTicket(context.Token);
            AuthenticationTicket ticket;
            string header = context.OwinContext.Request.Headers["Authorization"];

            if (_refreshTokens.TryRemove(context.Token, out ticket))
            {
             await Task.Run(()=>context.SetTicket(ticket));
            }
        }

    }
}