using API.Contracts;
using API.Models;
using API.Repositories;
using API.Utility;
using API.ViewModels.Educations;
using API.ViewModels.Others;
using API.ViewModels.Universities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UniversityController : BaseController<University, UniversityVM>
    {
        private readonly IUniversityRepository _universityRepository;
        private readonly IMapper<University, UniversityVM> _mapper;
        public UniversityController(IUniversityRepository universityRepository, IMapper<University, UniversityVM> mapper) : base(universityRepository, mapper)
        {
            _universityRepository = universityRepository;

            _mapper = mapper;
        }

    }

}
/*[HttpGet("WithEducation")]
public IActionResult GetAllWithEducation()
{
    var universities = _universityRepository.GetAll();
    if (!universities.Any())
    {
        return NotFound();
    }

    var results = new List<UniversityEducationVM>();
    foreach (var university in universities)
    {
        var education = _educationRepository.GetByUniversityId(university.Guid);
        var educationMapped = education.Select(_educationMapper.Map).ToList();

        var result = new UniversityEducationVM
        {
            Guid = university.Guid,
            Code = university.Code,
            Name = university.Name,
            Educations = educationMapped
        };

        results.Add(result);
    }

    return Ok(results);
}

[HttpGet]
public IActionResult GetAll()
{
    var universities = _universityRepository.GetAll();
    if (!universities.Any())
    {
        return NotFound(new ResponseVM<UniversityVM>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Not Found Data University"
        });
    }

    *//*var univeritiesConverted = new List<UniversityVM>();
    foreach (var university in universities) {
        var result = UniversityVM.ToVM(university);
        univeritiesConverted.Add(result);
    }*//*

    var resultConverted = universities.Select(_mapper.Map).ToList();

    return Ok(new ResponseVM<List<UniversityVM>>
    {
        Code = StatusCodes.Status200OK,
        Status = HttpStatusCode.OK.ToString(),
        Message = "Found Data University",
        Data = resultConverted
    });
}

[HttpGet("{guid}")]
public IActionResult GetByGuid(Guid guid)
{
    var university = _universityRepository.GetByGuid(guid);
    if (university is null)
    {
        return NotFound(new ResponseVM<UniversityVM>
        {
            Code = StatusCodes.Status404NotFound
                ,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Guid Not Found"
        });
    }

    var data = _mapper.Map(university);

    return Ok(new ResponseVM<UniversityVM>
    {
        Code = StatusCodes.Status200OK,
        Status = HttpStatusCode.OK.ToString(),
        Message = "Guid Found",
        Data = data
    });
}

[HttpPost]
public IActionResult Create(UniversityVM universityVM)
{
    var universityConverted = _mapper.Map(universityVM);

    var result = _universityRepository.Create(universityConverted);
    if (result is null)
    {
        return BadRequest(new ResponseVM<UniversityVM>
        {
            Code = StatusCodes.Status400BadRequest
                ,
            Status = HttpStatusCode.BadRequest.ToString(),
            Message = "Create University Failed"
        });
    }
    return Ok(new ResponseVM<UniversityVM>
    {
        Code = StatusCodes.Status200OK,
        Status = HttpStatusCode.OK.ToString(),
        Message = "Create University Success"
    });
}

[HttpPut]
public IActionResult Update(UniversityVM universityVM)
{
    var universityConverted = _mapper.Map(universityVM);
    var isUpdated = _universityRepository.Update(universityConverted);

    if (!isUpdated)
    {
        return BadRequest(new ResponseVM<UniversityVM>
        {
            Code = StatusCodes.Status400BadRequest,
            Status = HttpStatusCode.BadRequest.ToString(),
            Message = "Update University Failed"
        });
    }
    return Ok(new ResponseVM<UniversityVM>
    {
        Code = StatusCodes.Status200OK,
        Status = HttpStatusCode.OK.ToString(),
        Message = "Update University Success"
    });
}

[HttpDelete("{guid}")]
public IActionResult Delete(Guid guid)
{
    var isDeleted = _universityRepository.Delete(guid);
    if (!isDeleted)
    {
        return BadRequest(new ResponseVM<UniversityVM>
        {
            Code = StatusCodes.Status400BadRequest,
            Status = HttpStatusCode.BadRequest.ToString(),
            Message = "Delete University Failed"
        });
    }
    return Ok(new ResponseVM<UniversityVM>
    {
        Code = StatusCodes.Status200OK,
        Status = HttpStatusCode.OK.ToString(),
        Message = "Update University Success"
    });
}
}
*/