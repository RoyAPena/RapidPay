using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using RapidPay.Application.Abstractions;
using RapidPay.Application.Authentication.Command.Login;
using RapidPay.Domain.Users;

namespace RapidPay.Application.Test.Authentication
{
    internal class LoginCommandHandlerTests
    {
        private IJwtProvider _jwtProvider;
        private IUserServices _userServices;
        private LoginCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _jwtProvider = Substitute.For<IJwtProvider>();
            _userServices = Substitute.For<IUserServices>();
            _handler = new LoginCommandHandler(_jwtProvider, _userServices);
        }

        [Test]
        public async Task Handle_ShouldReturnFailureResult_WhenLoginFails()
        {
            // Arrange
            var command = new LoginCommand("username", "password");
            _userServices.LoginAsync(command.Username, command.Password).Returns(false);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            ClassicAssert.True(result.IsFailure);
            Assert.That(UserErrors.InvalidCredential, Is.EqualTo(result.Error));
        }

        [Test]
        public async Task Handle_ShouldReturnSuccessResult_WhenLoginSucceeds()
        {
            // Arrange
            var command = new LoginCommand("username", "password");
            var jwt = "jwt";
            _userServices.LoginAsync(command.Username, command.Password).Returns(true);
            _jwtProvider.Generate(command.Username).Returns(jwt);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            ClassicAssert.True(result.IsSuccess);
        }
    }
}