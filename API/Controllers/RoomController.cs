using API.Contracts;
using API.DTOs.Room;
using API.Models;
using API.Services;
using API.Utilities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace API.Controllers;
[ApiController]
[Route("api/rooms")]
[Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
public class RoomController : ControllerBase
{
    private readonly RoomService _service;

    public RoomController(RoomService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _service.GetRoom();

        if (entities == null)
        {
            return NotFound(new ResponseHandler<GetRoomDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<GetRoomDto>>
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
        var room = _service.GetRoom(guid);
        if (room is null)
        {
            return NotFound(new ResponseHandler<GetRoomDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<GetRoomDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = room
        });
    }

    [HttpPost]
    public IActionResult Create(NewRoomDto newRoomDto)
    {
        var createRoom = _service.CreateRoom(newRoomDto);
        if (createRoom is null)
        {
            return BadRequest(new ResponseHandler<GetRoomDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created"
            });
        }

        return Ok(new ResponseHandler<GetRoomDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully created",
            Data = createRoom
        });
    }

    [HttpPut]
    public IActionResult Update(UpdateRoomDto updateRoomDto)
    {
        var update = _service.UpdateRoom(updateRoomDto);
        if (update is -1)
        {
            return NotFound(new ResponseHandler<UpdateRoomDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }
        if (update is 0)
        {
            return BadRequest(new ResponseHandler<UpdateRoomDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data"
            });
        }
        return Ok(new ResponseHandler<UpdateRoomDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully updated"
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _service.DeleteRoom(guid);

        if (delete is -1)
        {
            return NotFound(new ResponseHandler<GetRoomDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }
        if (delete is 0)
        {
            return BadRequest(new ResponseHandler<GetRoomDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }

        return Ok(new ResponseHandler<GetRoomDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }
    [HttpGet("unused")]
    /*[Authorize(Roles = $"{nameof(RoleLevel.Admin)}, {nameof(RoleLevel.Manager)}")]*/
    public IActionResult GetUnusedRooms()
    {
        var unusedRooms = _service.GetUnusedRoom();

        if (unusedRooms.Count() == 0)
        {
            return Ok(new ResponseHandler<IEnumerable<UnusedRoomDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Semua room sedang dipakai",
                Data = unusedRooms
            });
        }

        return Ok(new ResponseHandler<IEnumerable<UnusedRoomDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = unusedRooms
        });
    }

    [HttpGet("used")]
    /*[Authorize(Roles = $"{nameof(RoleLevel.Admin)}, {nameof(RoleLevel.Manager)}")]*/
    public IActionResult GetUsedRooms()
    {
        var usedRooms = _service.GetUsedRooms();

        if (usedRooms is null || usedRooms.Count() == 0)
        {
            return Ok(new ResponseHandler<IEnumerable<UsedRoomDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Tidak ada room yang sedang dipakai",
                Data = usedRooms
            });
        }

        return Ok(new ResponseHandler<IEnumerable<UsedRoomDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = usedRooms
        });
    }
}