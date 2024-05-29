using Microsoft.EntityFrameworkCore.Storage;
using RapidPay.Application.Abstractions.Data;

namespace RapidPay.Infrastructure.Data
{
    public class UnitOfWork(RapidPayContext context) : IUnitOfWork
    {
        private readonly RapidPayContext _context = context;
        private IDbContextTransaction _transaction;

        public async Task BeginTransaction(CancellationToken cancelationToken)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancelationToken);
        }

        public async Task Commit(CancellationToken cancellationToken)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                await _transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await Rollback(cancellationToken);
                throw;
            }
        }

        public async Task Rollback(CancellationToken cancellationToken)
        {
            await _transaction.RollbackAsync(cancellationToken);
        }

        public async void Dispose()
        {
            await _transaction.DisposeAsync();
            await _context.DisposeAsync();
        }
    }
}