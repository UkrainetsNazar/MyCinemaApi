using AutoMapper;
using Cinema.Application.DTO.HallDTOs;
using Cinema.Application.DTO.MovieDTOs;
using Cinema.Application.DTO.RowDTOs;
using Cinema.Application.DTO.SeatDTOs;
using Cinema.Application.DTO.SessionDTOs;
using Cinema.Application.DTO.TicketDTOs;
using Cinema.Application.DTO.TmdbDTO;
using Cinema.Application.DTO.UserDTOs;
using Cinema.Domain.Entities;

namespace Cinema.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, GetMovieDTO>()
            .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.MovieTitle))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => src.DurationMinutes))
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
            .ForMember(dest => dest.PosterUrl, opt => opt.MapFrom(src => src.PosterUrl))
            .ForMember(dest => dest.TrailerUrl, opt => opt.MapFrom(src => src.TrailerUrl))
            .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating));
            CreateMap<Movie, CreateMovieDTO>().ReverseMap();
            CreateMap<Movie, UpdateMovieDTO>().ReverseMap();

            CreateMap<Hall, GetHallDTO>().ReverseMap();
            CreateMap<Hall, CreateHallDTO>().ReverseMap();

            CreateMap<Row, GetRowDTO>().ReverseMap();
            CreateMap<Row, CreateRowDTO>().ReverseMap();

            CreateMap<Seat, GetSeatDTO>().ReverseMap();
            CreateMap<Seat, CreateSeatDTO>().ReverseMap();

            
            CreateMap<Session, GetSessionDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId))
                .ForMember(dest => dest.HallId, opt => opt.MapFrom(src => src.HallId))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
            CreateMap<Session, CreateSessionDTO>().ReverseMap();
            CreateMap<Session, UpdateSessionDTO>().ReverseMap();
            CreateMap<Session, SessionDetailsDTO>().ReverseMap();

            CreateMap<Ticket, GetTicketDTO>().ReverseMap();
            CreateMap<Ticket, CreateTicketDTO>().ReverseMap();
            CreateMap<Ticket, GetTicketForUserDTO>()
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Session!.Movie!.MovieTitle))
                .ForMember(dest => dest.SessionStartTime, opt => opt.MapFrom(src => src.Session!.StartTime))
                .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.Seat!.SeatNumber))
                .ForMember(dest => dest.HallNumber, opt => opt.MapFrom(src => src.Session!.Hall!.NumberOfHall.ToString()))
                .ForMember(dest => dest.RowNumber, opt => opt.MapFrom(src => src.Seat!.Row!.RowNumber));

            CreateMap<User, GetUserDTO>().ReverseMap();
            CreateMap<User, CreateUserDTO>().ReverseMap();

            CreateMap<TmdbDto, Movie>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.MovieTitle))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => DateTime.Parse(src.ReleaseDate!)))
                .ForMember(dest => dest.PosterUrl, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.PosterPath)
                ? "https://example.com/default-poster.jpg"
                : $"https://image.tmdb.org/t/p/w500{src.PosterPath}"
                ))
                .ForMember(dest => dest.Rating, opt => opt.Ignore())
                .ForMember(dest => dest.RatingCount, opt => opt.Ignore())
                .ForMember(dest => dest.StartDate, opt => opt.Ignore())
                .ForMember(dest => dest.EndDate, opt => opt.Ignore())
                .ForMember(dest => dest.Sessions, opt => opt.Ignore())
                .ForMember(dest => dest.TrailerUrl, opt => opt.Ignore());
        }
    }
}
