using Cinema.Application.DTO.HallDTOs;
using FluentValidation;

namespace Cinema.Application.Validators
{
    public class CreateHallValidator : AbstractValidator<CreateHallDTO>
    {
        public CreateHallValidator()
        {
            RuleFor(x => x.NumberOfHall).GreaterThan(0).WithMessage("Hall number must be greater than 0.");
        }
    }
}
