namespace RapidPay.Presentation.Card.Dtos.Request
{
    internal sealed record PayDto(
        Guid CardId,
        decimal Amount);
}