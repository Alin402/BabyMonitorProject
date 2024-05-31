using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.Dtos.Livestream;
using BabyMonitorApiDataAccess.Entities;

namespace BabyMonitorApiBusiness.Abstractions
{
    public interface ILivestreamService
    {
        Task<Livestream> CreateLivestream(CreateLivestreamDto createLivestream);
        Task<List<GetAllLivestreamsDto>> GetAllLivestreams(string userEmail);
        Task<GetLivestreamDto> GetLivestream(Guid id, string userEmail);
        Task DeleteLivestream(Guid id, string userEmail);
        Task<GetStreamStatisticsDto> GetLivestreamStatistics(Guid id, string userEmail);
    }
}
