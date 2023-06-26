﻿using API.Utilities.Enums;

namespace API.DTOs.Employee;

public class NewEmployeeDto
{
    public string Nik { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime Birtdate { get; set; }
    public GenderEnum Gender { get; set; }
    public DateTime HiringDate { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
