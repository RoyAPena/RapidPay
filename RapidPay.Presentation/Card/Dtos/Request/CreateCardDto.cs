namespace RapidPay.Presentation.Card.Dtos.Request
{
    internal sealed record CreateCardDto(
        string CardNumber,
        string CardHolderName,
        DateTime ExpiryDate,
        string IssuingBank,
        decimal Balance);
}