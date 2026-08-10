using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealEstatePlatform.Core.Entities;
using RealEstatePlatform.Core.Interfaces;
using RealEstatePlatform.DataAccess.Contexts;

namespace RealEstatePlatform.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly RealEstateDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(RealEstateDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Property))
            {
                return (IEnumerable<T>)await _context.Properties
                    .Include(p => p.Location)
                    .Include(p => p.Images)
                    .ToListAsync();
            }

            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            // Eğer sorgulanan nesne Property ise Resimleri ve Konumu veritabanından BİRLİKTE getir
            if (typeof(T) == typeof(Property))
            {
                return await _context.Properties
                    .Include(p => p.Images)   // Resimleri dahil et
                    .Include(p => p.Location) // Konumu dahil et
                    .FirstOrDefaultAsync(p => p.Id == id) as T;
            }

            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
}