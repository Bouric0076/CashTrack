using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CashTrack.Data;
using CashTrack.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CashTrack.Pages.Transactions
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Transaction> Transactions { get; set; } = new List<Transaction>();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null) // Check if user is authenticated
            {
                Transactions = await _context.Transactions
                    .Where(t => t.UserId == user.Id)
                    .OrderByDescending(t => t.Date)
                    .AsNoTracking() // Ensure fresh data from the database
                    .ToListAsync();
            }
        }
    }
}
