using ForesTitle.web.Data;
using ForesTitle.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForesTitle.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]

    //[Authorize(Roles = "Admin, Admin Editor, Moderator")]

    public class TagController : Controller
    {
        private readonly AppDbContext _context;
        public TagController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var tags = _context.Tags.ToList();
            return View(tags);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Tag tag)
        {
            var tagName = _context.Tags.FirstOrDefault(y => y.TagName == tag.TagName);
            if (tagName != null)
            {
                ModelState.AddModelError("Error", "404");
                return View();
            }
            if (ModelState.IsValid)
            {
                tag.CreateData = DateTime.Now;
                _context.Tags.Add(tag);
                _context.SaveChanges();
                return RedirectToAction("Index", "Tag");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult Detail(int? id)
        {
            if (id == null) return NotFound();
            Tag tag = _context.Tags.FirstOrDefault(x => x.Id == id);
            if (tag == null) return NotFound();
            return View(tag);
        }

        [HttpGet]
        public IActionResult UpData(int? id)
        {
            if (id == null) return NotFound();
            Tag tag = _context.Tags.FirstOrDefault();
            if (tag == null) return NotFound();
            return View(tag);
        }
        [HttpPost]
        public IActionResult UpData(Tag tag)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(tag);
                }
                tag.UpdatedDate = DateTime.Now;
                _context.Tags.Update(tag);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                throw;
            }
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            Tag tag = _context.Tags.FirstOrDefault(x => x.Id == id);
            if (tag == null) return NotFound();

            return View(tag);
        }
        [HttpPost]
        public IActionResult Delete(Tag tag)
        {
            _context.Tags.Remove(tag);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));

        }
    }
}
