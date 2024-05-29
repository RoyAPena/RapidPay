using RapidPay.Domain.Transactions;
using RapidPay.Infrastructure.Data;

namespace RapidPay.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly RapidPayContext _context;

        public TransactionRepository(RapidPayContext context)
        {
            _context = context;
        }

        public async Task Insert(Transaction transaction, CancellationToken cancelationToken)
        {
            await _context.Transactions.AddAsync(transaction, cancelationToken);
        }
    }
}