using FluentValidation;
using RandomTasks.Models;

namespace RandomTasks.Validation
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        public UserViewModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Age).NotEmpty().WithMessage("Age is required.")
                .InclusiveBetween(18, 60).WithMessage("Age must be between 18 and 60.");
        }
    }
}
