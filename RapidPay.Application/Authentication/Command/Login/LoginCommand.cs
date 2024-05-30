using RapidPay.Application.Abstractions.Messaging;

namespace RapidPay.Application.Authentication.Command.Login
{
    public sealed record LoginCommand(
        string Username,
        string Password) : ICommand;
}