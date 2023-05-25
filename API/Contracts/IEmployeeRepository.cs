using API.Models;
namespace API.Contracts;
using API.ViewModels.Employees;

public interface IEmployeeRepository : IGeneralRepository<Employee>
{
    public Guid? FindGuidByEmail(string email);
    IEnumerable<MasterEmployeeVM> GetAllMasterEmployee();
    MasterEmployeeVM? GetMasterEmployeeByGuid(Guid guid);
    int CreateWithValidate(Employee employee);

    /*    Employee Create(Employee employee);
        bool Update(Employee employee);
        bool Delete(Guid guid);
        IEnumerable<Employee> GetAll();
        Employee? GetByGuid(Guid guid);*/
}
