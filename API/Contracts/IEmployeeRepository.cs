using API.DTOs.Universities;
using API.Models;
namespace API.Contracts;

public interface IEmployeeRepository : IGeneralRepository<Employee>
{
    public Employee? GetByEmailAndPhoneNumber(string nama);
    public Employee? GetByEmail(string email);
    public Employee? GetEmailLogin(string email);
}
