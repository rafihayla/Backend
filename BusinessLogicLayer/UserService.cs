using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<(int status, string message)> AddUserAsync(UserDto user)
        {
            return await _userRepository.AddUserAsync(user); 
        }

        public (int status, string message, UserDto? user) Login(LoginCredentials credentials)
        {
            return _userRepository.Login(credentials);
        }

        public UserDto? GetUserById(Guid id)
        {
            return _userRepository.GetUserById(id);
        }

        public Response UpdateUser(Guid id, UserEditModel value)
        {
            return _userRepository.UpdateUser(id, value);
        }

        public Task<Response> LogoutUser(string id)
        {
            return _userRepository.LogoutUser(id);
        }

        public bool IsAuthorized(ClaimsIdentity? identity)
        {
            return _userRepository.IsAuthorized(identity);
        }
    }
}
