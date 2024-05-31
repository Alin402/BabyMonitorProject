using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BabyMonitorApiDataAccess.Repositories.Concretes
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly BabyMonitorContext _context;

        public BaseRepository(BabyMonitorContext context)
        {
            _context = context;
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                TEntity? entity = await _context.Set<TEntity>().FindAsync(id);

                if (entity == null)
                {
                    throw new Exception("Entity not found");
                }

                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception in delete {ex.Message}");
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await _context.Set<TEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception in get all {ex.Message}");
            }
        }

        public virtual async Task<TEntity?> GetByIdAsync(Guid? id)
        {
            try
            {
                TEntity? entity = await _context.Set<TEntity>().FindAsync(id);

                if (entity == null)
                {
                    throw new Exception("Entity not found");
                }

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception in get by id {ex.Message}");
            }
        }

        public virtual async Task<TEntity> PostAsync(TEntity entity)
        {
            try
            {
                await _context.Set<TEntity>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception in post {ex.Message}");
            }
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception in update {ex.Message}");
            }
        }
    }
}
