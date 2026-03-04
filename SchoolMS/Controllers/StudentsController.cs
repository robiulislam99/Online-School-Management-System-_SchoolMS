using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolMS.Data;
using SchoolMS.Models;

namespace SchoolMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentsController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> Index(int? classId, string? search)
        {
            var query = _context.Students.Include(s => s.Class).AsQueryable();
            if (classId.HasValue) query = query.Where(s => s.ClassId == classId);
            if (!string.IsNullOrEmpty(search)) query = query.Where(s => s.FullName.Contains(search) || s.RollNumber!.Contains(search));
            ViewBag.Classes = new SelectList(await _context.Classes.ToListAsync(), "Id", "Name");
            return View(await query.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Classes = new SelectList(await _context.Classes.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student model)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Student added!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Classes = new SelectList(await _context.Classes.ToListAsync(), "Id", "Name");
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();
            ViewBag.Classes = new SelectList(await _context.Classes.ToListAsync(), "Id", "Name", student.ClassId);
            return View(student);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student model)
        {
            if (id != model.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Student updated!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Classes = new SelectList(await _context.Classes.ToListAsync(), "Id", "Name", model.ClassId);
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var student = await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Marks).ThenInclude(m => m.Subject)
                .Include(s => s.Attendances)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null) { _context.Students.Remove(student); await _context.SaveChangesAsync(); TempData["Success"] = "Student deleted."; }
            return RedirectToAction(nameof(Index));
        }
    }
}