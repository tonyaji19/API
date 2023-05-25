using API.Models;
using API.ViewModels.Bookings;

namespace API.Contracts;

public interface IBookingRepository : IGeneralRepository<Booking>
{
    IEnumerable<BookingDurationVM> GetBookingDuration();
    /*    Booking Create(Booking booking);
        bool Update(Booking booking);
        bool Delete(Guid guid);
        IEnumerable<Booking> GetAll();
        Booking? GetByGuid(Guid guid);*/
}