
using API.Contexts;
using API.Contracts;
using API.Models;
using API.ViewModels.Bookings;
using RestAPI.ViewModels.Bookings;

namespace API.Repositories;

public class BookingRepository : GeneralRepository<Booking>, IBookingRepository
{
    private readonly IRoomRepository _roomRepository;
    public BookingRepository(BookingManagementDbContext context, IRoomRepository roomRepository) : base(context)
    {
        _roomRepository = roomRepository;
    }
    public IEnumerable<BookingDetailVM> GetAllBookingDetail()
    {

        var bookings = GetAll();
        var employees = _context.Employees.ToList();
        var rooms = _context.Rooms.ToList();

        var BookingDetails = from b in bookings
                             join e in employees on b.EmployeeGuid equals e.Guid
                             join r in rooms on b.RoomGuid equals r.Guid
                             select new
                             {
                                 b.Guid,
                                 e.Nik,
                                 BookedBy = e.FirstName + "" + e.LastName,
                                 r.Name,
                                 b.StartDate,
                                 b.EndDate,
                                 b.Status,
                                 b.Remarks
                             };
        var BookingDetailConverteds = new List<BookingDetailVM>();
        foreach (var dataBookingDetail in BookingDetails)
        {
            var newBookingDetail = new BookingDetailVM
            {
                Guid = dataBookingDetail.Guid,
                StartDate = dataBookingDetail.StartDate,
                EndDate = dataBookingDetail.EndDate,
                Status = dataBookingDetail.Status,
                Remarks = dataBookingDetail.Remarks,
                BookedNIK = dataBookingDetail.Nik,
                Fullname = dataBookingDetail.BookedBy,
                RoomName = dataBookingDetail.Name
            };
            BookingDetailConverteds.Add(newBookingDetail);
        }

        return BookingDetailConverteds;
    }

    public BookingDetailVM GetBookingDetailByGuid(Guid guid)
    {
        var booking = GetByGuid(guid);
        var employee = _context.Employees.Find(booking.EmployeeGuid);
        var room = _context.Rooms.Find(booking.RoomGuid);
        var bookingDetail = new BookingDetailVM
        {
            Guid = booking.Guid,
            BookedNIK = employee.Nik,
            Fullname = employee.FirstName + " " + employee.LastName,
            RoomName = room.Name,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Status = booking.Status,
            Remarks = booking.Remarks,

        };
        return bookingDetail;
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
