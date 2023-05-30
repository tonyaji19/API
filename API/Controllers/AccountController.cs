using API.Contracts;
using API.Models;
using API.Repositories;
using API.Utility;
using API.ViewModels.Account;
using API.ViewModels.Accounts;
using API.ViewModels.Employees;
using API.ViewModels.Login;
using API.ViewModels.Others;
using API.ViewModels.Universities;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : BaseController<Account, AccountVM>
{
    private readonly IMapper<Account, AccountVM> _mapper;
    private readonly IAccountRepository _accountRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmailService _emailService;
    private readonly ITokenService _tokenService;

    public AccountController(IAccountRepository accountRepository, IEmployeeRepository employeeRepository, ITokenService tokenService,
    IEducationRepository educationRepository, IUniversityRepository univeristyRepository,
        IMapper<Account, AccountVM> mapper, IEmailService emailService) : base(accountRepository, mapper)
    {
        _mapper = mapper;
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _universityRepository = univeristyRepository;
        _emailService = emailService;
        _tokenService = tokenService;
    }
    [AllowAnonymous]
    [HttpPost("Register")]
    /*[EnableCors("Tokopedia")]*/
    public IActionResult Register(RegisterVM registerVM)
    {

        var result = _accountRepository.Register(registerVM);
        switch (result)
        {
            case 0:
                //return BadRequest("Registration failed");
                return BadRequest(new ResponseVM<AccountVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Registration failed"
                });

            case 1:
                //return BadRequest("Email already exists");
                return BadRequest(new ResponseVM<AccountVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Email already exists"
                });
            case 2:
                //return BadRequest("Phone number already exists");
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

    [AllowAnonymous]
    [HttpPost("Login")]

    public IActionResult Login(LoginVM loginVM)
    {
        var account = _accountRepository.Login(loginVM);
        var employee = _employeeRepository.GetbyEmail(loginVM.Email);

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

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, employee.Nik),
            new(ClaimTypes.Name, $"{employee.FirstName} {employee.LastName}"),
            new(ClaimTypes.Email, employee.Email),
        };

        var roles = _accountRepository.GetRoles(employee.Guid);
        //claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        foreach(var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = _tokenService.GenerateToken(claims);

        return Ok(new ResponseVM<string>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Login Success",
            Data = token
        });
        
    }

    [AllowAnonymous]
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
    [AllowAnonymous]
    [HttpPost("ForgotPassword/{email}")]
    public IActionResult UpdateResetPass(String email)
    {

        var getGuid = _employeeRepository.FindGuidByEmail(email);
        if (getGuid == null)
        {
            return NotFound(new ResponseVM<AccountVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Email Not Found"
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

                _emailService.SetEmail(email)
                .SetSubject("Forgot Passowrd")
                .SetHtmlMessage($"Your OTP is {isUpdated}")
                .SendEmailAsync();

                return Ok(new ResponseVM<AccountResetPasswordVM>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Account Reset Success"
                });
        }
    }

    [HttpGet("GetAllByToken")]
        public IActionResult GetByToken(string token)
        {
            var data = _tokenService.ExtractClaimsFromJWT(token);
            if (data is null)
            {
                return BadRequest(new ResponseVM<ClaimVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Not Found",
                });
            }

            return Ok(new ResponseVM<ClaimVM>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Found Data from jwt",
                Data = data
            });
        }
}