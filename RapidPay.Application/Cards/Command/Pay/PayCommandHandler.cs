﻿using RapidPay.Application.Abstractions.Data;
using RapidPay.Application.Abstractions.Messaging;
using RapidPay.Domain.Abstractions;
using RapidPay.Domain.Cards;
using RapidPay.Domain.Transactions;
using SharedKernel;

namespace RapidPay.Application.Cards.Command.Pay
{
    internal sealed class PayCommandHandler : ICommandHandler<PayCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICardRepository _cardRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPaymentFeeService _paymentFeeService;
        private readonly ISecurityServices _securityServices;

        public PayCommandHandler(
            IUnitOfWork unitOfWork,
            ICardRepository cardRepository,
            ITransactionRepository transactionRepository,
            IPaymentFeeService paymentFeeService,
            ISecurityServices securityServices)
        {
            _unitOfWork = unitOfWork;
            _cardRepository = cardRepository;
            _transactionRepository = transactionRepository;
            _paymentFeeService = paymentFeeService;
            _securityServices = securityServices;
        }

        public async Task<Result> Handle(PayCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransaction(cancellationToken);

            if (request.Amount <= 0)
            {
                return Result.Failure(CardErrors.AmountMoreThan0());
            }

            var tokenizedCardNumber = _securityServices.Tokenize(request.CardNumber);

            var card = await _cardRepository.GetWithLockAsync(tokenizedCardNumber, cancellationToken);
            
            if (card == null)
            {
                await _unitOfWork.Rollback(cancellationToken);
                return Result.Failure(CardErrors.CardNotFound(request.CardNumber));
            }

            var currentFee = _paymentFeeService.GetCurrentFee();
            
            var fee = request.Amount * currentFee;
            var totalAmount = request.Amount + fee;

            if (card.Balance < totalAmount)
            {
                await _unitOfWork.Rollback(cancellationToken);
                return Result.Failure(CardErrors.InsufficientBalance());
            }

            card = Card.Debit(card, totalAmount);

            _cardRepository.UpdateBalance(card);

            var transaction = Transaction.CreateDebit(card.Id, request.Amount, fee);

            await _transactionRepository.Insert(transaction, cancellationToken);

            await _unitOfWork.Commit(cancellationToken);

            return Result.Success();
        }
    }
}