
﻿using Expense_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;


namespace Expense_Tracker.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {

        private readonly ApplicatonDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(ApplicatonDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
                 

        public async Task<ActionResult> Index()
        {
            string? UserId = _userManager.GetUserId(HttpContext.User);

            //Last 7 Days
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            List<Transaction> SelectedTransactions = await _context.Transactions
                 .Include(x => x.Category)
                 .Where(y => y.Date >= StartDate && y.Date <= EndDate && y.UserId == UserId)
                 .ToListAsync();

            //Total Income
            int TotalIncome = SelectedTransactions
                .Where(i => i.Category.Type == "Income" )
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");

            //Total Expense
            int TotalExpense = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .Sum(j => j.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("C0");

            //Balance Amount
            int Balance = TotalIncome - TotalExpense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-in");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture,"{0:C0}",Balance);
            //ViewBag.Balance = Balance.ToString("C0");


            //Doughnut chart
            ViewBag.DoughnutChartData = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Category.CategoryId)
                .Select(k => new
                {
                    categoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
                    amount = k.Sum(j => j.Amount),
                    formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
                })
                .OrderByDescending(l => l.amount)
                .ToList();


            //Spline Chart - Income vs Expense
            //Income
            List<SplineChartData> IncomeSummery = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    income = k.Sum(l => l.Amount)
                }).ToList();

            //Expense
            List<SplineChartData> ExpenseSummery = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    expense = k.Sum(l => l.Amount)
                }).ToList();

            //Combine Income and Expense
            string[] last7Days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();
            ViewBag.SplineChartData = from day in last7Days
                                      join income in IncomeSummery on day equals income.day into dayIncomeJoined
                                      from income in dayIncomeJoined.DefaultIfEmpty()
                                      join expense in ExpenseSummery on day equals expense.day into expenseJoined
                                      from expense in expenseJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          expense = expense == null ? 0 : expense.expense
                                      };

            ViewBag.RecentTransactions = await _context.Transactions
                .Where(x => x.UserId == UserId)
                .Include(i => i.Category)
                .OrderByDescending(j => j.Date)
                .Take(5).
                ToListAsync();

            return View();
        }
    }

    public class SplineChartData
    {
        public string day;
        public int income;
        public int expense;
    }

}
