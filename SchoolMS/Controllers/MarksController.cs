using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolMS.Data;
using SchoolMS.Models;

namespace SchoolMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MarksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MarksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? classId)
        {
            var query = _context.StudentMarks
                .Include(m => m.Student).ThenInclude(s => s.Class)
                .Include(m => m.Subject)
                .AsQueryable();

            if (classId.HasValue)
                query = query.Where(m => m.Student.ClassId == classId);

            ViewBag.Classes = new SelectList(
                await _context.Classes.ToListAsync(), "Id", "Name");

            return View(await query.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            // Always provide non-null lists
            var students = await _context.Students
                .Include(s => s.Class)
                .ToListAsync();

            var subjects = await _context.Subjects
                .Include(s => s.Class)
                .ToListAsync();

            ViewBag.Students = new SelectList(
                students, "Id", "FullName");

            ViewBag.Subjects = new SelectList(
                subjects.Select(s => new
                {
                    s.Id,
                    Name = $"{s.Name} ({s.Class.Name})"
                }), "Id", "Name");

            return View(new StudentMark
            {
                ExamDate = DateTime.Now,
                TotalMarks = 100
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentMark model)
        {
            ModelState.Remove("Student");
            ModelState.Remove("Subject");

            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                _context.StudentMarks.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Marks added successfully!";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in ModelState.Values
                .SelectMany(v => v.Errors))
            {
                TempData["Error"] = error.ErrorMessage;
            }

            await LoadViewBags();
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var mark = await _context.StudentMarks.FindAsync(id);
            if (mark == null) return NotFound();

            await LoadViewBags(mark.StudentId, mark.SubjectId);
            return View(mark);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentMark model)
        {
            if (id != model.Id) return NotFound();

            ModelState.Remove("Student");
            ModelState.Remove("Subject");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Marks updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.StudentMarks.Any(m => m.Id == id))
                        return NotFound();
                    throw;
                }
            }

            await LoadViewBags(model.StudentId, model.SubjectId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var mark = await _context.StudentMarks.FindAsync(id);
            if (mark != null)
            {
                _context.StudentMarks.Remove(mark);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Marks deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        // Helper to load dropdowns safely
        private async Task LoadViewBags(
            int? selectedStudentId = null,
            int? selectedSubjectId = null)
        {
            var students = await _context.Students
                .Include(s => s.Class)
                .ToListAsync();

            var subjects = await _context.Subjects
                .Include(s => s.Class)
                .ToListAsync();

            ViewBag.Students = new SelectList(
                students, "Id", "FullName", selectedStudentId);

            ViewBag.Subjects = new SelectList(
                subjects.Select(s => new
                {
                    s.Id,
                    Name = $"{s.Name} ({s.Class.Name})"
                }), "Id", "Name", selectedSubjectId);
        }
    }
}