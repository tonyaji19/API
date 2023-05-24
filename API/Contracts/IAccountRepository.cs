using API.Models;
using API.ViewModels.Account;

namespace API.Contracts;

public interface IAccountRepository : IGeneralRepository<Account>
{
/*    Account GetByEmail(string email);
*/
    public int GetByEmployeeId(Guid? employeeId, ChangePasswordVM changePasswordVM);

    /* bool VerifyOTP(string email, string otp);
     bool IsOTPUsed(string email, string otp);
     bool IsOTPExpired(string email, string otp);
     bool ChangePassword(string email, string newPassword);
     bool MarkOTPAsUsed(string email, string otp);*/

    /*    Account Create(Account account);
        bool Update(Account account);
        bool Delete(Guid guid);
        IEnumerable<Account> GetAll();
        Account? GetByGuid(Guid guid);*/
}