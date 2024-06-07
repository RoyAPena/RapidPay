using NUnit.Framework;
using RapidPay.Domain.Cards;

namespace RapidPay.Domaint.Tests.Entities
{
    [TestFixture]
    internal class CardTests
    {
        [Test]
        public void Create_Should_Create_Card_With_Provided_Details()
        {
            // Arrange
            string cardNumber = "1234567890123456";
            string cardHolderName = "John Doe";
            DateTime expiryDate = new DateTime(2025, 12, 31);
            string issuingBank = "RapidPay Bank";
            decimal balance = 100.00m;

            // Act
            var card = Card.Create(cardNumber, cardHolderName, expiryDate, issuingBank, balance);

            // Assert
            Assert.NotNull(card); // Verify card is created
            Assert.That(cardNumber, Is.EqualTo(card.CardNumber));
            Assert.That(cardHolderName, Is.EqualTo(card.CardHolderName));
            Assert.That(expiryDate, Is.EqualTo(card.ExpiryDate));
            Assert.That(issuingBank, Is.EqualTo(card.IssuingBank));
            Assert.That(balance, Is.EqualTo(card.Balance));
        }

        [Test]
        public void Debit_Should_Reduce_Balance_By_Specified_Amount()
        {
            // Arrange
            string cardNumber = "1234567890123456";
            string cardHolderName = "John Doe";
            DateTime expiryDate = new DateTime(2025, 12, 31);
            string issuingBank = "RapidPay Bank";
            decimal balance = 100.00m;
            decimal debitAmount = 25.00m;

            var card = Card.Create(cardNumber, cardHolderName, expiryDate, issuingBank, balance);

            // Act
            var debitedCard = Card.Debit(card, debitAmount);

            // Assert
            Assert.That(card, Is.SameAs(debitedCard));
            Assert.That(balance - debitAmount, Is.EqualTo(debitedCard.Balance));
        }
    }
}