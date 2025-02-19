using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;

namespace Cinema.Application.UseCases.HallUseCases
{
    public class CreateHallHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateHallHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Hall> HandleAsync(int hallNumber, int rowCount, int seatsPerRow)
        {
            var existingHall = await _unitOfWork.Halls.GetByNumberAsync(hallNumber);
            if (existingHall != null)
            {
                throw new Exception($"Зал №{hallNumber} вже існує.");
            }

            var hall = new Hall
            {
                NumberOfHall = hallNumber,
                Rows = new List<Row>()
            };

            for (int i = 1; i <= rowCount; i++)
            {
                var row = new Row
                {
                    RowNumber = i,
                    Hall = hall,
                    Seats = new List<Seat>()
                };

                for (int j = 1; j <= seatsPerRow; j++)
                {
                    row.Seats.Add(new Seat
                    {
                        SeatNumber = j,
                        Row = row,
                        IsBooked = false
                    });
                }

                hall.Rows.Add(row);
            }

            await _unitOfWork.Halls.AddHallAsync(hall);
            await _unitOfWork.SaveChangesAsync();

            return hall;
        }
    }
}
