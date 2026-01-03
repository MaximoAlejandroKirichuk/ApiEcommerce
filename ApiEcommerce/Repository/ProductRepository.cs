using ApiEcommerce.Data;
using ApiEcommerce.Models;
using ApiEcommerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiEcommerce.Repository;

public class ProductRepository: IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context )
    {
       _context = context; 
    }
    
    public IEnumerable<Product> GetProducts()
    {
        return _context.Products.Include(c => c.Category).ToList();
    }

    public IEnumerable<Product> GetProductsByCategory(int categoryId)
    {
        if (categoryId <= 0) return new List<Product> ();
        return _context.Products.Where(p => p.CategoryId == categoryId).Include(p => p.Category).OrderBy(p => p.Name);
    }

    public IEnumerable<Product> SearchProducts(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm)) 
            return new List<Product>();
        
        return _context.Products
            .Include(p => p.Category)
            .Where(p =>
                p.Name.Contains(searchTerm) ||
                (p.Category.Name.Contains(searchTerm)) 
            )
            .OrderBy(p => p.Name);
    }
    public Product? GetProductById(int productId)
    {
        var product = 
            _context.Products.
                Include(p => p.Category).
                FirstOrDefault(p => p.ProductId == productId);
        return product;
    }

    public bool BuyProduct(string productName, int quantity)
    {
        if(productName == string.Empty || quantity <= 0 ) return false;
        var product = _context.Products.FirstOrDefault(p => p.Name.ToLower().Trim() == productName.ToLower().Trim());
        if (product == null || product.Stock < quantity) return false;
        
        product.Stock -= quantity;
        return SaveChanges(); 
    }

    public bool ProductExistsById(int id)
    {
        if (id <= 0) return false;
        var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
        return product != null;
    }

    public bool ProductExistsByName(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        return _context.Products
            .Any(p => p.Name.ToLower().Trim() == name.ToLower().Trim());

    }

    public bool CreateProduct(Product product)
    {
        if(product == null) throw new Exception("Product empty");
        if (_context.Products.Any(p => p.ProductId == product.ProductId)) throw new Exception("Product already exists");
        _context.Products.Add(product);
        return SaveChanges();
    }

    public bool UpdateProduct(Product product)
    {
        if(product == null) throw new Exception("Product empty");
        if (!_context.Products.Any(p => p.ProductId == product.ProductId)) throw new Exception("Product already exists");
        _context.Products.Update(product);
        return SaveChanges();
    }

    public bool DeleteProduct(Product product)
    {
        if(product == null) return false;
        if (! _context.Products.Any(p => p.ProductId == product.ProductId)) return false;
        _context.Products.Remove(product);
        return SaveChanges();
    }

    public bool SaveChanges()
    {
       return _context.SaveChanges() > 0;
    }
}