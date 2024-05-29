using RapidPay.Application.Abstractions.Data;
using RapidPay.Application.Abstractions.Messaging;
using RapidPay.Domain.Abstractions;
using RapidPay.Domain.User;
using RapidPay.Domain.Users;
using SharedKernel;

namespace RapidPay.Application.Users.Command.CreateUser
{
    internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ISecurityServices _securityServices;

        public CreateUserCommandHandler(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            ISecurityServices securityServices)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _securityServices = securityServices;
        }

        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransaction(cancellationToken);

            var existsUsername = await _userRepository.ExistsUsername(request.Username, cancellationToken);

            if (existsUsername)
            {
                return Result.Failure(UserErrors.UsernameShouldBeUnique());
            }

            var encryptedPassword = _securityServices.Encrypt(request.Password);

            var user = User.Create(request.Username, encryptedPassword);

            await _userRepository.Insert(user, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return Result.Success();
        }
    }
}