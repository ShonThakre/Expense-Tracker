using Expense_Tracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using static Expense_Tracker.Controllers.ReportController;

namespace Expense_Tracker.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicatonDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReportController(ApplicatonDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        [HttpGet]
        public IActionResult Index()
        {
            var reportViewModel = new ReportViewModel();
            PopulateDdl(reportViewModel);



            return View(reportViewModel);

        }

        [HttpPost]
        public IActionResult Index(ReportViewModel reportViewModel)
        {
   

   
            if (ModelState.IsValid)
            {
                string? UserId = _userManager.GetUserId(HttpContext.User);
                PopulateDdl(reportViewModel);


                string SelectedType = reportViewModel.SelectedType;
                int SelectedCategory = reportViewModel.CategoryId;

                DateTime SelectedDate1;
                DateTime SelectedDate2;
                    
                DateTime.TryParseExact(reportViewModel.DateRange[0], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out SelectedDate1);
                DateTime.TryParseExact(reportViewModel.DateRange[1], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out SelectedDate2);

                //var ReportData = _context.Transactions.Where(x => x.UserId == UserId
                //&& x.Date >= SelectedDate1
                //&& x.Date <= SelectedDate1
                //);

                var ReportData = _context.Transactions
        .Where(x => x.UserId == UserId
            && x.Date >= SelectedDate1
            && x.Date <= SelectedDate2)
        .Select(x => new
        {
            x.Amount,
            x.Date,
            x.Category.Title, // Assuming Category is a navigation property in the Transaction class
            x.Category.Type
        })
        .ToList();

                StringBuilder htmlString = new StringBuilder();

                htmlString.Append("<table border='1'>");
                htmlString.Append("<tr><th>Sr Number</th><th>Date</th><th>Amount</th><th>Title</th><th>Type</th></tr>");

                int srNumber = 1;

                foreach (var transaction in ReportData)
                {
                    htmlString.Append("<tr>");
                    htmlString.Append("<td>").Append(srNumber++).Append("</td>");
                    htmlString.Append("<td>").Append(transaction.Date.ToShortDateString()).Append("</td>");
                    htmlString.Append("<td>").Append(transaction.Amount).Append("</td>");
                    htmlString.Append("<td>").Append(transaction.Title).Append("</td>"); // Assuming there's a property like CategoryName in your Transaction model
                    htmlString.Append("<td>").Append(transaction.Type).Append("</td>"); // Assuming there's a property like CategoryName in your Transaction model
                    htmlString.Append("</tr>");
                }

                htmlString.Append("</table>");

                // Now 'htmlString' contains the HTML representation of the table
                // reportViewModel.ReportString = htmlString.ToString();

                ViewBag.HtmlString = htmlString.ToString();

                return View(reportViewModel);
            }
            else
            {
                ModelState.AddModelError(nameof(reportViewModel.DateRange), "Please select a valid date range.");
            }
            return View(reportViewModel);
            
        }

        [NonAction]
        public void PopulateDdl(ReportViewModel reportViewModel)
        {
            reportViewModel.Types = new List<SelectListItem>
    {
        new SelectListItem { Value = "0", Text = "Select Type" },
        new SelectListItem { Value = "1", Text = "Income" },
        new SelectListItem { Value = "2", Text = "Expense" },
        new SelectListItem { Value = "3", Text = "Both" }
    };

            ViewBag.Types = reportViewModel.Types;

            // Populate Categories
            string? UserId = _userManager.GetUserId(HttpContext.User);
            reportViewModel.Categories = _context.Categories.Where(x => x.UserId == UserId).ToList();
            reportViewModel.Categories.Insert(0, new Category { CategoryId = 0, Title = "Choose a Category" });

            ViewBag.categories = reportViewModel.Categories;
        }

        public class ReportViewModel
        {
            public ReportViewModel()
            {
                // Set default value for SelectedType
                SelectedType = "0"; // Assuming "0" corresponds to "Select Type"
                CategoryId = 0; // Assuming 0 corresponds to the default category
                DateRange = new string[2] { DateTime.Today.ToString("dd/MM/yyyy").Replace('-','/'), DateTime.Today.ToString("dd/MM/yyyy").Replace('-', '/') };
            }

            [Range(1, int.MaxValue, ErrorMessage = "Please Select Type.")]
            public string SelectedType { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
            public int CategoryId { get; set; }

            [Required(ErrorMessage = "Please select a date range.")]
            [MinLength(1, ErrorMessage = "Please select a date range.")]
            public string[] DateRange { get; set; }

            [ValidateNever]
            public List<SelectListItem> Types { get; set; }

            [ValidateNever]
            public List<Category> Categories { get; set; }

            [ValidateNever]
            public string ReportString { get; set; }
        }



    }

}
