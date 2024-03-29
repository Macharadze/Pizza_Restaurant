using FluentValidation;
using RestaurantManagement.Api.Infrastructure.Localizations;
using RestaurantManagement.Application.Interfaces.IPizza;
using RestaurantManagement.Application.Interfaces.IUser;
using RestaurantManagement.Application.RequestModel;

namespace RestaurantManagement.Infrastructure.Validators
{
    public class OrderValidation : AbstractValidator<OrderRequest>
    {

        private readonly IPizzaRepository _pizzaRepository;
        private readonly IUserRepository _UserRepository;
        public OrderValidation(IUserRepository userRepository, IPizzaRepository pizzaRepository)
        {
            _pizzaRepository = pizzaRepository;
            _UserRepository = userRepository;

            RuleFor(x => x.UserId)
                .NotEmpty()
                .MustAsync(async (x, token) =>
                {
                    return await CheckUser(x,token);
                }
                )
                .WithMessage(Language.NotFound); ;

            RuleForEach(x => x.PizzaNames)
                .NotEmpty()
                .MustAsync(async (x,token) =>
                {
                    return await CheckPizza(x, token);
                })
                .WithMessage(Language.NotFound);

            RuleFor(x => x.PizzaNames)
                .NotEmpty();
        }
        private async Task<bool> CheckPizza(string pizzaId, CancellationToken token = default)
        {
            return await _pizzaRepository.GetPizzaByName(pizzaId) != null;
        }
        private async Task<bool> CheckUser(string userId, CancellationToken token = default)
        {
            return await _UserRepository.GetById(token, userId) != null;
        }
    }
}
