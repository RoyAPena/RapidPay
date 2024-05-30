using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RapidPay.Application.Cards.Command.AddBalance;
using RapidPay.Application.Cards.Command.CreateCard;
using RapidPay.Application.Cards.Command.Pay;
using RapidPay.Application.Cards.Query.GetBalance;

namespace RapidPay.Presentation.Card
{
    public class CardModule : CarterModule
    {
        public CardModule() 
            : base("api/v1/card")
        {
            RequireAuthorization();
        }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/", async (IMediator mediator, CreateCardCommand query) =>
            {
                var result = await mediator.Send(query);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

            app.MapGet("/", async (IMediator mediator, Guid cardId) =>
            {
                var command = new GetBalanceByCardNumberQuery(cardId);
                var result = await mediator.Send(command);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

            app.MapPatch("/", async (IMediator mediator, PayCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

            app.MapPatch("/{Id}", async (Guid Id, IMediator mediator, decimal amount) =>
            {
                var command = new AddBalanceCommand(Id, amount);
                var result = await mediator.Send(command);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });
        }
    }
}