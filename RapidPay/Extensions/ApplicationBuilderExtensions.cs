using Microsoft.EntityFrameworkCore;
using RapidPay.Infrastructure.Data;

namespace RapidPay.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<RapidPayContext>();

            //Since this is a test, this will be ensure the database will created and apply migration for the first time only if doesn't exists.
            if (!context.Database.CanConnect())
            {
                context.Database.Migrate();
            }
        }
    }
}