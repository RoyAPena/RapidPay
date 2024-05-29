using RapidPay.Application.Abstractions.Data;
using RapidPay.Application.Abstractions.Messaging;
using RapidPay.Domain.Abstractions;
using RapidPay.Domain.Cards;
using RapidPay.Domain.Transactions;
using SharedKernel;

namespace RapidPay.Application.Cards.Command.AddBalance
{
    internal sealed class AddBalanceCommandHandler : ICommandHandler<AddBalanceCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICardRepository _cardRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ISecurityServices _securityServices;

        public AddBalanceCommandHandler(
            IUnitOfWork unitOfWork,
            ICardRepository cardRepository,
            ITransactionRepository transactionRepository,
            ISecurityServices securityServices)
        {
            _unitOfWork = unitOfWork;
            _cardRepository = cardRepository;
            _transactionRepository = transactionRepository;
            _securityServices = securityServices;
        }

        public async Task<Result> Handle(AddBalanceCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransaction(cancellationToken);

            var tokenizedCardNumber = _securityServices.Tokenize(request.CardNumber);

            var card = await _cardRepository.GetWithLockAsync(tokenizedCardNumber, cancellationToken);

            if (card == null)
            {
                await _unitOfWork.Rollback(cancellationToken);
                return Result.Failure(CardErrors.CardNotFound(request.CardNumber));
            }

            card = Card.Credit(card, request.Amount);

            _cardRepository.UpdateBalance(card);

            var transaction = Transaction.CreateCredit(card.Id, request.Amount);

            await _transactionRepository.Insert(transaction, cancellationToken);

            await _unitOfWork.Commit(cancellationToken);

            return Result.Success();
        }
    }
}