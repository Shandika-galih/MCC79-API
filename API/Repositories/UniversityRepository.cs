using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class UniversityRepository : GeneralRepository<University>, IUniversityRepository
{
    public UniversityRepository(BookingDbContext context) : base(context) { }

    public IEnumerable<University> GetByName(string name)
    {
        return _context.Set<University>().Where(u => u.Name.Contains(name));
    }
    public University? CreateWithDuplicateCheck(University university)
    {
        var getUniversity = _context.Universities.FirstOrDefault(u => u.Name == university.Name && u.Code == university.Code);

        if (getUniversity != null)
        {
            return getUniversity;
        }

        return Create(university);
    }
}