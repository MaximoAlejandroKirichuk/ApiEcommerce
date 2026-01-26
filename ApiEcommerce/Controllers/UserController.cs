using ApiEcommerce.Mapping;
using ApiEcommerce.Models.Dtos.User;
using ApiEcommerce.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();
            if(users.Count == 0) return NotFound("User not found");
            var usersDto = new List<UserDto>();
            foreach (var user in users)
            {
                var userDto =user.MapToUserDto();
                usersDto.Add(userDto);
            }
            return Ok(usersDto);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetUser(int id)
        {
            if(id <= 0) return BadRequest("Invalid id");
            var user = _userRepository.GetUserById(id);
            if(user == null) return NotFound("User not found");
            var userDto = user.MapToUserDto();
            return Ok(userDto);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        { 
            if(!ModelState.IsValid || createUserDto == null) return BadRequest(ModelState);
            if(string.IsNullOrEmpty(createUserDto.Username)) return BadRequest("Username is required");
            var uniqueUsername = _userRepository.IsUniqueUsername(createUserDto.Username);
            if(!uniqueUsername) return NotFound("User already exist with that username");

            var result = await _userRepository.Register(createUserDto);
            if(result==null) return StatusCode(500,"User creation failed");
            return Ok(result.MapToUserDto());
        }
        [AllowAnonymous]
        [HttpPost("Login", Name = "LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto  userLoginDto)
        { 
            if(!ModelState.IsValid) return BadRequest(ModelState); ;
            
            var userLoginResponse = await _userRepository.Login(userLoginDto);
            if(userLoginResponse==null) return Unauthorized();
            return Ok(userLoginResponse);
        }
    }
}
