using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface IUserService
    {
        Task<(int status, string message)> AddUserAsync(UserDto user);
        (int status, string message, UserDto? user) Login(LoginCredentials credentials);
        UserDto? GetUserById(Guid id); 
        Response UpdateUser(Guid id, UserEditModel value);
        Task<Response> LogoutUser(string id);
        bool IsAuthorized(ClaimsIdentity? identity);
    }
}
