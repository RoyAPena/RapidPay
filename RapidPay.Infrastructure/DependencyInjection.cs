using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RapidPay.Application.Abstractions;
using RapidPay.Application.Abstractions.Data;
using RapidPay.Domain.Abstractions;
using RapidPay.Domain.Cards;
using RapidPay.Domain.Transactions;
using RapidPay.Infrastructure.Authentication;
using RapidPay.Infrastructure.Data;
using RapidPay.Infrastructure.Repositories;
using RapidPay.Infrastructure.Services;

namespace RapidPay.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<ISecurityServices, SecurityServices>();
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<RapidPayContext>();
            services.AddScoped<IUserServices, UserServices>();

            services.AddSingleton<Random>();
            services.AddSingleton<IPaymentFeeService, PaymentFeeServices>();

            return services;
        }
    }
}