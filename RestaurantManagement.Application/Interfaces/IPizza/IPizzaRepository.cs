using RestaurantManagement.Application.Interfaces.IBase;
using RestaurantManagement.Application.RequestModel;
using RestaurantManagement.Domain.Entity;

namespace RestaurantManagement.Application.Interfaces.IPizza
{
    public interface IPizzaRepository : IRepository<Pizza>
    {

        Task<double> GetRating(CancellationToken cancellationToken, string id);
        Task AddRate(CancellationToken cancellationToken, RankRequest rankRequest, string pizzaId, string userId);
        Task<Pizza> GetPizzaByName(string name);
    }
}
