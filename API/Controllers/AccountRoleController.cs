
    
using API.Contracts;
using API.Models;
using API.Repositories;
using API.ViewModels.AccountRoles;
using API.ViewModels.Others;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AccountRoleController : BaseController<AccountRole, AccountRoleVM>
{
    public AccountRoleController(IAccountRoleRepository accountRoleRepository, IMapper<AccountRole, AccountRoleVM> mapper)
        : base(accountRoleRepository, mapper)
    {
    }
}
/*public class AccountRoleController : ControllerBase
{
    private readonly IMapper<AccountRole, AccountRoleVM> _mapper;
    private readonly IAccountRoleRepository _accountRoleRepository;
    public AccountRoleController(IAccountRoleRepository accountRoleRepository, IMapper<AccountRole, AccountRoleVM> mapper)
    {
        _mapper = mapper;
        _accountRoleRepository = accountRoleRepository;
    }*/

/*[HttpGet]
    public IActionResult GetAll()
    {
        var accountRoles = _accountRoleRepository.GetAll();
        if (!accountRoles.Any())
        {
            return NotFound(new ResponseVM<AccountRoleVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Not Found"
            });
        }

        var data = accountRoles.Select(_mapper.Map).ToList();
        return Ok(new ResponseVM<List<AccountRoleVM>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Found Data AccountRole",
            Data = data
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var accountRole = _accountRoleRepository.GetByGuid(guid);
        if (accountRole is null)
        {
            return NotFound(new ResponseVM<AccountRoleVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Not Found Guid"
            });
        }

        var data = _mapper.Map(accountRole);
        return Ok(new ResponseVM<AccountRoleVM>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Found Guid",
            Data = data
        });
    }

    [HttpPost]
    public IActionResult Create(AccountRoleVM accountRoleVM)
    {
        var accountRoleConverted = _mapper.Map(accountRoleVM);
        var result = _accountRoleRepository.Create(accountRoleConverted);
        if (result is null)
        {
            return BadRequest(new ResponseVM<AccountRoleVM>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Create Account Failed"
            });
        }

        return Ok(new ResponseVM<AccountRoleVM>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Create Account Success"
        });
    }

    [HttpPut]
    public IActionResult Update(AccountRoleVM accountRoleVM)
    {
        var accountRoleConverted = _mapper.Map(accountRoleVM);
        var isUpdated = _accountRoleRepository.Update(accountRoleConverted); 
        if (!isUpdated)
        {
            return BadRequest(new ResponseVM<AccountRoleVM>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Update Account Failed"
            });
        }

        return Ok(new ResponseVM<AccountRoleVM>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Update Account Success"
        });
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _accountRoleRepository.Delete(guid);
        if (!isDeleted)
        {
            return BadRequest(new ResponseVM<AccountRoleVM>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Delete Account Failed"
            });
        }

        return Ok(new ResponseVM<AccountRoleVM>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Delete Account Success"
        });
    }
}*/


