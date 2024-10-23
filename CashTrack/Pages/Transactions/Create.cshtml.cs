using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashTrack.Data;
using CashTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace CashTrack.Pages.Transactions
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Transaction Transaction { get; set; } = new Transaction();

        public List<SelectListItem> TypeOptions { get; set; }

        // Populate type dropdown options
        public async Task OnGetAsync()
        {
            TypeOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Income", Text = "Income" },
                new SelectListItem { Value = "Expense", Text = "Expense" }
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TypeOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Income", Text = "Income" },
                    new SelectListItem { Value = "Expense", Text = "Expense" }
                };
                return Page();
            }

            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to identify user.");
                return Page();
            }

            // Assign the logged-in user's ID to the transaction
            Transaction.UserId = user.Id;

            try
            {
                // Add and save the transaction
                _context.Transactions.Add(Transaction);
                await _context.SaveChangesAsync();
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return Page();
            }
        }
    }
}
