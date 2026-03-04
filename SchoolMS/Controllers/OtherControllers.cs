using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolMS.Data;
using SchoolMS.Models;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SchoolMS.Controllers
{
    // =============================================
    // EXPENSES CONTROLLER
    // =============================================
    [Authorize(Roles = "Admin")]
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var expenses = await _context.Expenses
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();

            ViewBag.TotalAmount = expenses.Sum(e => e.Amount);
            return View(expenses);
        }

        public IActionResult Create()
        {
            return View(new Expense
            {
                ExpenseDate = DateTime.Now
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expense model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                _context.Expenses.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Expense added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) return NotFound();
            return View(expense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Expense model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Expense updated!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Expense deleted.";
            }
            return RedirectToAction(nameof(Index));
        }
    }

    // =============================================
    // CLASS FEES CONTROLLER
    // =============================================
    [Authorize(Roles = "Admin")]
    public class ClassFeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassFeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var fees = await _context.ClassFees
                .Include(f => f.Class)
                .ToListAsync();
            return View(fees);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Classes = new SelectList(
                await _context.Classes.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassFee model)
        {
            ModelState.Remove("Class");

            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                _context.ClassFees.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Class fee added successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Classes = new SelectList(
                await _context.Classes.ToListAsync(), "Id", "Name");
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var fee = await _context.ClassFees.FindAsync(id);
            if (fee == null) return NotFound();

            ViewBag.Classes = new SelectList(
                await _context.Classes.ToListAsync(),
                "Id", "Name", fee.ClassId);
            return View(fee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClassFee model)
        {
            if (id != model.Id) return NotFound();

            ModelState.Remove("Class");

            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Fee updated!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Classes = new SelectList(
                await _context.Classes.ToListAsync(),
                "Id", "Name", model.ClassId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var fee = await _context.ClassFees.FindAsync(id);
            if (fee != null)
            {
                _context.ClassFees.Remove(fee);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Fee deleted.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
