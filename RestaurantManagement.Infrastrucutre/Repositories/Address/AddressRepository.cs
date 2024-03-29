using RestaurantManagement.Application.Exceptions;
using RestaurantManagement.Application.Interfaces.IAddress;
using RestaurantManagement.Application.Interfaces.IUser;
using RestaurantManagement.Domain.Database;
using RestaurantManagement.Domain.Entity;

namespace RestaurantManagement.Infrastrucutre.Repositories.AddressRepository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _database;
        private readonly IUserRepository _userRepository;
        public AddressRepository(ApplicationDbContext applicationDb, IUserRepository userRepository)
        {
            _database = applicationDb;
            _userRepository = userRepository;
        }

        public async Task Create(CancellationToken cancellationToken, Address entity)
        {
            User targetUser;
            if (_userRepository.Exists(cancellationToken, entity.UserId.ToString(), out targetUser).Result)
                throw new UserNotExistsException("user with the id "+ entity.UserId.ToString() +" not exists");

            _database.Add(entity);
            _database.Users.Update(targetUser);
            await _database.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> Delete(CancellationToken cancellationToken, string entityId)
        {
            Address address;
            if (Exists(cancellationToken, entityId, out address).Result)
                return false;
            address.IsDeleted = true;
            _database.Addresss.Update(address);
            await _database.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<bool> Exists(CancellationToken cancellationToken, string entityId, out Address existingEntity)
        {
            cancellationToken.ThrowIfCancellationRequested();

            existingEntity = _database.Addresss.FirstOrDefault(i => i.Id.ToString().Equals(entityId));

            return Task.FromResult(existingEntity == null || existingEntity.IsDeleted);
        }

        public async Task<List<Address>> GetAll(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var addresses = _database.Addresss.Where(i => !i.IsDeleted).ToList();

            return await Task.FromResult(addresses);
        }

        public async Task<Address> GetById(CancellationToken cancellationToken, string entityId)
        {
            Address address;
            if (Exists(cancellationToken, entityId, out address).Result)
                throw new AddressNotExistsException("address with the id " + entityId + " not exists");


            return await Task.FromResult(address);
        }

        public async Task<bool> Update(CancellationToken cancellationToken, Address entity, string id)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Address address;
            if (Exists(cancellationToken, id, out address).Result)
                return false;
            address.Region = entity.Region;
            address.City = entity.City;
            address.Country = entity.Country;
            address.ModifiedOn = DateTime.Now;
            _database.Update(address);
            await _database.SaveChangesAsync(cancellationToken);

            return true;

        }
    }
}
