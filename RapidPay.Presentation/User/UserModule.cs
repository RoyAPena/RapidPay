using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RapidPay.Application.Users.Command.CreateUser;
using RapidPay.Application.Users.Command.Login;

namespace RapidPay.Presentation.User
{
    public class UserModule : CarterModule
    {
        public UserModule()
            : base("api/v1/user")
        {
        }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/", [Authorize] async (IMediator mediator, CreateUserCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

            app.MapPost("/login", async (IMediator mediator, LoginCommand query) =>
            {
                var result = await mediator.Send(query);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });
        }
    }
}