using RapidPay.Application.Abstractions.Messaging;

namespace RapidPay.Application.Cards.Command.AddBalance
{
    public sealed record AddBalanceCommand (
        string CardNumber,
        decimal Amount) : ICommand;
}