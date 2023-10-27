using Microsoft.AspNetCore.Identity;

namespace ForesTitle.web.Models
{
    public class User: IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhotoUsl { get; set; }
        public string? AboutAuthor { get; set; }

    }
}
