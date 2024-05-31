using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.Entities;

namespace BabyMonitorApiDataAccess.Repositories.Abstractions
{
    public interface IBabyRepository
    {
        Task<Baby?> GetByIdAsync(Guid id);
    }
}
