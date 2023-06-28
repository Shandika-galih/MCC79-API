using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Employee;

public class UpdateEmployeeDto
{
    public Guid Guid { get; set; }
    [Required]
    public string Nik { get; set; }
    [Required]
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    [Required]
    public DateTime Birthdate { get; set; }
    [Required]
    public GenderEnum Gender { get; set; }
    public DateTime HiringDate { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^\(?([0-9]{2})[-. ]?([0-9]{4})[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "Not a valid Phone number")]
    public string PhoneNumber { get; set; }
}
