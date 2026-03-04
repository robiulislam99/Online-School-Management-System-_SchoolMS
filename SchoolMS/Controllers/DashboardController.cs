using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMS.Data;

namespace SchoolMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalStudents = await _context.Students.CountAsync();
            ViewBag.TotalTeachers = await _context.Teachers.CountAsync();
            ViewBag.TotalClasses = await _context.Classes.CountAsync();
            ViewBag.TotalSubjects = await _context.Subjects.CountAsync();
            ViewBag.TotalExpenses = await _context.Expenses.SumAsync(e => (decimal?)e.Amount) ?? 0;
            ViewBag.RecentStudents = await _context.Students
                .Include(s => s.Class)
                .OrderByDescending(s => s.AdmissionDate)
                .Take(5).ToListAsync();
            return View();
        }
    }
}