﻿using API.Contracts;
using API.Models;
using API.ViewModels.Employees;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IMapper<Employee, EmployeeVM> _mapper;
    private readonly IEmployeeRepository _employeeRepository;
    public EmployeeController(IEmployeeRepository employeeRepository, IMapper<Employee, EmployeeVM> mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }
    [HttpGet("GetAllMasterEmployee")]
    public IActionResult GetAll()
    {
        var masterEmployees = _employeeRepository.GetAllMasterEmployee();
        if (!masterEmployees.Any())
        {
            return NotFound();
        }

        return Ok(masterEmployees);
    }

    [HttpGet("GetMasterEmployeeByGuid")]
    public IActionResult GetMasterEmployeeByGuid(Guid guid)
    {
        var masterEmployees = _employeeRepository.GetMasterEmployeeByGuid(guid);
        if (masterEmployees is null)
        {
            return NotFound();
        }

        return Ok(masterEmployees);
    }

    [HttpGet]
    public IActionResult GetEmployee()
    {
        var employees = _employeeRepository.GetAll();
        if (!employees.Any())
        {
            return NotFound();
        }

        var resultConverted = employees.Select(_mapper.Map).ToList();
        return Ok(resultConverted);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var employee = _employeeRepository.GetByGuid(guid);
        if (employee is null)
        {
            return NotFound();
        }

        var data = _mapper.Map(employee);
        return Ok(data);
    }

    [HttpPost]
    public IActionResult Create(EmployeeVM employeeVM)
    {
        var employeeConverted = _mapper.Map(employeeVM);
        var result = _employeeRepository.Create(employeeConverted);
        if (result is null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPut]
    public IActionResult Update(EmployeeVM employeeVM)
    {
        var employeeConverted = _mapper.Map(employeeVM);
        var isUpdated = _employeeRepository.Update(employeeConverted);
        if (!isUpdated)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _employeeRepository.Delete(guid);
        if (!isDeleted)
        {
            return BadRequest();
        }

        return Ok();
    }
}


