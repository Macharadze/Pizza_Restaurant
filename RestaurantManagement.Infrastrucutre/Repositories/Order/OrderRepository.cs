using RestaurantManagement.Application.Exceptions;
using RestaurantManagement.Application.Interfaces.IAddress;
using RestaurantManagement.Application.Interfaces.IOrder;
using RestaurantManagement.Application.Interfaces.IPizza;
using RestaurantManagement.Application.Interfaces.IUser;
using RestaurantManagement.Domain.Database;
using RestaurantManagement.Domain.Entity;

namespace RestaurantManagement.Infrastrucutre.Repositories.OrderRepository
{

    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _database;
        private readonly IUserRepository _userRepository;
        private readonly IPizzaRepository _pizzaRepository;
        private readonly IAddressRepository _addressRepository;

        public OrderRepository(ApplicationDbContext context, IPizzaRepository pizzaRepository, IUserRepository userRepository, IAddressRepository addressRepository)
        {
            _database = context;
            _userRepository = userRepository;
            _pizzaRepository = pizzaRepository;
            _addressRepository = addressRepository;
        }

        public async Task Create(CancellationToken cancellationToken, Order entity)
        {
            _database.Orders.Add(entity);
            await _database.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateOrderPizaa(CancellationToken token, List<string> pizzas, string userId, string orderId, string addressId)
        {
            var user = await _userRepository.GetById(token, userId);
            var address = await _addressRepository.GetById(token, addressId);
            foreach (var item in pizzas)
            {
                var piza = await _pizzaRepository.GetPizzaByName(item);
                var pizzaOrder = new OrderPizzaa
                {
                    PizzaId = piza.Id,
                    UserId = user.Id,
                    OrderId = Guid.Parse(orderId),
                    AddresId = address.Id,
                };

                _database.OrderPizzaas.Add(pizzaOrder);
                await _database.SaveChangesAsync(token);
            }

        }

        public async Task<bool> Delete(CancellationToken cancellationToken, string entityId)
        {
            var existingOrder = _database.Orders.Where(i => i.Id.ToString().Equals(entityId)).FirstOrDefault();
            if (existingOrder == null)
            {
                return false; 
            }
            existingOrder.IsDeleted = true;
            _database.Orders.Update(existingOrder);
            await _database.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<bool> Exists(CancellationToken cancellationToken, string entityId, out Order existingEntity)
        {
            cancellationToken.ThrowIfCancellationRequested();

            existingEntity = _database.Orders.FirstOrDefault(o => o.Id.ToString().Equals(entityId));

            return Task.FromResult(existingEntity != null);
        }

        public async Task<List<Order>> GetAll(CancellationToken cancellationToken)
        {
            var orders = _database.Orders.Where(i => !i.IsDeleted).ToList();
            return await Task.FromResult(orders);
        }

        public async Task<Order> GetById(CancellationToken cancellationToken, string entityId)
        {
            var order = _database.Orders.Where(i => i.Id.ToString().Equals(entityId)).FirstOrDefault();
            if (order == null || order.IsDeleted)
            {
                throw new OrderNotExistsException("order with the id " + entityId + " not exists");
            }

            return await Task.FromResult(order);
        }

        public async Task<List<string>> GetPizzas(CancellationToken token, string orderId)
        {
            var order = _database.Orders.Where(i => i.Id.ToString().Equals(orderId)).FirstOrDefault();
            if (order == null || order.IsDeleted)
                throw new OrderNotExistsException("order with the id " + orderId +  "not exists");

            var pizzasIds = _database.OrderPizzaas.Where(i => i.OrderId.ToString().Equals(orderId)).Select(i => i.PizzaId).ToList();
            var result = _database.Pizzas.Where(i => pizzasIds.Contains(i.Id)).Select(i => i.Name).ToList();

            return await Task.FromResult(result);
        }

        public async Task<bool> Update(CancellationToken cancellationToken, Order entity, string entityId)
        {
          /*  var existingOrder = _db.Orders.Where(i => i.Id.ToString().Equals(entityId)).FirstOrDefault();
            if (existingOrder == null)
            {
                return false;
            }
            existingOrder.

            _db.Update(existingOrder);
            await _db.SaveChangesAsync(cancellationToken);*/
            return true;
        }

    }
}
