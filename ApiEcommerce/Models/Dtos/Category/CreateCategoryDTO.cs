using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.Dtos;

public class CreateCategoryDTO
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
    [MinLength(3, ErrorMessage = "Name cannot be smaller than 3 characters")]
    public string Name { get; set; } = string.Empty;
}