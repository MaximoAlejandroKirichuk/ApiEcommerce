using ApiEcommerce.Models.Dtos.User;
using ApiEcommerce.Models;

namespace ApiEcommerce.Mapping;

public static class UserMapping
{
    //User -> DTO
    public static UserDto MapToUserDto(this User user)
    {
        return new UserDto()
        {
            Username = user.Username,
            Name = user.Name,
            Role = user.Role,
            Id = user.Id,
            Password = user.Password,
        };
    }
    // User <-> CreateUserDto
    public static User ToEntity(this CreateUserDto dto)
    {

        return new User
        {
            Username = dto.Username,
            Name = dto.Name,
            Password = dto.Password,
            Role = dto.Role
        };
    }

    public static CreateUserDto ToCreateUserDto(this User user)
    {
        if (user is null) return null;

        return new CreateUserDto
        {
            Username = user.Username,
            Name = user.Name,
            Password = user.Password,
            Role = user.Role
        };
    }


    // User <-> UserLoginDto

    public static User ToEntity(this UserLoginDto dto)
    {
        if (dto is null) return null;

        return new User
        {
            Username = dto.Username,
            Password = dto.Password
        };
    }

    public static UserLoginDto ToUserLoginDto(this User user)
    {
        if (user is null) return null;

        return new UserLoginDto
        {
            Username = user.Username,
            Password = user.Password
        };
    }


    // User <-> UserRegisterDto

    public static UserRegisterDto ToUserRegisterDto(this User user)
    {
        if (user is null) return null;

        return new UserRegisterDto
        {
            Id = user.Id,
            Name = user.Name,
            Username = user.Username!,
            Password = user.Password!,
            Role = user.Role
        };
    }

    public static User ToEntity(this UserRegisterDto dto)
    {
        if (dto is null) return null;

        return new User
        {
            Id = dto.Id,
            Name = dto.Name,
            Username = dto.Username,
            Password = dto.Password,
            Role = dto.Role
        };
    }


    // User -> UserLoginResponseDto

    public static UserLoginResponseDto ToUserLoginResponseDto(
        this User user,
        string token,
        string? message = null)
    {
        if (user is null) return null;

        return new UserLoginResponseDto
        {
            User = user.ToUserRegisterDto(),
            Token = token,
            Message = message
        };
    }
}
