using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMS.Data;
using SchoolMS.Models;

namespace SchoolMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ClassesController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> Index()
        {
            var classes = await _context.Classes
                .Include(c => c.Students)
                .Include(c => c.Subjects)
                .ToListAsync();
            return View(classes);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Class model)
        {
            if (ModelState.IsValid)
            {
                _context.Classes.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Class created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cls = await _context.Classes.FindAsync(id);
            if (cls == null) return NotFound();
            return View(cls);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Class model)
        {
            if (id != model.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Class updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var cls = await _context.Classes
                .Include(c => c.Subjects)
                .Include(c => c.Students)
                .Include(c => c.ClassFees)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (cls == null) return NotFound();
            return View(cls);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var cls = await _context.Classes.FindAsync(id);
            if (cls != null)
            {
                _context.Classes.Remove(cls);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Class deleted.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}