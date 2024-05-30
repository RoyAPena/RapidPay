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

        public GetBalanceByCardNumberQueryHandler(
            ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public async Task<Result> Handle(GetBalanceByCardNumberQuery request, CancellationToken cancellationToken)
        {
            var card = await _cardRepository.GetCard(request.CardId, cancellationToken);

            if (card is null)
            {
                return Result.Failure(CardErrors.CardNotFound());
            }

            var response =  Result.Success(new BalanceResponse(card.Balance));

            return response;
        }
    }
}