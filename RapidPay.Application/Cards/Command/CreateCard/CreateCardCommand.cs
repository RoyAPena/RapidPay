using RapidPay.Application.Abstractions.Messaging;

namespace RapidPay.Application.Cards.Command.CreateCard
{
    public sealed record CreateCardCommand(
        string CardNumber, 
        string CardHolderName,
        DateTime ExpiryDate,
        string IssuingBank,
        decimal Balance) : ICommand;
}