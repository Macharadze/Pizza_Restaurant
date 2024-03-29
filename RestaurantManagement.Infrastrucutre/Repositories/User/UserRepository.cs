using RestaurantManagement.Application.Exceptions;
using RestaurantManagement.Application.Interfaces.IUser;
using RestaurantManagement.Domain.Database;
using RestaurantManagement.Domain.Entity;

namespace RestaurantManagement.Infrastrucutre.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _database;
        public UserRepository(ApplicationDbContext context)
        {
            _database = context;
        }
        public async Task Create(CancellationToken cancellationToken, User entity)
        {
            _database.Users.Add(entity);
            await _database.SaveChangesAsync();
        }

        public async Task<bool> Delete(CancellationToken token, string id)
        {
            User target;
            if (Exists(token, id, out target).Result)
                return false;

            target.IsDeleted = true;
            _database.Update(target);
            await _database.SaveChangesAsync(token);
            return true;
        }

        public Task<bool> Exists(CancellationToken cancellationToken, string id, out User user)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user = _database.Users.FirstOrDefault(i => i.Id.ToString().Equals(id));

            return Task.FromResult(user == null || user.IsDeleted == true);
        }

        public async Task<User> GetById(CancellationToken token, string id)
        {
            User target;
            if (Exists(token, id, out target).Result)
                throw new UserNotExistsException("user with the id " + id + " not exists");

            return await Task.FromResult(target);
        }

        public async Task<List<User>> GetAll(CancellationToken token)
        {
            var users = _database.Users.Where(i => !i.IsDeleted).ToList();

            return await Task.FromResult(users);
        }

        public async Task<bool> Update(CancellationToken token, User user, string id)
        {
            User target;
            if (Exists(token, id, out target).Result)
                return false;

            target.PhoneNumber = user.PhoneNumber;
            target.Firstname = user.Firstname;
            target.Lastname = user.Lastname;
            target.Email = user.Email;


            target.ModifiedOn = DateTime.UtcNow;
            _database.Update(target);
            await _database.SaveChangesAsync(token);

            return true;
        }

        public async Task<List<Address>> GetAddresses(CancellationToken token, string id)
        {
            User target;
            if (Exists(token, id, out target).Result)
                throw new UserNotExistsException("user with the id " + id + " not exists");

            var result = _database.Addresss.Where(i => i.UserId.ToString().Equals(id) && !i.IsDeleted).ToList();

            return await Task.FromResult(result);
        }
    }
}
