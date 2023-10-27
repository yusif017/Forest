
using ForesTitle.web.Data;
using ForesTitle.web.Helper;
using ForesTitle.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ForesTitle.Areas.admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin, Admin Editor, Moderator")]

    public class ArticleSabController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IWebHostEnvironment _env;
        public ArticleSabController(AppDbContext context, IHttpContextAccessor contextAccessor, IWebHostEnvironment env)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _env = env;
        }

        public IActionResult Index()
        {
            var articlesabs = _context.ArticleSabs
             .Include(x=>x.Category)
             .Include(x => x.User)
             .Include(x => x.ArticleSabTag)
             .ThenInclude(x => x.Tag)
             .OrderBy(x => x.CreateData)
             .ToList();
            return View(articlesabs);
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


        public async Task<IActionResult> Create(ArticleSab articlesab, List<int> tagIds, IFormFile Photo)
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                var tags = await _context.Tags.ToListAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
                ViewData["Tags"] = tags;

                var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                articlesab.UserId = userId;
                articlesab.CreateData = DateTime.Now;
                articlesab.SeoUrl = articlesab.Title.ReplaceInvalidChars();
                articlesab.PhotoUrl = await FileUploud.SeveFileAsync(Photo, _env.WebRootPath);
                await _context.ArticleSabs.AddAsync(articlesab);
                await _context.SaveChangesAsync();

                List<ArticleSabTag> articlesabTags = new();
                for (int i = 0; i < tagIds.Count; i++)
                {
                    ArticleSabTag articlesabTag = new()
                    {
                        ArticleSabId = articlesab.Id,
                        TagId = tagIds[i],

                    };
                    articlesabTags.Add(articlesabTag);
                }
                await _context.ArticleSabTags.AddRangeAsync(articlesabTags);
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
            var articlesab = _context.ArticleSabs.Include(x => x.ArticleSabTag).ThenInclude(x => x.Tag).FirstOrDefault(a => a.Id == id);
            if (articlesab == null) return NotFound();
            var categories = _context.Categories.ToList();
            var tags = _context.Tags.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
            ViewData["Tags"] = tags;
            return View(articlesab);

        }
        [HttpPost]

        public async Task<IActionResult> Edit(ArticleSab articlesab, List<int> tagIds, IFormFile Photo)
        {
            try
            {

                var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                articlesab.UserId = userId;
                var categories = _context.Categories.ToList();
                var tags = _context.Tags.ToList();
                ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
                ViewData["Tags"] = tags;
                articlesab.CreateData = DateTime.Now;
                articlesab.SeoUrl = articlesab.Title.ReplaceInvalidChars();
                if (Photo != null)
                {
                    articlesab.PhotoUrl = await FileUploud.SeveFileAsync(Photo, _env.WebRootPath);
               
                }
                var art = _context.ArticleSabTags.Where(x => x.ArticleSabId == articlesab.Id).ToList();
                _context.ArticleSabTags.RemoveRange(art);
                List<ArticleSabTag> articleSabTags = new();
                for (int i = 0; i < tagIds.Count; i++)
                {
                    ArticleSabTag articleSabTag = new()
                    {
                        ArticleSabId = articlesab.Id,
                        TagId = tagIds[i],

                    };
                    articleSabTags.Add(articleSabTag);

                }

                await _context.ArticleSabTags.AddRangeAsync(articleSabTags);
                _context.ArticleSabs.Update(articlesab);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(articlesab);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var articlesab = await _context.ArticleSabs.FirstOrDefaultAsync(x => x.Id == id);
            _context.ArticleSabs.Remove(articlesab);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }
    }
}
