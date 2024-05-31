using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.CustomValidationExceptions;
using BabyMonitorApiDataAccess.Entities;
using BabyMonitorApiDataAccess.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BabyMonitorApiDataAccess.Repositories.Concretes
{
    public class BabyRepository : BaseRepository<Baby>
    {
        public BabyRepository(BabyMonitorContext context) : base(context)
        {
        }

        public override async Task<Baby?> GetByIdAsync(Guid? id)
        {
            try
            {
                Baby? baby = await _context.Babies.FirstOrDefaultAsync(b => b.Id == id);
                if (baby == null)
                {
                    throw new BabyNotFoundException("Baby not found");
                }
                return baby;
            }
            catch (BabyNotFoundException)
            {
                throw;
            }
        }
    }
}
