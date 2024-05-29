using Carter;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using RapidPay.Api.Infrastructure;
using RapidPay.Api.OptionSetup;
using RapidPay.Application.Abstractions;
using RapidPay.Application.Abstractions.Data;
using RapidPay.Domain.Abstractions;
using RapidPay.Domain.Cards;
using RapidPay.Domain.Transactions;
using RapidPay.Domain.User;
using RapidPay.Infrastructure.Authentication;
using RapidPay.Infrastructure.Data;
using RapidPay.Infrastructure.Repositories;
using RapidPay.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RapidPayContext>
    (options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("RapidPay.Infrastructure")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<ISecurityServices, SecurityServices>();

builder.Services.AddSingleton<IPaymentFeeService, PaymentFeeService>();

//Injected all command and query from the application layer. this for use CQRS
builder.Services.AddMediatR(RapidPay.Application.AssemblyReference.Assembly);

//Add global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add carter for get all the endpoint from presentation layer.
builder.Services.AddCarter();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

builder.Services.ConfigureOptions<JwtOptionsSetup>();

builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCarter();

app.Run();
