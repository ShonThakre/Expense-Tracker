using Expense_Tracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
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
            PopulateDdl(reportViewModel);

            string SelectedType = reportViewModel.SelectedType;

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
            }

            [Range(1, int.MaxValue, ErrorMessage = "Please Select Type.")]
            public string SelectedType { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
            public int CategoryId { get; set; }

            [Required]
            public string DateRange { get; set; } 

            public List<SelectListItem> Types { get; set; }
            public List<Category> Categories { get; set; }
        }

    }

}
