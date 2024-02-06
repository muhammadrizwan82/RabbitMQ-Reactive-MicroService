using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using ReactiveMicroService.CustomerService.API.Models;
using ReactiveMicroService.CustomerService.API.Repository;

namespace ReactiveMicroService.CustomerService.API.Service
{
    public class GenericService<T> where T : BaseModel
    {
        private readonly IGenericRepository<T> _repository;
        private readonly UtilityService _utilityService;

        public GenericService(IGenericRepository<T> repository, UtilityService utilityService)
        {
            _repository = repository;
            _utilityService = utilityService;
        }

        public async Task<T> CreateAsync(T item, string dataType)
        {
            item.CreatedAt = DateTime.UtcNow;
            item.CreatedIP = _utilityService.GetClientIP();
            var insertedItem = await _repository.Insert(item);
            await _utilityService.AddDatatoQueue(insertedItem, "report." + dataType
                           , new Dictionary<string, object> { { dataType, "new" } });
            return insertedItem;
        }

        public async Task<T> GetAsync(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<List<T>> GetAllAsync()
        {            
            return await _repository.GetAll();
        }

        public async Task<T> UpdateAsync(int id, T item, string dataType)
        {
            item.UpdatedIP = _utilityService.GetClientIP();
            item.UpdatedAt = DateTime.UtcNow;
            item.IsDeleted = !item.IsActive;
            //var updatedItem = await _repository.Update(id,item);
            var updatedItem = await _repository.Update(id, item, e => e.Id, e => e.CreatedAt, e => e.CreatedIP, e => e.CreatedBy);
            await _utilityService.AddDatatoQueue(updatedItem, "report." + dataType
                           , new Dictionary<string, object> { { dataType, "update" } });
            return updatedItem;
        }

        public async Task RemoveAsync(int id, T item, string dataType)
        {
            item.UpdatedIP = _utilityService.GetClientIP();
            item.UpdatedAt = DateTime.UtcNow;
            item.IsActive = false;
            item.IsDeleted = !item.IsActive;
            var deletedItem = await _repository.Update(id, item, e => e.Id, e => e.CreatedAt, e => e.CreatedIP, e => e.CreatedBy);
            await _utilityService.AddDatatoQueue(deletedItem, "report." + dataType
                          , new Dictionary<string, object> { { dataType, "update" } });
            //await Task.CompletedTask;
        }

        public async Task<List<T>> GetByColumnFilter(Dictionary<string, object> filters)
        {
            return await _repository.GetByColumns(filters);
        }

        public async Task<T> GetSingleByColumnFilter(Dictionary<string, object> filters)
        {
            return await _repository.GetByColumnsFirstOrDefault(filters);
        }

    }
}
