using Cinema.Application.DTO.TicketDTOs;
using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;
using AutoMapper;

namespace Cinema.Application.UseCases.TicketUseCases
{
    public class GetUserTicketsHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserTicketsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GetTicketForUserDTO>> HandleAsync(string userId)
        {
            var tickets = await _unitOfWork.Tickets.GetTicketsByUserIdAsync(userId);
            if (tickets == null || !tickets.Any())
                return new List<GetTicketForUserDTO> { };

            var ticketDTOs = _mapper.Map<List<GetTicketForUserDTO>>(tickets);

            return ticketDTOs;
        }
    }
}
