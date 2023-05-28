
using API.Contracts;
using API.Models;
using API.ViewModels.Account;
using API.ViewModels.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using API.Repositories;
using Microsoft.EntityFrameworkCore;
using API.ViewModels.Accounts;
using API.Utility;
using API.ViewModels.Login;
using Azure;
using System.Net;
using API.ViewModels.Others;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IMapper<Account, AccountVM> _mapper;
    private readonly IMapper<Employee, EmployeeVM> _EmailMapper;
    private readonly IMapper<Account, ChangePasswordVM> _changePasswordMapper;
    private readonly IAccountRepository _accountRepository;
    private readonly IEmployeeRepository _employeeRepository;


    public AccountController(IAccountRepository accountRepository, IEmployeeRepository employeeRepository, IMapper<Account, AccountVM> mapper, IMapper<Account, ChangePasswordVM> changePasswordMapper, IMapper<Employee, EmployeeVM> emailMapper)
    {
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _mapper = mapper;
        _changePasswordMapper = changePasswordMapper;
        _EmailMapper = emailMapper;
    }

    /*[HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);
        if (account is null || account.Employee is null)
        {
            return NotFound();
        }

        // Join dengan EmployeeVM untuk mendapatkan Email
        var data = new
        {
            Account = _mapper.Map(account),
            Email = account.Employee.Email
        };

        return Ok(data);
    }*/

    [HttpPost("login")]

    public IActionResult Login(LoginVM loginVM)
    {
        var account = _accountRepository.Login(loginVM);

        if (account == null)
        {
            return NotFound(new ResponseVM<LoginVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Account Not Found"
            });
        }

        if (account.Password != loginVM.Password)
        {
            return BadRequest(new ResponseVM<LoginVM>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Password Invalid"
            });
        }

        return Ok(new ResponseVM<LoginVM>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Login Success"
        });

    }
    [HttpPost("Register")]

    public IActionResult Register(RegisterVM registerVM)
    {

        var result = _accountRepository.Register(registerVM);
        switch (result)
        {
            case 0:
                return BadRequest(new ResponseVM<AccountVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Registration failed"
                });
            case 1:
                return BadRequest(new ResponseVM<AccountVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Email already exists"
                });
            case 2:
                return BadRequest(new ResponseVM<AccountVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Phone number already exists"
                });
            case 3:
                return Ok(new ResponseVM<AccountVM>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Registration success"
                });
        }

        return Ok(new ResponseVM<AccountVM>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Registration success"
        });

    }

    [HttpPost("ChangePassword")]
    public IActionResult ChangePassword(ChangePasswordVM changePasswordVM)
    {
        // Cek apakah email dan OTP valid
        var account = _employeeRepository.FindGuidByEmail(changePasswordVM.Email);
        var changePass = _accountRepository.ChangePasswordAccount(account, changePasswordVM);
        switch (changePass)
        {
            case 0:
                return BadRequest(new ResponseVM<ChangePasswordVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "ChangePassword Failed"
                });
            case 1:
                return Ok(new ResponseVM<ChangePasswordVM>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "ChangePassword Success"
                });
            case 2:
                return BadRequest(new ResponseVM<ChangePasswordVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "invalidOTP "
                });
            case 3:

                return BadRequest(new ResponseVM<ChangePasswordVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "OTP has already been used"
                });
            case 4:
                return BadRequest(new ResponseVM<ChangePasswordVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "OTP expired"
                });
            case 5:
                return BadRequest(new ResponseVM<ChangePasswordVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Wrong Password No Same"
                });
            default:
                return BadRequest(new ResponseVM<ChangePasswordVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Error"
                });
        }

    }

    [HttpPost("ForgotPassword" + "{email}")]
    public IActionResult UpdateResetPass(String email)
    {

        var getGuid = _employeeRepository.FindGuidByEmail(email);
        if (getGuid == null)
        {
            return NotFound(new ResponseVM<AccountVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Account Not Found"
            });
        }

        var isUpdated = _accountRepository.UpdateOTP(getGuid);

        switch (isUpdated)
        {
            case 0:
                return BadRequest(new ResponseVM<AccountVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Failed Update OTP"
                });
            default:
                var hasil = new AccountResetPasswordVM
                {
                    Email = email,
                    OTP = isUpdated
                };

                MailService mailService = new MailService();
                mailService.WithSubject("Kode OTP")
                           .WithBody("OTP anda adalah: " + isUpdated.ToString() + ".\n" +
                                     "Mohon kode OTP anda tidak diberikan kepada pihak lain" + ".\n" + "Terima kasih.")
                           .WithEmail(email)
                           .Send();

                return Ok(new ResponseVM<AccountResetPasswordVM>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Account Reset Success"
                });

        }
    }


    [HttpGet]
    public IActionResult GetAll()
    {
        var accounts = _accountRepository.GetAll();
        if (!accounts.Any())
        {
            return NotFound(new ResponseVM<AccountVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        var data = accounts.Select(_mapper.Map).ToList();
        return Ok(new ResponseVM<List<AccountVM>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success",
            Data = data
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);
        if (account is null)
        {
            return NotFound(new ResponseVM<List<AccountVM>>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid Not Found",
            });
        }

        var data = _mapper.Map(account);
        return Ok(new ResponseVM<AccountVM>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Guid Found",
            Data = data
        });
    }

    [HttpPost]
    public IActionResult Create(AccountVM accountVM)
    {
        var accountConverted = _mapper.Map(accountVM);
        var result = _accountRepository.Create(accountConverted);
        if (result is null)
        {
            return BadRequest(new ResponseVM<AccountVM>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Failed Create Account"
            });
        }

        return Ok(new ResponseVM<AccountVM>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success Create Account"
        });
    }

    [HttpPut]
    public IActionResult Update(AccountVM accountVM)
    {
        var accountConverted = _mapper.Map(accountVM);
        var isUpdated = _accountRepository.Update(accountConverted);
        if (!isUpdated)
        {
            return BadRequest(new ResponseVM<AccountVM>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Failed Update Account"
            });
        }

        return Ok(new ResponseVM<AccountVM>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success Update Account"
        });
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _accountRepository.Delete(guid);
        if (!isDeleted)
        {
            return BadRequest(new ResponseVM<AccountVM>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Failed Delete Account"
            });
        }

        return Ok(new ResponseVM<AccountVM>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success Delete Account"
        });
    }



    /*[HttpPost("ChangePassword")]
    public IActionResult ChangePassword(ChangePasswordVM changePasswordVM, [FromQuery] EmployeeVM employeeVM)
    {
        var accountVM = new AccountVM
        {
            Email = employeeVM.Email,
            Password = changePasswordVM.NewPassword
        };
        // Verify OTP
        if (!_accountRepository.VerifyOTP(changePasswordVM.Email, changePasswordVM.OTP))
        {
            return BadRequest("Invalid OTP");
        }

        // Check if OTP has already been used
        if (_accountRepository.IsOTPUsed(changePasswordVM.Email, changePasswordVM.OTP))
        {
            return BadRequest("OTP has already been used");
        }

        // Check if OTP has expired
        if (_accountRepository.IsOTPExpired(changePasswordVM.Email, changePasswordVM.OTP))
        {
            return BadRequest("OTP has expired");
        }

        // Check if NewPassword and ConfirmPassword match
        if (changePasswordVM.NewPassword != changePasswordVM.ConfirmPassword)
        {
            return BadRequest("New password and confirm password do not match");
        }

        // Update password
        var isPasswordUpdated = _accountRepository.ChangePassword(changePasswordVM.Email, changePasswordVM.NewPassword);
        if (!isPasswordUpdated)
        {
            return BadRequest("Failed to update password");
        }

        // Mark OTP as used
        _accountRepository.MarkOTPAsUsed(changePasswordVM.Email, changePasswordVM.OTP);

        return Ok("Password updated successfully");

    }*/
}


