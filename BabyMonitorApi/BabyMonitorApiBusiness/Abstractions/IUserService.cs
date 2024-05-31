using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.Dtos.Users;
using BabyMonitorApiDataAccess.Entities;

namespace BabyMonitorApiBusiness.Abstractions
{
    public interface IUserService
    {
        Task<User> PostUserAsync(User user);
        Task<User?> AuthenticateUserAsync(string? email, string? password);
        Task<User?> AuthenticateUserCompareHashesAsync(string? email, string? password);
        Task<GetUserDto?> GetUserAsync(string? email);
    }
}
