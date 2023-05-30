using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.ViewModels.Rooms
{
    public class RoomVM
    {
        public Guid? Guid { get; set; }
        public string Name { get; set; }

        [Range(0, 18, ErrorMessage = "Max Floor is 12")]
        public int Floor { get; set; }

        [Range(0, 100, ErrorMessage = "Max Capacity is 100 Person")]
        public int Capacity { get; set; }
    }
}
