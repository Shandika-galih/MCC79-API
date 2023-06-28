using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Education;

public class UpdateEducationDto
{
    public Guid Guid { get; set; }
    public string Major { get; set; }
    [Required]
    public string Degree { get; set; }
    [Range(0, 4, ErrorMessage = "Gpa must be between 0 - 4")]
    public double Gpa { get; set; }
    [Required]
    public Guid UniversityGuid { get; set; }
}
