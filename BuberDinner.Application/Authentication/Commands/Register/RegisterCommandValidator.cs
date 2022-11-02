using FluentValidation;

namespace BuberDinner.Application.Authentication.Commands.Register
{
    //Fluent Validation in action
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty();
            RuleFor(c => c.LastName).NotEmpty();
            RuleFor(c => c.Email).NotEmpty();
            RuleFor(c => c.Password).NotEmpty();
        }
    }
}
