using DataAccess.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterUserAsync(RegisterDTO registerDto);
        Task<string> AuthenticateUserAsync(LoginDTO loginDto);
    }
}
