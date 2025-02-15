using AutoMapper;
using Cinema.Application.DTO.HallDTOs;
using Cinema.Application.DTO.MovieDTOs;
using Cinema.Application.DTO.RowDTOs;
using Cinema.Application.DTO.SeatDTOs;
using Cinema.Application.DTO.SessionDTOs;
using Cinema.Application.DTO.TicketDTOs;
using Cinema.Application.DTO.UserDTOs;
using Cinema.Domain.Entities;

namespace Cinema.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, GetMovieDTO>().ReverseMap();
            CreateMap<Movie, CreateMovieDTO>().ReverseMap();
            CreateMap<Movie, UpdateMovieDTO>().ReverseMap();

            CreateMap<Hall, GetHallDTO>().ReverseMap();
            CreateMap<Hall, CreateHallDTO>().ReverseMap();

            CreateMap<Row, GetRowDTO>().ReverseMap();
            CreateMap<Row, CreateRowDTO>().ReverseMap();

            CreateMap<Seat, GetSeatDTO>().ReverseMap();
            CreateMap<Seat, CreateSeatDTO>().ReverseMap();

            CreateMap<Session, GetSessionDTO>().ReverseMap();
            CreateMap<Session, CreateSessionDTO>().ReverseMap();
            CreateMap<Session, UpdateSessionDTO>().ReverseMap();

            CreateMap<Ticket, GetTicketDTO>().ReverseMap();
            CreateMap<Ticket, CreateTicketDTO>().ReverseMap();

            CreateMap<User, GetUserDTO>().ReverseMap();
            CreateMap<User, CreateUserDTO>().ReverseMap();
        }
    }
}
