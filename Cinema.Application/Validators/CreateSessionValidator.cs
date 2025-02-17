using Cinema.Application.DTO.SessionDTOs;
using FluentValidation;

namespace Cinema.Application.Validators
{
    public class CreateSessionValidator : AbstractValidator<CreateSessionDTO>
    {
        public CreateSessionValidator()
        {
            RuleFor(x => x.MovieId).GreaterThan(0).WithMessage("MovieId must be greater than 0.");
            RuleFor(x => x.StartTime).GreaterThan(DateTime.Now).WithMessage("Start time must be in the future.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(x => x.NumberOfHall).GreaterThan(0).WithMessage("Hall number must be greater than 0.");
        }
    }
}
