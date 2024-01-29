using Microsoft.EntityFrameworkCore;

namespace ReactiveMicroService.OrderService.API.Repository
{
    public class GenericDBContext<TContext> : DbContext where TContext : DbContext
    {
        public GenericDBContext(DbContextOptions<TContext> options) : base(options) { }

        public DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        // Generic method to get all records for any entity type
        public async Task<List<TEntity>> GetAll<TEntity>() where TEntity : class
        {
            return await Set<TEntity>().ToListAsync();
        }

        // Method to get an entity by its ID
        public async Task<TEntity> GetById<TEntity>(object id) where TEntity : class
        {
            return await Set<TEntity>().FindAsync(id);
        }

        // Method to get entities by specific column values
        public async Task<List<TEntity>> GetByColumns<TEntity>(Dictionary<string, object> columnValues) where TEntity : class
        {
            IQueryable<TEntity> query = Set<TEntity>();

            foreach (var kvp in columnValues)
            {
                query = query.Where(e => EF.Property<object>(e, kvp.Key) == kvp.Value);
            }

            return await query.ToListAsync();
        }

        // Method to add a new entity
        public async Task<TEntity> Insert<TEntity>(TEntity entity) where TEntity : class
        {
            await Set<TEntity>().AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        // Method to update an existing entity
        public async Task<TEntity> Update<TEntity>(TEntity entity) where TEntity : class
        {
            var dbResult = Set<TEntity>().Update(entity);
            await SaveChangesAsync();
            return dbResult.Entity;
        }

        // Method to delete an entity by its ID
        public async Task Delete<TEntity>(object id, TEntity entity) where TEntity : class
        {
            var dbResult = Set<TEntity>().Update(entity);
            await SaveChangesAsync();
        }
    }
}
