using MediatR;
using SharedKernel;

namespace RapidPay.Application.Abstractions.Messaging
{
    public interface IQuery : IRequest<Result>
    {
    }
}