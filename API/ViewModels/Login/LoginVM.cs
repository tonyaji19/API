using API.Utility;
using System.ComponentModel.DataAnnotations;

namespace API.ViewModels.Login;

public class LoginVM
{
    [EmailAddress]
    public string Email { get; set; }
    
    public string Password { get; set; }
}