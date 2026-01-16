using Microsoft.AspNetCore.Authentication;

namespace LibraryAPI.Handlers.Authentification
{
    public class AppHeaderAuthOptions : AuthenticationSchemeOptions
    {
        public string[] AllowedNames { get; set; }
    }
}
