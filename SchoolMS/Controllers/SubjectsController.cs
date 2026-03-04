using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolMS.Data;
using SchoolMS.Models;

namespace SchoolMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SubjectsController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> Index()
        {
            var subjects = await _context.Subjects.Include(s => s.Class).ToListAsync();
            return View(subjects);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Classes = new SelectList(await _context.Classes.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subject model)
        {
            if (ModelState.IsValid)
            {
                _context.Subjects.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Subject created!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Classes = new SelectList(await _context.Classes.ToListAsync(), "Id", "Name");
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return NotFound();
            ViewBag.Classes = new SelectList(await _context.Classes.ToListAsync(), "Id", "Name", subject.ClassId);
            return View(subject);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Subject model)
        {
            if (id != model.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Subject updated!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Classes = new SelectList(await _context.Classes.ToListAsync(), "Id", "Name", model.ClassId);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null) { _context.Subjects.Remove(subject); await _context.SaveChangesAsync(); TempData["Success"] = "Subject deleted."; }
            return RedirectToAction(nameof(Index));
        }
    }
}