using FluentValidation;
using RestaurantManagement.Api.Model.Dto;

namespace RestaurantManagement.Infrastructure.Validators
{
    public class UserValidation : AbstractValidator<UserDto>
    {
        public UserValidation()
        {
            RuleFor(x => x.Firstname)
                .NotEmpty()
                .Length(2, 20)
                .WithMessage("The length Should be between 2,20");

            RuleFor(x => x.Firstname)
                .NotEmpty()
                .Length(2, 30)
                .WithMessage("The length Should be between 2,20");

            RuleFor(x => x.Email)
                .NotEmpty()
                .Matches(@"^[^\s@]+@[^\s@]+\.[^\s@]+$")
                .WithMessage("Invalid email format.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^5\d{8}$")
                .WithMessage("Phone number must start with 5 and have a total length of 9 digits.");

        }
    }
}
