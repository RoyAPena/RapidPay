using RapidPay.Application.Abstractions.Messaging;

namespace RapidPay.Application.Cards.Query.GetBalance
{
    public sealed record GetBalanceByCardNumberQuery(Guid CardId) 
        : IQuery;
}
