using RestaurantManagement.Application.Interfaces.IBase;
using RestaurantManagement.Domain.Entity;

namespace RestaurantManagement.Application.Interfaces.IUser
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<Address>> GetAddresses(CancellationToken token, string id);
    }
}
