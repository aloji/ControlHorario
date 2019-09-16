using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ControlHorario.Api.Auth
{
    public class AuthKeySchemeHandler : AuthenticationHandler<AuthKeySchemeOptions>
    {
        public AuthKeySchemeHandler(IOptionsMonitor<AuthKeySchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!AuthenticationHeaderValue.TryParse(Request.Headers[HeaderNames.Authorization], out AuthenticationHeaderValue headerValue))
            {
                return Task.FromResult(AuthenticateResult.Fail($"Cannot read {HeaderNames.Authorization} header."));
            }

            if (!Scheme.Name.Equals(headerValue.Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail($"Not {Scheme.Name} authentication header."));
            }

            if (!Options.Keys.Any(x => x.Equals(headerValue.Parameter, StringComparison.InvariantCultureIgnoreCase)))
            {
                return Task.FromResult(AuthenticateResult.Fail($"Invalid {Scheme.Name} authentication header."));
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, Options.RoleValue)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
