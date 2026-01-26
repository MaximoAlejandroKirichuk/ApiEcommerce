using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.Dtos.User;

public class UserLoginDto
{
    [Required(ErrorMessage = "The username is required")]
    public string? Username { get; set; }
    [Required(ErrorMessage = "The password is required")]
    public string? Password { get; set; }
}