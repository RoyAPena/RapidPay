using RapidPay.Application.Abstractions.Data;
using RapidPay.Application.Abstractions.Messaging;
using RapidPay.Domain.Abstractions;
using RapidPay.Domain.Cards;
using SharedKernel;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RapidPay.Application.Tests")]
namespace RapidPay.Application.Cards.Command.CreateCard
{
    internal sealed class CreateCardCommandHandler : ICommandHandler<CreateCardCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICardRepository _cardRepository;
        private readonly ISecurityServices _securityServices;

        public CreateCardCommandHandler(
            IUnitOfWork unitOfWork,
            ICardRepository cardRepository,
            ISecurityServices securityServices)
        {
            _unitOfWork = unitOfWork;
            _cardRepository = cardRepository;
            _securityServices = securityServices;
        }

        public async Task<Result> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransaction(cancellationToken);

            if (request.CardNumber.Length != 15)
            {
                return Result.Failure(CardErrors.CardNumberLength());
            }

            var tokenizedCardNumber = _securityServices.Tokenize(request.CardNumber);

            var existsCard = await _cardRepository.CardExists(tokenizedCardNumber, cancellationToken);

            if (existsCard)
            {
                return Result.Failure(CardErrors.CardAlreadyExists());
            }

            var card = Card.Create(tokenizedCardNumber, request.CardHolderName, request.ExpiryDate, request.IssuingBank, request.Balance);

            await _cardRepository.Insert(card, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return Result.Success(card.Id);
        }
    }
}