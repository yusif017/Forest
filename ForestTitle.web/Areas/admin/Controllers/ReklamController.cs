
using ForesTitle.web.Data;
using ForesTitle.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ForesTitle.Areas.admin.Controllers
{
    [Area(nameof(Admin))]
    public class ReklamController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ReklamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var reklams = _context.Reklams.ToList();
            return View(reklams);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Reklam reklam)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(reklam);
                }
                reklam.CreateData = DateTime.Now;
                _context.Reklams.Add(reklam);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(reklam);
            }
        }

        [HttpGet]
        public IActionResult Detail(int? id)
        {
            if (id == null) return NotFound();
            Reklam reklam = _context.Reklams.FirstOrDefault(x => x.Id == id);
            if (reklam == null) return NotFound();
            return View(reklam);
        }

        [HttpGet]
        public IActionResult UpData(int? id)
        {
            if (id == null) return NotFound();
            Reklam reklam = _context.Reklams.FirstOrDefault();
            if (reklam == null) return NotFound();
            return View(reklam);
        }
        [HttpPost]
        public IActionResult UpData(Reklam reklam)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(reklam);
                }
                reklam.UpdatedDate = DateTime.Now;
                _context.Reklams.Update(reklam);
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
            Reklam reklam = _context.Reklams.FirstOrDefault(x => x.Id == id);
            if (reklam == null) return NotFound();

            return View(reklam);
        }
        [HttpPost]
        public IActionResult Delete(Reklam reklam)
        {
            _context.Reklams.Remove(reklam);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));

        }

    }

  
    }
