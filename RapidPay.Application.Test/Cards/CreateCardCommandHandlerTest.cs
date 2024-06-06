using AutoFixture;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using RapidPay.Application.Abstractions.Data;
using RapidPay.Application.Cards.Command.CreateCard;
using RapidPay.Domain.Abstractions;
using RapidPay.Domain.Cards;

namespace RapidPay.Application.Test.Cards
{
    [TestFixture]
    public class CreateCardCommandHandlerTest
    {
        private IUnitOfWork _unitOfWorkSubstitute;
        private ICardRepository _cardRepositorySubsitute;
        private ISecurityServices _securityServicesSubsitute;
        private CreateCardCommandHandler _handler;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkSubstitute = Substitute.For<IUnitOfWork>(); // Substitute only the needed dependency;
            _cardRepositorySubsitute = Substitute.For<ICardRepository>();
            _securityServicesSubsitute = Substitute.For<ISecurityServices>();
            _handler = new CreateCardCommandHandler(_unitOfWorkSubstitute, _cardRepositorySubsitute, _securityServicesSubsitute);
            _fixture = new Fixture();
        }

        [Test]
        public async Task Handle_Should_Return_Failure_When_CardNumberLength_Is_Incorrect()
        {
            var request = _fixture.Build<CreateCardCommand>()
                                  .With(x => x.CardNumber, "12345678901234") // 14 characters
                                  .Create();

            var result = await _handler.Handle(request, CancellationToken.None);

            ClassicAssert.IsFalse(result.IsSuccess);
            ClassicAssert.AreEqual(CardErrors.CardNumberLength, result.Error);
        }

        [Test]
        public async Task Handle_Should_Return_Failure_When_Card_Already_Exists()
        {
            var request = _fixture.Build<CreateCardCommand>()
                                  .With(x => x.CardNumber, "123456789012345") // 15 characters
                                  .Create();

            _cardRepositorySubsitute.CardExists(Arg.Any<string>(), Arg.Any<CancellationToken>())
                           .Returns(Task.FromResult(true));

            var result = await _handler.Handle(request, CancellationToken.None);

            ClassicAssert.IsFalse(result.IsSuccess);
            ClassicAssert.AreEqual(CardErrors.CardAlreadyExists, result.Error);
        }

        [Test]
        public async Task Handle_Should_Create_Card_Successfully()
        {
            var request = _fixture.Build<CreateCardCommand>()
                                  .With(x => x.CardNumber, "123456789012345") // 15 characters
                                  .Create();

            _cardRepositorySubsitute.CardExists(Arg.Any<string>(), Arg.Any<CancellationToken>())
                           .Returns(Task.FromResult(false));

            var result = await _handler.Handle(request, CancellationToken.None);

            ClassicAssert.IsTrue(result.IsSuccess);
        }
    }
}