using System.Collections.Generic;

namespace ControlHorario.Api.Auth
{
    public class AuthSchemeOptions
    {
        public string Scheme { get; set; }
        public string RoleValue { get; set; }
        public List<string> Keys { get; set; }
    }
}
