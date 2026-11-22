using ContactManagement.Data;
using ContactManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Pages.Contacts;

// lists all contacts and also handles the delete action from the table
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Contact> Contacts { get; set; } = new List<Contact>();

    // loads all contacts sorted alphabetically, deleted ones are already filtered out
    public async Task OnGetAsync()
    {
        Contacts = await _context.Contacts
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    // does not actually remove the contact, just marks it as deleted
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);
        if (contact == null)
        {
            return NotFound();
        }

        contact.IsDeleted = true;
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }
}
