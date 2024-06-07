using RapidPay.Domain.Cards;
using SharedKernel;

namespace RapidPay.Domain.Transactions
{
    public class Transaction : Entity
    {
        public Guid CardId { get; private set; }
        public decimal Amount { get; private set; }
        public decimal Fee { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public DateTime TransactionDate { get; private set; }

        public Card? Card { get; set; }

        private Transaction()
        {

        }

        private Transaction(Guid cardId, decimal amount, TransactionType transactionType, decimal fee = 0m)
        {
            Id = Guid.NewGuid();
            CardId = cardId;
            Amount = amount;
            Fee = fee;
            TransactionType = transactionType;
            TransactionDate = DateTime.Now;
        }

        public static Transaction CreateDebit(Guid cardId, decimal amount, decimal fee)
        {
            var transaction = new Transaction(cardId, amount, TransactionType.Debit, fee);
            return transaction;
        }
    }
}