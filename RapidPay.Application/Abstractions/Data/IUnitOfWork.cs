namespace RapidPay.Application.Abstractions.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransaction(CancellationToken cancellationToken);
        Task Commit(CancellationToken cancellationToken);
        Task Rollback(CancellationToken cancelationToken);
    }
}