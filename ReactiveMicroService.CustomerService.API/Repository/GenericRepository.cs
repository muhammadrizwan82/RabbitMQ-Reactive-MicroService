using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
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

        public async Task<T> Update(int id, T entity, params Expression<Func<T, object>>[] propertiesToExclude)
        {
            // Retrieve the existing entity from the database
            var existingEntity = await _dbSet.FindAsync(id);

            if (existingEntity == null)
            {
                throw new ArgumentException("Entity not found.");
            }

            try
            {
                // Update only the properties that are allowed to be modified
                _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);

                // Mark specific properties as unchanged to exclude them from the update
                foreach (var property in propertiesToExclude)
                {
                    _dbContext.Entry(existingEntity).Property(property).IsModified = false;
                }

                // Save changes to the database
                await _dbContext.SaveChangesAsync();

                return existingEntity;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the entity.", ex);
            }
        }

        public async Task<T> Update(int id, T entity)
        {
            // Retrieve the existing entity from the database
            var existingEntity = await _dbSet.FindAsync(id);

            if (existingEntity == null)
            {
                throw new ArgumentException("Entity not found.");
            }

            try
            {
                // Update only the properties that are allowed to be modified

                // Mark foreign key properties as unchanged to avoid modification
                foreach (var property in _dbContext.Entry(existingEntity).Properties)
                {
                    if (property.Metadata.IsForeignKey())
                    {
                        property.IsModified = false;
                    }
                    if (property.Metadata.Name == "CreatedBy")
                    {
                        property.IsModified = false;
                    }
                    if (property.Metadata.Name == "CreatedIP")
                    {
                        property.IsModified = false;
                    }
                }

                _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);

                // Save changes to the database
                await _dbContext.SaveChangesAsync();

                return existingEntity;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the entity {ex.Message}");
            }
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

        public async Task<T> GetByColumnsFirstOrDefault(Dictionary<string, object> filters)
        {
            IQueryable<T> query = _dbSet;

            foreach (var filter in filters)
            {
                query = ApplyFilter(query, filter.Key, filter.Value);
            }
            var result = await query.FirstOrDefaultAsync<T>();
            return result;
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
