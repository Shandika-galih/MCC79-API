using API.DTOs.Booking;
using API.Models;

namespace API.Contracts;

public interface IBookingRepository : IGeneralRepository<Booking>
{
    IEnumerable<BookingDetailDto> GetBookingDetails();
}
