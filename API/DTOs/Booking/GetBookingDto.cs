using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs.AccountRole;

public class GetBookingDto
{
    public Guid Guid { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public StatusLevel Status { get; set; }
    public string Remarks { get; set; }
    public Guid RoomGuid { get; set; }
    public Guid EmployeeGuid { get; set; }

}
