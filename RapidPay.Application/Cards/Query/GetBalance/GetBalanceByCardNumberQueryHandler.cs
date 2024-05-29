using RapidPay.Application.Abstractions.Messaging;
using RapidPay.Domain.Abstractions;
using RapidPay.Domain.Cards;
using SharedKernel;

namespace RapidPay.Application.Cards.Query.GetBalance
{
    public sealed class GetBalanceByCardNumberQueryHandler
        : IQueryHandler<GetBalanceByCardNumberQuery>
    {
        private readonly ICardRepository _cardRepository;
        private readonly ISecurityServices _securityServices;

        public GetBalanceByCardNumberQueryHandler(
            ICardRepository cardRepository, ISecurityServices securityServices)
        {
            _cardRepository = cardRepository;
            _securityServices = securityServices;
        }

        public async Task<Result> Handle(GetBalanceByCardNumberQuery request, CancellationToken cancellationToken)
        {
            var cardNumberTokenized = _securityServices.Tokenize(request.CardNumber);

            var card = await _cardRepository.GetCard(cardNumberTokenized, cancellationToken);

            if (card is null)
            {
                return Result.Failure(CardErrors.CardNotFound(request.CardNumber));
            }

            var response =  Result.Success(new BalanceResponse(card.Balance));

            return response;
        }
    }
}