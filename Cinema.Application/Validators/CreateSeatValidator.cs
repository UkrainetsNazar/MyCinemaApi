using Cinema.Application.DTO.SeatDTOs;
using FluentValidation;

namespace Cinema.Application.Validators
{
    public class CreateSeatValidator : AbstractValidator<CreateSeatDTO>
    {
        public CreateSeatValidator()
        {
            RuleFor(x => x.RowId).GreaterThan(0).WithMessage("RowId must be greater than 0.");
            RuleFor(x => x.SeatNumber).GreaterThan(0).WithMessage("Seat number must be greater than 0.");
        }
    }
}
