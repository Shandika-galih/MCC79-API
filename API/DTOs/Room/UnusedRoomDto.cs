using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Room;

public class UnusedRoomDto
{
    [Required]
    public Guid RoomGuid { get; set; }
    [Required]
    public string RoomName { get; set; }
    [Required]
    public int Floor { get; set; }
    [Required]
    public int Capacity { get; set; }
}
