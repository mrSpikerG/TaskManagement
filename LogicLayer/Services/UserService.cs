using DataAccess;
using DataAccess.DTOs;
using DataAccess.Interfaces;
using DataAccess.Models;
using LogicLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Services
{
    public class UserService:IUserService
    {

        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository repository, IJwtTokenService jwtTokenService)
        {
            _userRepository= repository;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<string> RegisterUserAsync(RegisterDTO registerDto)
        {

            if (_userRepository.Get().ToList().Any(u => u.Username == registerDto.Username || u.Email == registerDto.Email))
            {
                throw new Exception("Username or Email already exists.");
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _userRepository.Insert(user);   


            return "User registered successfully.";
        }

        public async Task<string> AuthenticateUserAsync(LoginDTO loginDto)
        {
            var user = _userRepository.Get().ToList().FirstOrDefault(u => u.Username == loginDto.Username || u.Email==loginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            return _jwtTokenService.GenerateToken(user);
        }

        public async Task<Guid?> GetUserIdByUsernameAsync(string username)
        {

            return await _userRepository.GetUserIdByUsernameAsync(username);
        }
    }
}
