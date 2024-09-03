using DataAccess;
using DataAccess.DTOs;
using DataAccess.Models;
using LogicLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Servieces
{
    public class UserService:IUserService
    {

        private readonly DatabaseContext _context;
        private readonly IJwtTokenService _jwtTokenService;

        public UserService(DatabaseContext context, IJwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<string> RegisterUserAsync(RegisterDTO registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username || u.Email == registerDto.Email))
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

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "User registered successfully.";
        }

        public async Task<string> AuthenticateUserAsync(LoginDTO loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            return _jwtTokenService.GenerateToken(user);
        }
    }
}
