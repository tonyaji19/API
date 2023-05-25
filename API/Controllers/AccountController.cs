﻿
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
            return NotFound("Account not found");
        }

        if (account.Password != loginVM.Password)
        {
            return BadRequest("Password is invalid");
        }

        return Ok();

    }
    [HttpPost("Register")]

    public IActionResult Register(RegisterVM registerVM)
    {

        var result = _accountRepository.Register(registerVM);
        switch (result)
        {
            case 0:
                return BadRequest("Registration failed");
            case 1:
                return BadRequest("Email already exists");
            case 2:
                return BadRequest("Phone number already exists");
            case 3:
                return Ok("Registration success");
        }

        return Ok();

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
                return BadRequest("");
            case 1:
                return Ok("Password has been changed successfully");
            case 2:
                return BadRequest("Invalid OTP");
            case 3:
                return BadRequest("OTP has already been used");
            case 4:
                return BadRequest("OTP expired");
            case 5:
                return BadRequest("Wrong Password No Same");
            default:
                return BadRequest();
        }
        return null;

    }

    [HttpPost("ForgotPassword" + "{email}")]
    public IActionResult UpdateResetPass(String email)
    {

        var getGuid = _employeeRepository.FindGuidByEmail(email);
        if (getGuid == null)
        {
            return NotFound("Akun tidak ditemukan");
        }

        var isUpdated = _accountRepository.UpdateOTP(getGuid);

        switch (isUpdated)
        {
            case 0:
                return BadRequest();
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

                return Ok(hasil);

        }
    }


    [HttpGet]
    public IActionResult GetAll()
    {
        var accounts = _accountRepository.GetAll();
        if (!accounts.Any())
        {
            return NotFound();
        }

        var data = accounts.Select(_mapper.Map).ToList();
        return Ok(data);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);
        if (account is null)
        {
            return NotFound();
        }

        var data = _mapper.Map(account);
        return Ok(data);
    }

    [HttpPost]
    public IActionResult Create(AccountVM accountVM)
    {
        var accountConverted = _mapper.Map(accountVM);
        var result = _accountRepository.Create(accountConverted);
        if (result is null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPut]
    public IActionResult Update(AccountVM accountVM)
    {
        var accountConverted = _mapper.Map(accountVM);
        var isUpdated = _accountRepository.Update(accountConverted);
        if (!isUpdated)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _accountRepository.Delete(guid);
        if (!isDeleted)
        {
            return BadRequest();
        }

        return Ok();
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


