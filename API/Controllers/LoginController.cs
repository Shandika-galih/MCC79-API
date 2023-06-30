/*using API.Contracts;
using API.DTOs.Universities;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LoginController
{

    

    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public GetAccountDto GetAccountByEmail(string email)
    {
        return _accountRepository.GetByEmail(email);
    }

    public bool VerifyPassword(GetAccountDto account, string password)
    {
        // Misalkan Anda menggunakan algoritma hashing untuk menyimpan password di database
        // Di sini Anda dapat membandingkan password yang diberikan dengan password yang disimpan dalam akun

        // Contoh sederhana menggunakan algoritma BCrypt
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, account.Password);

        return isPasswordValid;
    }
}
*/