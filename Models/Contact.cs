using System.ComponentModel.DataAnnotations;

namespace ContactManagement.Models;

// represents a contact in the system, maps straight to the db table
public class Contact
{
    public int Id { get; set; }

    // name is required and must be at least 6 characters long
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(200, MinimumLength = 6, ErrorMessage = "Name must be longer than 5 characters.")]
    public string Name { get; set; } = string.Empty;

    // must be exactly 9 digits, validated with regex
    [Required(ErrorMessage = "Contact is required.")]
    [RegularExpression(@"^\d{9}$", ErrorMessage = "Contact must be exactly 9 digits.")]
    public string ContactNumber { get; set; } = string.Empty;

    // just a standard email check here
   [Required(ErrorMessage = "Email is required.")]
   [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
   [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]
   [StringLength(200)]
   public string Email { get; set; } = string.Empty;

    // we use soft delete so contacts are never really removed from the database
    public bool IsDeleted { get; set; }
}
