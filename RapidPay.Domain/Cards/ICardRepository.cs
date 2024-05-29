namespace RapidPay.Domain.Cards
{
    public interface ICardRepository
    {
        Task Insert(Card card, CancellationToken cancelationToken);
        void UpdateBalance(Card card);
        Task<Card?> GetCard(string cardNumber, CancellationToken cancelationToken);
        Task<Card?> GetWithLockAsync(string cardNumber, CancellationToken cancelationToken);
    }
}