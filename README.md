## Project Description

This api manage the payment and save the transaction providing to you the necessary method and security for manage all your transaction.
For use this api you need to has a credential for start using it, you can be available to create card, pay balance and add balance.
All payment will be charge with a Fee, the add balance is out of charge.

## Project Structure

The project is divided in 4 layers using Clean Architectur:

RapidPay.Domain: This layer contains the heart of your application, representing the core business concepts and rules.
				 It defines entities, value objects, and domain logic that model your application's functionality.
				 This layer has no dependencies on any external technologies or frameworks.

RapidPay.Application: This layer sits between the Domain and other layers.
					  It uses domain objects and logic to implement application-specific use cases.
					  It defines interfaces that the outer layers can interact with.
					  It depends on the Domain layer but not on the Presentation or Infrastructure layers (ideal scenario).

RapidPay.Infrastructure: This layer contains the technical details of how the application interacts with external systems and frameworks.
						 It implements concrete mechanisms for data access (databases), external APIs, and other infrastructure concerns.
						 It depends on the Application layer but shouldn't directly reference the Domain layer or Presentation layer.

RapidPay.Presentation: This layer is responsible for presenting information to the user and receiving user input.
					   It can be a web UI, mobile app, API, or any other interaction point.
					   It depends on the Application layer to access and manipulate domain logic.

## Technologies And Frameworks

- .NET 8
- Entity Framework
- SQL Server
- NUnit
- MediatR
- FluentValidation
- AutoMapper
- Serilog
- Swagger
- JWT

## Patterns

- CQRS
- DDD
- Repository Pattern
- Unit Of Work Pattern
- 
## Installation

This project use sql server database, so you need to have a sql server installed in your machine. the version that is used for this project is SQL Server 2019.

The connection string is in the appsettings.json file, you can change the connection string to your database.

The database will be created inmediately you run the project the first time and will be provide to you the following user credential.

username: Admin
password: Abc_12345

this credential is necessary for test the project, if you have some issue triying to login with this credentials, you can create a new user using the endpoint /api/auth/register
for do this you need to remove first the RequireAuthorization in the UserModule inside RapidPay.Presentation layer.

## Usage

You can test the project using the postman collection attached or consume in your project, for this one you need to ensure has the last version of postman.

the postman collection has 3 environment variable:
bearerToken: you need to put the token that you get when you login in the application.
cardId: you need to put the card id that you get when you create a card.
url: this is the url that you need to put to test the project.

## Test

The project has some test builded using NUnit, for run the unit test you can open the project in visual studio 2022 and go to Test option, then click in Run All Test.

if you don't have Visual Studio 2022 you can follow the next instructions:

You need to have installer net core sdk in your machine, you can download it from the official page of microsoft: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
Download NUnit from their website (https://nunit.org/).

Once you have these, you can follow these steps:

Open the NUnit Test Runner application.
Locate the folder containing the project's test files.
Select the test assembly (the .dll file containing the test code).
Click the "Run" button to execute all the tests in the assembly.