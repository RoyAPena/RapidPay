﻿using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RapidPay.Application.Users.Command.CreateUser;

namespace RapidPay.Presentation.User
{
    public class UserModule : CarterModule
    {
        public UserModule()
            : base("api/v1/user")
        {
            RequireAuthorization();
        }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/", async (IMediator mediator, CreateUserCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });
        }
    }
}