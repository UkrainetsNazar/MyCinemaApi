using Cinema.Application.DTO.TicketDTOs;
using FluentValidation;

namespace Cinema.Application.Validators
{
    public class CreateTicketValidator : AbstractValidator<CreateTicketDTO>
    {
        public CreateTicketValidator()
        {
            RuleFor(x => x.SessionId).GreaterThan(0).WithMessage("SessionId must be greater than 0.");
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0.");
            RuleFor(x => x.SeatId).GreaterThan(0).WithMessage("SeatId must be greater than 0.");
        }
    }
}
