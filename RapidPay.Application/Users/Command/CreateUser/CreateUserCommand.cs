using RapidPay.Application.Abstractions.Messaging;

namespace RapidPay.Application.Users.Command.CreateUser
{
    public sealed record CreateUserCommand(
        string Username,
        string Password
        ) : ICommand;
}