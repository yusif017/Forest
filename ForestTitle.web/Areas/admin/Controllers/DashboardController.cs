using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForesTitle.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin, Admin Editor, Editor,Moderator")]


    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
