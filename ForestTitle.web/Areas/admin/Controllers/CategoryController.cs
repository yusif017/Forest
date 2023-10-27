using Azure;
using ForesTitle.web.Data;
using ForesTitle.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace ForesTitle.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
  //  [Authorize(Roles = "Admin, Admin Editor, Moderator")]

    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(category);
                }
                category.CreateData = DateTime.Now;
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(category);
            }
        }

        [HttpGet]
        public IActionResult Detail(int? id)
        {
            if (id == null) return NotFound();
            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpGet]
        public IActionResult UpData(int? id)
        {
            if (id == null) return NotFound();
            Category category = _context.Categories.FirstOrDefault();
            if (category == null) return NotFound();
            return View(category);
        }
        [HttpPost]
        public IActionResult UpData(Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(category);
                }
                category.UpdatedDate = DateTime.Now;
                _context.Categories.Update(category);
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
            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null) return NotFound();

            return View(category);
        }
        [HttpPost]
        public IActionResult Delete(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));

        }
    }


}
    
