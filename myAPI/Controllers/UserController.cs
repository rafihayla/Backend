using BusinessLogicLayer;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using myAPI.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace myAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class UserController(IUserService userService, IConfiguration config) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IConfiguration _config = config;

        // POST api/account/register
        [HttpPost("register"), AllowAnonymous]
        public async Task<ActionResult<UserDto>> Register([FromBody] UserRegisterModel value)
        {
            // email validation
            if (value.Email == null)
            {
                return BadRequest("Email is required");
            }
            else if (value.Email.Contains('@') == false)
            {
                return BadRequest("Email is invalid");
            }
            else if (value.Email.Contains('.') == false)
            {
                return BadRequest("Email is invalid");
            }
            else if (value.Email.Length < 5)
            {
                return BadRequest("Email is invalid");
            }

            // password validation
            if (value.Password == null)
            {
                return BadRequest("Password is required");
            }
            else if (value.Password.Length < 8)
            {
                return BadRequest("Password must be at least 8 characters long");
            }
            //check if password doesn't contain at least one uppercase letter
            else if (value.Password.Any(char.IsUpper) == false)
            {
                return BadRequest("Password must contain at least one uppercase letter");
            }
            //check if password doesn't contain at least one lowercase letter
            else if (value.Password.Any(char.IsLower) == false)
            {
                return BadRequest("Password must contain at least one lowercase letter");
            }
            //check if password doesn't contain at least one digit
            else if (value.Password.Any(char.IsDigit) == false)
            {
                return BadRequest("Password must contain at least one digit");
            }

            var user = new UserDto
            {
                FullName = value.FullName,
                Password = value.Password,
                Email = value.Email,
                Address = value.Address,
                BirthDate = value.BirthDate,
                Gender = value.Gender,
                PhoneNumber = value.PhoneNumber,
            };
            var (status, message) = await _userService.AddUserAsync(user);

            if (status == StatusCodes.Status400BadRequest)
            {
                return BadRequest(message);
            }
            else if (status == StatusCodes.Status500InternalServerError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            var token = JwtToken.GenerateToken(_config, user);

            return Ok(new
            {
                token
            });
        }

        // POST api/account/login
        [HttpPost("login"), AllowAnonymous]
        public ActionResult<UserDto> Login([FromBody] LoginCredentials value)
        {
            var (status, message, user) = _userService.Login(value);
            if (status == StatusCodes.Status404NotFound)
            {
                return NotFound(message);
            }
            else if (status == StatusCodes.Status401Unauthorized)
            {
                return Unauthorized(message);
            }
            else if (status == StatusCodes.Status500InternalServerError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            if (user == null)
            {
                return NotFound(message);
            }

            var token = JwtToken.GenerateToken(_config, user);

            return Ok(new
            {
                token
            });
        }

        // POST api/account/logout
        [HttpPost("logout"), Authorize]
        public async Task<ActionResult<Response>> Logout()
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                // Remove the 'sub' claim, which typically represents the user ID
                var subClaim = identity.FindFirst("Id");
                if (subClaim != null)
                {
                    return await _userService.LogoutUser(subClaim.Value);
                }
            }
            return Unauthorized();
        }

        // GET api/account/profile
        [Authorize]
        [HttpGet("profile"), Authorize]
        public ActionResult<UserDto?> GetProfile()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (_userService.IsAuthorized(identity) == false)
            {
                return Unauthorized();
            }

            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                var user = _userService.GetUserById(Guid.Parse(userId));
                if (user != null)
                {
                    return Ok(user);
                }
            }
            return NotFound();
        }

        // PUT api/account/profile
        [Authorize]
        [HttpPut("profile"), Authorize]
        public ActionResult<Response> UpdateProfile([FromBody] UserEditModel value)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if(userId != null)
            {
                return Ok(_userService.UpdateUser(Guid.Parse(userId), value));
            }

            return Unauthorized();
        }
    }
}
