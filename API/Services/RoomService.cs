using API.Contracts;
using API.DTOs.Room;
using API.Models;
using API.Repositories;
using API.Utilities.Enums;

namespace API.Services;

public class RoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IEmployeeRepository _employeeRepository;
    public RoomService(IRoomRepository roomRepository, IBookingRepository bookingRepository, IEmployeeRepository employeeRepository)
    {
        _roomRepository = roomRepository;
        _bookingRepository = bookingRepository;
        _employeeRepository = employeeRepository;   
    }

    public IEnumerable<GetRoomDto>? GetRoom()
    {
        var rooms = _roomRepository.GetAll();
        if (!rooms.Any())
        {
            return null; // No room  found
        }

        var toDto = rooms.Select(room =>
                                            new GetRoomDto
                                            {
                                                Guid = room.Guid,
                                                Name = room.Name,
                                                Capacity = room.Capacity,
                                                Floor = room.Floor,
                                            }).ToList();

        return toDto; // room found
    }

    public GetRoomDto? GetRoom(Guid guid)
    {
        var room = _roomRepository.GetByGuid(guid);
        if (room is null)
        {
            return null; // room not found
        }

        var toDto = new GetRoomDto
        {
            Guid = room.Guid,
            Name = room.Name,
            Capacity = room.Capacity,
            Floor = room.Floor,
        };

        return toDto; // rooms found
    }

    public GetRoomDto? CreateRoom(NewRoomDto newRoomDto)
    {
        var room = new Room
        {
            Guid = new Guid(),
            Name = newRoomDto.Name,
            Capacity = newRoomDto.Capacity,
            Floor = newRoomDto.Floor,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdRoom = _roomRepository.Create(room);
        if (createdRoom is null)
        {
            return null; // room not created
        }

        var toDto = new GetRoomDto
        {
            Guid = room.Guid,
            Name = room.Name,
            Capacity = room.Capacity,
            Floor = room.Floor,
        };

        return toDto; // room created
    }

    public int UpdateRoom(UpdateRoomDto updateRoomDto)
    {
        var isExist = _roomRepository.IsExist(updateRoomDto.Guid);
        if (!isExist)
        {
            return -1; // room not found
        }

        var getRole = _roomRepository.GetByGuid(updateRoomDto.Guid);

        var room = new Room
        {
            Guid = updateRoomDto.Guid,
            Name = updateRoomDto.Name,
            Capacity = updateRoomDto.Capacity,
            Floor = updateRoomDto.Floor,
            ModifiedDate = DateTime.Now,
            CreatedDate = getRole!.CreatedDate
        };

        var isUpdate = _roomRepository.Update(room);
        if (!isUpdate)
        {
            return 0; // room not updated
        }

        return 1;
    }

    public int DeleteRoom(Guid guid)
    {
        var isExist = _roomRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // room not found
        }

        var room = _roomRepository.GetByGuid(guid);
        var isDelete = _roomRepository.Delete(room!);
        if (!isDelete)
        {
            return 0; // room not deleted
        }

        return 1;
    }
    public IEnumerable<UnusedRoomDto> GetUnusedRoom()
    {
        var rooms = _roomRepository.GetAll().ToList();

        var usedRooms = from room in _roomRepository.GetAll()
                        join booking in _bookingRepository.GetAll()
                        on room.Guid equals booking.RoomGuid
                        where booking.Status == StatusLevel.OnGoing
                        select new GetRoomDto
                        {
                            Guid = room.Guid,
                            Name = room.Name,
                            Floor = room.Floor,
                            Capacity = room.Capacity,
                        };

        List<Room> tmpRooms = new List<Room>(rooms);

        foreach (var room in rooms)
        {
            foreach (var usedRoom in usedRooms)
            {
                if (room.Guid == usedRoom.Guid)
                {
                    tmpRooms.Remove(room);
                    break;
                }
            }
        }

        var unusedRooms = from room in tmpRooms
                          select new UnusedRoomDto
                          {
                              RoomGuid = room.Guid,
                              RoomName = room.Name,
                              Floor = room.Floor,
                              Capacity = room.Capacity
                          };

        return unusedRooms;
    }

    public IEnumerable<UsedRoomDto>? GetUsedRooms()
    {
        var bookings = _bookingRepository.GetAll();

        if (bookings is null)
        {
            return null;
        }
        var usedRooms = (from booking in bookings
                         join employee in _employeeRepository.GetAll()
                         on booking.EmployeeGuid equals employee.Guid
                         join room in _roomRepository.GetAll()
                         on booking.RoomGuid equals room.Guid
                         where booking.Status == StatusLevel.OnGoing
                         select new UsedRoomDto
                         {
                             BookingGuid = booking.Guid,
                             RoomName = room.Name,
                             Status = booking.Status,
                             Floor = room.Floor,
                             BookedBy = employee.FirstName + " " + employee.LastName,
                         });

        if (usedRooms.Count() == 0)
        {
            return null;
        }

        return usedRooms;
    }
}
