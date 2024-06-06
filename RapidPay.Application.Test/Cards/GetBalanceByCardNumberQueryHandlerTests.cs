using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using RapidPay.Application.Cards.Query.GetBalance;
using RapidPay.Domain.Cards;

namespace RapidPay.Application.Test.Cards
{
    [TestFixture]
    internal class GetBalanceByCardNumberQueryHandlerTests
    {
        private ICardRepository _cardRepository;
        private GetBalanceByCardNumberQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _cardRepository = Substitute.For<ICardRepository>();
            _handler = new GetBalanceByCardNumberQueryHandler(_cardRepository);
        }

        [Test]
        public async Task Handle_ShouldReturnFailureResult_WhenCardNotFound()
        {
            // Arrange
            var query = new GetBalanceByCardNumberQuery(Guid.NewGuid());
            _cardRepository.GetCard(query.CardId, Arg.Any<CancellationToken>()).Returns((Card)null);

            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            ClassicAssert.True(result.IsFailure);
            Assert.That(CardErrors.CardNotFound, Is.EqualTo(result.Error));
        }

        [Test]
        public async Task Handle_ShouldReturnSuccessResult_WhenCardFound()
        {
            // Arrange
            var query = new GetBalanceByCardNumberQuery(Guid.NewGuid());
            var card = Card.Create("1234567890123456", "John Doe", DateTime.Now.AddYears(1), "Test Bank", 1000m);
            _cardRepository.GetCard(query.CardId, Arg.Any<CancellationToken>()).Returns(card);

            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            ClassicAssert.True(result.IsSuccess);
            Assert.That(card.Balance, Is.EqualTo(result.Value.Balance));
        }
    }
}