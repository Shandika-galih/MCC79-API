using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Room;

public class UpdateRoomDto
{
    public Guid Guid { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    public int Floor { get; set; }
    [Required]
    public int Capacity { get; set; }
}
