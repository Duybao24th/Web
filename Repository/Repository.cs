using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using DrinkStore.Data;
using DrinkStore.Interfaces;

namespace DrinkStore.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DrinkStoreContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DrinkStoreContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> Query() => _dbSet.AsQueryable();

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
            await _dbSet.Where(predicate).ToListAsync();

        public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached) _dbSet.Attach(entity);
            _dbSet.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;
    }
}
