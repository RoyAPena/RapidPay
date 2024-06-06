namespace RapidPay.Presentation.User.Dtos.Request
{
    public sealed record CreateUserDto(
        string Username,
        string Password);
}