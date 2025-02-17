using Cinema.Application.DTO.RowDTOs;
using FluentValidation;

namespace Cinema.Application.Validators
{
    public class CreateRowValidator : AbstractValidator<CreateRowDTO>
    {
        public CreateRowValidator()
        {
            RuleFor(x => x.RowNumber).GreaterThan(0).WithMessage("Row number must be greater than 0.");
            RuleFor(x => x.SeatCount).GreaterThan(0).WithMessage("Seat count must be greater than 0.");
        }
    }

}
