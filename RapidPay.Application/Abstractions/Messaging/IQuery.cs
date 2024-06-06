using MediatR;
using SharedKernel;

namespace RapidPay.Application.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}