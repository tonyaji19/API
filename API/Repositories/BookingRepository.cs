
using API.Contexts;
using API.Contracts;
using API.Models;
using API.ViewModels.Bookings;

namespace API.Repositories;

public class BookingRepository : GeneralRepository<Booking>, IBookingRepository
{
    private readonly IRoomRepository _roomRepository;
    public BookingRepository(BookingManagementDbContext context, IRoomRepository roomRepository) : base(context)
    {
        _roomRepository = roomRepository;
    }

    private int CalculateBookingDuration(DateTime startDate, DateTime endDate)
    {
        int totalDays = 0; // Untuk menghitung hari
        DateTime currentDate = startDate.Date; // Perhitungan tergantung dari tanggal yang digunakan

        while (currentDate <= endDate.Date)
        {
            // Mengecek apakah currentDate adalah hari kerja (Selain sabtu dan minggu) 
            if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
            {
                // Jika currentDate hari kerja, totalDays bertambah
                totalDays++;
            }
            currentDate = currentDate.AddDays(1); // untuk maju ke tanggal berikutnya.
        }

        return totalDays;
    }

    public IEnumerable<BookingDurationVM> GetBookingDuration()
    {
        /* Menampung semua data room di var rooms*/
        var rooms = _roomRepository.GetAll();

        /* Mengambil semua data booking*/
        var bookings = GetAll();

        /* Melakukan instance/membuat object untuk setiap pemesanan yang memenuhi kondisi diatas..
           Pada part ini, value RoomName akan diisi dengan nama Room yang dicari berdasarkan RoomGuid. */
        var bookingduration = bookings.Select(b => new BookingDurationVM
        {
            RoomName = rooms.FirstOrDefault(r => r.Guid == b.RoomGuid)?.Name, // Di set menjadi (?) untuk memastikan tidak terjadi kesalahan ketika objek tidak ditemukan, sehingga valuenya akan otomatis NULL
            DurationOfBooking = CalculateBookingDuration(b.StartDate, b.EndDate)
        });

        return bookingduration;
    }


}
    /*private readonly BookingManagementDbContext _context;
    public BookingRepository(BookingManagementDbContext context)
    {
        _context = context;
    }

    *//*
     * <summary>
     * Create a new university
     * </summary>
     * <param name="university">University object</param>
     * <returns>University object</returns>
     *//*
    public Booking Create(Booking booking)
    {
        try
        {
            _context.Set<Booking>().Add(booking);
            _context.SaveChanges();
            return booking;
        }
        catch
        {
            return new Booking();
        }
    }

    *//*
     * <summary>
     * Update a university
     * </summary>
     * <param name="university">University object</param>
     * <returns>true if data updated</returns>
     * <returns>false if data not updated</returns>
     *//*
    public bool Update(Booking booking)
    {
        try
        {
            _context.Set<Booking>().Update(booking);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    *//*
     * <summary>
     * Delete a university
     * </summary>
     * <param name="guid">University guid</param>
     * <returns>true if data deleted</returns>
     * <returns>false if data not deleted</returns>
     *//*
    public bool Delete(Guid guid)
    {
        try
        {
            var booking = GetByGuid(guid);
            if (booking == null)
            {
                return false;
            }

            _context.Set<Booking>().Remove(booking);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    *//*
     * <summary>
     * Get all universities
     * </summary>
     * <returns>List of universities</returns>
     * <returns>Empty list if no data found</returns>
     *//*
    public IEnumerable<Booking> GetAll()
    {
        return _context.Set<Booking>().ToList();
    }

    *//*
     * <summary>
     * Get a university by guid
     * </summary>
     * <param name="guid">University guid</param>
     * <returns>University object</returns>
     * <returns>null if no data found</returns>
     *//*
    public Booking? GetByGuid(Guid guid)
    {
        return _context.Set<Booking>().Find(guid);
    }*/
