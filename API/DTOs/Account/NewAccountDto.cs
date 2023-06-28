using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Universities;

public class NewAccountDto
{
    public Guid Guid { get; set; }
    [PasswordPolicy]
    public string Password { get; set; }
    [Required]
    public int Otp { get; set; }
    public Boolean IsDeleted { get; set; }
    public Boolean IsUsed { get; set; }
}
