using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeService.Infrastructure.Data;

public class DbInitializer
{
    public static void Execute(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope();
        ArgumentNullException.ThrowIfNull(scope);

        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ExchangeDbContext>>();
        using var context = dbContextFactory.CreateDbContext();
        var a = context.Database.GetConnectionString();
        context.Database.Migrate();
    }
}