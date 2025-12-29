using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;

namespace ApiEcommerce.Mapping;

public static class CategoryMapping
{
    // Adapter to convert CreateCategoryDTO to an Entity (Write)
    public static CategoryDTO ToDto(this Category category)
    {
        if (category == null) return null!;

        return new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            CreationDate = category.CreationDate
        };
    }

    // Adapter to convert CreateCategoryDTO to an Entity (Write)
    public static Category ToEntity(this CreateCategoryDTO dto)
    {
        // Fix 1: Explicit null check
        if (dto == null) 
        {
            throw new ArgumentNullException(nameof(dto));
        }

        return new Category
        {
            Name = dto.Name,
            // The ID is not set here; the Database/EF will handle it.
            CreationDate = DateTime.UtcNow 
        };
    }
}