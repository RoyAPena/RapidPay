using RapidPay.Application.Abstractions.Messaging;

namespace RapidPay.Application.Cards.Command.Pay
{
    public sealed record PayCommand(
        Guid CardId,
        decimal Amount) : ICommand;
}