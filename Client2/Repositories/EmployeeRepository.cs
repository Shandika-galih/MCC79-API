using API.DTOs.Employee;
using API.Models;
using Client.Contracts;

namespace Client.Repositories
{
    public class EmployeeRepository : GeneralRepository<GetEmployeeDto, Guid>, IEmployeeRepository
    {
        public EmployeeRepository(string request = "employees/") : base(request)
        {
        }
    }
}