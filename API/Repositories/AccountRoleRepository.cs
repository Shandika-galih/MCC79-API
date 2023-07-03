using API.Contracts;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class AccountRoleRepository : GeneralRepository<AccountRole>, IAccountRoleRepository
{
    public AccountRoleRepository(BookingDbContext context) : base(context) { }
    public ICollection<AccountRole> GetByGuidEmployee(Guid employeeGuid)
    {
        return _context.Set<AccountRole>().Where(a => a.AccountGuid == employeeGuid).ToList();
    }
}
