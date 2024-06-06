using AutoMapper;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RapidPay.Application.Authentication.Command.Login;
using RapidPay.Presentation.Authentication.Dtos.Request;

namespace RapidPay.Presentation.Authentication
{
    public class AuthenticationModule : CarterModule
    {
        private readonly IMapper _mapper;

        public AuthenticationModule(IMapper mapper)
            : base("api/v1/Authentication")
        {
            _mapper = mapper;
        }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/", async (IMediator _mediator, LoginDto request) =>
            {
                var command = _mapper.Map<LoginCommand>(request);

                var result = await _mediator.Send(command);

                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });
        }
    }
}