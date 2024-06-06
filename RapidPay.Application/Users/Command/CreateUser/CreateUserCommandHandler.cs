using RapidPay.Application.Abstractions;
using RapidPay.Application.Abstractions.Messaging;
using RapidPay.Domain.Users;
using SharedKernel;

namespace RapidPay.Application.Users.Command.CreateUser
{
    internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IUserServices _userService;

        public CreateUserCommandHandler(IUserServices userService)
        {
            _userService = userService;
        }

        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
            {
                return Result.Failure(UserErrors.UsernameCannotBeEmpty);
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return Result.Failure(UserErrors.PasswordCannotBeEmpty);
            }

            if (await _userService.UsernameExists(request.Username))
            {
                return Result.Failure(UserErrors.UsernameShouldBeUnique);
            }

            if (!await _userService.ValidatePassword(request.Password))
            {
                return Result.Failure(UserErrors.PasswordIsNotValid);
            }

            return Result.Success(await _userService.CreateUserAsync(request.Username, request.Password));
        }
    }
}