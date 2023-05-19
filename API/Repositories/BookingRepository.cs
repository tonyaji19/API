
using API.Contexts;
using API.Contracts;
using API.Models;

namespace API.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly BookingManagementDbContext _context;
    public BookingRepository(BookingManagementDbContext context)
    {
        _context = context;
    }

    /*
     * <summary>
     * Create a new university
     * </summary>
     * <param name="university">University object</param>
     * <returns>University object</returns>
     */
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

    /*
     * <summary>
     * Update a university
     * </summary>
     * <param name="university">University object</param>
     * <returns>true if data updated</returns>
     * <returns>false if data not updated</returns>
     */
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

    /*
     * <summary>
     * Delete a university
     * </summary>
     * <param name="guid">University guid</param>
     * <returns>true if data deleted</returns>
     * <returns>false if data not deleted</returns>
     */
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

    /*
     * <summary>
     * Get all universities
     * </summary>
     * <returns>List of universities</returns>
     * <returns>Empty list if no data found</returns>
     */
    public IEnumerable<Booking> GetAll()
    {
        return _context.Set<Booking>().ToList();
    }

    /*
     * <summary>
     * Get a university by guid
     * </summary>
     * <param name="guid">University guid</param>
     * <returns>University object</returns>
     * <returns>null if no data found</returns>
     */
    public Booking? GetByGuid(Guid guid)
    {
        return _context.Set<Booking>().Find(guid);
    }
}