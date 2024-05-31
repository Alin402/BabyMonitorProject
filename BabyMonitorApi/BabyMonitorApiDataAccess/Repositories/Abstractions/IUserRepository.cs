using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.Entities;
using Microsoft.Identity.Client;

namespace BabyMonitorApiDataAccess.Repositories.Abstractions
{
    public interface IUserRepository
    {
        Task<User?> FindByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid? id);
    }
}
