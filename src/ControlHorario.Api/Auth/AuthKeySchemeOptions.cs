using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;

namespace ControlHorario.Api.Auth
{
    public class AuthKeySchemeOptions : AuthenticationSchemeOptions
    {
        public List<string> Keys { get; set; }
        public string RoleValue { get; set; }
    }
}
