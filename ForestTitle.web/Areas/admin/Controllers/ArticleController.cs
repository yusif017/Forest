using ForesTitle.web.Data;
using ForesTitle.web.Helper;
using ForesTitle.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;


namespace ForesTitle.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin, Admin Editor, Moderator")]

    public class ArticleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IWebHostEnvironment _env;
        private readonly string uploadsPath;
        public ArticleController(AppDbContext context, IHttpContextAccessor contextAccessor, IWebHostEnvironment env)
        {
            
            _context = context;
            _contextAccessor = contextAccessor;
            _env = env;
            uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
        }

        public IActionResult Index()
        {
            var articles = _context.Articles
            .Include(x => x.Category)
            .Include(x => x.User)
             .Include(x => x.ArticleTag)
             .ThenInclude(x => x.Tag)

      
            .ToList();
            return View(articles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            var tags = _context.Tags.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName"); 
            ViewData["Tags"] = tags;
            return View();
        }
        [HttpPost]


        public async Task<IActionResult> Create(Article article, List<int> tagIds, IFormFile Photo)
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                var tags = await _context.Tags.ToListAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
                ViewData["Tags"] = tags;

                var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                article.UserId = userId;
                article.CreateData = DateTime.Now;
                article.SeoUrl = article.Title.ReplaceInvalidChars();
                article.PhotoUrl = await FileUploud.SeveFileAsync(Photo, _env.WebRootPath);
                await _context.Articles.AddAsync(article);
                await _context.SaveChangesAsync();

                List<ArticleTag> articleTags = new();
                for (int i = 0; i < tagIds.Count; i++)
                {
                    ArticleTag articleTag = new()
                    {
                        ArticleId = article.Id,
                        TagId = tagIds[i],

                    };
                    articleTags.Add(articleTag);
                }
                await _context.ArticleTags.AddRangeAsync(articleTags);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == null) return NotFound();
            var article = _context.Articles.Include(x => x.ArticleTag).ThenInclude(x => x.Tag).FirstOrDefault(a => a.Id == id);
            if (article == null) return NotFound();
            var categories = _context.Categories.ToList();
            var tags = _context.Tags.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
            ViewData["Tags"] = tags;
            return View(article);

        }
        [HttpPost]

        public async Task<IActionResult> Edit(Article article, List<int> tagIds, IFormFile Photo)
        {
            try
            {
                
                var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                article.UserId = userId;
                var categories = _context.Categories.ToList();
                var tags = _context.Tags.ToList();
                ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
                ViewData["Tags"] = tags;
                article.CreateData = DateTime.Now;
                article.SeoUrl = article.Title.ReplaceInvalidChars();
                if (Photo != null)
                {
                    article.PhotoUrl = await FileUploud.SeveFileAsync(Photo, _env.WebRootPath);
             
                }
               
                var art = _context.ArticleTags.Where(x => x.ArticleId == article.Id).ToList();
                _context.ArticleTags.RemoveRange(art);
                List<ArticleTag> articleTags = new();
                for (int i = 0; i < tagIds.Count; i++)
                {
                    ArticleTag articleTag = new()
                    {
                        ArticleId = article.Id,
                        TagId = tagIds[i],

                    };
                    articleTags.Add(articleTag);

                }
                
                await _context.ArticleTags.AddRangeAsync(articleTags);
                _context.Articles.Update(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(article);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(x => x.Id == id);
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }
    }
}
