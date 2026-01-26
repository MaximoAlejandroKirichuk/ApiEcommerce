using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos.User;

namespace ApiEcommerce.Repository.IRepository;

public interface IUserRepository
{
    ICollection<User> GetUsers();
    User? GetUserById(int id);
    User? GetUserByUsername(string username);

    bool IsUniqueUsername(string username);
    Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
    Task<User> Register(CreateUserDto createUserDto);
}