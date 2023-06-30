using API.Contracts;
using API.DTOs.Account;
using API.DTOs.Education;
using API.DTOs.Employee;
using API.DTOs.Universities;
using API.Models;
using API.Repositories;
using API.Utilities.Enums;
using System.Net;
using System.Security.Claims;

namespace API.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly ITokenHandler _tokenHandler;

    public AccountService(IAccountRepository accountRepository,
             IEmployeeRepository employeeRepository,
             IUniversityRepository universityRepository,
             IEducationRepository educationRepository,
             ITokenHandler tokenHandler)
    {
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _universityRepository = universityRepository;
        _educationRepository = educationRepository;
        _tokenHandler = tokenHandler;
    }

    public IEnumerable<GetAccountDto>? GetAccount()
    {
        var accounts = _accountRepository.GetAll();
        if (!accounts.Any())
        {
            return null; // No Account  found
        }

        var toDto = accounts.Select(account =>
                                            new GetAccountDto
                                            {
                                                Guid = account.Guid,
                                                Password = account.Password,
                                                Otp = account.Otp,
                                                IsDeleted = account.IsDeleted,
                                                IsUsed = account.IsUsed,
                                            }).ToList();

        return toDto; // Account found
    }

    public GetAccountDto? GetAccount(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);
        if (account is null)
        {
            return null; // account not found
        }

        var toDto = new GetAccountDto
        {
            Guid = account.Guid,
            Password = account.Password,
            Otp = account.Otp,
            IsDeleted = account.IsDeleted,
            IsUsed = account.IsUsed,
        };

        return toDto; // accounts found
    }

    public GetAccountDto? CreateAccount(NewAccountDto newAccountDto)
    {
        var account = new Account
        {
            Guid = newAccountDto.Guid,
            Password = newAccountDto.Password,
            Otp = newAccountDto.Otp,
            IsDeleted = newAccountDto.IsDeleted,
            IsUsed = newAccountDto.IsUsed,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdAccount = _accountRepository.Create(account);
        if (createdAccount is null)
        {
            return null; // Account not created
        }

        var toDto = new GetAccountDto
        {
            Guid = createdAccount.Guid,
            Password = Hashing.HashPassword(createdAccount.Password),
            Otp = createdAccount.Otp,
            IsDeleted = createdAccount.IsDeleted,
            IsUsed = createdAccount.IsUsed,
        };

        return toDto; // Account created
    }

    public int UpdateAccount(UpdateAccountDto updateAccountDto)
    {
        var isExist = _accountRepository.IsExist(updateAccountDto.Guid);
        if (!isExist)
        {
            return -1; // Account not found
        }

        var getAccount = _accountRepository.GetByGuid(updateAccountDto.Guid);

        var account = new Account
        {
            Guid = updateAccountDto.Guid,
            Password = Hashing.HashPassword(updateAccountDto.Password),
            Otp = updateAccountDto.Otp,
            IsUsed = updateAccountDto.IsUsed,
            IsDeleted = updateAccountDto.IsDeleted,
            ModifiedDate = DateTime.Now,
            CreatedDate = getAccount!.CreatedDate
        };

        var isUpdate = _accountRepository.Update(account);
        if (!isUpdate)
        {
            return 0; // Account not updated
        }

        return 1;
    }

    public int DeleteAccount(Guid guid)
    {
        var isExist = _accountRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // Account not found
        }

        var account = _accountRepository.GetByGuid(guid);
        var isDelete = _accountRepository.Delete(account!);
        if (!isDelete)
        {
            return 0; // Account not deleted
        }

        return 1;
    }

    public RegisterAccount? Register(RegisterAccount registerDto)
    {
        EmployeeService employeeService = new EmployeeService(_employeeRepository);
        Employee employee = new Employee
        {
            Guid = new Guid(),
            Nik = employeeService.GenerateNik(),
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Birtdate = registerDto.BirthDate,
            Gender = registerDto.Gender,
            HiringDate = registerDto.HiringDate,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdEmployee = _employeeRepository.Create(employee);
        if (createdEmployee is null)
        {
            return null;
        }

        University university = new University
        {
            Guid = new Guid(),
            Code = registerDto.UniversityCode,
            Name = registerDto.UniversityName
        };

        var createdUniversity = _universityRepository.Create(university);
        if (createdUniversity is null)
        {
            return null;
        }

        Education education = new Education
        {
            Guid = employee.Guid,
            Major = registerDto.Major,
            Degree = registerDto.Degree,
            Gpa = registerDto.Gpa,
            UniversityGuid = university.Guid
        };

        var createdEducation = _educationRepository.Create(education);
        if (createdEducation is null)
        {
            return null;
        }

        Account account = new Account
        {
            Guid = employee.Guid,
            Password = Hashing.HashPassword(registerDto.Password),
        };

        if (registerDto.Password != registerDto.ConfirmPassword)
        {
            return null;
        }

        var createdAccount = _accountRepository.Create(account);
        if (createdAccount is null)
        {
            return null;
        }


        var toDto = new RegisterAccount
        {
            FirstName = createdEmployee.FirstName,
            LastName = createdEmployee.LastName,
            BirthDate = createdEmployee.BirthDate,
            Gender = createdEmployee.Gender,
            HiringDate = createdEmployee.HiringDate,
            Email = createdEmployee.Email,
            PhoneNumber = createdEmployee.PhoneNumber,
            Password = createdAccount.Password,
            Major = createdEducation.Major,
            Degree = createdEducation.Degree,
            Gpa = createdEducation.Gpa,
            UniversityCode = createdUniversity.Code,
            UniversityName = createdUniversity.Name
        };

        return toDto;
    }
    public string Login(LoginDto loginDto)
    {
        var EmailEmployee = _employeeRepository.GetEmailLogin(loginDto.Email);
        if (EmailEmployee == null)
        {
            return "0";
        }

        var password = _accountRepository.GetByGuid(EmailEmployee.Guid);
        var isValid = Hashing.ValidatePassword(loginDto.Password, password!.Password);
        if (!isValid)
        {
            return "-1";
        }
        /* var roleEmployee = ;
         var role = _roleRepository.GetByGuid;*/
        var claims = new List<Claim>() {
            new Claim("NIK", EmailEmployee.Nik),
            new Claim("FullName", $"{EmailEmployee.FirstName} {EmailEmployee.LastName}"),
            new Claim("Email", loginDto.Email)
        };

        try
        {
            var getToken = _tokenHandler.GenerateToken(claims);
            return getToken;
        }
        catch
        {
            return "-2";
        }
    }

}
