using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReactiveMicroService.OrderService.API.Models;
using ReactiveMicroService.OrderService.API.Repository;

namespace ReactiveMicroService.OrderService.API.Service
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

        public async Task<T> CreateAsync(T item)
        {
            item.CreatedAt = DateTime.UtcNow;
            item.CreatedIP = _utilityService.GetClientIP();
            return await _repository.Insert(item);
        }

        public async Task<T> GetAsync(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<T> UpdateAsync(int id, T item)
        {
            item.UpdatedIP = _utilityService.GetClientIP();
            item.UpdatedAt = DateTime.UtcNow;
            item.IsDeleted = !item.IsActive;
            return await _repository.Update(item);
        }

        public async Task RemoveAsync(int id, T item)
        {
            item.UpdatedIP = _utilityService.GetClientIP();
            item.UpdatedAt = DateTime.UtcNow;
            item.IsActive = false;
            item.IsDeleted = !item.IsActive;
            await _repository.Delete(id, item);
        }

        public async Task<List<T>> FilterAsync(Dictionary<string, object> filters)
        {
            return await _repository.GetByColumns(filters);
        }
    }
}
