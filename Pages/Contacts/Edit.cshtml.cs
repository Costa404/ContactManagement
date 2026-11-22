using ContactManagement.Data;
using ContactManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Pages.Contacts;

// loads a contact for editing and saves the changes back to the database
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Contact Contact { get; set; } = default!;

    // loads the existing contact data into the form
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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // check if the phone number belongs to a different contact, not our own
        var contactExists = await _context.Contacts
            .IgnoreQueryFilters()
            .AnyAsync(c => c.ContactNumber == Contact.ContactNumber && c.Id != Contact.Id);

        if (contactExists)
        {
            ModelState.AddModelError("Contact.ContactNumber", "This contact number is already in use.");
        }

        // same uniqueness check for email, excluding the current contact
        var emailExists = await _context.Contacts
            .IgnoreQueryFilters()
            .AnyAsync(c => c.Email == Contact.Email && c.Id != Contact.Id);

        if (emailExists)
        {
            ModelState.AddModelError("Contact.Email", "This email address is already in use.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        // mark the whole entity as modified so EF knows to update all fields
        _context.Attach(Contact).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // maybe someone deleted this contact while we were editing it
            var exists = await _context.Contacts.AnyAsync(c => c.Id == Contact.Id);
            if (!exists)
            {
                return NotFound();
            }
            // if it still exists then something else went wrong, just throw
            throw;
        }
        catch (DbUpdateException)
        {
            // handles the edge case where a duplicate slipped in between our check and the save
            ModelState.AddModelError(string.Empty, "Unable to update contact due to a duplicate value.");
            return Page();
        }

        return RedirectToPage("./Index");
    }
}
