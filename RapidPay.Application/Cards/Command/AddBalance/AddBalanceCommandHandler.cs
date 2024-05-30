using RapidPay.Application.Abstractions.Data;
using RapidPay.Application.Abstractions.Messaging;
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

        public AddBalanceCommandHandler(
            IUnitOfWork unitOfWork,
            ICardRepository cardRepository,
            ITransactionRepository transactionRepository)
        {
            _unitOfWork = unitOfWork;
            _cardRepository = cardRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<Result> Handle(AddBalanceCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransaction(cancellationToken);

            var card = await _cardRepository.GetWithLockAsync(request.CardId, cancellationToken);

            if (card == null)
            {
                await _unitOfWork.Rollback(cancellationToken);
                return Result.Failure(CardErrors.CardNotFound());
            }

            card = Card.Credit(card, request.Amount);

            _cardRepository.UpdateBalance(card);

            var transaction = Transaction.CreateCredit(card.Id, request.Amount);

            await _transactionRepository.Insert(transaction, cancellationToken);

            await _unitOfWork.Commit(cancellationToken);

            return Result.Success(card.Balance);
        }
    }
}