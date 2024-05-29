using Moq;
using RapidPay.Application.Abstractions.Data;
using RapidPay.Application.Cards.Command.CreateCard;
using RapidPay.Domain.Abstractions;
using RapidPay.Domain.Cards;
using Xunit;

namespace RapidPay.Application.Tests.Command
{
    public class CreateCardCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICardRepository> _cardRepositoryMock;
        private readonly Mock<ISecurityServices> _securityServicesMock;

        public CreateCardCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cardRepositoryMock = new Mock<ICardRepository>();
            _securityServicesMock = new Mock<ISecurityServices>();
        }

        [Fact]
        public async Task Handle_ValidCardInfo_Success()
        {
            // Arrange
            var command = new CreateCardCommand("123456789012345", "John Doe", DateTime.UtcNow.AddYears(2), "Test Bank", 100);

            _securityServicesMock.Setup(s => s.Tokenize(command.CardNumber)).Returns("1234-tokenized");
            _cardRepositoryMock.Setup(r => r.GetCard(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Card?>(null));
            _unitOfWorkMock.Setup(u => u.BeginTransaction(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.Commit(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new CreateCardCommandHandler(_unitOfWorkMock.Object, _cardRepositoryMock.Object, _securityServicesMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _cardRepositoryMock.Verify(r => r.Insert(It.IsAny<Card>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.BeginTransaction(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidCardNumberLength_Failure()
        {
            // Arrange
            var command = new CreateCardCommand("123456789012", "John Doe", DateTime.UtcNow.AddYears(2), "Test Bank", 100);

            var handler = new CreateCardCommandHandler(Mock.Of<IUnitOfWork>(), Mock.Of<ICardRepository>(), Mock.Of<ISecurityServices>());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(CardErrors.CardNumberLength().Code, result.Error.Code);
        }

        [Fact]
        public async Task Handle_ExistingCard_Failure()
        {
            // Arrange
            var command = new CreateCardCommand("123456789012345", "John Doe", DateTime.UtcNow.AddYears(2), "Test Bank", 100);

            _securityServicesMock.Setup(s => s.Tokenize(command.CardNumber)).Returns("existing-token");
            _cardRepositoryMock.Setup(r => r.GetCard(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Card.Create("existing-token", "Another User", command.ExpiryDate, command.IssuingBank, command.Balance))); // Existing card

            var handler = new CreateCardCommandHandler(_unitOfWorkMock.Object, _cardRepositoryMock.Object, _securityServicesMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(CardErrors.CardAlreadyExists().Code, result.Error.Code);
        }

        [Fact]
        public async Task Handle_UnitOfWorkBeginTransactionFails_Failure()
        {
            // Arrange
            var command = new CreateCardCommand("123456789012345", "John Doe", DateTime.UtcNow.AddYears(2), "Test Bank", 100);

            _unitOfWorkMock.Setup(u => u.BeginTransaction(It.IsAny<CancellationToken>()))
                .Throws(new Exception("Simulated UnitOfWork exception"));

            var handler = new CreateCardCommandHandler(_unitOfWorkMock.Object, Mock.Of<ICardRepository>(), Mock.Of<ISecurityServices>());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task Handle_UnitOfWorkCommitFails_Rollback()
        {
            // Arrange (Similar to successful test, but UnitOfWork.Commit throws exception)
            var command = new CreateCardCommand("123456789012345", "John Doe", DateTime.UtcNow.AddYears(2), "Test Bank", 100);

            _securityServicesMock.Setup(s => s.Tokenize(command.CardNumber)).Returns("1234-tokenized");
            _cardRepositoryMock.Setup(r => r.GetCard(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Card?>(null));
            _unitOfWorkMock.Setup(u => u.BeginTransaction(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => _unitOfWorkMock.Setup(u => u.Commit(It.IsAny<CancellationToken>()))
                        .Throws(new Exception("Simulated UnitOfWork commit exception")));

            var handler = new CreateCardCommandHandler(_unitOfWorkMock.Object, _cardRepositoryMock.Object, _securityServicesMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            _unitOfWorkMock.Verify(u => u.Rollback(It.IsAny<CancellationToken>()), Times.Once);
            // No calls to card repository expected due to rollback
        }
    }
}