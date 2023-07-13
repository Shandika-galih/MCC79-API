using API.DTOs.Employee;
using Client.Contracts;
using API.Models;

namespace Client.Repositories
{
    public class EmployeeRepository : GeneralRepository<GetEmployeeDto, Guid>, IEmployeeRepository
    {
        public EmployeeRepository(string request = "employees/") : base(request)
        {
        }
    }
}
