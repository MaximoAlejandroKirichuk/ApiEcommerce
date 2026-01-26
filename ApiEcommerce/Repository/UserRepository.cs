using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiEcommerce.Data;
using ApiEcommerce.Mapping;
using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos.User;
using ApiEcommerce.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;

namespace ApiEcommerce.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;
    private string? _secretKey;

    public UserRepository(ApplicationDbContext dbContext, IConfiguration config)
    {
        _db = dbContext;
        _secretKey = config.GetValue<string>("ApiSettings:SecretKey");
    }

    public ICollection<User> GetUsers()
    {
        return _db.Users.OrderBy(u => u.Username).ToList();
    }

    public User? GetUserById(int id)
    {
        return _db.Users.FirstOrDefault(u => u.Id == id);
    }

    public User? GetUserByUsername(string username)
    {
        return _db.Users.FirstOrDefault(u => u.Username == username);
    }

    public bool IsUniqueUsername(string username)
    {
        var usernameLowerAndTrim = username.ToLower().Trim();
        var uniqueUsername = !_db.Users.Any(u => u.Username.ToLower().Trim() == usernameLowerAndTrim);
        return uniqueUsername;
    }

    public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
    {
        ValidateLoginDto(userLoginDto);
        var normalizedUsername =
            NormalizeUsername(userLoginDto.Username!); // ! because I know userlogindto is not null in this point
        var user = GetUserByUsername(normalizedUsername);
        if (user is null)
            return LoginFailed("User not found");

        if (!VerifyPassword(userLoginDto.Password!, user.Password))
            return LoginFailed("Credentials are incorrect");


        var token = GenerateJwtToken(user);

        return LoginSuccess(user, token);
    }


    private void ValidateLoginDto(UserLoginDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        if (string.IsNullOrWhiteSpace(dto.Username))
            throw new ArgumentException("Username is required");

        if (string.IsNullOrWhiteSpace(dto.Password))
            throw new ArgumentException("Password is required");
    }

    private string NormalizeUsername(string username)
    {
        return username.ToLower().Trim();
    }

    private static UserLoginResponseDto LoginFailed(string message)
    {
        return new UserLoginResponseDto
        {
            Token = string.Empty,
            Message = message,
            User = null
        };
    }

    private static bool VerifyPassword(string inputPassword, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
    }

    private string GenerateJwtToken(User user)
    {
        if (string.IsNullOrWhiteSpace(_secretKey)) throw new InvalidOperationException("Secret Key is empty or null");
        var handleToken = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes((_secretKey));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("username", user.Username),
                new Claim(ClaimTypes.Role, user.Role ?? string.Empty),
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = handleToken.CreateToken(tokenDescriptor);
        return handleToken.WriteToken(token);
    }

    private UserLoginResponseDto LoginSuccess(User user, string token)
    {
        return new UserLoginResponseDto
        {
            Token = token,
            Message = "User login successfully",
            User = user.ToUserRegisterDto() // mapping 
        };
    }

    public async Task<User> Register(CreateUserDto createUserDto)
    {
        var encriptedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
        var user = new User()
        {
            Username = createUserDto.Username ?? "No username",
            Password = encriptedPassword,
            Role = createUserDto.Role,
            Name = createUserDto.Name,
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }
}