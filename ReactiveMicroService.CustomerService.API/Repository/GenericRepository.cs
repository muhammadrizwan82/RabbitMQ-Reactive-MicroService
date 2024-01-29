using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace ReactiveMicroService.CustomerService.API.Repository
{
    public class GenericRepository<TContext, T> : IGenericRepository<T>
        where TContext : DbContext
        where T : class
    {
        private readonly TContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(TContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> Insert(T entity)
        {
            var insertedEntity = await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return insertedEntity.Entity;
        }

        public async Task<T> Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(int id, T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<T>> GetByColumns(Dictionary<string, object> filters)
        {
            IQueryable<T> query = _dbSet;

            foreach (var filter in filters)
            {
                query = ApplyFilter(query, filter.Key, filter.Value);
            }

            return await query.ToListAsync();
        }

        private IQueryable<T> ApplyFilter(IQueryable<T> query, string columnName, object value)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "entity");
            MemberExpression property = Expression.Property(parameter, columnName);
            ConstantExpression constant = Expression.Constant(value);

            BinaryExpression body = Expression.Equal(property, constant);

            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

            return query.Where(lambda);
        }
    }
}
