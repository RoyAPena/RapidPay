using AutoFixture;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using RapidPay.Application.Abstractions.Data;
using RapidPay.Application.Cards.Command.AddBalance;
using RapidPay.Domain.Cards;
using RapidPay.Domain.Transactions;

namespace RapidPay.Application.Test.Cards
{
    [TestFixture]
    public class AddBalanceCommandHandlerTest
    {
        private IUnitOfWork _unitOfWork;
        private ICardRepository _cardRepository;
        private ITransactionRepository _transactionRepository;
        private AddBalanceCommandHandler _handler;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _cardRepository = Substitute.For<ICardRepository>();
            _transactionRepository = Substitute.For<ITransactionRepository>();
            _handler = new AddBalanceCommandHandler(_unitOfWork, _cardRepository, _transactionRepository);
            _fixture = new Fixture();
        }

        [Test]
        public async Task Handle_Should_Return_Failure_When_Card_Is_Not_Found()
        {
            var request = _fixture.Create<AddBalanceCommand>();

            _cardRepository.GetWithLockAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                           .Returns(Task.FromResult<Card>(null));

            var result = await _handler.Handle(request, CancellationToken.None);

            ClassicAssert.IsFalse(result.IsSuccess);
            ClassicAssert.AreEqual(CardErrors.CardNotFound, result.Error);
        }
        
        [Test]
        public async Task Handle_Should_Return_Success_When_Add_Balance_Is_Made()
        {
            var request = _fixture.Build<AddBalanceCommand>()
                                  .With(x => x.Amount, 100) // Amount is 100
                                  .Create();

            var card = Card.Create("123456789012345", "Card Holder", DateTime.Now.AddYears(1), "Bank", 200);

            _cardRepository.GetWithLockAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                           .Returns(Task.FromResult(card));

            var result = await _handler.Handle(request, CancellationToken.None);

            ClassicAssert.IsTrue(result.IsSuccess);
        }
    }
}