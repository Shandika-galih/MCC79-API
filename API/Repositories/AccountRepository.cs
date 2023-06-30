using API.Contracts;
using API.Data;
using API.DTOs.Universities;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class AccountRepository : GeneralRepository<Account>, IAccountRepository
{
    public AccountRepository(BookingDbContext context) : base(context) { }

    public GetAccountDto GetByEmail(string email)
    {
        throw new NotImplementedException();
    }
}
