using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Room;

public class NewRoomDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    public int Floor { get; set; }
    [Required]
    public int Capacity { get; set; }
}
