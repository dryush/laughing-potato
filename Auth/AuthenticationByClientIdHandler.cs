

using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MailBank {
    public class AuthenticationByClientIdHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public AuthenticationByClientIdHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock
        ) 
            : base(options, logger, encoder, clock)
        {
        }
 
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return BadHeader;
 
            string authorizationHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader))
                return BadHeader;
            
            const string authType = "ClientId";
            if (!authorizationHeader.StartsWith(authType))
                return BadHeader;
            
            string clientIdStr = authorizationHeader.Substring(authType.Length).Trim();

            if (long.TryParse(clientIdStr, out var clientId)) {
                if (clientId % 2 == 1) 
                    return Success(clientId);
            }
            return BadClientId;
        }
 
        private AuthenticateResult BadHeader => AuthenticateResult.Fail("Bad Authorization Header");
        private AuthenticateResult BadClientId => AuthenticateResult.Fail("Bad Client Id");
        
        private AuthenticateResult Success(long clientId) {
            
            var claims = new [] { new Claim(ClaimTypes.NameIdentifier, clientId.ToString()) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new System.Security.Principal.GenericPrincipal(identity, null);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        } 
    }
}