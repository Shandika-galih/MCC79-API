﻿using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Account;

public class RegisterAccount
{
    [Required]
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    [Required]
    public DateTime Birtdate { get; set; }
    [Required]
    [Range(0, 1)]
    public GenderEnum Gender { get; set; }
    [Required]
    public DateTime HiringDate { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
    [Required]
    public string Major { get; set; }
    [Required]
    public string Degree { get; set; }
    [Required]
    [Range(0, 4)]
    public double Gpa { get; set; }
    [Required]
    public string UniversityCode { get; set; }
    [Required]
    public string UniversityName { get; set; }
    [Required]
    [PasswordPolicy]
    public string Password { get; set; }
    [Required]
    [PasswordPolicy]
    public string ConfirmPassword { get; set; }
}
