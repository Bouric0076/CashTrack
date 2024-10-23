using Microsoft.AspNetCore.Authorization;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using CashTrack.Data;  
using CashTrack.Models;  
using Microsoft.AspNetCore.Identity;  
using Microsoft.AspNetCore.Mvc.Rendering;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.EntityFrameworkCore;  

namespace CashTrack.Pages.Transactions  
{  
    [Authorize]  
    public class EditModel : PageModel  
    {  
        private readonly ApplicationDbContext _context;  
        private readonly UserManager<IdentityUser> _userManager;  

        public EditModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)  
        {  
            _context = context;  
            _userManager = userManager;  
        }  

        [BindProperty]  
        public Transaction Transaction { get; set; } = new Transaction();  

        public List<SelectListItem> TypeOptions { get; set; }  
        public List<SelectListItem> UserOptions { get; set; }  

        public async Task<IActionResult> OnGetAsync(int? id)  
        {  
            if (id == null)  
            {  
                return NotFound();  
            }  

            var user = await _userManager.GetUserAsync(User);  
            Transaction = await _context.Transactions.FirstOrDefaultAsync(m => m.Id == id && m.UserId == user.Id);  

            if (Transaction == null)  
            {  
                return NotFound();  
            }  

            // Populate dropdowns  
            await PopulateDropdownsAsync();  

            return Page();  
        }  

        public async Task<IActionResult> OnPostAsync()  
        {  
            if (!ModelState.IsValid)  
            {  
                // Repopulate dropdowns if model state is invalid  
                await PopulateDropdownsAsync();  
                return Page();  
            }  

            var user = await _userManager.GetUserAsync(User);  
            var transactionToUpdate = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == Transaction.Id && t.UserId == user.Id);  

            if (transactionToUpdate == null)  
            {  
                return NotFound();  
            }  

            transactionToUpdate.Description = Transaction.Description;  
            transactionToUpdate.Amount = Transaction.Amount;  
            transactionToUpdate.Type = Transaction.Type;  
            transactionToUpdate.Date = Transaction.Date;  
            transactionToUpdate.PaymentMethod = Transaction.PaymentMethod;  

            try  
            {  
                await _context.SaveChangesAsync();  
            }  
            catch (DbUpdateConcurrencyException)  
            {  
                if (!TransactionExists(Transaction.Id))  
                {  
                    return NotFound();  
                }  
                else  
                {  
                    throw;  
                }  
            }  

            return RedirectToPage("./Index");  
        }  

        private async Task PopulateDropdownsAsync()  
        {  
            // Populate TypeOptions with transaction types  
            TypeOptions = new List<SelectListItem>  
            {  
                new SelectListItem { Value = "Income", Text = "Income" },  
                new SelectListItem { Value = "Expense", Text = "Expense" }  
            };  

            // Populate UserOptions with the current users  
            var users = await _userManager.Users.ToListAsync();  
            UserOptions = users.Select(u => new SelectListItem  
            {  
                Value = u.Id,  
                Text = u.UserName  
            }).ToList();  
        }  

        private bool TransactionExists(int id)  
        {  
            return _context.Transactions.Any(e => e.Id == id);  
        }  
    }  
}