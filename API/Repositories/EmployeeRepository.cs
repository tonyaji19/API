using API.Contexts;
using API.Contracts;
using API.Models;
using API.ViewModels.Employees;

namespace API.Repositories;

public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(BookingManagementDbContext context) : base(context) { }

    public IEnumerable<MasterEmployeeVM> GetAllMasterEmployee()
    {
        var employees = GetAll();
        var educations = _context.Educations.ToList();
        var universities = _context.Universities.ToList();

        var employeeEducations = new List<MasterEmployeeVM>();

        foreach (var employee in employees)
        {
            var education = educations.FirstOrDefault(e => e.Guid == employee?.Guid);
            var university = universities.FirstOrDefault(u => u.Guid == education?.UniversityGuid);

            if (education != null && university != null)
            {
                var employeeEducation = new MasterEmployeeVM
                {
                    Guid = employee.Guid,
                    NIK = employee.Nik,
                    FullName = employee.FirstName + " " + employee.LastName,
                    BirthDate = employee.BirthDate,
                    Gender = employee.Gender.ToString(),
                    HiringDate = employee.HiringDate,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    Major = education.Major,
                    Degree = education.Degree,
                    GPA = education.Gpa,
                    UniversityName = university.Name
                };

                employeeEducations.Add(employeeEducation);
            }
        }

        return employeeEducations;
    }

    MasterEmployeeVM? IEmployeeRepository.GetMasterEmployeeByGuid(Guid guid)
    {
        var employee = GetByGuid(guid);
        var education = _context.Educations.FirstOrDefault(e => e.Guid == guid);
        var university = _context.Universities.FirstOrDefault(u => u.Guid == education.UniversityGuid);

        var data = new MasterEmployeeVM
        {
            Guid = employee.Guid,
            NIK = employee.Nik,
            FullName = employee.FirstName + " " + employee.LastName,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender.ToString(),
            HiringDate = employee.HiringDate,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            Major = education.Major,
            Degree = education.Degree,
            GPA = education.Gpa,
            UniversityName = university.Name
        };

        return data;
    }
    public Guid? FindGuidByEmail(string email)
    {
        try
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Email == email);
            if (employee == null)
            {
                return null;
            }
            return employee.Guid;
        }
        catch
        {
            return null;
        }
    }

    /*private readonly BookingManagementDbContext _context;
    public EmployeeRepository(BookingManagementDbContext context)
    {
        _context = context;
    }

    *//*
     * <summary>
     * Create a new university
     * </summary>
     * <param name="university">University object</param>
     * <returns>University object</returns>
     *//*
    public Employee Create(Employee employee)
    {
        try
        {
            _context.Set<Employee>().Add(employee);
            _context.SaveChanges();
            return employee;
        }
        catch
        {
            return new Employee();
        }
    }

    *//*
     * <summary>
     * Update a university
     * </summary>
     * <param name="university">University object</param>
     * <returns>true if data updated</returns>
     * <returns>false if data not updated</returns>
     *//*
    public bool Update(Employee employee)
    {
        try
        {
            _context.Set<Employee>().Update(employee);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    *//*
     * <summary>
     * Delete a university
     * </summary>
     * <param name="guid">University guid</param>
     * <returns>true if data deleted</returns>
     * <returns>false if data not deleted</returns>
     *//*
    public bool Delete(Guid guid)
    {
        try
        {
            var employee = GetByGuid(guid);
            if (employee == null)
            {
                return false;
            }

            _context.Set<Employee>().Remove(employee);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    *//*
     * <summary>
     * Get all universities
     * </summary>
     * <returns>List of universities</returns>
     * <returns>Empty list if no data found</returns>
     *//*
    public IEnumerable<Employee> GetAll()
    {
        return _context.Set<Employee>().ToList();
    }

    *//*
     * <summary>
     * Get a university by guid
     * </summary>
     * <param name="guid">University guid</param>
     * <returns>University object</returns>
     * <returns>null if no data found</returns>
     *//*
    public Employee? GetByGuid(Guid guid)
    {
        return _context.Set<Employee>().Find(guid);
    }*/
}