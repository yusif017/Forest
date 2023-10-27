
using ForesTitle.web.Data;
using ForesTitle.web.Helper;
using ForesTitle.web.Models;
using ForesTitle.web.Viewmodels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public ILogger<HomeController> Logger => _logger;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index(int pg = 1)
        {
            const int pageSize = 9;
            if (pg < 1)
            {
                pg = 1;
            }

         //   var sqwl = _context.Set<SearchDto>().FromSqlRaw($"FindSimilarTitlesWithIdSeoURL '{prefix}' ").ToList();

            int articleCount = _context.Articles.Count();

            var pager = new Pager(articleCount, pg, pageSize);

            int arcSkip = (pg - 1) * pageSize;

            var articles = _context.Articles.Include(x => x.User).Include(x => x.Category).OrderByDescending(x => x.CreateData).Skip(arcSkip).Take(pager.PageSize).ToList();
            var articlessab = _context.ArticleSabs.Include(x => x.User).Include(x => x.Category).OrderByDescending(x => x.CreateData).Skip(arcSkip).Take(pager.PageSize).ToList();

            var firsArticle = _context.Articles.Include(x => x.Category).Include(x => x.User).Include(x => x.ArticleTag).ThenInclude(x => x.Tag).OrderByDescending(x => x.CreateData).ToList();
           // var reklam = _context.Reklams.ToList();
           // var category = _context.Categories.ToList();
            //var tag = _context.Tags.ToList();



            ViewBag.Pager = pager;


            HomeVM homeVM = new()
            {
                Articles = articles,
                FirsSlot = firsArticle,
                ArticleSab = articlessab
            };
            return View(homeVM);
        }




        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}