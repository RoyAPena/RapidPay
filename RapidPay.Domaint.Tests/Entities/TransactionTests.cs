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
            Assert.That(cardId, Is.EqualTo(transaction.CardId));
            Assert.That(amount, Is.EqualTo(transaction.Amount));
            Assert.That(fee, Is.EqualTo(transaction.Fee));
            Assert.That(TransactionType.Debit, Is.EqualTo(transaction.TransactionType));
        }
    }
}