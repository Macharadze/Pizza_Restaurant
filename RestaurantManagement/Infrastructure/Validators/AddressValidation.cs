using FluentValidation;
using RestaurantManagement.Application.Interfaces.IUser;
using RestaurantManagement.Application.RequestModel;

namespace RestaurantManagement.Infrastructure.Validators
{
    public class AddressValidation : AbstractValidator<AddressRequest>
    {
        private readonly IUserRepository _UserRepository;

        public AddressValidation(IUserRepository userRepository)
        {
            _UserRepository = userRepository;


            RuleFor(x => x.UserId)
                .NotEmpty()
                .MustAsync(async (x,tokne) =>
                {
                    return await CheckUser(x,tokne);
                 });

            RuleFor(address => address.City)
            .NotEmpty()
            .Length(1, 15)
            .WithMessage("The length must be between 1 and 15");

            RuleFor(address => address.Country)
                .NotEmpty()
                .Length(1, 15)
                .WithMessage("The length must be between 1 and 15");


            RuleFor(address => address.Region)
                .Length(0, 15)
                .WithMessage("The length must be between 1 and 15");


            RuleFor(address => address.Description)
                .Length(0, 100)
                .WithMessage("The length must be between 1 and 100");

        }
        private async Task<bool> CheckUser(string userId, CancellationToken token = default)
        {
            return await _UserRepository.GetById(token, userId) != null;
        }

    }
}
