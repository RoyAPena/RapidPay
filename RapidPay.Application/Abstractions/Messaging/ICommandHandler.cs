using MediatR;
using SharedKernel;

namespace RapidPay.Application.Abstractions.Messaging
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
        where TCommand : ICommand
    {
        Task<Result> Handle(TCommand command, CancellationToken cancellation);
    }
}
