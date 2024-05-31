using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.Dtos.Babies;
using BabyMonitorApiDataAccess.Entities;

namespace BabyMonitorApiBusiness.Abstractions
{
    public interface IBabyService
    {
        Task<CreatedBabyDto> CreateBaby(CreateBabyDto createBaby, string userEmail);
        Task<List<CreatedBabyDto>> GetAllBabies(string userEmail);
        Task<CreatedBabyDto> GetBaby(Guid id, string userEmail);
        Task DeleteBaby(Guid id, string userEmail);
        Task<CreatedBabyDto> UpdateBaby(Guid id, UpdateBabyDto updateBaby, string userEmail);
    }
}
