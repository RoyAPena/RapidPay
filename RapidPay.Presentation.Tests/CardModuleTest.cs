using AutoFixture;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;
using RapidPay.Application.Cards.Command.CreateCard;
using RapidPay.Presentation.Card;
using RapidPay.Presentation.Card.Dtos.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Presentation.Tests
{
    [TestFixture]
    internal class CardModuleTest
    {
        private IMapper _substituteMapper;
        private IMediator _substituteMediator;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _substituteMapper = Substitute.For<IMapper>();
            _substituteMediator = Substitute.For<IMediator>();
            _fixture = new Fixture();
        }

        [Test]
        public async Task CreateCard_Should_Map_Request_And_Send_Command_On_Post()
        {
            // Arrange
            var module = new CardModule(_substituteMapper);
            var request = _fixture.Build<CreateCardDto>()
                                  .With(x => x.CardNumber, "123456789012340")
                                  .Create(); ;

            CreateCardCommand mappedCommand = _fixture.Build<CreateCardCommand>()
                                  .With(x => x.CardNumber, "123456789012340")
                                  .Create();

            _substituteMapper.Map<CreateCardCommand>(request).Returns(mappedCommand);
            var successfulResult = new Unit(); // Mock successful result (adjust based on actual return type)
            _substituteMediator.Send(mappedCommand).Returns(successfulResult);

            // Act
            module.AddRoutes(null); // Assuming you don't need IEndpointRouteBuilder here

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = HttpMethods.Post;
            httpContext.Request.ContentType = "application/json";
            httpContext.Request.Body = Stream.Null; // Simulate request body
            httpContext.SetRouteValue("card", request);

            var response = await module.HandleRequest(httpContext); // Assuming a HandleRequest method

            // Assert
            SubstituteMapper.Received().Map<CreateCardCommand>(request);
            SubstituteMediator.Received().Send(mappedCommand);
            Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);
        }

        [Test]
        public async Task CreateCard_Should_Return_BadRequest_On_Mapping_Error()
        {
            // Arrange
            var module = new CardModule(SubstituteMapper);
            var request = new CreateCardDto();

            SubstituteMapper.Map<CreateCardCommand>(request).Throws(new Exception());
            SubstituteMediator.Send(It.IsAny<CreateCardCommand>()).Returns(new Unit());

            // Act
            module.AddRoutes(null); // Assuming you don't need IEndpointRouteBuilder here

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = HttpMethods.Post;
            httpContext.Request.ContentType = "application/json";
            httpContext.Request.Body = Stream.Null;
            httpContext.SetRouteValue("card", request);

            var response = await module.HandleRequest(httpContext); // Assuming a HandleRequest method

            // Assert
            SubstituteMapper.Received().Map<CreateCardCommand>(request);
            SubstituteMediator.DidNotReceive().Send(It.IsAny<CreateCardCommand>());
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        [Test]
        public async Task CreateCard_Should_Return_BadRequest_On_Mediator_Error()
        {
            // Arrange
            var module = new CardModule(SubstituteMapper);
            var request = new CreateCardDto();

            var mappedCommand = new CreateCardCommand();
            SubstituteMapper.Map<CreateCardCommand>(request).Returns(mappedCommand);
            SubstituteMediator.Send(mappedCommand).Returns(Task.FromResult(new ValidationResult("Error")));

            // Act
            module.AddRoutes(null); // Assuming you don't need IEndpointRouteBuilder here

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = HttpMethods.Post;
            httpContext.Request.ContentType = "application/json";
            httpContext.Request.Body = Stream.Null;
            httpContext.SetRouteValue("card", request);

            var response = await module.HandleRequest(httpContext); // Assuming a HandleRequest method

            // Assert
            SubstituteMapper.Received().Map<CreateCardCommand>(request);
            SubstituteMediator.Received().Send(mappedCommand);
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);
        }
    }
}