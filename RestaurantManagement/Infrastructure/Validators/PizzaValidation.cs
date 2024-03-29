using FluentValidation;
using RestaurantManagement.Api.Model.Dto;

namespace RestaurantManagement.Infrastructure.Validators
{
    public class PizzaValidation : AbstractValidator<PizzaDTO>
    {
        public PizzaValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(3, 20)
                .WithMessage("The length must be between 3 and 20");

            RuleFor(x => x.Price)
                .Must(x => x > 0)
                .WithMessage("Must be more than 0");

            RuleFor(x => x.Description)
                .MaximumLength(100)
                .When(p => !string.IsNullOrEmpty(p.Description))
                .WithMessage("Description length should be at most 100 characters.");

            RuleFor(x=>x.CaloryCount)
                .Must(x=>x>0)
                .WithMessage("Must be more than 0");
        }
    }
}
