using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolMS.Data;
using SchoolMS.Models;

namespace SchoolMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TeachersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeachersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var teachers = await _context.Teachers
                .Include(t => t.TeacherSubjects).ThenInclude(ts => ts.Subject)
                .ToListAsync();
            return View(teachers);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher model, string password)
        {
            if (ModelState.IsValid)
            {
                // Create login account for teacher
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    Role = "Teacher",
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user, password ?? "Teacher@123");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Teacher");
                    model.UserId = user.Id;
                    _context.Teachers.Add(model);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Teacher added with login credentials!";
                    return RedirectToAction(nameof(Index));
                }
                foreach (var err in result.Errors)
                    ModelState.AddModelError("", err.Description);
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Teacher model)
        {
            if (id != model.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Teacher updated!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.TeacherSubjects).ThenInclude(ts => ts.Subject).ThenInclude(s => s.Class)
                .Include(t => t.Attendances)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Teacher deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        // Assign Subject
        public async Task<IActionResult> AssignSubject(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();
            var assigned = await _context.TeacherSubjects.Where(ts => ts.TeacherId == id).Select(ts => ts.SubjectId).ToListAsync();
            ViewBag.Subjects = new MultiSelectList(await _context.Subjects.Include(s => s.Class).ToListAsync(), "Id", "Name", assigned);
            ViewBag.Teacher = teacher;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignSubject(int id, int[] subjectIds)
        {
            var existing = _context.TeacherSubjects.Where(ts => ts.TeacherId == id);
            _context.TeacherSubjects.RemoveRange(existing);
            foreach (var sid in subjectIds)
                _context.TeacherSubjects.Add(new TeacherSubject { TeacherId = id, SubjectId = sid });
            await _context.SaveChangesAsync();
            TempData["Success"] = "Subjects assigned!";
            return RedirectToAction(nameof(Index));
        }
    }
}