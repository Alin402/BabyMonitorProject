using BabyMonitorApiDataAccess.CustomValidationExceptions;
using BabyMonitorApiDataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BabyMonitorApiDataAccess.Repositories.Concretes;

public class LivestreamRepository : BaseRepository<Livestream>
{
    public LivestreamRepository(BabyMonitorContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Livestream>> GetAllAsync()
    {
        return await _context.Livestreams.Include(l => l._Baby).ToListAsync();
    }

    public override async Task<Livestream?> GetByIdAsync(Guid? id)
    {
        var foundLivestream = await _context.Livestreams
            .Include(l => l._Baby)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (foundLivestream == null)
        {
            throw new LivestreamNotFoundException("Livestream not found");
        }

        return foundLivestream;
    }
}