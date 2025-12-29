using ApiEcommerce.Models;

namespace ApiEcommerce.Repository.IRepository;

public interface ICategoryRepository
{
    Task <IEnumerable<Category>>  GetAll();
    Task<Category?> GetById(int id);
    bool CategoryExists(int id);
    bool CategoryExistsByName(string name);
    Task Create(Category category);
    bool Update(Category category);
    bool Delete(Category category);
    
}