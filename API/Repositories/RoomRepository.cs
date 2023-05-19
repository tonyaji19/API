using API.Contexts;
using API.Contracts;
using API.Models;

namespace API.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly BookingManagementDbContext _context;
    public RoomRepository(BookingManagementDbContext context)
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
    public Room Create(Room room)
    {
        try
        {
            _context.Set<Room>().Add(room);
            _context.SaveChanges();
            return room;
        }
        catch
        {
            return new Room();
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
    public bool Update(Room room)
    {
        try
        {
            _context.Set<Room>().Update(room);
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
            var room = GetByGuid(guid);
            if (room == null)
            {
                return false;
            }

            _context.Set<Room>().Remove(room);
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
    public IEnumerable<Room> GetAll()
    {
        return _context.Set<Room>().ToList();
    }

    /*
     * <summary>
     * Get a university by guid
     * </summary>
     * <param name="guid">University guid</param>
     * <returns>University object</returns>
     * <returns>null if no data found</returns>
     */
    public Room? GetByGuid(Guid guid)
    {
        return _context.Set<Room>().Find(guid);
    }
}
