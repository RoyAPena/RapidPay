using RapidPay.Domain.Transactions;
using SharedKernel;

namespace RapidPay.Domain.Cards
{
    public class Card : Entity
    {
        public string CardNumber { get; private set; }
        public string CardHolderName { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public string IssuingBank { get; private set; }
        public decimal Balance { get; private set; }

        public ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();

        private Card()
        {

        }

        private Card(string cardNumber, string cardHolderName, DateTime expiryDate, string issuingBank, decimal balance)
        {
            Id = Guid.NewGuid();
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            ExpiryDate = expiryDate;
            IssuingBank = issuingBank;
            Balance = balance;
        }

        public static Card Create(string cardNumber, string cardHolderName, DateTime expiryDate, string issuingBank, decimal balance)
        {
            var card = new Card(cardNumber, cardHolderName, expiryDate, issuingBank, balance);
            return card;
        }

        public static Card Debit(Card card, decimal amount)
        {
            card.Balance -= amount;
            return card;
        }

        public static Card Credit(Card card, decimal amount)
        {
            card.Balance += amount;
            return card;
        }
    }
}