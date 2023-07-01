using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Account;

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public int Otp { get; set; }

    public DateTime ExpireTime { get; set; }
}
