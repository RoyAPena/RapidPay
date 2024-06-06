namespace RapidPay.Presentation.Authentication.Dtos.Request
{
    public sealed record LoginDto(
        string Username,
        string Password);
}