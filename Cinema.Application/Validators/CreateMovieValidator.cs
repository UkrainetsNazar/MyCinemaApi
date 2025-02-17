using Cinema.Application.DTO.MovieDTOs;
using FluentValidation;

namespace Cinema.Application.Validators
{
    public class CreateMovieValidator : AbstractValidator<CreateMovieDTO>
    {
        public CreateMovieValidator()
        {
            RuleFor(x => x.MovieTitle).NotEmpty().WithMessage("Movie title is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.DurationMinutes).GreaterThan(0).WithMessage("Duration must be greater than 0.");
            RuleFor(x => x.Genre).NotEmpty().WithMessage("Genre is required.");
            RuleFor(x => x.PosterUrl).Must(IsValidUrl).WithMessage("Invalid URL for Poster.");
            RuleFor(x => x.TrailerUrl).Must(IsValidUrl).WithMessage("Invalid URL for Trailer.");
            RuleFor(x => x.ReleaseDate).LessThanOrEqualTo(DateTime.Now).WithMessage("Release date cannot be in the future.");
        }
        private bool IsValidUrl(string url) => Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}
