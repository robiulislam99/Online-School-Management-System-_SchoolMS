using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMS.Data;
using SchoolMS.Models;

namespace SchoolMS.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeacherDashboardController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ── Shared helper: get teacher with subjects fully loaded ──────────────
        private async Task<Teacher?> GetTeacherAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return null;

            return await _context.Teachers
                .Include(t => t.TeacherSubjects)
                    .ThenInclude(ts => ts.Subject)
                        .ThenInclude(s => s.Class)
                .FirstOrDefaultAsync(t => t.UserId == user.Id);
        }

        // ── Index / Dashboard ──────────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var teacher = await GetTeacherAsync();
            if (teacher == null) return RedirectToAction("Login", "Account");

            ViewBag.TodayAttendance = await _context.StudentAttendances
                .Where(a => a.TakenByTeacherId == teacher.Id
                         && a.AttendanceDate.Date == DateTime.Today)
                .CountAsync();

            return View(teacher);
        }

        // ── My Subjects ────────────────────────────────────────────────────────
        public async Task<IActionResult> MySubjects()
        {
            var teacher = await GetTeacherAsync();
            if (teacher == null) return RedirectToAction("Login", "Account");

            // TeacherSubjects already loaded by GetTeacherAsync()
            return View(teacher.TeacherSubjects.ToList());
        }

        // ── View Marks ─────────────────────────────────────────────────────────
        public async Task<IActionResult> ViewMarks()
        {
            var teacher = await GetTeacherAsync();
            if (teacher == null) return RedirectToAction("Login", "Account");

            // Get subject IDs this teacher is assigned to (not class IDs)
            // This is more precise: teacher sees marks only for their subjects
            var assignedSubjectIds = teacher.TeacherSubjects
                .Select(ts => ts.SubjectId)
                .ToList();

            if (!assignedSubjectIds.Any())
            {
                // No subjects assigned — return empty list with a message
                TempData["Info"] = "You have no assigned subjects yet.";
                return View(new List<StudentMark>());
            }

            var marks = await _context.StudentMarks
                .Include(m => m.Student).ThenInclude(s => s.Class)
                .Include(m => m.Subject)
                .Where(m => assignedSubjectIds.Contains(m.SubjectId))
                .OrderByDescending(m => m.ExamDate)
                .ToListAsync();

            return View(marks);
        }
    }
}