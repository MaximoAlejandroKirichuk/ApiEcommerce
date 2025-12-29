using ApiEcommerce.Data;
using ApiEcommerce.Models;
using ApiEcommerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiEcommerce.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Category> _dbSet;
    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Category>();
    }
    public async Task<IEnumerable<Category>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<Category?> GetById(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public bool CategoryExists(int id)
    {
        var result = _dbSet.Any(c => c.Id == id);
      
        return result;
    }

    public async Task Create(Category category)
    { 
        await _dbSet.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public bool Update(Category category)
    { 
        _dbSet.Update(category);
        return _context.SaveChanges() > 0;
    }

    public bool Delete(Category category)
    {
        _dbSet.Remove(category);
        return _context.SaveChanges() > 0;
    }

    public bool CategoryExistsByName(string name)
    {
        var result = _dbSet.Any(c => c.Name == name);
        return result;
    }

    
}