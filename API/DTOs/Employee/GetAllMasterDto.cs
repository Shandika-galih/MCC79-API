﻿using API.Utilities.Enums;

namespace API.DTOs.Employee;

public class GetAllMasterDto
{
    public Guid Guid { get; set; }
    public string Nik { get; set; }
    public string FullName { get; set; }
    public DateTime Birtdate { get; set; }
    public GenderEnum Gender { get; set; }
    public DateTime HiringDate { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Major { get; set; }
    public string Degree { get; set; }
    public double Gpa { get; set; }
    public string UniversityName { get; set; }
    public string Role { get; set; }
}
