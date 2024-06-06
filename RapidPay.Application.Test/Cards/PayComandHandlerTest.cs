using AutoFixture;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using RapidPay.Application.Abstractions.Data;
using RapidPay.Application.Cards.Command.Pay;
using RapidPay.Domain.Abstractions;
using RapidPay.Domain.Cards;
using RapidPay.Domain.Transactions;

namespace RapidPay.Application.Test.Cards
{
    [TestFixture]
    public class PayCommandHandlerTest
    {
        private IUnitOfWork _unitOfWork;
        private ICardRepository _cardRepository;
        private ITransactionRepository _transactionRepository;
        private IPaymentFeeService _paymentFeeService;
        private PayCommandHandler _handler;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _cardRepository = Substitute.For<ICardRepository>();
            _transactionRepository = Substitute.For<ITransactionRepository>();
            _paymentFeeService = Substitute.For<IPaymentFeeService>();
            _handler = new PayCommandHandler(_unitOfWork, _cardRepository, _transactionRepository, _paymentFeeService);
            _fixture = new Fixture();
        }

        [Test]
        public async Task Handle_Should_Return_Failure_When_Amount_Is_Less_Than_Or_Equal_To_Zero()
        {
            var request = _fixture.Build<PayCommand>()
                                  .With(x => x.Amount, 0) // Amount is 0
                                  .Create();

            var result = await _handler.Handle(request, CancellationToken.None);

            ClassicAssert.IsFalse(result.IsSuccess);
            ClassicAssert.AreEqual(CardErrors.AmountMoreThan0, result.Error);
        }

        [Test]
        public async Task Handle_Should_Return_Failure_When_Card_Is_Not_Found()
        {
            var request = _fixture.Create<PayCommand>();

            _cardRepository.GetWithLockAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                           .Returns(Task.FromResult<Card>(null));

            var result = await _handler.Handle(request, CancellationToken.None);

            ClassicAssert.IsFalse(result.IsSuccess);
            ClassicAssert.AreEqual(CardErrors.CardNotFound, result.Error);
        }

        [Test]
        public async Task Handle_Should_Return_Failure_When_Balance_Is_Insufficient()
        {
            var request = _fixture.Build<PayCommand>()
                                  .With(x => x.Amount, 100) // Amount is 100
                                  .Create();


            var card = Card.Create("123456789012345", "Card Holder", DateTime.Now.AddYears(1), "Bank", 50);

            _cardRepository.GetWithLockAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                           .Returns(Task.FromResult(card));

            _paymentFeeService.GetCurrentFee().Returns(0.1m); // Fee is 10%

            var result = await _handler.Handle(request, CancellationToken.None);

            ClassicAssert.IsFalse(result.IsSuccess);
            ClassicAssert.AreEqual(CardErrors.InsufficientBalance, result.Error);
        }

        [Test]
        public async Task Handle_Should_Return_Success_When_Payment_Is_Made()
        {
            var request = _fixture.Build<PayCommand>()
                                  .With(x => x.Amount, 100) // Amount is 100
                                  .Create();

            var card = Card.Create("123456789012345", "Card Holder", DateTime.Now.AddYears(1), "Bank", 200);

            _cardRepository.GetWithLockAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                           .Returns(Task.FromResult(card));

            _paymentFeeService.GetCurrentFee().Returns(0.1m); // Fee is 10%

            var result = await _handler.Handle(request, CancellationToken.None);

            ClassicAssert.IsTrue(result.IsSuccess);
            //ClassicAssert.AreEqual(card.Balance, result.Value);
        }
    }
}