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
            this.RequireAuthorization();
        }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/", async (IMediator mediator, CreateCardCommand query) =>
            {
                var result = await mediator.Send(query);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

            app.MapGet("/", async (IMediator mediator, string cardNumber) =>
            {
                var command = new GetBalanceByCardNumberQuery(cardNumber);
                var result = await mediator.Send(command);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

            app.MapPut("/", async (IMediator mediator, PayCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

            app.MapPut("/addbalance", async (IMediator mediator, AddBalanceCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });
        }
    }
}