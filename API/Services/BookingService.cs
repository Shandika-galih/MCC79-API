using API.Contracts;
using API.DTOs.AccountRole;
using API.DTOs.Booking;
using API.Models;
using API.Repositories;

namespace API.Services;

public class BookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRoomRepository _roomRepository;

    public BookingService(IBookingRepository bookingRepository, IEmployeeRepository employeeRepository, IRoomRepository roomRepository)
    {
        _bookingRepository = bookingRepository;
        _employeeRepository = employeeRepository;
        _roomRepository = roomRepository;
    }

    public IEnumerable<GetBookingDto>? GetBooking()
    {
        var bookings = _bookingRepository.GetAll();
        if (!bookings.Any())
        {
            return null; // No Booking  found
        }

        var toDto = bookings.Select(booking =>
                                            new GetBookingDto
                                            {
                                                Guid = booking.Guid,
                                                StartDate = booking.StartDate,
                                                EndDate = booking.EndDate,
                                                Status = booking.Status,
                                                Remarks = booking.Remarks,
                                                RoomGuid = booking.RoomGuid,
                                                EmployeeGuid = booking.EmployeeGuid
                                            }).ToList();

        return toDto; // Booking found
    }

    public GetBookingDto? GetBooking(Guid guid)
    {
        var booking = _bookingRepository.GetByGuid(guid);
        if (booking is null)
        {
            return null; // booking not found
        }

        var toDto = new GetBookingDto
        {
            Guid = booking.Guid,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Status = booking.Status,
            Remarks = booking.Remarks,
            RoomGuid = booking.RoomGuid,
            EmployeeGuid = booking.EmployeeGuid
        };

        return toDto; // bookings found
    }

    public GetBookingDto? CreateBooking(NewBookingDto newBookingDto)
    {
        var booking = new Booking
        {
            Guid = new Guid(),
            StartDate = newBookingDto.StartDate,
            EndDate = newBookingDto.EndDate,
            Status = newBookingDto.Status,
            Remarks = newBookingDto.Remarks,
            RoomGuid = newBookingDto.RoomGuid,
            EmployeeGuid = newBookingDto.EmployeeGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdBooking = _bookingRepository.Create(booking);
        if (createdBooking is null)
        {
            return null; // Booking not created
        }

        var toDto = new GetBookingDto
        {
            Guid = createdBooking.Guid,
            StartDate = newBookingDto.StartDate,
            EndDate = newBookingDto.EndDate,
            Status = newBookingDto.Status,
            Remarks = newBookingDto.Remarks,
            RoomGuid = newBookingDto.RoomGuid,
            EmployeeGuid = newBookingDto.EmployeeGuid,
        };

        return toDto; // Booking created
    }

    public int UpdateBooking(UpdateBookingDto updateBookingDto)
    {
        var isExist = _bookingRepository.IsExist(updateBookingDto.Guid);
        if (!isExist)
        {
            return -1; // Booking not found
        }

        var getBooking = _bookingRepository.GetByGuid(updateBookingDto.Guid);

        var booking = new Booking
        {
            Guid = updateBookingDto.Guid,
            StartDate = updateBookingDto.StartDate,
            EndDate = updateBookingDto.EndDate,
            Status = updateBookingDto.Status,
            Remarks = updateBookingDto.Remarks,
            RoomGuid = updateBookingDto.RoomGuid,
            EmployeeGuid = updateBookingDto.EmployeeGuid,
            ModifiedDate = DateTime.Now,
            CreatedDate = getBooking!.CreatedDate
        };

        var isUpdate = _bookingRepository.Update(booking);
        if (!isUpdate)
        {
            return 0; // Booking not updated
        }

        return 1;
    }

    public int DeleteBooking(Guid guid)
    {
        var isExist = _bookingRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // Booking not found
        }

        var booking = _bookingRepository.GetByGuid(guid);
        var isDelete = _bookingRepository.Delete(booking!);
        if (!isDelete)
        {
            return 0; // Booking not deleted
        }

        return 1;
    }
    public IEnumerable<GetRoomTodayDto> GetRoomToday()
    {
        var room = (from r in _roomRepository.GetAll()
                    join b in _bookingRepository.GetAll() on r.Guid equals b.RoomGuid
                    join e in _employeeRepository.GetAll() on b.EmployeeGuid equals e.Guid
                    where b.StartDate <= DateTime.Now && b.EndDate >= DateTime.Now
                    select new GetRoomTodayDto
                    {
                        BookingGuid = b.Guid,
                        RoomName = r.Name,
                        Status = b.Status,
                        Floor = r.Floor,
                        BookedBy = e.FirstName + e.LastName
                    }).ToList();

        if (!room.Any())
        {
            return null;
        }
        var toDto = room.Select(r => new GetRoomTodayDto
        {
            BookingGuid = r.BookingGuid,
            RoomName = r.RoomName,
            Status = r.Status,
            Floor = r.Floor,
            BookedBy = r.BookedBy
        });
        return toDto;
    }

    public List<BookingDetailDto>? GetBookingDetails()
    {
        var bookings = _bookingRepository.GetBookingDetails();
        var bookingDetails = bookings.Select(b => new BookingDetailDto
        {
            Guid = b.Guid,
            BookedNIK = b.BookedNIK,
            BookedBy = b.BookedBy,
            RoomName = b.RoomName,
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            Status = b.Status,
            Remarks = b.Remarks
        }).ToList();

        return bookingDetails;
    }

    public BookingDetailDto? GetBookingDetailByGuid(Guid guid)
    {
        var relatedBooking = GetBookingDetails().FirstOrDefault(b => b.Guid == guid);
        return relatedBooking;
    }

    public IEnumerable<GetBookingDurationDto> BookingDuration()
    {
        var bookings = _bookingRepository.GetAll();
        var rooms = _roomRepository.GetAll();

        var entities = (from booking in bookings
                        join room in rooms on booking.RoomGuid equals room.Guid
                        select new
                        {
                            guid = room.Guid,
                            startDate = booking.StartDate,
                            endDate = booking.EndDate,
                            roomName = room.Name
                        }).ToList();

        var listBookingDurations = new List<GetBookingDurationDto>();

        foreach (var entity in entities)
        {
            TimeSpan duration = entity.endDate - entity.startDate;

            // Count the number of weekends within the duration
            int totalDays = (int)duration.TotalDays;
            int weekends = 0;

            for (int i = 0; i <= totalDays; i++)
            {
                var currentDate = entity.startDate.AddDays(i);
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekends++;
                }
            }

            // Calculate the duration without weekends
            TimeSpan bookingLength = duration - TimeSpan.FromDays(weekends);

            var bookingDurationDto = new GetBookingDurationDto
            {
                RoomGuid = entity.guid,
                RoomName = entity.roomName,
                BookingLength = $"{bookingLength.Days} Hari"
            };

            listBookingDurations.Add(bookingDurationDto);
        }

        return listBookingDurations;
    }


}
