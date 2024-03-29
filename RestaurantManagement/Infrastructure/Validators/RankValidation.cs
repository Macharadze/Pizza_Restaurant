using FluentValidation;
using RestaurantManagement.Application.Exceptions;
using RestaurantManagement.Application.Interfaces.IPizza;
using RestaurantManagement.Application.Interfaces.IUser;
using RestaurantManagement.Application.RequestModel;

namespace RestaurantManagement.Infrastructure.Validators
{
    public class RankValidation : AbstractValidator<RankRequest>
    {
        private readonly IPizzaRepository _pizzaRepository;
        private readonly IUserRepository _UserRepository;
        public RankValidation(IUserRepository userRepository, IPizzaRepository pizzaRepository)
        {
            _pizzaRepository = pizzaRepository;
            _UserRepository = userRepository;

            RuleFor(x => x.UserId)
                .NotEmpty()
                .MustAsync(async (x,token) =>
                {
                    return await CheckPizza(x,token);
                })
                .WithMessage("user does not exists");

            RuleFor(x => x.PizzaId)
                .NotEmpty()
                .MustAsync(async (x, token) =>
                {
                    return await CheckUser(x, token);
                })
                .WithMessage("the pizza does not exist");

           
              RuleFor(x => x.Rate)
                  .NotEmpty()
                  .Must(x => x >= 1 && x <= 10);
        }

        private async Task<bool> CheckPizza(string pizzaId,CancellationToken token = default)
        {
            return await _pizzaRepository.GetById(token,pizzaId) != null;
        }
        private async Task<bool> CheckUser(string userId, CancellationToken token = default)
        {
            return await _UserRepository.GetById(token,userId) != null;
        }

    }
}
