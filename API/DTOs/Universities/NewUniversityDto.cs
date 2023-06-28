using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Universities;

public class NewUniversityDto
{
    [Required]
    [MaxLength(100)]
    public string Code { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
}
