

using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EducationController : ControllerBase
{
    private readonly IEducationRepository _educationRepository;
    public EducationController(IEducationRepository educationRepository)
    {
        _educationRepository = educationRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var education = _educationRepository.GetAll();
        if (!education.Any())
        {
            return NotFound();
        }

        return Ok(education);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var education = _educationRepository.GetByGuid(guid);
        if (education is null)
        {
            return NotFound();
        }

        return Ok(education);
    }

    [HttpPost]
    public IActionResult Create(Education education)
    {
        var result = _educationRepository.Create(education);
        if (result is null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPut]
    public IActionResult Update(Education education)
    {
        var isUpdated = _educationRepository.Update(education);
        if (!isUpdated)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _educationRepository.Delete(guid);
        if (!isDeleted)
        {
            return BadRequest();
        }

        return Ok();
    }
}


