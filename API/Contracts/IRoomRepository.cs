using API.Models;
using API.ViewModels.Rooms;
using RestAPI.ViewModels.Rooms;

namespace API.Contracts;

public interface IRoomRepository : IGeneralRepository<Room>
{

    IEnumerable<MasterRoomVM> GetByDate(DateTime dateTime);
    IEnumerable<RoomUsedVM> GetCurrentlyUsedRooms();
    IEnumerable<RoomBookedTodayVM> GetAvailableRoom();


    /*    Room Create(Room room);
        bool Update(Room room);
        bool Delete(Guid guid);
        IEnumerable<Room> GetAll();
        Room? GetByGuid(Guid guid);*/
}
