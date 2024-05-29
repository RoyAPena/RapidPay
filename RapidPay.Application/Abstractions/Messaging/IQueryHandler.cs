using MediatR;
using SharedKernel;

namespace RapidPay.Application.Abstractions.Messaging
{
    public interface IQueryHandler<TQuery> : IRequestHandler<TQuery, Result>
        where TQuery : IQuery
    {
        Task<Result> Handle(TQuery request, CancellationToken cancellationToken);
    }
}