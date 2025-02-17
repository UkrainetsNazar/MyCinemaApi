using Cinema.Application.DTO.UserDTOs;
using FluentValidation;

namespace Cinema.Application.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserDTO>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
