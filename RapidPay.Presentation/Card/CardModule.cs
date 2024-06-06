﻿using AutoMapper;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RapidPay.Application.Cards.Command.AddBalance;
using RapidPay.Application.Cards.Command.CreateCard;
using RapidPay.Application.Cards.Command.Pay;
using RapidPay.Application.Cards.Query.GetBalance;
using RapidPay.Presentation.Card.Dtos.Request;

namespace RapidPay.Presentation.Card
{
    public class CardModule : CarterModule
    {
        private readonly IMapper _mapper;

        public CardModule(IMapper mapper)
            : base("api/v1/card")
        {
            RequireAuthorization();
            _mapper = mapper;
        }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/", async (IMediator _mediator, CreateCardDto request) =>
            {
                var command = _mapper.Map<CreateCardCommand>(request);

                var result = await _mediator.Send(command);

                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

            app.MapGet("/", async (IMediator _mediator, Guid cardId) =>
            {
                var query = new GetBalanceByCardNumberQuery(cardId);
                
                var result = await _mediator.Send(query);

                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

            app.MapPatch("/", async (IMediator _mediator, PayDto request) =>
            {
                var command = _mapper.Map<PayCommand>(request);
                
                var result = await _mediator.Send(command);

                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

            app.MapPatch("/{Id}", async (IMediator _mediator, Guid Id, decimal amount) =>
            {
                var command = new AddBalanceCommand(Id, amount);
                var result = await _mediator.Send(command);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });
        }
    }
}