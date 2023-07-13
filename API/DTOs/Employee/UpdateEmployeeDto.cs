using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Employee;

public class UpdateEmployeeDto
{
    [Required]
    public Guid Guid { get; set; }

    [Required]
    public string Nik { get; set; }

    [Required]
    public string FirstName { get; set; }

    public string? LastName { get; set; }
    [Required]
    public DateTime Birtdate { get; set; }
    [Required]
    public GenderEnum Gender { get; set; }
    [Required]
    public DateTime HiringDate { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
}
