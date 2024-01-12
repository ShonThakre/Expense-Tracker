using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Identity;

namespace Expense_Tracker.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicatonDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TransactionController(ApplicatonDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            string? UserId = _userManager.GetUserId(HttpContext.User);
            var applicatonDbContext = _context.Transactions.Where(x=> x.UserId == UserId).Include(t => t.Category);
            return View(await applicatonDbContext.ToListAsync());
        }



        // GET: Transaction/AddorEdit
        public IActionResult AddorEdit(int id=0)
        {
            PopulateCategories();
            if (id== 0)
            {
                return View(new Transaction());
            }
            else
            {
                return View(_context.Transactions.Find(id));
            }
   

        }

        // POST: Transaction/AddorEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddorEdit([Bind("TransactionId,CategoryId,Amount,Note,Date")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.UserId = _userManager.GetUserId(HttpContext.User);

                if (transaction.TransactionId == 0)
                {
                    _context.Add(transaction);
                }
                else
                {
                    _context.Update(transaction);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateCategories();
            return View(transaction);
        }


        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        public void PopulateCategories()
        {
            string? UserId = _userManager.GetUserId(HttpContext.User);
            var CategoryCollection = _context.Categories.Where(x => x.UserId == UserId).ToList();
            Category DefaultCategory = new Category()
            {
                CategoryId = 0,
                Title = "Choose a Category"
            };
            CategoryCollection.Insert(0, DefaultCategory);
            ViewBag.categories = CategoryCollection;
        }

    }
}
