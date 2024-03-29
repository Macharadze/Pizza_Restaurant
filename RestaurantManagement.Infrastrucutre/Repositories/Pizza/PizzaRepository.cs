using RestaurantManagement.Application.Exceptions;
using RestaurantManagement.Application.Interfaces.IPizza;
using RestaurantManagement.Application.Interfaces.IUser;
using RestaurantManagement.Application.RequestModel;
using RestaurantManagement.Domain.Database;
using RestaurantManagement.Domain.Entity;

namespace RestaurantManagement.Infrastrucutre.Repositories.PizzaReposity
{
    public class PizzaRepository : IPizzaRepository
    {
        private readonly ApplicationDbContext _database;
        private readonly IUserRepository _userRepository;
        public PizzaRepository(ApplicationDbContext context, IUserRepository userRepository)
        {
            _database = context;
            _userRepository = userRepository;
        }

        public async Task AddRate(CancellationToken cancellationToken, RankRequest rankRequest, string pizzaId, string userId)
        {
             await GetById(cancellationToken, pizzaId);
             await _userRepository.GetById(cancellationToken, userId);


            var rank = new RankHistory
            {
                UserId = Guid.Parse(userId),
                PizzaId = Guid.Parse(pizzaId),
                Rank = rankRequest.Rate
            };

            _database.RankHistories.Add(rank);
            await _database.SaveChangesAsync(cancellationToken);
        }

        public async Task Create(CancellationToken cancellationToken, Pizza entity)
        {
            _database.Pizzas.Add(entity);
            await _database.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> Delete(CancellationToken cancellationToken, string entityId)
        {
            var existingPizza = _database.Pizzas.Where(i => i.Id.ToString().Equals(entityId)).FirstOrDefault();
            if (existingPizza == null)
            {
                return false;
            }
            existingPizza.IsDeleted = true;
            _database.Pizzas.Update(existingPizza);
            await _database.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<bool> Exists(CancellationToken cancellationToken, string entityId, out Pizza existingEntity)
        {

            existingEntity = _database.Pizzas.FirstOrDefault(p => p.Id.ToString() == entityId);

            return Task.FromResult(existingEntity != null);
        }

        public async Task<List<Pizza>> GetAll(CancellationToken cancellationToken)
        {
            var pizzas = _database.Pizzas.Where(i => !i.IsDeleted).ToList();
            return await Task.FromResult(pizzas);
        }

        public async Task<Pizza> GetById(CancellationToken cancellationToken, string entityId)
        {
            var pizza = _database.Pizzas.Where(i => i.Id.ToString().Equals(entityId)).FirstOrDefault();
            if (pizza == null)
            {
                throw new PizzaNotExistsException("order with the id " + entityId + " not exists");
            }
            return await Task.FromResult(pizza);
        }

        public async Task<Pizza> GetPizzaByName(string name)
        {
            var pizza = _database.Pizzas.Where(i => i.Name.Equals(name)).FirstOrDefault();
            if (pizza == null)
            {
                throw new PizzaNotExistsException("order with the name " + name + " not exists");
            }
            return await Task.FromResult(pizza);
        }

        public async Task<double> GetRating(CancellationToken cancellationToken, string id)
        {
            var rate = _database.RankHistories.Where(i => i.PizzaId.ToString().Equals(id)).Average(i => i.Rank);
            return await Task.FromResult(rate == 0 ? -1 : rate);
        }

        public async Task<bool> Update(CancellationToken cancellationToken, Pizza entity, string entityId)
        {
            var existingPizza = _database.Pizzas.Where(i => i.Id.ToString().Equals(entityId)).FirstOrDefault();
            if (existingPizza == null)
            {
                return false;
            }

            existingPizza.Name = entity.Name;
            existingPizza.Price = entity.Price;
            existingPizza.CaloryCount = entity.CaloryCount;
            existingPizza.Description = entity.Description;
            existingPizza.ModifiedOn = DateTime.UtcNow;

            _database.Update(existingPizza);
            await _database.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

