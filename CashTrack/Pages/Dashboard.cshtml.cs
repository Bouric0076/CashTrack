// Pages/Dashboard.cshtml.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CashTrack.Data;
using CashTrack.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CashTrack.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal Balance { get; set; }

        public IList<Transaction> RecentTransactions { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            var userTransactions = await _context.Transactions
                .Where(t => t.UserId == user.Id)
                .ToListAsync();

            TotalIncome = userTransactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            TotalExpenses = userTransactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            Balance = TotalIncome - TotalExpenses;

            RecentTransactions = userTransactions
                .OrderByDescending(t => t.Date)
                .Take(5) // Display last 5 transactions
                .ToList();
        }
    }
}
