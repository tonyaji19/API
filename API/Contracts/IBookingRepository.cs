using API.Models;
using API.ViewModels.Bookings;
using RestAPI.ViewModels.Bookings;

namespace API.Contracts;

public interface IBookingRepository : IGeneralRepository<Booking>
{
    IEnumerable<BookingDurationVM> GetBookingDuration();
    BookingDetailVM GetBookingDetailByGuid(Guid guid);
    IEnumerable<BookingDetailVM> GetAllBookingDetail();

    /*    Booking Create(Booking booking);
        bool Update(Booking booking);
        bool Delete(Guid guid);
        IEnumerable<Booking> GetAll();
        Booking? GetByGuid(Guid guid);*/
}