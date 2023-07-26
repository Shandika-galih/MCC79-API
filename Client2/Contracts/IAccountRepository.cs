using API.DTOs.Account;
using API.Utilities;
using API.Utilities.Enums;
using Client.Repositories;

namespace Client.Contracts
{
    public interface IAccountRepository : IRepository<RegisterAccount, string>
    {
        Task<ResponseHandler<AccountRepository>> Register(RegisterAccount entity);
        Task<ResponseHandler<string>> Login(LoginDto entity);
    }
}