using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolMS.Data;
using SchoolMS.Models;

namespace SchoolMS.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AttendanceController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ===== TEACHER ATTENDANCE (Admin Only) =====
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TeacherAttendance(DateTime? date)
        {
            date ??= DateTime.Today;
            var teachers = await _context.Teachers.ToListAsync();
            var existing = await _context.TeacherAttendances
                .Where(a => a.AttendanceDate.Date == date.Value.Date)
                .ToListAsync();
            ViewBag.Date = date;
            ViewBag.ExistingAttendance = existing;
            return View(teachers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveTeacherAttendance(DateTime date, int[] teacherIds, int[] presentIds)
        {
            var existing = _context.TeacherAttendances.Where(a => a.AttendanceDate.Date == date.Date);
            _context.TeacherAttendances.RemoveRange(existing);
            foreach (var tid in teacherIds)
            {
                _context.TeacherAttendances.Add(new TeacherAttendance
                {
                    TeacherId = tid,
                    AttendanceDate = date,
                    IsPresent = presentIds.Contains(tid)
                });
            }
            await _context.SaveChangesAsync();
            TempData["Success"] = "Teacher attendance saved!";
            return RedirectToAction(nameof(TeacherAttendance), new { date });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewTeacherAttendance(int? teacherId, DateTime? from, DateTime? to)
        {
            from ??= DateTime.Today.AddDays(-30);
            to ??= DateTime.Today;
            var query = _context.TeacherAttendances.Include(a => a.Teacher).AsQueryable();
            if (teacherId.HasValue) query = query.Where(a => a.TeacherId == teacherId);
            query = query.Where(a => a.AttendanceDate >= from && a.AttendanceDate <= to);
            ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "Id", "FullName");
            ViewBag.From = from; ViewBag.To = to;
            return View(await query.ToListAsync());
        }

        // ===== STUDENT ATTENDANCE (Teacher & Admin) =====
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> StudentAttendance(int? classId, DateTime? date)
        {
            date ??= DateTime.Today;
            ViewBag.Classes = new SelectList(await _context.Classes.ToListAsync(), "Id", "Name");
            ViewBag.Date = date;
            if (!classId.HasValue) return View(new List<Student>());

            var students = await _context.Students.Where(s => s.ClassId == classId).ToListAsync();
            var existing = await _context.StudentAttendances
                .Where(a => a.AttendanceDate.Date == date.Value.Date && students.Select(s => s.Id).Contains(a.StudentId))
                .ToListAsync();
            ViewBag.ExistingAttendance = existing;
            ViewBag.ClassId = classId;
            return View(students);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveStudentAttendance(DateTime date, int classId, int[] studentIds, int[] presentIds)
        {
            var user = await _userManager.GetUserAsync(User);
            var teacher = User.IsInRole("Teacher")
                ? await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == user!.Id)
                : null;

            var existing = _context.StudentAttendances
                .Where(a => a.AttendanceDate.Date == date.Date && studentIds.Contains(a.StudentId));
            _context.StudentAttendances.RemoveRange(existing);

            foreach (var sid in studentIds)
            {
                _context.StudentAttendances.Add(new StudentAttendance
                {
                    StudentId = sid,
                    AttendanceDate = date,
                    IsPresent = presentIds.Contains(sid),
                    TakenByTeacherId = teacher?.Id
                });
            }
            await _context.SaveChangesAsync();
            TempData["Success"] = "Student attendance saved!";
            return RedirectToAction(nameof(StudentAttendance), new { classId, date });
        }

        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> ViewStudentAttendance(int? studentId, int? classId, DateTime? from, DateTime? to)
        {
            from ??= DateTime.Today.AddDays(-30);
            to ??= DateTime.Today;
            var query = _context.StudentAttendances
                .Include(a => a.Student).ThenInclude(s => s.Class)
                .AsQueryable();
            if (studentId.HasValue) query = query.Where(a => a.StudentId == studentId);
            if (classId.HasValue) query = query.Where(a => a.Student.ClassId == classId);
            query = query.Where(a => a.AttendanceDate >= from && a.AttendanceDate <= to);
            ViewBag.Classes = new SelectList(await _context.Classes.ToListAsync(), "Id", "Name");
            ViewBag.Students = new SelectList(await _context.Students.ToListAsync(), "Id", "FullName");
            ViewBag.From = from; ViewBag.To = to;
            return View(await query.OrderByDescending(a => a.AttendanceDate).ToListAsync());
        }
    }
}