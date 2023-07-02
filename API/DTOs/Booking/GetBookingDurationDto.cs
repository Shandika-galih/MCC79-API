using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs.AccountRole;

public class GetBookingDurationDto
{
    public Guid? RoomGuid { get; set; }
    public string RoomName { get; set; }
    public string BookingLength { get; set; }

}
