namespace ApiEcommerce.Models.Dtos.Product;

public class CreateProductDto
{
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Price { get; set; }
    public string ImgUrl { get; set; }
    public string SKU { get; set; }
    public int Stock { get; set; }

    public DateTime DateModified { get; set; }
    public int CategoryId { get; set; }
    
}