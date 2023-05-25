using API.Models;
using API.ViewModels.Rooms;

namespace API.Contracts;

public interface IRoomRepository : IGeneralRepository<Room>
{

    IEnumerable<MasterRoomVM> GetByDate(DateTime dateTime);
    IEnumerable<RoomUsedVM> GetCurrentlyUsedRooms();


    /*    Room Create(Room room);
        bool Update(Room room);
        bool Delete(Guid guid);
        IEnumerable<Room> GetAll();
        Room? GetByGuid(Guid guid);*/
}
