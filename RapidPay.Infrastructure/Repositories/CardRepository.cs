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

        public async Task<Card?> GetCard(string cardNumber, CancellationToken cancelationToken)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(c => c.CardNumber == cardNumber, cancelationToken);
            return card;
        }

        public async Task<Card?> GetWithLockAsync(string cardNumber, CancellationToken cancelationToken)
        {
            // Implemented this query for avoid another transaction modified the same card at same time.
            try
            {
                var sqlString = $@"
                     SELECT 
                          {nameof(Card.Id)},
                          {nameof(Card.CardHolderName)}, 
                          {nameof(Card.ExpiryDate)},
                          {nameof(Card.IssuingBank)}, 
                          {nameof(Card.Balance)},
                          {nameof(Card.CardNumber)}
                     FROM {nameof(Card)}s
                    WITH (UPDLOCK, READPAST)
                    WHERE {nameof(Card.CardNumber)} = @cardNumber";
                return await _context
            .Cards
            .FromSqlRaw(
                sqlString,
                new SqlParameter("@cardNumber", cardNumber))
            .FirstOrDefaultAsync(cancelationToken);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}