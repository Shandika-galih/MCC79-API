using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Universities;

public class UpdateUniversityDto
{
    public Guid Guid { get; set; }

    [Required]
    [MaxLength(100)]
    public string Code { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
}
