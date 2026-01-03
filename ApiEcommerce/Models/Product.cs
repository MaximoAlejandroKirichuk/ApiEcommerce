using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiEcommerce.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    [Required] public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    [Range(0, double.MaxValue)] public int Price { get; set; }
    public string ImgUrl { get; set; }
    [Required] public string SKU { get; set; }
    [Range(0,int.MaxValue)] public int Stock { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}