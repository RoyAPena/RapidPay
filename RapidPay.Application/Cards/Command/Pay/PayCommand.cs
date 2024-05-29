using RapidPay.Application.Abstractions.Messaging;

namespace RapidPay.Application.Cards.Command.Pay
{
    public sealed record PayCommand(
        string CardNumber,
        decimal Amount) : ICommand;
}