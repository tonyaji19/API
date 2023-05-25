using API.Contexts;
using API.Contracts;
using API.Models;
using API.ViewModels.Bookings;
using API.ViewModels.Rooms;
using RestAPI.ViewModels.Rooms;

namespace API.Repositories;

public class RoomRepository : GeneralRepository<Room>, IRoomRepository
{

    public RoomRepository(BookingManagementDbContext context) : base(context)
    {
        
    }
    
    public IEnumerable<MasterRoomVM> GetByDate(DateTime dateTime)
    {
        var rooms = GetAll();
        var bookings = _context.Bookings.ToList();
        var employees = _context.Employees.ToList();

        var usedRooms = new List<MasterRoomVM>();

        foreach (var room in rooms)
        {
            var booking = bookings.FirstOrDefault(b => b.RoomGuid == room?.Guid && b.StartDate <= dateTime && b.EndDate >= dateTime);
            if (booking != null)
            {
                var employee = employees.FirstOrDefault(e => e.Guid == booking.EmployeeGuid);

                var result = new MasterRoomVM
                {
                    BookedBy = employee.FirstName + " " + employee?.LastName,
                    Status = booking.Status.ToString(),
                    RoomName = room.Name,
                    Floor = room.Floor,
                    Capacity = room.Capacity,
                    StartDate = booking.StartDate,
                    EndDate = booking.EndDate,

                };

                usedRooms.Add(result);
            }
        }

        return usedRooms;
    }

    public IEnumerable<RoomUsedVM> GetCurrentlyUsedRooms()
    {
        var rooms = GetAll();
        var bookings = _context.Bookings.ToList();
        var employees = _context.Employees.ToList();

        var usedRooms = new List<RoomUsedVM>();

        var currentTime = DateTime.Now;

        foreach (var room in rooms)
        {
            var booking = bookings.FirstOrDefault(b => b.RoomGuid == room?.Guid && b.StartDate <= currentTime && b.EndDate >= currentTime);
            if (booking != null)
            {
                var employee = employees.FirstOrDefault(e => e.Guid == booking.EmployeeGuid);

                var result = new RoomUsedVM
                {
                    RoomName = room.Name,
                    Status = booking.Status.ToString(),
                    Floor = room.Floor,
                    BookedBy = employee.FirstName + " " + employee?.LastName,
                };

                usedRooms.Add(result);
            }
        }
        return usedRooms;
    }
    public IEnumerable<RoomBookedTodayVM> GetAvailableRoom()
    {
        try
        {
            //get all data from booking and rooms
            var booking = _context.Bookings.ToList();
            var rooms = GetAll();

            var startToday = DateTime.Today;
            var endToday = DateTime.Today.AddHours(23).AddMinutes(59);

            var roomUse = rooms.Join(booking, Room => Room.Guid, booking => booking.RoomGuid, (Room, booking) => new { Room, booking })
                    .Select(joinResult => new {
                        joinResult.Room.Name,
                        joinResult.Room.Floor,
                        joinResult.Room.Capacity,
                        joinResult.booking.StartDate,
                        joinResult.booking.EndDate
                    }
             );

            var roomUseTodays = new List<RoomBookedTodayVM>();


            foreach (var room in roomUse)
            {
                if ((room.StartDate < startToday && room.EndDate < startToday) || (room.StartDate > startToday && room.EndDate > endToday))
                {
                    var roomDay = new RoomBookedTodayVM
                    {
                        RoomName = room.Name,
                        Floor = room.Floor,
                        Capacity = room.Capacity
                    };
                    roomUseTodays.Add(roomDay);
                }
            }
            return roomUseTodays;
        }

        catch
        {
            return null;

        }
    }
    //k1
 
    /*private readonly BookingManagementDbContext _context;
    public RoomRepository(BookingManagementDbContext context)
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

    *//*
     * <summary>
     * Update a university
     * </summary>
     * <param name="university">University object</param>
     * <returns>true if data updated</returns>
     * <returns>false if data not updated</returns>
     *//*
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

    *//*
     * <summary>
     * Get all universities
     * </summary>
     * <returns>List of universities</returns>
     * <returns>Empty list if no data found</returns>
     *//*
    public IEnumerable<Room> GetAll()
    {
        return _context.Set<Room>().ToList();
    }

    *//*
     * <summary>
     * Get a university by guid
     * </summary>
     * <param name="guid">University guid</param>
     * <returns>University object</returns>
     * <returns>null if no data found</returns>
     *//*
    public Room? GetByGuid(Guid guid)
    {
        return _context.Set<Room>().Find(guid);
    }*/
}
