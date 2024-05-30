using RapidPay.Application.Abstractions.Messaging;

namespace RapidPay.Application.Cards.Command.AddBalance
{
    public sealed record AddBalanceCommand (
        Guid CardId,
        decimal Amount) : ICommand;
}