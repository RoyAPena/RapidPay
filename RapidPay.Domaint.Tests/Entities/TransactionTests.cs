using NUnit.Framework;
using RapidPay.Domain.Transactions;

namespace RapidPay.Domain.Tests.Entities
{
    [TestFixture]
    internal class TransactionTests
    {
        [Test]
        public void CreateDebit_ShouldReturnTransactionWithCorrectValues()
        {
            // Arrange
            var cardId = Guid.NewGuid();
            var amount = 100m;
            var fee = 10m;

            // Act
            var transaction = Transaction.CreateDebit(cardId, amount, fee);

            // Assert
            Assert.AreEqual(cardId, transaction.CardId);
            Assert.AreEqual(amount, transaction.Amount);
            Assert.AreEqual(fee, transaction.Fee);
            Assert.AreEqual(TransactionType.Debit, transaction.TransactionType);
        }

        [Test]
        public void CreateCredit_ShouldReturnTransactionWithCorrectValues()
        {
            // Arrange
            var cardId = Guid.NewGuid();
            var amount = 100m;

            // Act
            var transaction = Transaction.CreateCredit(cardId, amount);

            // Assert
            Assert.AreEqual(cardId, transaction.CardId);
            Assert.AreEqual(amount, transaction.Amount);
            Assert.AreEqual(0m, transaction.Fee);
            Assert.AreEqual(TransactionType.Credit, transaction.TransactionType);
        }
    }
}
