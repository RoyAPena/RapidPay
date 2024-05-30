namespace RapidPay.Domain.Cards
{
    public interface ICardRepository
    {
        Task Insert(Card card, CancellationToken cancelationToken);
        void UpdateBalance(Card card);
        Task<Card?> GetCard(Guid cardId, CancellationToken cancelationToken);
        Task<Card?> GetWithLockAsync(Guid cardId, CancellationToken cancelationToken);
        Task<bool> CardExists(string cardNumber, CancellationToken cancelationToken);
    }
}