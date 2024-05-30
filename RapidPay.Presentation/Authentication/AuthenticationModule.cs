using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RapidPay.Application.Authentication.Command.Login;

namespace RapidPay.Presentation.Authentication
{
    public class AuthenticationModule : CarterModule
    {
        public AuthenticationModule()
            : base("api/v1/Authentication")
        {
        }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/", async (IMediator mediator, LoginCommand query) =>
            {
                var result = await mediator.Send(query);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });
        }
    }
}