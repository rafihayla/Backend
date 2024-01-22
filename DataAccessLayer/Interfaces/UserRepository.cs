using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DataAccessLayer.Interfaces
{
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        public readonly ApplicationDbContext _context = context;

        public async Task<(int status, string message)> AddUserAsync(UserDto user)
        {
            try
            {
                var userDb = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                if (userDb != null)
                {
                    return (StatusCodes.Status400BadRequest, "User already exists");
                }
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return (StatusCodes.Status201Created, "User registered");
            }
            catch (Exception ex)
            {
                return (StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public (int status, string message, UserDto? user) Login(LoginCredentials credentials)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == credentials.Email);
                if (user == null)
                {
                    return (StatusCodes.Status404NotFound, "User not found", null);
                }
                if (user.Password != credentials.Password)
                {
                    return (StatusCodes.Status401Unauthorized, "Invalid credentials", null);
                }
                return (StatusCodes.Status200OK, "Login successful", user);
            }
            catch (Exception ex)
            {
                return (StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public UserDto? GetUserById(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }

        public Response UpdateUser(Guid id, UserEditModel value)
        {
            var userDb = _context.Users.FirstOrDefault(u => u.Id == id);
            if (userDb == null)
            {
                return new Response()
                {
                    Status = "Error",
                    Message = "User not found"
                };
            }
            userDb.FullName = value.FullName;
            userDb.BirthDate = value.BirthDate;
            userDb.Gender = value.Gender;
            userDb.Address = value.Address;
            userDb.PhoneNumber = value.PhoneNumber;
            _context.SaveChanges();
            return new()
            {
                Status = "Success",
                Message = "Profile updated"
            };

        }

        public async Task<Response> LogoutUser(string id)
        {
            var token = _context.Tokens.FirstOrDefault(t => t.Token == id);
            if (token == null)
            {
                var idToken = new LoggedOutToken()
                {
                    Token = id
                };
                await _context.Tokens.AddAsync(idToken);
                _context.SaveChanges();
                return new()
                {
                    Status = "Success",
                    Message = "User logged out"
                };
            }

            return new()
            {
                Status = "Error",
                Message = "You don't have access"
            };
        }

        public bool IsAuthorized(ClaimsIdentity? identity)
        {
            return AuthHelper.IsAuthorized(_context, identity);
        }
    }
}
