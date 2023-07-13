using API.DTOs.Employee;

namespace Client.Contracts
{
    public interface IEmployeeRepository : IRepository<GetEmployeeDto, Guid>
    {
    }
}
