using API.Models;

namespace API.Contracts;

public interface IEducationRepository : IGeneralRepository<Education>
{
    IEnumerable<Education> GetByUniversityId(Guid universityId);

    /*    Education Create(Education education);
        bool Update(Education education);
        bool Delete(Guid guid);
        IEnumerable<Education> GetAll();
        Education? GetByGuid(Guid guid);*/
}