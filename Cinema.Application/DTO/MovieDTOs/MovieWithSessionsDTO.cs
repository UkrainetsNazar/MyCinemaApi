using Cinema.Application.DTO.SessionDTOs;
using Cinema.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.DTO.MovieDTOs
{
    public class MovieWithSessionsDTO
    {
        public GetMovieDTO? Movie { get; set; }
        public IEnumerable<GetSessionDTO>? Sessions { get; set; }
    }

}
