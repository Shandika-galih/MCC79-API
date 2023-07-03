using API.Contracts;
using API.Data;
using API.DTOs.Account;
using API.DTOs.Education;
using API.DTOs.Employee;
using API.DTOs.Universities;
using API.Models;
using API.Repositories;
using API.Utilities.Enums;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace API.Services;

public class AccountService
{
    private readonly BookingDbContext _context;
    private readonly IAccountRepository _accountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly ITokenHandler _tokenHandler;
    private readonly IRoleRepository _roleRepository;
    private readonly IAccountRoleRepository _accountRoleRepository;

    public AccountService(IAccountRepository accountRepository,
             IEmployeeRepository employeeRepository,
             IUniversityRepository universityRepository,
             IEducationRepository educationRepository,
             ITokenHandler tokenHandler,
             IRoleRepository roleRepository,
             IAccountRoleRepository accountRoleRepository,
             BookingDbContext context)
    {
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _universityRepository = universityRepository;
        _educationRepository = educationRepository;
        _tokenHandler = tokenHandler;
        _roleRepository = roleRepository;
        _accountRoleRepository = accountRoleRepository;
        _context = context;
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
                                                ExpiredTime = account.ExpiredDate,
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
            Otp = GenerateOtp(),
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

    /*public RegisterAccount? Register(RegisterAccount registerDto)
    {
        // Step 1: Create the employee

        // Create an instance of EmployeeService
        EmployeeService employeeService = new EmployeeService(_employeeRepository);

        // Generate NIK for the employee
        string nik = employeeService.GenerateNik();

        // Create the Employee object
        Employee employee = new Employee
        {
            Guid = Guid.NewGuid(),
            Nik = nik,
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

        // Create the employee in the repository
        var createdEmployee = _employeeRepository.Create(employee);

        if (createdEmployee is null)
        {
            return null;
        }

        // Step 2: Create the university

        // Create the University object
        University university = new University
        {
            Guid = Guid.NewGuid(),
            Code = registerDto.UniversityCode,
            Name = registerDto.UniversityName
        };

        // Create the university in the repository
        var createdUniversity = _universityRepository.Create(university);

        if (createdUniversity is null)
        {
            return null;
        }

        // Step 3: Create the education

        // Create the Education object
        Education education = new Education
        {
            Guid = employee.Guid,
            Major = registerDto.Major,
            Degree = registerDto.Degree,
            Gpa = registerDto.Gpa,
            UniversityGuid = university.Guid
        };

        // Create the education in the repository
        var createdEducation = _educationRepository.Create(education);

        if (createdEducation is null)
        {
            return null;
        }

        // Step 4: Create the account

        // Hash the password
        string hashedPassword = Hashing.HashPassword(registerDto.Password);

        // Create the Account object
        Account account = new Account
        {
            Guid = employee.Guid,
            Password = hashedPassword
        };

        // Create the account in the repository
        var createdAccount = _accountRepository.Create(account);

        if (createdAccount is null)
        {
            return null;
        }

        // Step 5: Create account roles

        // Get the roles for 'User' and 'Admin'
        Role userRole = _roleRepository.GetByName("User");
        Role adminRole = _roleRepository.GetByName("Admin");

        // Create account role for 'User'
        AccountRole userAccountRole = new AccountRole
        {
            AccountGuid = createdAccount.Guid,
            RoleGuid = userRole.Guid
        };
        _accountRoleRepository.Create(userAccountRole);

        // Create account role for 'Admin'
        AccountRole adminAccountRole = new AccountRole
        {
            AccountGuid = createdAccount.Guid,
            RoleGuid = adminRole.Guid
        };
        _accountRoleRepository.Create(adminAccountRole);

        // Step 6: Generate JWT payload with roles

        // Create a list of claims
        var claims = new List<Claim>
    {
        new Claim("NIK", createdEmployee.Nik),
        new Claim("FullName", $"{createdEmployee.FirstName} {createdEmployee.LastName}"),
        new Claim("Email", registerDto.Email),
        new Claim("Roles", userRole.Name), // Add 'User' role to the claims
        new Claim("Roles", adminRole.Name) // Add 'Admin' role to the claims
    };

        // Generate the JWT token
        var getToken = _tokenHandler.GenerateToken(claims);

        // Step 7: Return the created account data

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
    }*/

    /*public string Login(LoginDto loginDto)
    {
        var emailEmployee = _employeeRepository.GetByEmail(loginDto.Email);
        if (emailEmployee == null)
        {
            return "0"; // Account not found
        }

        var password = _accountRepository.GetByGuid(emailEmployee.Guid);
        var isValid = Hashing.ValidatePassword(loginDto.Password, password!.Password);
        if (!isValid)
        {
            return "-1"; // Invalid password
        }

        var roleEmployee = _roleRepository.GetByName("User"); // Mengambil peran 'User' dari repository
        var roleAdmin = _roleRepository.GetByName("Admin"); // Mengambil peran 'Admin' dari repository

        if (roleEmployee == null || roleAdmin == null)
        {
            return "-2"; // Peran tidak ditemukan
        }

        var claims = new List<Claim>
    {
        new Claim("NIK", emailEmployee.Nik),
        new Claim("FullName", $"{emailEmployee.FirstName} {emailEmployee.LastName}"),
        new Claim("Email", loginDto.Email),
        new Claim(ClaimTypes.Role, roleEmployee.Name), // Menambahkan peran 'User' ke dalam claims
        new Claim(ClaimTypes.Role, roleAdmin.Name), // Menambahkan peran 'Admin' ke dalam claims
    };

        try
        {
            var token = _tokenHandler.GenerateToken(claims);
            return token;
        }
        catch
        {
            return "-3"; // Error generating token
        }
    }*/
    public string Login(LoginDto loginDto)
    {
        var employee = _employeeRepository.GetByEmail(loginDto.Email);
        if (employee is null)
        {
            return "0";
        }

        var account = _accountRepository.GetByGuid(employee.Guid);
        if (account is null)
        {
            return "0";
        }

        if (!Hashing.ValidatePassword(loginDto.Password, account.Password))
        {
            return "-1";
        }

        var accountRole = _accountRoleRepository.GetByGuidEmployee(account.Guid);
        var getRoleNameByAccountRole = from ar in accountRole
                                       join r in _roleRepository.GetAll() on ar.RoleGuid equals r.Guid
                                       select r.Name;


        var claims = new List<Claim>() {
                new Claim("NIK", employee.Nik),
                new Claim("FullName", $"{employee.FirstName} {employee.LastName}"),
                new Claim("Email", loginDto.Email)
            };

        claims.AddRange(getRoleNameByAccountRole.Select(role => new Claim(ClaimTypes.Role, role)));

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


    public string GetRoleFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken == null)
        {
            // Token tidak valid
            return null;
        }

        var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        if (roleClaim == null)
        {
            // Tidak ada klaim peran dalam token
            return null;
        }

        var role = roleClaim.Value;
        return role;
    }
    public int GenerateOtp()
    {

        Random random = new Random();
        int otp = random.Next(100000, 999999);
        return otp;

    }
    public ForgotPasswordDto ForgotPassword(string email)
    {
        var employee = _employeeRepository.GetAll().SingleOrDefault(employee => employee.Email == email);
        if (employee is null)
        {
            return null;
        }

        var toDto = new ForgotPasswordDto
        {
            Email = employee.Email,
            Otp = GenerateOtp(),
            ExpireTime = DateTime.Now.AddMinutes(5)
        };

        var relatedAccount = _accountRepository.GetByGuid(employee.Guid);

        var updateAccountDto = new Account
        {
            Guid = relatedAccount.Guid,
            Password = relatedAccount.Password,
            IsDeleted = (bool)relatedAccount.IsDeleted,
            Otp = toDto.Otp,
            IsUsed = false,
            ExpiredDate = DateTime.Now.AddMinutes(5)
        };

        var updateResult = _accountRepository.Update(updateAccountDto);

        return toDto;
    }

/// <summary>
/// benar
/// </summary>
/// <param name="loginDto"></param>
/// <returns></returns>
/*  public string Login(LoginDto loginDto)
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
      *//* var roleEmployee = ;
       var role = _roleRepository.GetByGuid;*//*
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
  }*/
public IEnumerable<GetAllMasterDto>? GetMaster()
    {

        var master = (from e in _employeeRepository.GetAll()
                      join education in _educationRepository.GetAll() on e.Guid equals education.Guid
                      join u in _universityRepository.GetAll() on education.UniversityGuid equals u.Guid
                      join ar in _accountRoleRepository.GetAll() on e.Guid equals ar.AccountGuid
                      join r in _roleRepository.GetAll() on ar.RoleGuid equals r.Guid
                      select new
                      {
                          Guid = e.Guid,
                          FullName = e.FirstName + e.LastName,
                          Nik = e.Nik,
                          BirthDate = e.BirthDate,
                          Email = e.Email,
                          Gender = e.Gender,
                          HiringDate = e.HiringDate,
                          PhoneNumber = e.PhoneNumber,
                          Major = education.Major,
                          Degree = education.Degree,
                          Gpa = education.Gpa,
                          UniversityName = u.Name,
                          Role = r.Name
                      }).ToList();

        if (!master.Any())
        {
            return null;
        }
        var toDto = master.Select(master => new GetAllMasterDto
        {
            Guid = master.Guid,
            FullName = master.FullName,
            Nik = master.Nik,
            BirthDate = master.BirthDate,
            Email = master.Email,
            Gender = master.Gender,
            HiringDate = master.HiringDate,
            PhoneNumber = master.PhoneNumber,
            Major = master.Major,
            Degree = master.Degree,
            Gpa = master.Gpa,
            UniversityName = master.UniversityName,
            Role = master.Role


        });

        return toDto;
    }
    public GetAllMasterDto? GetMasterByGuid(Guid guid)
    {

        var master = GetMaster();

        var masterByGuid = master.FirstOrDefault(master => master.Guid == guid);

        return masterByGuid;
    }
    public int ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var isExist = _employeeRepository.GetByEmail(changePasswordDto.Email);
        if (isExist is null)
        {
            return -1; // Account not found
        }

        var getAccount = _accountRepository.GetByGuid(isExist.Guid);
        if (getAccount.Otp != changePasswordDto.Otp)
        {
            return 0;
        }

        if (getAccount.IsUsed == true)
        {
            return 1;
        }

        if (getAccount.ExpiredDate < DateTime.Now)
        {
            return 2;
        }

        var account = new Account
        {
            Guid = getAccount.Guid,
            IsUsed = getAccount.IsUsed,
            IsDeleted = getAccount.IsDeleted,
            ModifiedDate = DateTime.Now,
            CreatedDate = getAccount!.CreatedDate,
            Otp = getAccount.Otp,
            ExpiredDate = getAccount.ExpiredDate,
            Password = Hashing.HashPassword(changePasswordDto.NewPassword),
        };

        var isUpdate = _accountRepository.Update(account);
        if (!isUpdate)
        {
            return 0; // Account not updated
        }

        return 3;
    }

    public RegisterAccount Register(RegisterAccount registerDto)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {

            var role = _roleRepository.GetByName("User");

            EmployeeService employeeService = new EmployeeService(_employeeRepository);
            string nik = employeeService.GenerateNik();

            if (role is null)
            {
                return null;
            }

            var employee = new Employee
            {
                Guid = new Guid(),
                PhoneNumber = registerDto.PhoneNumber,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName ?? "",
                Gender = registerDto.Gender,
                HiringDate = registerDto.HiringDate,
                Email = registerDto.Email,
                Birtdate = registerDto.BirthDate,
                Nik = nik,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            var createdEmployee = _employeeRepository.Create(employee);

            var account = new Account
            {
                Guid = createdEmployee.Guid,
                IsDeleted = false,
                IsUsed = true,
                Otp = 0,
                ExpiredDate = DateTime.Now,
                Password = Hashing.HashPassword(registerDto.Password),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };

            var createdAccount = _accountRepository.Create(account);

            var universityEntity = _universityRepository.CreateWithDuplicateCheck(registerDto.UniversityCode, registerDto.UniversityName);

            if (universityEntity == null)
            {
                var university = new University
                {
                    Guid = new Guid(),
                    Code = registerDto.UniversityCode,
                    Name = registerDto.UniversityName,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };
                universityEntity = _universityRepository.Create(university);
            }

            var education = new Education
            {
                Guid = createdEmployee.Guid,
                Major = registerDto.Major,
                Degree = registerDto.Degree,
                Gpa = registerDto.Gpa,
                UniversityGuid = universityEntity.Guid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };
            var createdEducation = _educationRepository.Create(education);

            var accountRole = new AccountRole
            {
                Guid = new Guid(),
                AccountGuid = createdAccount.Guid,
                RoleGuid = role.Guid
            };
            var createdAccountRole = _accountRoleRepository.Create(accountRole);

            var toDto = new RegisterAccount
            {
                FirstName = createdEmployee.FirstName,
                LastName = createdEmployee.LastName,
                BirthDate = createdEmployee.BirthDate,
                Gender = createdEmployee.Gender,
                HiringDate = createdEmployee.HiringDate,
                Email = createdEmployee.Email,
                PhoneNumber = createdEmployee.PhoneNumber,
                Major = createdEducation.Major,
                Degree = createdEducation.Degree,
                Gpa = createdEducation.Gpa,
                UniversityCode = universityEntity.Code,
                UniversityName = universityEntity.Name,
                Password = createdAccount.Password
            };
            transaction.Commit();
            return toDto;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }

    }
}
