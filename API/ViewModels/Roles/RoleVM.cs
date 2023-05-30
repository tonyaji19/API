using System.ComponentModel.DataAnnotations;

namespace API.ViewModels.Roles
{
    public class RoleVM
    {
        public Guid? Guid { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public string Name { get; set; }

    }
}
