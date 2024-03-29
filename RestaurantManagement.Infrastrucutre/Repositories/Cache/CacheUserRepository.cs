using Microsoft.Extensions.Caching.Memory;
using RestaurantManagement.Application.Interfaces.IUser;
using RestaurantManagement.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Infrastrucutre.Repositories.Cache
{
    public class CacheUserRepository : IUserRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;

        public CacheUserRepository(IUserRepository userRepository,IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
        }

        public Task Create(CancellationToken cancellationToken, User entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(CancellationToken cancellationToken, string entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(CancellationToken cancellationToken, string entityId, out User existingEntity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Address>> GetAddresses(CancellationToken token, string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAll(CancellationToken cancellationToken)
        {
            return _userRepository.GetAll(cancellationToken);
        }

        public Task<User> GetById(CancellationToken cancellationToken, string entityId)
        {
            string key = $"user - {entityId}";


            return _memoryCache.GetOrCreateAsync(
               key,
               entry =>
               {
                   entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                   return _userRepository.GetById(cancellationToken, entityId);
               });
        }

        public Task<bool> Update(CancellationToken cancellationToken, User entity, string entityId)
        {
            throw new NotImplementedException();
        }
    }
}
