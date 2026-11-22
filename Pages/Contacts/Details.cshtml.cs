using ContactManagement.Data;
using ContactManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Pages.Contacts;

// shows a single contact and also allows deleting it from here
public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Contact Contact { get; set; } = default!;

    // finds the contact by id, returns 404 if not found
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);
        if (contact == null)
        {
            return NotFound();
        }

        Contact = contact;
        return Page();
    }

    // soft deletes the contact and redirects back to the list
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);
        if (contact == null)
        {
            return NotFound();
        }

        contact.IsDeleted = true;
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
