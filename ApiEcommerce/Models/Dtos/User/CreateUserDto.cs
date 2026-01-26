using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.Dtos.User;

public class CreateUserDto
{
    [Required(ErrorMessage = "The username is required")]
    public string? Username { get; set; }
    [Required(ErrorMessage = "The name is required")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "The password is required")]
    public string? Password { get; set; }
    [Required(ErrorMessage = "The role is required")]
    public string? Role { get; set; }
}