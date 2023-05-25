
using API.Contexts;
using API.Contracts;
using API.Models;
using API.Utility;
using API.ViewModels.Account;
using API.ViewModels.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class AccountRepository : GeneralRepository<Account>, IAccountRepository
{
    /*    private Dictionary<string, ChangePasswordVM> OTPDictionary;
    */
    public AccountRepository(BookingManagementDbContext context) : base(context)
    { }
    /*public Account GetByEmail(string email)
    {
        return _context.Accounts.FirstOrDefault(a => a.Email == email);
    }*/

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

/*    public bool UpdatePassword(Account account)
    {
        try
        {
            _context.Entry(account).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }*/
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

