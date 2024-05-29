using RapidPay.Application.Abstractions.Messaging;

namespace RapidPay.Application.Users.Command.Login
{
    public sealed record LoginCommand(
        string Username,
        string Password) : ICommand;
}