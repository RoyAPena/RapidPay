using RapidPay.Application.Abstractions;
using RapidPay.Application.Abstractions.Messaging;
using RapidPay.Domain.Users;
using SharedKernel;

namespace RapidPay.Application.Authentication.Command.Login
{
    internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand>
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly IUserServices _userServices;

        public LoginCommandHandler(
            IJwtProvider jwtProvider,
            IUserServices userServices)
        {
            _jwtProvider = jwtProvider;
            _userServices = userServices;
        }

        public async Task<Result> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var loginSuccess = await _userServices.LoginAsync(command.Username, command.Password);

            if (!loginSuccess)
            {
                return Result.Failure(UserErrors.InvalidCredential);
            }

            return Result.Success(_jwtProvider.Generate(command.Username));
        }
    }
}