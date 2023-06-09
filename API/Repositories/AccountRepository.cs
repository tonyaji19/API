﻿
using API.Contexts;
using API.Contracts;
using API.Models;
using API.Utility;
using API.ViewModels.Account;
using API.ViewModels.Accounts;
using API.ViewModels.Employees;
using API.ViewModels.Login;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class AccountRepository : GeneralRepository<Account>, IAccountRepository
{
    /*    private Dictionary<string, ChangePasswordVM> OTPDictionary;
     *    
    */
    protected readonly BookingManagementDbContext _context;

    private readonly IUniversityRepository _universityRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IRoleRepository _roleRepository;
    public AccountRepository(BookingManagementDbContext context, IEmployeeRepository employeeRepository, IRoleRepository roleRepository, IUniversityRepository universityRepository, IEducationRepository educationRepository) : base(context)
    {
        _universityRepository = universityRepository;
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _roleRepository = roleRepository;
        _context = context;
    }
    /*public Account GetByEmail(string email)
    {
        return _context.Accounts.FirstOrDefault(a => a.Email == email);
    }*/
    public LoginVM Login(LoginVM loginVM)
    {
        var account = GetAll();
        var employee = _employeeRepository.GetAll();
        var query = from emp in employee
                    join acc in account
                    on emp.Guid equals acc.Guid
                    where emp.Email == loginVM.Email
                    select new LoginVM
                    {
                        Email = emp.Email,
                        Password = acc.Password

                    };
        var validate = query.FirstOrDefault();

        if (validate != null && Hashing.ValidatePassword(loginVM.Password, validate.Password))
        {

            loginVM.Password = validate.Password;
        }
        return validate;
    }
    public int ChangePasswordAccount(Guid? employeeId, ChangePasswordVM changePasswordVM)
    {
        var account = new Account();
        account = _context.Set<Account>().FirstOrDefault(a => a.Guid == employeeId);
        if (account == null || account.OTP != changePasswordVM.OTP)
        {
            return 2;
        }
        // Cek apakah OTP sudah digunakan
        if (account.IsUsed)
        {
            return 3;
        }
        // Cek apakah OTP sudah expired
        if (account.ExpiredTime < DateTime.Now)
        {
            return 4;
        }
        // Cek apakah NewPassword dan ConfirmPassword sesuai
        if (changePasswordVM.NewPassword != changePasswordVM.ConfirmPassword)
        {
            return 5;
        }
        // Update password
        account.Password = changePasswordVM.NewPassword;
        account.IsUsed = true;
        try
        {
            var updatePassword = Update(account);
            if (!updatePassword)
            {
                return 0;
            }
            return 1;
        }
        catch
        {
            return 0;
        }
    }

    public int UpdateOTP(Guid? employeeId)
    {
        var account = new Account();
        account = _context.Set<Account>().FirstOrDefault(a => a.Guid == employeeId);
        //Generate OTP
        Random rnd = new Random();
        var getOtp = rnd.Next(100000, 999999);
        account.OTP = getOtp;

        //Add 5 minutes to expired time
        account.ExpiredTime = DateTime.Now.AddMinutes(5);
        account.IsUsed = false;
        try
        {
            var check = Update(account);


            if (!check)
            {
                return 0;
            }
            return getOtp;
        }
        catch
        {
            return 0;
        }
    }

    public int Register(RegisterVM registerVM)
    {
        try
        {
            var university = new University
            {
                Code = registerVM.UniversityCode,
                Name = registerVM.UniversityName

            };
            _universityRepository.CreateWithValidate(university);

            var employee = new Employee
            {
                Nik = GenerateNIK(),
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                BirthDate = registerVM.BirthDate,
                Gender = registerVM.Gender,
                HiringDate = registerVM.HiringDate,
                Email = registerVM.Email,
                PhoneNumber = registerVM.PhoneNumber,
            };
            var result = _employeeRepository.Create(employee);


            var education = new Education
            {
                Guid = employee.Guid,
                Major = registerVM.Major,
                Degree = registerVM.Degree,
                Gpa = registerVM.GPA,
                UniversityGuid = university.Guid
            };
            _educationRepository.Create(education);

            var account = new Account
            {
                Guid = employee.Guid,
                Password = Hashing.HashPassword(registerVM.Password),
                IsDeleted = false,
                IsUsed = true,
                OTP = 0
            };

            Create(account);

            var accountRole = new AccountRole
            {
                RoleGuid = Guid.Parse("31b3f1ea-61d1-4fc4-4571-08db60bf2a9b"),
                AccountGuid = employee.Guid
            };
            _context.AccountRoles.Add(accountRole);
            _context.SaveChanges();

            return 3;

        }
        catch
        {
            return 0;
        }

    }

    private string GenerateNIK()
    {
        var lastNik = _employeeRepository.GetAll().OrderByDescending(e => int.Parse(e.Nik)).FirstOrDefault();

        if (lastNik != null)
        {
            int lastNikNumber;
            if (int.TryParse(lastNik.Nik, out lastNikNumber))
            {
                return (lastNikNumber + 1).ToString();
            }
        }

        return "100000";
    }

    public IEnumerable<string> GetRoles(Guid guid)
    {
        var getAccount = GetByGuid(guid);
        if (getAccount == null) return Enumerable.Empty<string>();
        var getAccountRoles = from accountRoles in _context.AccountRoles
                              join roles in _context.Roles on accountRoles.RoleGuid equals roles.Guid
                              where accountRoles.AccountGuid == guid
                              select roles.Name;

        return getAccountRoles.ToList();
        /*var roles = _context.AccountRoles
            .Include(ar => ar.Role)
            .Where(ar => ar.AccountGuid == guid)
            .Select(ar => ar.Role.Name)
            .ToList();

        return roles.ToList();*/
    }

    /*        OTPDictionary = new Dictionary<string, ChangePasswordVM>();
    */
    /* public AccountEmpVM Login(LoginVM loginVM)
     {
         var account = GetAll();
         var employee = _employeeRepository.GetAll();
         var query = from emp in employee
                     join acc in account
                     on emp.Guid equals acc.Guid
                     where emp.Email == loginVM.Email
                     select new AccountEmpVM
                     {
                         Email = emp.Email,
                         Password = acc.Password

                     };
         return query.FirstOrDefault();
     }*/
}


//Join email
/*    public IActionResult CreateAccount(EmployeeVM employeeVM)
    {
        var accountVM = MapEmployeeToAccount(employeeVM);
        return ((IActionResult)accountVM);
    }
    public AccountVM MapEmployeeToAccount(EmployeeVM employee)
    {
        return new AccountVM
        {
            Email = employee.Email
            // Assign properti lain dari employee ke account jika perlu
        };
    }*/


// Implementasi metode-metode lain dalam AccountRepository

/*public bool VerifyOTP(string email, string otp)
{
    // Implementasi logika verifikasi OTP
    // Misalnya, dengan memeriksa OTP yang tersimpan dalam database

    if (OTPDictionary.ContainsKey(email))
    {
        var otpData = OTPDictionary[email];
        return otpData.OTP == otp;
    }

    return false;
}

public bool IsOTPUsed(string email, string otp)
{
    // Implementasi logika untuk memeriksa apakah OTP sudah digunakan sebelumnya
    // Misalnya, dengan memeriksa status OTP dalam database

    if (OTPDictionary.ContainsKey(email))
    {
        var otpData = OTPDictionary[email];
        return otpData.IsUsed;
    }

    return false;
}

public bool IsOTPExpired(string email, string otp)
{
    // Implementasi logika untuk memeriksa apakah OTP sudah kedaluwarsa
    // Misalnya, dengan memeriksa timestamp OTP dalam database

    // Contoh implementasi sederhana menggunakan dictionary sebagai penyimpanan sementara
    if (OTPDictionary.ContainsKey(email))
    {
        var otpData = OTPDictionary[email];
        var currentTime = DateTime.Now;
        return otpData.ExpirationTime < currentTime;
    }

    return false;
}

public bool ChangePassword(string email, string newPassword)
{
    // Implementasi logika untuk mengubah password
    // Misalnya, dengan memperbarui password dalam database

    // Contoh implementasi sederhana menggunakan dictionary sebagai penyimpanan sementara
    // Anda perlu menggantinya dengan logika sesuai dengan penyimpanan data yang digunakan
    if (OTPDictionary.ContainsKey(email))
    {
        var user = OTPDictionary[email];
        user.Password = newPassword;
        return true;
    }

    return false;
}
public bool MarkOTPAsUsed(string email, string otp)
{
    // Implementasi logika untuk menandai OTP sebagai digunakan
    // Misalnya, dengan menyimpan status OTP dalam database

    // Contoh implementasi sederhana menggunakan dictionary sebagai penyimpanan sementara
    if (OTPDictionary.ContainsKey(email))
    {
        var otpData = OTPDictionary[email];
        if (otpData.OTP == otp && !otpData.IsUsed)
        {
            otpData.IsUsed = true;
            return true;
        }
    }

    return false;
}
*/




/*private readonly BookingManagementDbContext _context;
public AccountRepository(BookingManagementDbContext context)
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
public Account Create(Account account)
{
    try
    {
        _context.Set<Account>().Add(account);
        _context.SaveChanges();
        return account;
    }
    catch
    {
        return new Account();
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
public bool Update(Account account)
{
    try
    {
        _context.Set<Account>().Update(account);
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
        var account = GetByGuid(guid);
        if (account == null)
        {
            return false;
        }

        _context.Set<Account>().Remove(account);
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
public IEnumerable<Account> GetAll()
{
    return _context.Set<Account>().ToList();
}

*//*
 * <summary>
 * Get a university by guid
 * </summary>
 * <param name="guid">University guid</param>
 * <returns>University object</returns>
 * <returns>null if no data found</returns>
 *//*
public Account? GetByGuid(Guid guid)
{
    return _context.Set<Account>().Find(guid);
}*/

