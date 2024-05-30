using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RapidPay.Domain.Cards;
using RapidPay.Infrastructure.Data;

namespace RapidPay.Infrastructure.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly RapidPayContext _context;

        public CardRepository(RapidPayContext context)
        {
            _context = context;
        }

        public async Task Insert(Card card, CancellationToken cancelationToken)
        {
            await _context.Cards.AddAsync(card, cancelationToken);
        }

        public void UpdateBalance(Card card)
        {
            _context.Entry(card).Property(c => c.Balance).IsModified = true;
        }

        public async Task<Card?> GetCard(Guid cardId, CancellationToken cancelationToken)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == cardId, cancelationToken);
            return card;
        }

        public async Task<bool> CardExists(string cardNumber, CancellationToken cancelationToken)
        {
            return await _context.Cards.AnyAsync(c => c.CardNumber == cardNumber, cancelationToken);
        }

        public async Task<Card?> GetWithLockAsync(Guid cardId, CancellationToken cancelationToken)
        {
            // Implemented this query for avoid another transaction modified the same card at same time.
            try
            {
                var sqlString = $@"
                     SELECT 
                          {nameof(Card.Id)},
                          {nameof(Card.CardHolderName)}, 
                          {nameof(Card.ExpiryDate)},
                          {nameof(Card.CardNumber)},
                          {nameof(Card.IssuingBank)}, 
                          {nameof(Card.Balance)}
                     FROM {nameof(Card)}s
                    WITH (UPDLOCK, READPAST)
                    WHERE {nameof(Card.Id)} = @cardId";
                return await _context
            .Cards
            .FromSqlRaw(
                sqlString,
                new SqlParameter("@cardId", cardId))
            .FirstOrDefaultAsync(cancelationToken);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}