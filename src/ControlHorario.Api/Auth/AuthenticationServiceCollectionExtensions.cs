using ControlHorario.Api.Auth;
using Microsoft.AspNetCore.Authentication;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthenticationServiceCollectionExtensions
    {
        public static AuthenticationBuilder AddAuthKeyScheme(this IServiceCollection services,
            AuthSchemeOptions authSchemeOptions)
        {
            if (authSchemeOptions is null)
                throw new ArgumentNullException(nameof(authSchemeOptions));

            return
                services.AddAuthentication(authSchemeOptions.Scheme)
                        .AddScheme<AuthKeySchemeOptions, AuthKeySchemeHandler>(authSchemeOptions.Scheme, options => 
                        {
                            options.Keys = authSchemeOptions.Keys;
                            options.RoleValue = authSchemeOptions.RoleValue;
                        });
        }
    }
}
