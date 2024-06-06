using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using RapidPay.Application.Abstractions;
using RapidPay.Application.Users.Command.CreateUser;
using RapidPay.Domain.Users;

namespace RapidPay.Application.Test.Users
{
    [TestFixture]
    public class CreateUserCommandHandlerTests
    {
        private IUserServices _userService;
        private CreateUserCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _userService = Substitute.For<IUserServices>();
            _handler = new CreateUserCommandHandler(_userService);
        }

        [Test]
        public async Task Handle_ShouldReturnFailureResult_WhenUsernameIsEmpty()
        {
            // Arrange
            var command = new CreateUserCommand("", "password");

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            ClassicAssert.True(result.IsFailure);
            Assert.That(UserErrors.UsernameCannotBeEmpty, Is.EqualTo(result.Error));
        }

        [Test]
        public async Task Handle_ShouldReturnFailureResult_WhenPasswordIsEmpty()
        {
            // Arrange
            var command = new CreateUserCommand("username", "");

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            ClassicAssert.True(result.IsFailure);
            Assert.That(UserErrors.PasswordCannotBeEmpty, Is.EqualTo(result.Error));
        }

        [Test]
        public async Task Handle_ShouldReturnFailureResult_WhenUsernameExists()
        {
            // Arrange
            var command = new CreateUserCommand("username", "password");
            _userService.UsernameExists(command.Username).Returns(true);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            ClassicAssert.True(result.IsFailure);
            Assert.That(UserErrors.UsernameShouldBeUnique, Is.EqualTo(result.Error));
        }

        [Test]
        public async Task Handle_ShouldReturnFailureResult_WhenPasswordIsNotValid()
        {
            // Arrange
            var command = new CreateUserCommand("username", "password");
            _userService.UsernameExists(command.Username).Returns(false);
            _userService.ValidatePassword(command.Password).Returns(false);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            ClassicAssert.True(result.IsFailure);
            Assert.That(UserErrors.PasswordIsNotValid, Is.EqualTo(result.Error));
        }

        [Test]
        public async Task Handle_ShouldReturnSuccessResult_WhenUserIsCreated()
        {
            // Arrange
            var command = new CreateUserCommand("username", "password");
            _userService.UsernameExists(command.Username).Returns(false);
            _userService.ValidatePassword(command.Password).Returns(true);
            _userService.CreateUserAsync(command.Username, command.Password).Returns(Task.FromResult(Guid.NewGuid().ToString()));

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            ClassicAssert.True(result.IsSuccess);
        }
    }
}
