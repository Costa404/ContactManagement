using ContactManagement.Data;
using ContactManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Pages.Contacts;

// handles adding a new contact to the database
public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    // bound to the form so the data comes in automatically on submit
    [BindProperty]
    public Contact Contact { get; set; } = new();

    // shows the empty form
    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // checking against all contacts including deleted ones because
        // we do not want duplicated phone numbers even for soft-deleted records
        var contactExists = await _context.Contacts
            .IgnoreQueryFilters()
            .AnyAsync(c => c.ContactNumber == Contact.ContactNumber);

        if (contactExists)
        {
            ModelState.AddModelError("Contact.ContactNumber", "This contact number is already in use.");
        }

        // same thing for email, needs to be unique everywhere
        var emailExists = await _context.Contacts
            .IgnoreQueryFilters()
            .AnyAsync(c => c.Email == Contact.Email);

        if (emailExists)
        {
            ModelState.AddModelError("Contact.Email", "This email address is already in use.");
        }

        // check again because we might have added duplicate errors above
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // making sure the contact starts as active
        Contact.IsDeleted = false;
        _context.Contacts.Add(Contact);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // in case someone else created a duplicate between our check and the actual save
            ModelState.AddModelError(string.Empty, "Unable to save contact due to a duplicate value.");
            return Page();
        }

        return RedirectToPage("./Index");
    }
}
