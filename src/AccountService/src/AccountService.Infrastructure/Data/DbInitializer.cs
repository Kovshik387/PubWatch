using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService.Infrastructure.Data;

public static class DbInitializer
{
    public static void Execute(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope();
        ArgumentNullException.ThrowIfNull(scope);

        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AccountDbContext>>();
        using var context = dbContextFactory.CreateDbContext();
        var a = context.Database.GetConnectionString();
        context.Database.Migrate();
    }
}