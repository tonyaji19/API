using System.ComponentModel.DataAnnotations;

namespace RestAPI.ViewModels.Rooms
{
    public class RoomBookedTodayVM
    {
        public string RoomName { get; set; }
        public int Floor { get; set; }
        [Range(0, 100, ErrorMessage = "Max Capacity is 100 Person")]
        public int Capacity { get; set; }

    }
}