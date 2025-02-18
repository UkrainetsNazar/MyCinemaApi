namespace Cinema.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMovieRepository Movies { get; }
        ISessionRepository Sessions { get; }
        IUserRepository Users { get; }
        IHallRepository Halls { get; }
        ITicketRepository Tickets { get; }
        ISeatRepository Seats { get; }

        Task SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
