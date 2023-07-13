using API.Contracts;
using API.DTOs.AccountRole;
using API.DTOs.Booking;
using API.Models;
using API.Services;
using API.Utilities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace API.Controllers;
[ApiController]
[Route("Api/Bookings")]
public class BookingController : ControllerBase
{
    private readonly BookingService _service;

    public BookingController(BookingService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _service.GetBooking();

        if (entities == null)
        {
            return NotFound(new ResponseHandler<GetBookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<GetBookingDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = entities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var booking = _service.GetBooking(guid);
        if (booking is null)
        {
            return NotFound(new ResponseHandler<GetBookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<GetBookingDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = booking
        });
    }

   /* [Authorize(Roles = $"{nameof(RoleLevel.Manager)}, {nameof(RoleLevel.User)}")]*/
    [HttpPost]
    public IActionResult Create(NewBookingDto newBookingDto)
    {
        var createBooking = _service.CreateBooking(newBookingDto);
        if (createBooking is null)
        {
            return BadRequest(new ResponseHandler<GetBookingDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created"
            });
        }

        return Ok(new ResponseHandler<GetBookingDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully created",
            Data = createBooking
        });
    }

    [HttpPut]
    public IActionResult Update(UpdateBookingDto updateBookingDto)
    {
        var update = _service.UpdateBooking(updateBookingDto);
        if (update is -1)
        {
            return NotFound(new ResponseHandler<UpdateBookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }
        if (update is 0)
        {
            return BadRequest(new ResponseHandler<UpdateBookingDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data"
            });
        }
        return Ok(new ResponseHandler<UpdateBookingDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully updated"
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _service.DeleteBooking(guid);

        if (delete is -1)
        {
            return NotFound(new ResponseHandler<GetBookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }
        if (delete is 0)
        {
            return BadRequest(new ResponseHandler<GetBookingDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }

        return Ok(new ResponseHandler<GetBookingDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }
    [HttpGet("RoomToday")]
    public IActionResult GetDetail()
    {
        var roomToday = _service.GetRoomToday();

        if (roomToday == null)
        {
            return NotFound(new ResponseHandler<GetRoomTodayDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<GetRoomTodayDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = roomToday
        });
    }
    [HttpGet("Details")]
    public IActionResult GetBookingDetails()
    {
        var bookingDetails = _service.GetBookingDetails();
        if (bookingDetails == null)
        {
            return NotFound(new ResponseHandler<GetBookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<BookingDetailDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = bookingDetails
        });
    }

    [HttpGet("Details/{guid}")]
    public IActionResult GetBookingDetails(Guid guid)
    {
        var bookingDetails = _service.GetBookingDetailByGuid(guid);

        if (bookingDetails == null)
        {
            return NotFound(new ResponseHandler<GetBookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<BookingDetailDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = bookingDetails
        });

    }

    [HttpGet("GetBookingDuration")]
    public IActionResult GetBookingDuration()
    {
        var entities = _service.BookingDuration();

        if (entities == null || !entities.Any())
        {
            return NotFound(new ResponseHandler<GetBookingDurationDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<GetBookingDurationDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entities
        });
    }

}
