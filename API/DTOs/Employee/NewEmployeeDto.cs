using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Employee;

public class NewEmployeeDto
{
    public string Nik { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
    [Required]
    public DateTime Birthdate { get; set; }
    [Required]
    [Range(0,1, ErrorMessage ="must 0 or 1")]
    public GenderEnum Gender { get; set; }
    [Required]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}")]
    public DateTime HiringDate { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [DataType(DataType.PhoneNumber)]
    /*[RegularExpression(@"^\(?([0-9]{2})[-. ]?([0-9]{4})[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "Not a valid Phone number")]*/
    public string PhoneNumber { get; set; }
}
