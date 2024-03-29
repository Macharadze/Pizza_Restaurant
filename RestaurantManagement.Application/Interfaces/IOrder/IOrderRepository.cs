using RestaurantManagement.Application.Interfaces.IBase;
using RestaurantManagement.Domain.Entity;

namespace RestaurantManagement.Application.Interfaces.IOrder
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task CreateOrderPizaa(CancellationToken token, List<string> pizzas, string userId, string orderId, string addressId);
        Task<List<string>> GetPizzas(CancellationToken token, string orderId);
    }
}

