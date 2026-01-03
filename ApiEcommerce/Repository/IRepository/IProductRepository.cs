using ApiEcommerce.Models;

namespace ApiEcommerce.Repository.IRepository;

public interface IProductRepository
{
    public IEnumerable<Product> GetProducts(); 
    public IEnumerable<Product> GetProductsByCategory(int categoryId);
    public IEnumerable<Product> SearchProducts(string productName);
    public Product? GetProductById(int productId);
    public bool BuyProduct(string productName, int quantity);
    public bool ProductExistsById(int id);
    public bool ProductExistsByName(string name);
    public bool CreateProduct(Product product);
    public bool UpdateProduct(Product product);
    public bool DeleteProduct(Product product);
    public bool SaveChanges();
}