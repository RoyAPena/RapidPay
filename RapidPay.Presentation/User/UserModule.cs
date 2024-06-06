using AutoMapper;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RapidPay.Application.Users.Command.CreateUser;
using RapidPay.Presentation.User.Dtos.Request;

namespace RapidPay.Presentation.User
{
    public class UserModule : CarterModule
    {
        private readonly IMapper _mapper;

        public UserModule(IMapper mapper)
            : base("api/v1/user")
        {
            RequireAuthorization();
            _mapper = mapper;
        }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/", async (IMediator _mediator, CreateUserDto request) =>
            {
                var command = _mapper.Map<CreateUserCommand>(request);

                var result = await _mediator.Send(command);

                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });
        }
    }
}