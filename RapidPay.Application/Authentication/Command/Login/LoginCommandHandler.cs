using RapidPay.Application.Abstractions;
using RapidPay.Application.Abstractions.Messaging;
using RapidPay.Domain.Abstractions;
using RapidPay.Domain.User;
using RapidPay.Domain.Users;
using SharedKernel;

namespace RapidPay.Application.Authentication.Command.Login
{
    internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly ISecurityServices _securityServices;

        public LoginCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider, ISecurityServices securityServices)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _securityServices = securityServices;
        }

        public async Task<Result> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var encryptedPassword = _securityServices.Encrypt(command.Password);

            var user = await _userRepository.GetByUsernameAndPassword(command.Username, encryptedPassword, cancellationToken);

            if (user is null)
            {
                return Result.Failure(UserErrors.InvalidCredential());
            }

            string token = _jwtProvider.Generate(user);

            return Result.Success(token);
        }
    }
}