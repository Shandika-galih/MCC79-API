﻿using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs.Room;

public class GetRoomDto
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public int Floor { get; set; }
    public int Capacity { get; set; }
}
