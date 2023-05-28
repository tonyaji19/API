﻿
using API.Contracts;
using API.Models;
using API.Repositories;
using API.ViewModels.Bookings;
using API.ViewModels.Others;
using Microsoft.AspNetCore.Mvc;
using RestAPI.ViewModels.Bookings;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IMapper<Booking, BookingVM> _mapper;
    private readonly IBookingRepository _bookingRepository;
    public BookingController(IBookingRepository bookingRepository, IMapper<Booking, BookingVM> mapper)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var bookings = _bookingRepository.GetAll();
        if (!bookings.Any())
        {
            return NotFound(new ResponseVM<BookingVM>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not found"
            });
        }
        var data = bookings.Select(_mapper.Map).ToList();
        return Ok(new ResponseVM<List<BookingVM>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Di Tampilkan",
            Data = data
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var booking = _bookingRepository.GetByGuid(guid);
        if (booking is null)
        {
            return NotFound(new ResponseVM<BookingVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "GetByGuid NotFound"
            });
        }

        var data = _mapper.Map(booking);
        return Ok(new ResponseVM<BookingVM>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "GeByGuid Success",
            Data = data
        });
    }

    [HttpPost]
    public IActionResult Create(BookingVM bookingVM)
    {
        var bookingConverted = _mapper.Map(bookingVM);
        var result = _bookingRepository.Create(bookingConverted);
        if (result is null)
        {
            return BadRequest(new ResponseVM<BookingVM>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Failed Create Booking"
            });
        }

        return Ok(new ResponseVM<BookingVM>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success Create Booking"
        });
    }

    [HttpPut]
    public IActionResult Update(BookingVM bookingVM)
    {
        var bookingConverted = _mapper.Map(bookingVM);
        var isUpdated = _bookingRepository.Update(bookingConverted);
        if (!isUpdated)
        {
            return BadRequest(new ResponseVM<BookingVM>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Failed Update Booking"
            });
        }

        return Ok(new ResponseVM<BookingVM>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success Update Booking"
        });
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _bookingRepository.Delete(guid);
        if (!isDeleted)
        {
            return BadRequest(new ResponseVM<Guid>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Failed Delete Booking"
            });
        }

        return Ok(new ResponseVM<Guid>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success Delete Guid"
        });
    }
    [HttpGet("bookingduration")]
    public IActionResult GetDuration()
    {
        var bookingLengths = _bookingRepository.GetBookingDuration();
        if (!bookingLengths.Any())
        {
            return NotFound();
        }

        return Ok(bookingLengths);
    }

    [HttpGet("BookingDetail")]
    public IActionResult GetAllBookingDetail()
    {
        try
        {

            var data = _bookingRepository.GetAllBookingDetail();

            return Ok(new ResponseVM<List<BookingDetailVM>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Di Tampilkan",
                Data = data.ToList()
            });
        }
        catch
        {
            return NotFound(new ResponseVM<BookingDetailVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data tidak bisa ditampilkan"
            });
        }

    }
    [HttpGet("BookingDetail/{guid}")]
    public IActionResult GetDetailByGuid(Guid guid)
    {
        try
        {
            var bookingDetailVM = _bookingRepository.GetBookingDetailByGuid(guid);

            if (bookingDetailVM is null)
            {
                return NotFound(new ResponseVM<BookingDetailVM>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Not Found"
                });
            }


            return Ok(new ResponseVM<BookingDetailVM>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "GetByGuid Success",
                Data = bookingDetailVM
            });
        }
        catch
        {
            return NotFound(new ResponseVM<BookingDetailVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Not Found"
            });
        }
    }
}


