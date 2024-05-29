namespace RapidPay.Domain.Transactions
{
    public interface ITransactionRepository
    {
        Task Insert(Transaction transaction, CancellationToken cancelationToken);
    }
}