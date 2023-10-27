

using ForesTitle.web.Models;

namespace ForesTitle.Areas.Admin.ViewModels
{
    public class UserRoleVM
    {
		public User User { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
