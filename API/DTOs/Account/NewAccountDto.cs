namespace API.DTOs.Universities;

public class NewAccountDto
{
    public string Password { get; set; }
    public int Otp { get; set; }
    public Boolean IsDeleted { get; set; }
    public Boolean IsUsed { get; set; }
}
