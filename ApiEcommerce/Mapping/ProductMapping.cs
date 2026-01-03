using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos.Product;

namespace ApiEcommerce.Mapping;

public static class ProductMapping
{
    // Adapter to convert CreateProduct to an Entity (Write) without id
    public static Product ToEntity(this CreateProductDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ImgUrl = dto.ImgUrl,
            SKU = dto.SKU,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
        };
    }
    // Adapter to convert Entity to an ProductDto (read)
    public static ProductDto ToDto(this Product product)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));

        return new ProductDto
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            DateCreated = product.DateCreated,
            DateModified = product.DateModified,
            Stock = product.Stock,
            CategoryName = product.Category.Name
        };
    }

    
    // Adapter to convert UpdateProductDto to an Entity (Write)
    public static void UpdateEntity(this UpdateProductDto dto, Product product)
    {
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.ImgUrl = dto.ImgUrl;
        product.SKU = dto.SKU;
        product.Stock = dto.Stock;
        product.CategoryId = dto.CategoryId;
        product.DateModified = DateTime.UtcNow;
    }
    
     
}